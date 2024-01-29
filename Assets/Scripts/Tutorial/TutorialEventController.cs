using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class TutorialEventController : IInitializable, IDisposable,ITickable
    {
        public event Action OnTutorialCompletedEvent;

        private TutorialEvent[] _tutorialEvents;

        private TutorialEvent _currentEvent;


        private TutorialEventController(TutorialEvent[] tutorialEvents)
        {
            _tutorialEvents = tutorialEvents;
        }

        public void Initialize()
        {
        }

        public void StartNewTutorialEvent(TutorialEventType tutorialEventType,params object[] objects)
        {
            TutorialEvent tutorialEvent = GetTutorial(tutorialEventType);
            if (tutorialEvent == null)
            {
                Debug.LogError("No Tutorial Found!");
                return;
            }
            tutorialEvent.SetParameters(objects);
            tutorialEvent.OnEventStartedBase(this);
            _currentEvent = tutorialEvent;
        }

        public void Tick()
        {
            if (_currentEvent == null)
                return;

            _currentEvent.OnEventUpdateBase();
        }

        public TutorialEvent GetTutorial(TutorialEventType tutorialEventType)
        {
            switch (tutorialEventType)
            {
                case TutorialEventType.Move:
                    return GetTutorialEvent<TMoveEvent>();
                case TutorialEventType.Aim:
                    return GetTutorialEvent<TAimEvent>();
                case TutorialEventType.KillEnemy:
                    return GetTutorialEvent<TKillEvent>();
                case TutorialEventType.OpenChestDamage:
                    return GetTutorialEvent<TOpenChestDamageEvent>();
                case TutorialEventType.DamagePotionKill:
                    return GetTutorialEvent<TDamagePotionKillEvent>();
                case TutorialEventType.OpenChestMultiball:
                    return GetTutorialEvent<TOpenChestMultiballEvent>();
                case TutorialEventType.MultiballPotionKill:
                    return GetTutorialEvent<TMultiballPotionKillEvent>();
                case TutorialEventType.OpenChestHealth:
                    return GetTutorialEvent<TOpenChestHealthEvent>();
                default:
                    break;
            }
            return null;
        }

        private T GetTutorialEvent<T>() where T : TutorialEvent
        {
            for (int i = 0; i < _tutorialEvents.Length; i++)
            {
                if (_tutorialEvents[i] is T)
                {
                    return (T)_tutorialEvents[i];
                }
            }
            return null;
        }

        public void CompleteTutorialEvent(TutorialEvent tutorialEvent)
        {
            _currentEvent = null;
            OnTutorialCompletedEvent?.Invoke();
        }

        public void Dispose()
        {
            for (int i = 0; i < _tutorialEvents.Length; i++)
            {
                _tutorialEvents[i].Dispose();
            }
        }

    }
}
