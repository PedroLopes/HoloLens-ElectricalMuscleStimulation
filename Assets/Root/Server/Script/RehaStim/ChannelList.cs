using System.Collections.Generic;
using UnityEngine;

/**
 *	This Class implements the channel list pulse generation mode for the RehaStim EMS device.
 *	Module A: Channels 1-4
 *	Module B: Channels 5-8
 */

public class ChannelList // : MonoBehaviour
{
    //private static ChannelMask activeChannels;
    //private static ChannelMask activeChannelsLowFreq;
    private static SortedDictionary<int, UpdateInfo> currentConfig; //maps channel number to config

    public static bool isRunning = false;

    //private void OnDisable()
    //{
    //    ChannelList.Stop();
    //}

    public static void Stop()
    {
        byte[] cmd = ChannelList.GetStopCommand();
        RehaStimInterface.sendMessage(cmd);
        isRunning = false;
    }

    public struct channelListConfig
    {
        public int nFactor;
        public ChannelMask channelsStim;
        public ChannelMask channelsLowFreq;
        public int groupFrequency;
        public int mainFrequency;
        public int maxPulseGroup;
    }

    public static void InitCMFromConfig(channelListConfig config)
    {
        InitCM(config.nFactor
            , config.channelsStim
            , config.channelsLowFreq
            , config.groupFrequency
            , config.mainFrequency
            , config.maxPulseGroup);
    }

    public static channelListConfig lastConfig;

    public static void InitCM(int nFactor, ChannelMask channelsStim, ChannelMask channelsLowFreq, int groupFrequency, int mainFrequency, int maxPulseGroup = 1)
    {
        // save config
        lastConfig = new channelListConfig();
        lastConfig.nFactor = nFactor;
        lastConfig.channelsStim = channelsStim;
        lastConfig.channelsLowFreq = channelsLowFreq;
        lastConfig.groupFrequency = groupFrequency;
        lastConfig.mainFrequency = mainFrequency;
        lastConfig.maxPulseGroup = maxPulseGroup;

        int groupTime = ChannelList.HerzToGroupTime(groupFrequency);
        //groupTime needs to be at least max (#channels,#lowFreq)  * 1.5
        int minGroupTime = (int)Mathf.Ceil(
                                    Mathf.Max(channelsStim.getActiveChannels().Count
                                            , channelsLowFreq.getActiveChannels().Count)
                                    * 1.5f
                                );
        groupTime = Mathf.Max(groupTime, minGroupTime);

        int mainTime = ChannelList.HerzToMainTime(mainFrequency);
        //mainTime needs to be at least pusleGroupSize * groupTime + 1.5ms
        int minMainTime = (int)Mathf.Ceil(maxPulseGroup * groupTime + 1.5f);
        mainTime = Mathf.Max(mainTime, minMainTime);

        //Debug.Log(mainTime + "|" +  groupTime);

        byte[] cmd = ChannelList.GetInitCommand(nFactor, channelsStim.toInt(), channelsLowFreq.toInt(), groupTime, mainTime);
        RehaStimInterface.sendMessage(cmd);
        //ChannelList.activeChannels = channelsStim;
        //ChannelList.activeChannelsLowFreq = channelsLowFreq;
        currentConfig = new SortedDictionary<int, UpdateInfo>();
        //init config
        foreach (var channel in channelsStim.getActiveChannels())
        {
            currentConfig.Add(channel, new UpdateInfo(UpdateInfo.MODE_SINGLE, 0, 0));
        }
        isRunning = true;
    }

    /**
     * Approximates the needed mainTime for the given frequency in Hz
     * @returns the mainTime to achieve the desired frequency
     */

    private static int HerzToMainTime(int hz)
    {
        if (hz < 1)
            return 0;

        // t_s1 = Main_Time * 0.5 + 1ms
        // freq = 1000/ t_s1
        int value = (int)Mathf.Ceil(2.0f * ((1000.0f / hz) - 1.0f));

        //max value = 2047;
        return Mathf.Min(2047, value);
    }

    /**
     * Approximates the needed groupTime for the given frequency in Hz
     * @returns the groupTime to achieve the desired frequency
     */

    private static int HerzToGroupTime(int hz)
    {
        if (hz < 1)
            return 0;

        // t_s2 = Group_Time * 0.5 + 1.5ms
        // freq = 1000/ t_s1
        int value = (int)Mathf.Ceil(2.0f * ((1000.0f / hz) - 1.5f));

        //max value = 31;
        return Mathf.Min(31, value);
    }

    public static bool SetMaxCurrent(int channelNumber, int maxCurrent)
    {
        if (!currentConfig.ContainsKey(channelNumber))
            return false;

        currentConfig[channelNumber].maxCurrent = maxCurrent;
        return true;
    }

    public static bool SetMaxWidth(int channelNumber, int maxWidth)
    {
        if (!currentConfig.ContainsKey(channelNumber))
            return false;

        currentConfig[channelNumber].maxWidth = maxWidth;
        return true;
    }

    /**
     * Increase/Decrease the pulseWidth and/or current for a single channel
     * int channelNumber    - the id of the channel to update
     * Updateinfo info      - mode, pulseWidth, pulseCurrent to update
     * bool sendMessage     - wether or not to immediately update on the RehaStim device
     * @returns false when the channel is not active
     */

    public static bool IncreaseIntensity(int channelNumber, int pulseWidth, int pulseCurrent, bool sendMessage = true)
    {
        if (!currentConfig.ContainsKey(channelNumber))
            return false;

        UpdateInfo info = currentConfig[channelNumber];
        info.pulseWidth = Mathf.Max(0, info.pulseWidth + pulseWidth);
        info.pulseCurrent = Mathf.Max(0, info.pulseCurrent + pulseCurrent);

        ChannelList.UpdateChannel(channelNumber, info, sendMessage);
        return true;
    }

    /**
     * Set the configuration of a single channel
     * int channelNumber    - the id of the channel to update
     * Updateinfo info      - mode, pulseWidth, pulseCurrent to update
     * bool sendMessage     - wether or not to immediately update on the RehaStim device
     * returns false if the channel is not active
     */

    public static bool UpdateChannel(int channelNumber, UpdateInfo info, bool sendMessage = true)
    {
        if (!currentConfig.ContainsKey(channelNumber))
            return false;
        currentConfig.Remove(channelNumber);
        currentConfig.Add(channelNumber, info);

        if (sendMessage)
        {
            List<UpdateInfo> infos = new List<UpdateInfo>(currentConfig.Values);
            byte[] cmd = ChannelList.GetUpdateCommand(infos);
            RehaStimInterface.sendMessage(cmd);
        }
        return true;
    }

    public static bool UpdateChannel(Constants.Channels channelNumber, UpdateInfo info, bool sendMessage = true)
    {
        if (!currentConfig.ContainsKey((int)channelNumber))
            return false;
        currentConfig.Remove((int)channelNumber);
        currentConfig.Add((int)channelNumber, info);

        if (sendMessage)
        {
            List<UpdateInfo> infos = new List<UpdateInfo>(currentConfig.Values);
            byte[] cmd = ChannelList.GetUpdateCommand(infos);
            RehaStimInterface.sendMessage(cmd);
        }
        return true;
    }

    /**
     * Stops a single channel (set width/current to 0)
     * int channelNumber    - the id of the channel to stop
     * bool sendMessage     - wether or not to immediately update on the RehaStim device
     * returns false if the channel is not active
     */

    public static bool StopChannel(int channelNumber, bool sendMessage = true)
    {
        return ChannelList.UpdateChannel(channelNumber, new UpdateInfo(UpdateInfo.MODE_SINGLE, 0, 0), sendMessage);
    }

    /**
     * int channelNumber    - the id of the channel
     * returns the current configuration of the specified channel
     */

    public static UpdateInfo GetChannelConfig(int channelNumber)
    {
        if (!currentConfig.ContainsKey(channelNumber))
            return null;

        return currentConfig[channelNumber];
    }

    public static UpdateInfo GetChannelConfig(Constants.Channels channelNumber)
    {
        if (!currentConfig.ContainsKey((int)channelNumber))
            return null;

        return currentConfig[(int)channelNumber];
    }

    /**
     *
     *
     */

    public static byte[] GetStopCommand()
    {
        int ident = Constants.CHANNEL_STOP;
        int checksum = 0;
        int byte1 = (1 << 7) + (ident << 5) + checksum;
        return new byte[1] { (byte)byte1 };
    }

    /**
     *
     * int nFactor			- 0..7 		: Defines how many times the stimulation is skipped for specified channels
     * int channelsStim		- 0..255	: Defines the active channels as a bitmask, bit0 corresponds to channel 1
     * int channelsLowFreq	- 0..255	: Defines the low frequency channels as a bitmask, bit0 corresponds to channel 1
     * int groupTime (t2)	- 0..31		: Defines the interpulse-interval ts2 by groupTime * 0.5 ms + 1.5 ms
   	 * 																	needs to be at least 1.5 * max(#channelsA, #channelsB)
     * int mainTime	(t1)	- 0..2047	:	Defines the main time period ts1 by mainTime * 0.5 ms +1 ms
     */

    public static byte[] GetInitCommand(int nFactor, int channelsStim, int channelsLowFreq, int groupTime, int mainTime)
    {
        int ident = Constants.CHANNEL_INIT;
        int checksum = (nFactor + channelsStim + channelsLowFreq + groupTime + mainTime) % 8;

        int byte1 = (1 << 7)
                     + (ident << 5)
                     + (checksum << 2)
                     + (nFactor >> 1);

        int byte2 = ((nFactor & 0x1) << 6)
                     + (channelsStim >> 2);

        int byte3 = ((channelsStim & 0x3) << 5)
                     + (channelsLowFreq >> 3);

        int byte4 = ((channelsLowFreq & 0x7) << 4)
                     + (groupTime >> 3);

        int byte5 = ((groupTime & 0x7) << 4)
                     + (mainTime >> 7);

        int byte6 = mainTime & 0x7f;

        byte[] command = new byte[6] {
              (byte) byte1
            , (byte) byte2
            , (byte) byte3
            , (byte) byte4
            , (byte) byte5
            , (byte) byte6
            };

        /*
        //print (System.Convert.ToString (byte1, 2));
        //print (System.Convert.ToString (byte2, 2));
        //print (System.Convert.ToString (byte3, 2));
        //print (System.Convert.ToString (byte4, 2));
        //print (System.Convert.ToString (byte5, 2));
        //print (System.Convert.ToString (byte6, 2));
        //*/

        return command;
    }

    /**
     *
     *
     */

    public static byte[] GetUpdateCommand(List<UpdateInfo> UpdateInfos)
    {
        int ident = Constants.CHANNEL_UPDATE;
        int numChannels = UpdateInfos.Count;
        byte[] command = new byte[1 + 3 * numChannels];
        int checksum = 0;

        int i = 0;
        foreach (UpdateInfo info in UpdateInfos)
        {
            int mode = info.mode;
            int pulse_width = info.pulseWidth;
            int pulse_current = info.pulseCurrent;

            //safety check
            if (pulse_current >= info.maxCurrent)
            {
                //print("SAFETY LIMIT (of " + Constants.safetyLimit + " EXCEEDED. Request of " + pulse_current + "dropped to limit");
                pulse_current = info.maxCurrent;
            }

            if (pulse_width >= info.maxWidth)
            {
                //print("SAFETY LIMIT (of " + Constants.maxPulseWidth + " EXCEEDED. Request of " + pulse_width + "dropped to limit");
                pulse_width = info.maxWidth;
            }

            // update checksum
            checksum += mode + pulse_width + pulse_current;

            int byte2 = (mode << 5) + (pulse_width >> 7); // channel number on bit 4,5,6 ; pulse width 7,8 on bit 1,2
            int byte3 = pulse_width & 0x7f;
            int byte4 = pulse_current;

            /*
            //print (System.Convert.ToString (byte2, 2).PadLeft(8,'0'));
            //print (System.Convert.ToString (byte3, 2).PadLeft(8,'0'));
            //print (System.Convert.ToString (byte4, 2).PadLeft(8,'0'));
            //*/

            // add bytes to command
            command[i * 3 + 1] = (byte)byte2;
            command[i * 3 + 2] = (byte)byte3;
            command[i * 3 + 3] = (byte)byte4;

            //increase counter
            i++;
        }

        // apply modulo to checksum
        checksum = checksum % 32;
        //  add header byte
        int byte1 = (1 << 7) + (ident << 5) + checksum; // 1 Padding + 2 Bit ident + 5 bit checksum
        command[0] = (byte)byte1;

        /*
        //print ((byte1 << 24) + (byte2 << 16) + (byte3 << 8) + byte4);
        //print (System.Convert.ToString (byte1, 2));
        //print (System.Convert.ToString (byte2, 2));
        //print (System.Convert.ToString (byte3, 2));
        //print (System.Convert.ToString (byte4, 2));

        //print (System.BitConverter.ToString(command));
        //*/
        return command;
    }
}