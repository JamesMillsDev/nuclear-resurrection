// Property of TUNACORN STUDIOS PTY LTD 2018
//
// Creator: James Mills
// Creation Time: 30/10/2019 09:31 AM
// Created On: CHRONOS

using UnityEngine;

using System.Collections;

namespace TunaTK.Utilities
{
    public abstract class BaseTweener
    {
        public delegate void Callback(int _nodeIndex);

        protected AnimationCurve curve;
        protected TimeInput maxTime;
        protected TweenMode mode;
        protected Callback callback;

        private MonoBehaviour runner;
        private Coroutine tweenRoutine;

        private bool forward;
        private int index = 0;

        public BaseTweener() { }

        public virtual void Initialise(TimeInput _time, MonoBehaviour _runner, TweenMode _mode, AnimationCurve _curve, Callback _callback = null)
        {
            maxTime = _time;
            runner = _runner;
            mode = _mode;
            curve = _curve;
            callback = _callback;
            index = 0;

            forward = true;
        }

        public abstract void Tween(float _factor);

        public void Start(bool _force = false)
        {
            if (tweenRoutine == null || _force)
            {
                tweenRoutine = runner.StartCoroutine(Tween_CR());
            }
        }

        public void Stop()
        {
            if (tweenRoutine != null)
            {
                runner.StopCoroutine(tweenRoutine);
                tweenRoutine = null;
            }
        }

        protected virtual void OnTweenComplete() { }

        private IEnumerator Tween_CR()
        {
            float timer = 0;

            while (true)
            {
                if (mode != TweenMode.PingPong)
                {
                    if (timer < maxTime.Value)
                    {
                        timer += Time.deltaTime;
                    }
                    else
                    {
                        OnTweenComplete();

                        if (callback != null)
                        {
                            callback(index);
                            index++;
                        }

                        if (mode == TweenMode.Looped)
                        {
                            timer = 0;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    if (forward)
                    {
                        if (timer < maxTime.Value)
                        {
                            timer += Time.deltaTime;
                        }
                        else
                        {
                            OnTweenComplete();

                            if (callback != null)
                            {
                                callback(index);
                                index++;
                            }

                            forward = false;
                        }
                    }
                    else
                    {
                        if (timer > 0)
                        {
                            timer -= Time.deltaTime;
                        }
                        else
                        {
                            OnTweenComplete();

                            if (callback != null)
                            {
                                callback(index);
                                index++;
                            }

                            forward = true;
                        }
                    }
                }

                float factor = Mathf.Clamp01(timer / maxTime.Value);
                Tween(curve.Evaluate(factor));

                yield return null;
            }
        }
    }
}