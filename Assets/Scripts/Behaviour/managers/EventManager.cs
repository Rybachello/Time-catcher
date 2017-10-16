using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Behaviour
{
    public class EventManager : MonoBehaviour
    {
        private Dictionary<EventManagerType, UnityEvent> _eventDictionary;

        private static EventManager _eventManager;

        public void Init ( ) {
            if (_eventDictionary == null) {
                _eventDictionary = new Dictionary<EventManagerType, UnityEvent>();
            }
        }

        public void OnDestroy ( ) {
            _eventManager = null;
        }

        public static void StartListening (EventManagerType eventName, UnityAction listener) {
            UnityEvent thisEvent;
            if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent)) {
                thisEvent.AddListener(listener);
            } else {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Instance._eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening (EventManagerType eventType, UnityAction listener) {
            // No need create again for remove
            if (_eventManager == null)
                return;

            UnityEvent thisEvent;
            if (_eventManager._eventDictionary.TryGetValue(eventType, out thisEvent)) {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void TriggerEvent (EventManagerType eventType) {
            UnityEvent thisEvent;
            if (Instance._eventDictionary.TryGetValue(eventType, out thisEvent)) {
                thisEvent.Invoke();
            }
        }

        public static EventManager Instance {
            get {
                if (!_eventManager) {
                    _eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;
                    if (_eventManager == null) {
                        var eventManagerGameObject = new GameObject {name = "[EventManager]"};
                        _eventManager = eventManagerGameObject.AddComponent<EventManager>();
                    }
                    _eventManager.Init();
                }

                return _eventManager;
            }
        }
    }


    public enum EventManagerType
    {
        OnGameStart,
        OnGameEnd,
        CatchTime
    }
}