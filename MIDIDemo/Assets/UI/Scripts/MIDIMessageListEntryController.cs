using Sonosthesia.UI;
using UnityEngine.UIElements;

namespace Sonosthesia.UI
{
    public class MIDIMessageListEntryController : ISimpleListEntryController<MIDIMessageUIData>
    {
        private Label _countLabel;
        private Label _typeLabel;
        private Label _dataLabel;
        private Label _timestampLabel;

        public void SetVisualElement(VisualElement visualElement)
        {
            _countLabel = visualElement.Q<Label>("CountLabel");
            _typeLabel = visualElement.Q<Label>("TypeLabel");
            _dataLabel = visualElement.Q<Label>("DataLabel");
            _timestampLabel = visualElement.Q<Label>("TimestampLabel");
        }

        public void SetData(MIDIMessageUIData? data)
        {
            if (data.HasValue)
            {
                _countLabel.text = $"{data.Value.Count}";
                _typeLabel.text = data.Value.Type;
                _dataLabel.text = data.Value.Data;
                _timestampLabel.text = $"{data.Value.Timestamp.TotalMilliseconds:0.##} ms";    
            }
        }
    }

}

