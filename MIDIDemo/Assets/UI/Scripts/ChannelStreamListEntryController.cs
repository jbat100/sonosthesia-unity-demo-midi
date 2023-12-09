using UnityEngine.UIElements;
using Sonosthesia.UI;

namespace Sonosthesia.UI
{
    public class ChannelStreamListEntryController : ISimpleListEntryController<ChannelStreamUIData>
    {
        private Label _countLabel;
        private Label _descriptionLabel;
        private Label _timestampLabel;

        public void SetVisualElement(VisualElement visualElement)
        {
            _countLabel = visualElement.Q<Label>("CountLabel");
            _descriptionLabel = visualElement.Q<Label>("DescriptionLabel");
            _timestampLabel = visualElement.Q<Label>("TimestampLabel");
        }

        public void SetData(ChannelStreamUIData? data)
        {
            if (data.HasValue)
            {
                _countLabel.text = $"{data.Value.Count}";
                _descriptionLabel.text = data.Value.Description;
                _timestampLabel.text = $"{data.Value.Timestamp.TotalMilliseconds:0.##}";    
            }
        }
    }
}

