// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 30/10/2019 09:01 AM
// Created On: CHRONOS

using UnityEngine;

using System.Collections;

namespace TunaTK.Utilities
{
	public class Countdown 
	{
        public delegate void Callback();

        private MonoBehaviour runner;
        private Coroutine countdownRoutine;
        private Callback callback;

        private TimeInput maxTime;
        private bool loop;

		public Countdown(TimeInput _time, MonoBehaviour _runner, Callback _callback, bool _loop = false)
		{
            maxTime = _time;
            runner = _runner;
            callback = _callback;
            loop = _loop;
		}

        public void Start()
        {
            if (runner != null)
            {
                countdownRoutine = runner.StartCoroutine(Countdown_CR());
            }
        }

        public void Stop()
        {
            if (countdownRoutine != null)
            {
                runner.StopCoroutine(countdownRoutine);
                countdownRoutine = null;
            }
        }

        private IEnumerator Countdown_CR()
        {
            float timer = 0;

            while(true)
            {
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
                        break;
                    }
                }

                yield return null;
            }
        }
	}
}