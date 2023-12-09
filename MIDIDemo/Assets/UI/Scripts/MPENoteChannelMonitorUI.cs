using Sonosthesia.MIDI;

namespace Sonosthesia.UI
{
    public class MPENoteChannelMonitorUI : ChannelMonitorUI<MPENote>
    {
        protected override string GetDescription(MPENote data)
        {
            return $"<note {data.Note:D3} velocity {data.Velocity:D3} slide {data.Slide:D3} pressure {data.Pressure:D3} bend {data.Bend,2:F2}>";
        }
    }
}