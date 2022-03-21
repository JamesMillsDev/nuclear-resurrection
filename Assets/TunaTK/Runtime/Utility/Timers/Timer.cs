// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 29/10/2019 09:18 PM
// Created On: CHRONOS

using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Events;

namespace TunaTK.Utilities
{
#pragma warning disable CS0649
    public class Timer : InitialiseBehaviour
	{
        public UnityEvent OnStart
        {
            get => onStart;
            set => onStart = value;
        }

        public UnityEvent OnComplete
        {
            get => onComplete;
            set => onComplete = value;
        }

        public bool IsRunning { get; private set; }

        public bool IsPaused { get; private set; }

        public float TimeRemaining => maxTime - CurrentTime;

        public float CurrentTime { get; private set; }

        [SerializeField]
        private bool loop;
        [SerializeField]
        private TimeInput time;

        [Header("Events")]
        [SerializeField]
        private UnityEvent onStart = new UnityEvent();
        [SerializeField]
        private UnityEvent onComplete = new UnityEvent();

        private float maxTime = 0;

        private bool canFireStartEvent = false;

        protected override Task OnInitialisation(params object[] _params)
        {
            canFireStartEvent = true;
            ResetTimer();

            if (_params.Length > 0)
            {
                if ((bool)_params[0])
                {
                    StartTimer(true);
                }
            }
            
            return Task.CompletedTask;
        }

        protected override void InitialisedUpdate()
        {
            if (IsRunning)
            {
                if (CurrentTime < maxTime)
                {
                    CurrentTime += Time.deltaTime;
                }
                else
                {
                    onComplete.Invoke();

                    StopTimer();

                    if (loop)
                    {
                        StartTimer();
                    }
                }
            }
        }

        public void ResetTimer()
        {
            maxTime = time.Value;
            CurrentTime = 0;
            IsRunning = false;
            IsPaused = false;
        }

        public bool StartTimer(bool _forceStart = false)
        {
            if ((Initialised || _forceStart) && !IsRunning && !IsPaused)
            {
                IsRunning = true;
                IsPaused = false;

                if (canFireStartEvent)
                {
                    onStart.Invoke();
                }

                canFireStartEvent = false;
                return true;
            }

            return false;
        }

        public bool StopTimer(bool _fullStop = false)
        {
            if (Initialised && IsRunning)
            {
                ResetTimer();

                if (_fullStop)
                {
                    canFireStartEvent = true;
                }

                return true;
            }

            return false;
        }

        public void PauseTimer()
        {
            if (IsRunning && !IsPaused)
            {
                IsRunning = false;
                IsPaused = true;
            }
        }

        public void UnpauseTimer()
        {
            if (!IsRunning && IsPaused)
            {
                IsRunning = true;
                IsPaused = false;
            }
        }
    }
}