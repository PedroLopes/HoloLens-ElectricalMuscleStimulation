/**
 *	This Class implements the single pulse generation mode for the RehaStim EMS device.
 *
 */

public class SinglePulse
{
    public static bool sendSinglePulse(int channel_number, int pulse_width, int pulse_current)
    {
        if (channel_number == 0)
            return false;

        byte[] command = SinglePulse.getCommand(channel_number, pulse_width, pulse_current);
        RehaStimInterface.sendMessage(command);

        return true;
    }

    public static bool sendSinglePulse(Constants.Channels channel_number, int pulse_width, int pulse_current)
    {
        if (channel_number == Constants.Channels.ChannelNone)
            return false;

        byte[] command = SinglePulse.getCommand((int)channel_number, pulse_width, pulse_current);
        RehaStimInterface.sendMessage(command);

        return true;
    }

    private const int MAX_FREQ = 350; // max frequency in Hz that the EMS device can handle

    /**
     * Generates a 4 byte command for a single pulse to be sent to the EMS Device
     * int channel_number 	- 1..8 										: channel number on the back of the device
     * int pulse_width 		- 0, 10..500 							: pulse width in microseconds
     * int pulse_current 	- 0..127 (safety_limit 30): current in mA
     */

    public static byte[] getCommand(int channel_number, int pulse_width, int pulse_current)
    {
        channel_number -= 1; // channels are indexed from 0-7 internally
        int ident = Constants.SINGLE_PULSE;

        if (pulse_current >= RehaStimInterface.maxPulseCurrent)
        {
            //print("SAFETY LIMIT (of " + Constants.safetyLimit + " EXCEEDED. Request of " + pulse_current + "dropped to limit");
            pulse_current = RehaStimInterface.maxPulseCurrent;
        }
        if (pulse_width >= RehaStimInterface.maxPulseWidth)
        {
            //print("SAFETY LIMIT (of " + Constants.maxPulseWidth + " EXCEEDED. Request of " + pulse_width + "dropped to limit");
            pulse_width = RehaStimInterface.maxPulseWidth;
        }

        int checksum = (channel_number + pulse_width + pulse_current) % 32;
        int byte1 = (1 << 7) + (ident << (int)5) + checksum; // 1 Padding + 2 Bit ident + 5 bit checksum
        int byte2 = (channel_number << 4) + (pulse_width >> 7); // channel number on bit 4,5,6 ; pulse width 7,8 on bit 1,2
        int byte3 = pulse_width & 0x7f;
        int byte4 = pulse_current;

        byte[] command = new byte[4] {
            (byte)byte1
            , (byte)byte2
            , (byte)byte3
            , (byte)byte4
            };

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