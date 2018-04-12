using System.Collections.Generic;

/**
* Abstraction of the bitmask for stimulation channels
*/

public class ChannelMask
{
    private HashSet<int> activeChannels; // List of active channels

    public ChannelMask()
    {
        activeChannels = new HashSet<int>();
    }

    public bool setChannel(int channelNumber, bool active = true)
    {
        if (channelNumber < 1 || channelNumber > 8)
        {
            return false;
        }

        if (active)
        {
            activeChannels.Add(channelNumber);
        }
        else
        {
            activeChannels.Remove(channelNumber);
        }

        return true;
    }

    public bool setChannel(Constants.Channels channelNumber, bool active = true)
    {
        if (channelNumber == Constants.Channels.ChannelNone)
            return false;

        if (active)
        {
            activeChannels.Add((int)channelNumber);
        }
        else
        {
            activeChannels.Remove((int)channelNumber);
        }

        return true;
    }

    public int toInt()
    {
        int result = 0;
        foreach (var channel in activeChannels)
        {
            result += (1 << (channel - 1));
        }
        return result;
    }

    public HashSet<int> getActiveChannels()
    {
        return activeChannels;
    }
}