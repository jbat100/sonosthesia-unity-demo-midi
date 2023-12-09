using System;
using UnityEngine;
using UniRx;
using UnityEngine.UIElements;
using Sonosthesia.Metronome;
using Sonosthesia.Signal;

namespace Sonosthesia.UI
{
    public class TransportMonitorUI : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;

        [SerializeField] private Signal<Transport> _transportSignal;

        [SerializeField] private bool _zeroBased;

        private Label _barLabel;
        private Label _beatLabel;
        private Label _sixteenthLabel;

        private IDisposable _subscription;
        
        protected virtual void OnEnable()
        {
            VisualElement rootElement = _document.rootVisualElement;
            
            _barLabel = rootElement.Q<Label>("BarLabel");
            _beatLabel = rootElement.Q<Label>("BeatLabel");
            _sixteenthLabel = rootElement.Q<Label>("SixteenthLabel");

            _subscription?.Dispose();
            
            ClearUI();
            
            _subscription = _transportSignal.SignalObservable.Subscribe(transport =>
            {
                int offset = _zeroBased ? 0 : 1;
                _barLabel.text = $"{transport.Bars + offset}";
                _beatLabel.text = $"{transport.Beats + offset}";
                _sixteenthLabel.text = $"{transport.Sixteenths + offset}";
            });
        }

        protected virtual void OnDisable()
        {
            _subscription?.Dispose();
            _subscription = null;
            
            ClearUI();
        }

        private void ClearUI()
        {
            _barLabel.text = "-";
            _beatLabel.text = "-";
            _sixteenthLabel.text = "-";
        }
    }
}
