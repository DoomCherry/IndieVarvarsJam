using System;
using System.Collections;
using UnityEngine;
using static SimpleMan.AsyncOperations.Assert;

namespace SimpleMan.AsyncOperations
{
    public static class Coroutines
    {
        /// <summary>
        /// Skips the specified number of frames before executing an action.
        /// </summary>
        /// <param name="frames">The number of frames to skip.</param>
        /// <param name="onComplete">The action to be executed after skipping the frames.</param>
        /// <returns>An enumerator that can be used in a coroutine.</returns>
        public static IEnumerator SkipFramesProcess(byte frames, Action onComplete)
        {
            OnCompleteExist(onComplete);

            while (frames > 0)
            {
                frames--;
                yield return null;
            }

            onComplete?.Invoke();
        }

        /// <summary>
        /// Coroutine that waits for a specified amount of time, taking into account the given time scale.
        /// </summary>
        /// <param name="time">Amount of time to wait.</param>
        /// <param name="timeScaleGetter">Use for pause or your custom time scale in project.</param>
        /// <param name="onComplete">Action to perform after the specified time has elapsed.</param>
        /// <returns>Yield instruction to wait for the specified amount of time, taking into account the given time scale.</returns>
        public static IEnumerator DelayProcess(float time, Func<float> timeScaleGetter, Action onComplete)
        {
            TimeNonNegative(time);
            OnCompleteExist(onComplete);
            TimeScaleGetterExist(timeScaleGetter);

            float timeLeft = time;

            while (timeLeft >= 0)
            {
                timeLeft -= Time.deltaTime * timeScaleGetter();
                yield return null;
            }

            onComplete?.Invoke();
        }

        /// <summary>
        /// Waits while the specified `condition` is true and then calls `onComplete` delegate.
        /// Optionally skips `skipFrames` between each check of the `condition`.
        /// </summary>
        /// <param name="condition">Condition that should return false in order to stop the waiting process.</param>
        /// <param name="onComplete">Delegate that is called when the `condition` is no longer true.</param>
        /// <param name="skipFrames">Optional parameter that indicates how many frames to skip between each check of the `condition`.</param>
        /// <returns>IEnumerator that can be used in a coroutine.</returns>
        public static IEnumerator WaitFramesWhileProcess(Func<bool> condition, Action onComplete, byte skipFrames = 0)
        {
            OnCompleteExist(onComplete);

            byte framesLeft;
            while (condition())
            {
                framesLeft = skipFrames;
                while (framesLeft > 0)
                {
                    framesLeft--;
                    yield return null;
                }
            }

            onComplete?.Invoke();
        }

        /// <summary>
        /// Waits while the given condition is true and performs the specified action when it's false.
        /// </summary>
        /// <param name="condition">A function that returns a boolean indicating the waiting condition.</param>
        /// <param name="onComplete">An action to be performed when the waiting is completed.</param>
        /// <param name="timeScaleGetter">Use for pause or your custom time scale in project.</param>
        /// <param name="tickDilation">The time to wait between each check of the waiting condition. The default value is 0.</param>
        /// <returns>An enumerator for the coroutine.</returns>
        public static IEnumerator WaitWhileProcess(Func<bool> condition, Action onComplete, Func<float> timeScaleGetter, float tickDilation = 0)
        {
            TimeNonNegative(tickDilation);
            OnCompleteExist(onComplete);
            TimeScaleGetterExist(timeScaleGetter);


            float tickTimeLeft;
            while (condition())
            {
                tickTimeLeft = tickDilation;
                while (tickTimeLeft > 0)
                {
                    tickTimeLeft -= timeScaleGetter() * Time.deltaTime;
                    yield return null;
                }
            }

            onComplete?.Invoke();
        }

        /// <summary>
        /// Repeats an action while a condition is met. The action is executed once per frame with the option to skip frames in between. 
        /// The action will stop when the condition is no longer met and an optional completion action is invoked.
        /// </summary>
        /// <param name="condition">The condition to be met while repeating the action</param>
        /// <param name="onTick">The action to be executed repeatedly while the condition is met</param>
        /// <param name="onComplete">The action to be executed when the condition is no longer met</param>
        /// <param name="skipFrames">The number of frames to skip before executing the action again</param>
        /// <returns>An IEnumerator for use with the coroutine API</returns>
        public static IEnumerator RepeatFramesWhileProcess(Func<bool> condition, Action onTick, Action onComplete, byte skipFrames = 0)
        {
            ConditionExist(condition);

            byte framesLeft;
            while (condition())
            {
                onTick?.Invoke();
                framesLeft = skipFrames;

                while (framesLeft > 0)
                {
                    framesLeft--;
                    yield return null;
                }
            }

            onComplete?.Invoke();
        }

        /// <summary>
        /// Repeats an action while a condition is met. The time between each execution of the action can be adjusted using a custom time scale. 
        /// The action will stop when the condition is no longer met and an optional completion action is invoked.
        /// </summary>
        /// <param name="condition">The condition to be met while repeating the action</param>
        /// <param name="timeScaleGetter">Use for pause or your custom time scale in project</param>
        /// <param name="onTick">The action to be executed repeatedly while the condition is met</param>
        /// <param name="onComplete">The action to be executed when the condition is no longer met</param>
        /// <param name="tickDilation">The minimum time between each action execution</param>
        /// <returns>An IEnumerator for use with the coroutine API</returns>
        public static IEnumerator RepeatWhileProcess(Func<bool> condition, Func<float> timeScaleGetter, Action onTick, Action onComplete, float tickDilation = 0)
        {
            TimeNonNegative(tickDilation);
            ConditionExist(condition);
            TimeScaleGetterExist(timeScaleGetter);

            float tickTimeLeft;
            while (condition())
            {
                onTick?.Invoke();
                tickTimeLeft = tickDilation;

                while (tickTimeLeft > 0)
                {
                    tickTimeLeft -= timeScaleGetter() * Time.deltaTime;
                    yield return null;
                } 
            }

            onComplete?.Invoke();
        }
    }
}
