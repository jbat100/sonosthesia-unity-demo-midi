using System;
using System.Collections.Generic;
using System.Linq;
using Sonosthesia.Channel;
using Sonosthesia.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sonosthesia.UI
{
    public abstract class ChannelMonitorUI<T> : MonoBehaviour where T : struct 
    {
        [SerializeField] private Channel<T> _channel;
        
        [SerializeField] private UIDocument _document;

        [SerializeField] VisualTreeAsset _listEntryTemplate;

        private int _count;
        private bool _dirty;
        private int _messageCount;
        private SimpleListController<ChannelStreamUIData, ChannelStreamListEntryController> _listController;

        private IDisposable _subscription;

        private List<ChannelStreamUIData> _data = new();

        protected abstract string GetDescription(T data);
        
        protected virtual void OnValidate()
        {
            if (isActiveAndEnabled && Application.isPlaying)
            {
                ReloadSubscription();   
            }
            _dirty = true;
        }
        
        protected virtual void OnEnable()
        {
            VisualElement rootElement = _document.rootVisualElement;
            
            _listController = new SimpleListController<ChannelStreamUIData, ChannelStreamListEntryController>();
            ListView listView = rootElement.Q<ListView>("MessageList");
            _listController.InitializeList(listView, _listEntryTemplate);
            _listController.ImportData(Enumerable.Empty<ChannelStreamUIData>());
            
            _data = new List<ChannelStreamUIData>();
            ReloadSubscription();
            _dirty = true;
        }

        protected virtual void OnDisable()
        {
            _subscription?.Dispose();
            _subscription = null;
            _data.Clear();
        }

        protected virtual void Update()
        {
            if (_dirty)
            {
                _listController.ImportData(_data ?? Enumerable.Empty<ChannelStreamUIData>());
                _dirty = false;
            }
        }

        private void ReloadSubscription()
        {
            TimeSpan referenceTime = TimeSpan.FromSeconds(Time.time);
            _count = 0;
            _subscription?.Dispose();

            _subscription = _channel.StreamObservable.Subscribe(stream =>
            {
                int localCount = _count++;
                TimeSpan start = TimeSpan.FromSeconds(Time.time) - referenceTime;

                void Clean()
                {
                    int index = _data.FindIndex(data => data.Count == localCount);
                    if (index >= 0)
                    {
                        _data.RemoveAt(index);    
                    }
                    _dirty = true;
                }

                void UpdateValue(T value)
                {
                    ChannelStreamUIData updated = new ChannelStreamUIData(localCount, GetDescription(value), start);
                    int index = _data.FindIndex(data => data.Count == localCount);
                    if (index >= 0)
                    {
                        _data[index] = updated;
                    }
                    else
                    {
                        _data.Add(updated);
                    }
                    _dirty = true;
                }
                
                stream.TakeUntil(this.OnDisableAsObservable())
                    .Subscribe(UpdateValue, exception => Clean(), Clean);
            }); 

        }
    }
}