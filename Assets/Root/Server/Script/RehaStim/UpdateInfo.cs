/**
 * Class for the configuration of a channel for the ChannelList mode.
 */

public class UpdateInfo
{
    public int mode;            //  0: generate single pulse, 1: generate doublet, 2: generate triplet
    public int pulseWidth;     //  pulse width in microseconds
    public int pulseCurrent;   //  current in mA
    public int maxWidth = RehaStimInterface.maxPulseWidth;
    public int maxCurrent = RehaStimInterface.maxPulseCurrent;

    public const int MODE_SINGLE = 0;
    public const int MODE_DOUBLET = 1;
    public const int MODE_TRIBLET = 2;

    public UpdateInfo(int m, int w, int c)
    {
        mode = m;
        pulseWidth = w;
        pulseCurrent = c;
    }

    public UpdateInfo()
    {
        mode = 0;
        pulseWidth = 0;
        pulseCurrent = 0;
    }
}