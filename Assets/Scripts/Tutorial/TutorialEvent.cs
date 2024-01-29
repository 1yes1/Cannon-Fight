using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public abstract class TutorialEvent:IDisposable
    {
        protected TutorialEventController _controller;

        public abstract void SetParameters(params object[] objects);

        public void OnEventStartedBase(TutorialEventController tutorialEventController)
        {
            _controller = tutorialEventController;
            OnEventStarted();
        }

        protected abstract void OnEventStarted();

        public void OnEventUpdateBase()
        {

            OnEventUpdate();
        }

        protected abstract void OnEventUpdate();

        public void CompleteTutorial(TutorialEvent tutorialEvent)
        {
            _controller.CompleteTutorialEvent(tutorialEvent);
            Dispose();
        }

        public virtual void Dispose()
        {
        }
    }
}
