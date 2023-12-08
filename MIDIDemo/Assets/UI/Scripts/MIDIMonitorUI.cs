using System.Linq;
using Sonosthesia.AdaptiveMIDI;
using Sonosthesia.UI;
using Sonosthesia.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sonosthesia.UI
{
    public class MIDIMonitorUI : MonoBehaviour
    {
        [SerializeField] private MIDIMessageNode _broadcaster;
        
        [SerializeField] private UIDocument _document;

        [SerializeField] private int _messageCapacity = 10;
 
        [SerializeField] VisualTreeAsset _listEntryTemplate;

        private Label _titleLabel;
        private Button _clearButton;
        private Toggle _clockToggle;
        private Toggle _syncToggle;
        private Toggle _channelToggle;

        private bool _dirty;
        private int _messageCount;
        private CircularBuffer<MIDIMessageUIData> _messageBuffer;
        private SimpleListController<MIDIMessageUIData, MIDIMessageListEntryController> _listController;

        private readonly CompositeDisposable _subscriptions = new();

        protected virtual void OnValidate()
        {
            if (isActiveAndEnabled && Application.isPlaying)
            {
                ReloadSubscriptions();   
            }
            _dirty = true;
        }
        
        protected virtual void OnEnable()
        {
            VisualElement rootElement = _document.rootVisualElement;
            
            _listController = new SimpleListController<MIDIMessageUIData, MIDIMessageListEntryController>();
            ListView listView = rootElement.Q<ListView>("MessageList");
            _listController.InitializeList(listView, _listEntryTemplate);
            _listController.ImportData(Enumerable.Empty<MIDIMessageUIData>());
            
            void BindReloadToggle(VisualElement rootElement, string name, out Toggle toggle)
            {
                toggle = rootElement.Q<Toggle>(name);
                toggle?.RegisterValueChangedCallback(ReloadToggleValueChaned);
            }

            _titleLabel = rootElement.Q<Label>("Title");

            BindReloadToggle(rootElement, "ClockToggle", out _clockToggle);
            BindReloadToggle(rootElement, "SyncToggle", out _syncToggle);
            BindReloadToggle(rootElement, "ChannelToggle", out _channelToggle);
            
            _clockToggle.value = true;
            _syncToggle.value = true;
            _channelToggle.value = true;
            
            _clearButton = rootElement.Q<Button>("ClearButton");
            _clearButton.clickable.clicked += OnClearButtonClicked;
            
            _messageBuffer = new CircularBuffer<MIDIMessageUIData>(_messageCapacity);
            ReloadSubscriptions();
            _dirty = true;
        }

        protected virtual void OnDisable()
        {
            ClearMessages();
            _subscriptions.Clear();
            _messageBuffer = null;
            
            void UnbindReloadToggle(Toggle toggle)
            {
                toggle?.UnregisterValueChangedCallback(ReloadToggleValueChaned);
            }
            
            _clearButton.clickable.clicked -= OnClearButtonClicked;
            UnbindReloadToggle(_clockToggle);
            UnbindReloadToggle(_syncToggle);
            UnbindReloadToggle(_channelToggle);
        }

        protected virtual void Update()
        {
            if (_dirty)
            {
                _listController.ImportData(_messageBuffer ?? Enumerable.Empty<MIDIMessageUIData>());
                _dirty = false;
            }
        }

        public void ClearMessages()
        {
            _messageBuffer.Clear();
            _dirty = true;
        }
        
        private void ReloadSubscriptions()
        {
            _subscriptions.Clear();

            void Push(MIDIMessageUIData data)
            {
                _messageBuffer?.PushFront(data);
                _dirty = true;
            }
            
            if (_clockToggle.value)
            {
                _subscriptions.Add(_broadcaster.ClockObservable.Subscribe(m 
                    => Push(m.UIData(_messageCount++))));    
            }

            if (_syncToggle.value)
            {
                _subscriptions.Add(_broadcaster.SyncObservable.Subscribe(m 
                    => Push(m.UIData(_messageCount++))));
                _subscriptions.Add(_broadcaster.SongPositionPointerObservable.Subscribe(m 
                    => Push(m.UIData(_messageCount++))));    
            }

            if (_channelToggle.value)
            {
                _subscriptions.Add(_broadcaster.NoteOnObservable.Subscribe(m 
                    => Push(m.UIData(_messageCount++))));
                _subscriptions.Add(_broadcaster.NoteOffObservable.Subscribe(m 
                    => Push(m.UIData(_messageCount++))));
                _subscriptions.Add(_broadcaster.ControlObservable.Subscribe(m 
                    => Push(m.UIData(_messageCount++))));
                _subscriptions.Add(_broadcaster.PolyphonicAftertouchObservable.Subscribe(m 
                    => Push(m.UIData(_messageCount++))));
                _subscriptions.Add(_broadcaster.ChannelAftertouchObservable.Subscribe(m 
                    => Push(m.UIData(_messageCount++))));  
                _subscriptions.Add(_broadcaster.PitchBendObservable.Subscribe(m 
                    => Push(m.UIData(_messageCount++))));    
            }
        }

        protected virtual void ReloadToggleValueChaned(ChangeEvent<bool> changeEvent)
        {
            ReloadSubscriptions();
        }

        private void OnClearButtonClicked()
        {
            ClearMessages();
        }
    }    
}


