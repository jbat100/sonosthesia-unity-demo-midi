using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Sonosthesia.Demo
{
    public class MIDIMessageListContoller
    {
        // UXML template for list entries
        private VisualTreeAsset _listEntryTemplate;

        // UI element references
        private ListView _messageListView;

        public void InitializeList(ListView messageListView, VisualTreeAsset listElementTemplate)
        {
            Assert.IsNotNull(messageListView);
            Assert.IsNotNull(listElementTemplate);
            _listEntryTemplate = listElementTemplate;
            _messageListView = messageListView;
            Setup();
        }

        private readonly List<MIDIMessageUIData> _midiMessageData = new ();

        public void Apply(IEnumerable<MIDIMessageUIData> data)
        {
            _midiMessageData.Clear();
            _midiMessageData.AddRange(data);
            _messageListView.RefreshItems();
        }

        void Setup()
        {
            // Set up a make item function for a list entry
            _messageListView.makeItem = () =>
            {
                // Instantiate the UXML template for the entry
                TemplateContainer newListEntry = _listEntryTemplate.Instantiate();

                // Instantiate a controller for the data
                MIDIMessageListEntryController newListEntryLogic = new MIDIMessageListEntryController();

                // Assign the controller script to the visual element
                newListEntry.userData = newListEntryLogic;

                // Initialize the controller script
                newListEntryLogic.SetVisualElement(newListEntry);

                // Return the root of the instantiated visual tree
                return newListEntry;
            };

            // Set up bind function for a specific list entry
            _messageListView.bindItem = (item, index) =>
            {
                (item.userData as MIDIMessageListEntryController).SetData(_midiMessageData[index]);
            };

            // Set a fixed item height
            _messageListView.fixedItemHeight = 27;

            // Set the actual item's source list/array
            _messageListView.itemsSource = _midiMessageData;
        }
    }    
}


