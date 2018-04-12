public class Constants
{
    public const int MAX_FREQ = 350; // max frequency in Hz that the EMS device can handle
    public const int CHANNEL_INIT = 0;
    public const int CHANNEL_UPDATE = 1;
    public const int CHANNEL_STOP = 2;
    public const int SINGLE_PULSE = 3;

    public enum Channels : int
    {
        ChannelNone = 0,
        Channel1 = 1,
        Channel2 = 2,
        Channel3 = 3,
        Channel4 = 4,
        Channel5 = 5,
        Channel6 = 6,
        Channel7 = 7,
        Channel8 = 8,
    };
}