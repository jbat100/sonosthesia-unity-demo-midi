using UnityEngine.UIElements;

namespace Sonosthesia.Demo
{
    public class MIDIMessageListEntryController
    {
        private Label _countLabel;
        private Label _typeLabel;
        private Label _dataLabel;
        private Label _timestampLabel;

        //This function retrieves a reference to the 
        //character name label inside the UI element.

        public void SetVisualElement(VisualElement visualElement)
        {
            _countLabel = visualElement.Q<Label>("CountLabel");
            _typeLabel = visualElement.Q<Label>("TypeLabel");
            _dataLabel = visualElement.Q<Label>("DataLabel");
            _timestampLabel = visualElement.Q<Label>("TimestampLabel");
        }

        //This function receives the character whose name this list 
        //element displays. Since the elements listed 
        //in a `ListView` are pooled and reused, it's necessary to 
        //have a `Set` function to change which character's data to display.

        public void SetData(MIDIMessageUIData? data)
        {
            if (data.HasValue)
            {
                _countLabel.text = $"{data.Value.Count}";
                _typeLabel.text = data.Value.Type;
                _dataLabel.text = data.Value.Data;
                _timestampLabel.text = $"{data.Value.Timestamp.TotalMilliseconds:0.##}";    
            }
            
        }
    }

}

