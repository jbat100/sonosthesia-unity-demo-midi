using System;

namespace Sonosthesia.UI
{
    public readonly struct ChannelStreamUIData
    {
        public readonly int Count;
        public readonly string Description;
        public readonly TimeSpan Timestamp;

        public ChannelStreamUIData(int count, string description, TimeSpan timestamp)
        {
            Count = count;
            Description = description;
            Timestamp = timestamp;
        }
    }
}