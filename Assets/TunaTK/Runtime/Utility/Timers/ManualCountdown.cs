// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: Tunacorn Studios
// Creation Time: 24/11/2019 12:21 PM
// Created On: LAPTOP-DENHO8T8

using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace TunaTK.Utilities
{
	public class ManualCountdown 
	{
        public delegate void Callback();
        
        private Coroutine countdownRoutine;
        private Callback callback;

        private TimeInput maxTime;
        private bool loop;

        private float timer = 0;
        private bool isRunning = false;

        public ManualCountdown(TimeInput _time, Callback _callback, bool _loop = false)
        {
            maxTime = _time;
            callback = _callback;
            loop = _loop;
        }

        public void Start()
        {
            isRunning = true;
            timer = 0;
        }

        public void Stop()
        {
            isRunning = false;
            timer = 0;
        }

        public void Update()
        {
            if (!isRunning)
                return;

            if (timer < maxTime.Value)
            {
                timer += Time.deltaTime;
            }
            else
            {
                if (callback != null)
                {
                    callback();
                }

                if (loop)
                {
                    timer = 0;
                }
                else
                {
                    Stop();
                }
            }
        }
    }
}