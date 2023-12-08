using UnityEngine;
using UnityEngine.UIElements;
using Sonosthesia.AdaptiveMIDI;
using Sonosthesia.AdaptiveMIDI.Messages;

namespace Sonosthesia.UI
{
    public class MIDITestUI : MonoBehaviour
    {
        private const float PITCH_BEND_SEMITONE_RANGE = 48f;
        
        [SerializeField] private MIDIMessageNode _broadcaster;
        
        [SerializeField] private UIDocument _document;

        private Button _noteOnButton;
        private IntegerField _noteOnChannel;
        private IntegerField _noteOnPitch;
        private IntegerField _noteOnVelocity;
        
        private Button _noteOffButton;
        private IntegerField _noteOffChannel;
        private IntegerField _noteOffPitch;
        private IntegerField _noteOffVelocity;
        
        private Button _controlButton;
        private IntegerField _controlChannel;
        private IntegerField _controlNumber;
        private IntegerField _controlValue;
        
        private Button _polyAftertouchButton;
        private IntegerField _polyAftertouchChannel;
        private IntegerField _polyAftertouchPitch;
        private IntegerField _polyAftertouchValue;
        
        private Button _pitchBendButton;
        private IntegerField _pitchBendChannel;
        private FloatField _pitchBendSemitones;
        
        private Button _channelAftertouchButton;
        private IntegerField _channelAftertouchChannel;
        private IntegerField _channelAftertouchValue;
        
        protected virtual void OnEnable()
        {
            VisualElement rootElement = _document.rootVisualElement;
            
            _noteOnButton = rootElement.Q<Button>("NoteOnButton");
            _noteOnChannel = rootElement.Q<IntegerField>("NoteOnChannel");
            _noteOnPitch = rootElement.Q<IntegerField>("NoteOnPitch");
            _noteOnVelocity = rootElement.Q<IntegerField>("NoteOnVelocity");
            _noteOnButton.clickable.clicked += OnNoteOnClicked;
            
            _noteOffButton = rootElement.Q<Button>("NoteOffButton");
            _noteOffChannel = rootElement.Q<IntegerField>("NoteOffChannel");
            _noteOffPitch = rootElement.Q<IntegerField>("NoteOffPitch");
            _noteOffVelocity = rootElement.Q<IntegerField>("NoteOffVelocity");
            _noteOffButton.clickable.clicked += OnNoteOffClicked;
            
            _controlButton = rootElement.Q<Button>("ControlButton");
            _controlChannel = rootElement.Q<IntegerField>("ControlChannel");
            _controlNumber = rootElement.Q<IntegerField>("ControlNumber");
            _controlValue = rootElement.Q<IntegerField>("ControlValue");
            _controlButton.clickable.clicked += OnControlClicked;
            
            _polyAftertouchButton = rootElement.Q<Button>("PolyAftertouchButton");
            _polyAftertouchChannel = rootElement.Q<IntegerField>("PolyAftertouchChannel");
            _polyAftertouchPitch = rootElement.Q<IntegerField>("PolyAftertouchPitch");
            _polyAftertouchValue = rootElement.Q<IntegerField>("PolyAftertouchValue");
            _polyAftertouchButton.clickable.clicked += OnPolyAftertouchClicked;
            
            _pitchBendButton = rootElement.Q<Button>("PitchBendButton");
            _pitchBendChannel = rootElement.Q<IntegerField>("PitchBendChannel");
            _pitchBendSemitones = rootElement.Q<FloatField>("PitchBendSemitones");
            _pitchBendButton.clickable.clicked += OnPitchBendClicked;
            
            _channelAftertouchButton = rootElement.Q<Button>("ChannelAftertouchButton");
            _channelAftertouchChannel = rootElement.Q<IntegerField>("ChannelAftertouchChannel");
            _channelAftertouchValue = rootElement.Q<IntegerField>("ChannelAftertouchValue");
            _channelAftertouchButton.clickable.clicked += OnChannelAftertouchClicked;
        }

        protected virtual void OnDisable()
        {
            _noteOffButton.clickable.clicked -= OnNoteOnClicked;
            _noteOffButton.clickable.clicked -= OnNoteOffClicked;
            _controlButton.clickable.clicked -= OnControlClicked;
            _polyAftertouchButton.clickable.clicked -= OnPolyAftertouchClicked;
            _pitchBendButton.clickable.clicked -= OnPitchBendClicked;
            _channelAftertouchButton.clickable.clicked -= OnChannelAftertouchClicked;
        }
        
        private void OnNoteOnClicked()
        {
            _broadcaster.Broadcast(new MIDINoteOn(_noteOnChannel.value, _noteOnPitch.value, _noteOnVelocity.value));
        }
        
        private void OnNoteOffClicked()
        {
            _broadcaster.Broadcast(new MIDINoteOff(_noteOffChannel.value, _noteOffPitch.value, _noteOffVelocity.value));
        }
        
        private void OnControlClicked()
        {
            _broadcaster.Broadcast(new MIDIControl(_controlChannel.value, _controlNumber.value, _controlValue.value));
        }
        
        private void OnPolyAftertouchClicked()
        {
            _broadcaster.Broadcast(new MIDIPolyphonicAftertouch(
                _polyAftertouchChannel.value, _polyAftertouchPitch.value, _polyAftertouchValue.value));
        }
        
        private void OnPitchBendClicked()
        {
            _broadcaster.Broadcast(new MIDIPitchBend(_pitchBendChannel.value, _pitchBendSemitones.value, PITCH_BEND_SEMITONE_RANGE));
        }
        
        private void OnChannelAftertouchClicked()
        {
            _broadcaster.Broadcast(new MIDIChannelAftertouch(_channelAftertouchChannel.value, _channelAftertouchValue.value));
        }
    }    
}


