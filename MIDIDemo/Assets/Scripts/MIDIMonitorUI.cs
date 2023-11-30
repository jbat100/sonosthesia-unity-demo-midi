using System;
using System.Linq;
using Sonosthesia.AdaptiveMIDI;
using Sonosthesia.AdaptiveMIDI.Messages;
using Sonosthesia.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sonosthesia.Demo
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
            return new MIDIMessageUIData(count, "position", $"<beats {pointer.Position}>", pointer.Timestamp);
        }
        
        public static MIDIMessageUIData UIData(this MIDINote note, bool on, int count)
        {
            return new MIDIMessageUIData(count, $"note-{(on ? "on" : "off")}", $"<chan {note.Channel} pitch {note.Note} vel {note.Velocity}>", note.Timestamp);
        }
        
        public static MIDIMessageUIData UIData(this MIDIPolyphonicAftertouch aftertouch, int count)
        {
            return new MIDIMessageUIData(count, "poly-aftertouch", $"<chan {aftertouch.Channel} pitch {aftertouch.Note} val {aftertouch.Value}>", aftertouch.Timestamp);
        }
        
        public static MIDIMessageUIData UIData(this MIDIControl control, int count)
        {
            return new MIDIMessageUIData(count, "control", $"<chan {control.Channel} num {control.Number} val {control.Value}>", control.Timestamp);
        }
        
        public static MIDIMessageUIData UIData(this MIDIChannelAftertouch aftertouch, int count)
        {
            return new MIDIMessageUIData(count, "chan-aftertouch", $"<chan {aftertouch.Channel} val {aftertouch.Value}>", aftertouch.Timestamp);
        }
        
        public static MIDIMessageUIData UIData(this MIDIPitchBend bend, int count)
        {
            return new MIDIMessageUIData(count, "pitch-bend", $"<chan {bend.Channel} val {bend.Value}>", bend.Timestamp);
        }
    }
    
    public class MIDIMonitorUI : MonoBehaviour
    {
        [SerializeField] private MIDIMessageBroadcaster _broadcaster;
        
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
        private MIDIMessageListContoller _listController;

        private readonly CompositeDisposable _subscriptions = new();

        protected virtual void OnEnable()
        {
            VisualElement rootElement = _document.rootVisualElement;
            
            _listController = new MIDIMessageListContoller();
            ListView listView = rootElement.Q<ListView>("MessageList");
            _listController.InitializeList(listView, _listEntryTemplate);
            _listController.Apply(Enumerable.Empty<MIDIMessageUIData>());
            
            void BindReloadToggle(VisualElement rootElement, string name, out Toggle toggle)
            {
                toggle = rootElement.Q<Toggle>(name);
                toggle?.RegisterValueChangedCallback(ReloadToggleValueChaned);
            }

            _titleLabel = rootElement.Q<Label>("Title");

            BindReloadToggle(rootElement, "ClockToggle", out _clockToggle);
            BindReloadToggle(rootElement, "SyncToggle", out _syncToggle);
            BindReloadToggle(rootElement, "ChannelToggle", out _channelToggle);
            
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
                _listController.Apply(_messageBuffer ?? Enumerable.Empty<MIDIMessageUIData>());
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
                _messageBuffer.PushFront(data);
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
                    => Push(m.UIData(true, _messageCount++))));
                _subscriptions.Add(_broadcaster.NoteOffObservable.Subscribe(m 
                    => Push(m.UIData(false, _messageCount++))));
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


