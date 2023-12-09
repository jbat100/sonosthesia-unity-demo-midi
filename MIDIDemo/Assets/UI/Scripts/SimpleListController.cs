using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Sonosthesia.UI
{

    public interface ISimpleListEntryController<TEntryData> where TEntryData : struct
    {
        void SetVisualElement(VisualElement visualElement);

        void SetData(TEntryData? data);
    }
    
    // just data display, no selection or other interaction
    
    public class SimpleListController<TEntryData, TEntryController> 
        where TEntryData : struct
        where TEntryController : class, ISimpleListEntryController<TEntryData>, new()
    {
        public SimpleListController()
        {
            _fixedItemHeight = 27;
        }

        public SimpleListController(float fixedItemHeight)
        {
            _fixedItemHeight = fixedItemHeight;
        }

        private float _fixedItemHeight;
        
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

        private readonly List<TEntryData> _data = new ();

        public void ImportData(IEnumerable<TEntryData> data)
        {
            _data.Clear();
            _data.AddRange(data);
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
                TEntryController newListEntryLogic = new TEntryController();

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
                (item.userData as TEntryController).SetData(_data[index]);
            };

            // Set a fixed item height
            _messageListView.fixedItemHeight = _fixedItemHeight;

            // Set the actual item's source list/array
            _messageListView.itemsSource = _data;
        }
    }
}