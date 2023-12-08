using Sonosthesia.MIDI;

namespace Sonosthesia.UI
{
    public class MIDINoteChannelMonitorUI : ChannelMonitorUI<MIDINote>
    {
        protected override string GetDescription(MIDINote data)
        {
            return $"<channel {data.Channel:D2} note {data.Note:D3} velocity {data.Velocity:D3} pressure {data.Pressure:D3}>";
        }
    }
}
