using System;
using Sonosthesia.AdaptiveMIDI.Messages;

namespace Sonosthesia.UI
{
    public readonly struct MIDIMessageUIData
    {
        public readonly int Count;
        public readonly string Type;
        public readonly string Data;
        public readonly TimeSpan Timestamp;

        public MIDIMessageUIData(int count, string type, string data, TimeSpan timestamp)
        {
            Count = count;
            Type = type;
            Data = data;
            Timestamp = timestamp;
        }
    }
    
    static class MIDIMessageUIExtensions
    {
        private static MIDIMessageUIData FullDescription(string messageDescription)
        {
            return default;
        }
        
        public static MIDIMessageUIData UIData(this MIDIClock clock, int count)
        {
            return new MIDIMessageUIData(count, "clock", $"<tick {clock.Count}>", clock.Timestamp);
        }
        
        public static MIDIMessageUIData UIData(this MIDISync sync, int count)
        {
            return new MIDIMessageUIData(count, "sync", $"<{sync.Type}>", sync.Timestamp);
        }
        
        public static MIDIMessageUIData UIData(this MIDISongPositionPointer pointer, int count)
        {
            return new MIDIMessageUIData(count, "position", $"<beats {pointer.Position:D4}>", pointer.Timestamp);
        }
        
        public static MIDIMessageUIData UIData(this MIDINoteOn note, int count)
        {
            return new MIDIMessageUIData(count, $"note-on", $"<chan {note.Channel:D2} pitch {note.Note:D3} vel {note.Velocity:D3}>", note.Timestamp);
        }
        
        public static MIDIMessageUIData UIData(this MIDINoteOff note, int count)
        {
            return new MIDIMessageUIData(count, $"note-off", $"<chan {note.Channel:D2} pitch {note.Note:D3} vel {note.Velocity:D3}>", note.Timestamp);
        }
        
        public static MIDIMessageUIData UIData(this MIDIPolyphonicAftertouch aftertouch, int count)
        {
            return new MIDIMessageUIData(count, "poly-aftertouch", $"<chan {aftertouch.Channel:D2} pitch {aftertouch.Note:D3} val {aftertouch.Value:D3}>", aftertouch.Timestamp);
        }
        
        public static MIDIMessageUIData UIData(this MIDIControl control, int count)
        {
            return new MIDIMessageUIData(count, "control", $"<chan {control.Channel:D2} num {control.Number:D3} val {control.Value:D3}>", control.Timestamp);
        }
        
        public static MIDIMessageUIData UIData(this MIDIChannelAftertouch aftertouch, int count)
        {
            return new MIDIMessageUIData(count, "chan-aftertouch", $"<chan {aftertouch.Channel:D2} val {aftertouch.Value:D3}>", aftertouch.Timestamp);
        }
        
        public static MIDIMessageUIData UIData(this MIDIPitchBend bend, int count)
        {
            return new MIDIMessageUIData(count, "pitch-bend", $"<chan {bend.Channel:D2} val {bend.Value,2:F2}>", bend.Timestamp);
        }
    }
}