using SimpleMan.Utilities;
using System;
using UnityEngine;
using static SimpleMan.AsyncOperations.Coroutines;


namespace SimpleMan.AsyncOperations
{
    public static class MonoExtensions
    {
        #region SKIP FRAMES
        /// <summary>
        /// Skips the specified number of frames before executing an action.
        /// </summary>
        /// <param name="owner">The MonoBehaviour - coroutine runner.</param>
        /// <param name="framesToSkip">The number of frames to skip.</param>
        /// <param name="onComplete">The action to be executed after skipping the frames.</param>
        /// <returns>An enumerator that can be used in a coroutine.</returns>
        public static Coroutine SkipFrames(this MonoBehaviour owner, byte framesToSkip, Action onComplete)
        {
            Assert.OnwerExist(owner);
            return owner.StartCoroutine(SkipFramesProcess(framesToSkip, onComplete));
        }
        #endregion

        #region DELAY
        /// <summary>
        /// Coroutine that waits for a specified amount of time, taking into account the normal time scale.
        /// </summary>
        /// <param name="owner">The MonoBehaviour - coroutine runner.</param>
        /// <param name="time">Amount of time to wait.</param>
        /// <param name="onComplete">Action to perform after the specified time has elapsed.</param>
        /// <returns>Yield instruction to wait for the specified amount of time, taking into account the given time scale.</returns>
        public static Coroutine Delay(this MonoBehaviour owner, float time, Action onComplete)
        {
            return DelayCustomTimeScale(owner, time, () => Time.timeScale, onComplete);
        }

        /// <summary>
        /// Coroutine that waits for a specified amount of time, taking into account the unscaled time.
        /// </summary>
        /// <param name="owner">The MonoBehaviour - coroutine runner.</param>
        /// <param name="time">Amount of time to wait.</param>
        /// <param name="onComplete">Action to perform after the specified time has elapsed.</param>
        /// <returns>Yield instruction to wait for the specified amount of time, taking into account the given time scale.</returns>
        public static Coroutine DelayRealtime(this MonoBehaviour owner, float time, Action onComplete)
        {
            return DelayCustomTimeScale(owner, time, () => 1, onComplete);
        }

        /// <summary>
        /// Coroutine that waits for a specified amount of time, taking into account the given time scale.
        /// </summary>
        /// <param name="owner">The MonoBehaviour - coroutine runner.</param>
        /// <param name="time">Amount of time to wait.</param>
        /// <param name="timeScaleGetter">Use for pause or your custom time scale in project.</param>
        /// <param name="onComplete">Action to perform after the specified time has elapsed.</param>
        /// <returns>Yield instruction to wait for the specified amount of time, taking into account the given time scale.</returns>
        public static Coroutine DelayCustomTimeScale(this MonoBehaviour owner, float time, Func<float> timeScaleGetter, Action onComplete)
        {
            Assert.OnwerExist(owner);
            return owner.StartCoroutine(DelayProcess(time, timeScaleGetter, onComplete));
        }
        #endregion

        #region WAIT FRAMES
        /// <summary>
        /// Waits while the specified `condition` is true and then calls `onComplete` delegate.
        /// Optionally skips `skipFrames` between each check of the `condition`.
        /// </summary>
        /// <param name="owner">The MonoBehaviour - coroutine runner.</param>
        /// <param name="condition">Condition that should return false in order to stop the waiting process.</param>
        /// <param name="onComplete">Delegate that is called when the `condition` is no longer true.</param>
        /// <param name="skipFrames">Optional parameter that indicates how many frames to skip between each check of the `condition`.</param>
        /// <returns>IEnumerator that can be used in a coroutine.</returns>
        public static Coroutine WaitFramesWhile(this MonoBehaviour owner, Func<bool> condition, Action onComplete, byte skipFrames = 0)
        {
            Assert.OnwerExist(owner);
            return owner.StartCoroutine(WaitFramesWhileProcess(condition, onComplete, skipFrames));
        }

        /// <summary>
        /// Opposite to 'WaitFramesWhile'
        /// </summary>
        public static Coroutine WaitFramesUntil(this MonoBehaviour owner, Func<bool> condition, Action onComplete, byte skipFrames = 0)
        {
            Assert.ConditionExist(condition);
            return WaitFramesWhile(owner, () => !condition(), onComplete, skipFrames);
        }
        #endregion

        #region WAIT 
        /// <summary>
        /// Waits while the given condition is true and performs the specified action when it's false.
        /// </summary>
        /// <param name="owner">The MonoBehaviour - coroutine runner.</param>
        /// <param name="condition">A function that returns a boolean indicating the waiting condition.</param>
        /// <param name="onComplete">An action to be performed when the waiting is completed.</param>
        /// <param name="timeScaleGetter">Use for pause or your custom time scale in project.</param>
        /// <param name="tickDilation">The time to wait between each check of the waiting condition. The default value is 0.</param>
        /// <returns>An enumerator for the coroutine.</returns>
        public static Coroutine WaitWhileCustomTimeScale(this MonoBehaviour owner, Func<bool> condition, Action onComplete, Func<float> timeScaleGetter, float tickDilation = 0)
        {
            Assert.OnwerExist(owner);
            return owner.StartCoroutine(WaitWhileProcess(condition, onComplete, timeScaleGetter, tickDilation));
        }

        /// <summary>
        /// Waits while the given condition is true and performs the specified action when it's false.
        /// </summary>
        /// <param name="owner">The MonoBehaviour - coroutine runner.</param>
        /// <param name="condition">A function that returns a boolean indicating the waiting condition.</param>
        /// <param name="onComplete">An action to be performed when the waiting is completed.</param>
        /// <param name="tickDilation">The time to wait between each check of the waiting condition. The default value is 0.</param>
        /// <returns>An enumerator for the coroutine.</returns>
        public static Coroutine WaitWhile(this MonoBehaviour owner, Func<bool> condition, Action onComplete, float tickDilation = 0)
        {
            return WaitWhileCustomTimeScale(owner, condition, onComplete, () => Time.timeScale, tickDilation);
        }

        /// <summary>
        /// Waits while the given condition is true and performs the specified action when it's false.
        /// </summary>
        /// <param name="owner">The MonoBehaviour - coroutine runner.</param>
        /// <param name="condition">A function that returns a boolean indicating the waiting condition.</param>
        /// <param name="onComplete">An action to be performed when the waiting is completed.</param>
        /// <param name="tickDilation">The time to wait between each check of the waiting condition. The default value is 0.</param>
        /// <returns>An enumerator for the coroutine.</returns>
        public static Coroutine WaitWhileRealtime(this MonoBehaviour owner, Func<bool> condition, Action onComplete, float tickDilation = 0)
        {
            return WaitWhileCustomTimeScale(owner, condition, onComplete, () => 1, tickDilation);
        }





        /// <summary>
        /// Opposite to 'WaitWhileCustomTimeScale'
        /// </summary>
        public static Coroutine WaitUntilCustomTimeScale(this MonoBehaviour owner, Func<bool> condition, Action onComplete, Func<float> timeScaleGetter, float tickDilation = 0)
        {
            Assert.ConditionExist(condition);
            return WaitWhileCustomTimeScale(owner, () => !condition(), onComplete, timeScaleGetter, tickDilation);
        }

        /// <summary>
        /// Opposite to 'WaitWhile'
        /// </summary>
        public static Coroutine WaitUntil(this MonoBehaviour owner, Func<bool> condition, Action onComplete, float tickDilation = 0)
        {
            Assert.ConditionExist(condition);
            return WaitWhileCustomTimeScale(owner, () => !condition(), onComplete, () => Time.timeScale, tickDilation);
        }

        /// <summary>
        /// Opposite to 'WaitWhileRealtime'
        /// </summary>
        public static Coroutine WaitUntilRealTime(this MonoBehaviour owner, Func<bool> condition, Action onComplete, float tickDilation = 0)
        {
            Assert.ConditionExist(condition);
            return WaitWhileCustomTimeScale(owner, () => !condition(), onComplete, () => 1, tickDilation);
        }
        #endregion

        #region REPEAT FRAMES
        /// <summary>
        /// Repeats an action while a condition is met. The action is executed once per frame with the option to skip frames in between. 
        /// The action will stop when the condition is no longer met and an optional completion action is invoked.
        /// </summary>
        /// <param name="owner">The MonoBehaviour - coroutine runner.</param>
        /// <param name="condition">The condition to be met while repeating the action</param>
        /// <param name="onTick">The action to be executed repeatedly while the condition is met</param>
        /// <param name="onComplete">The action to be executed when the condition is no longer met</param>
        /// <param name="skipFrames">The number of frames to skip before executing the action again</param>
        /// <returns>An IEnumerator for use with the coroutine API</returns>
        public static Coroutine RepeatFramesWhile(this MonoBehaviour owner, Func<bool> condition, Action onTick, Action onComplete, byte skipFrames = 0)
        {
            Assert.OnwerExist(owner);
            return owner.StartCoroutine(RepeatFramesWhileProcess(condition, onTick, onComplete, skipFrames));
        }

        /// <summary>
        /// Opposite to 'RepeatFramesWhile'
        /// </summary>
        public static Coroutine RepeatFramesUntil(this MonoBehaviour owner, Func<bool> condition, Action onTick, Action onComplete, byte skipFrames = 0)
        {
            Assert.ConditionExist(condition);
            return RepeatFramesWhile(owner, () => !condition(), onTick, onComplete, skipFrames);
        }
        #endregion

        #region REPEAT
        /// <summary>
        /// Repeats an action while a condition is met. The time between each execution of the action can be adjusted using a custom time scale. 
        /// The action will stop when the condition is no longer met and an optional completion action is invoked.
        /// </summary>
        /// <param name="owner">The MonoBehaviour - coroutine runner.</param>
        /// <param name="condition">The condition to be met while repeating the action</param>
        /// <param name="timeScaleGetter">Use for pause or your custom time scale in project</param>
        /// <param name="onTick">The action to be executed repeatedly while the condition is met</param>
        /// <param name="onComplete">The action to be executed when the condition is no longer met</param>
        /// <param name="tickDilation">The minimum time between each action execution</param>
        /// <returns>An IEnumerator for use with the coroutine API</returns>
        public static Coroutine RepeatWhileCustomTimeScale(this MonoBehaviour owner, Func<bool> condition, Action onTick, Action onComplete, Func<float> timeScaleGetter, float tickDilation = 0)
        {
            Assert.OnwerExist(owner);
            return owner.StartCoroutine(RepeatWhileProcess(condition, timeScaleGetter, onTick, onComplete, tickDilation));
        }

        /// <summary>
        /// Repeats an action while a condition is met. The time between each execution of the action can be adjusted using a custom time scale. 
        /// The action will stop when the condition is no longer met and an optional completion action is invoked.
        /// </summary>
        /// <param name="owner">The MonoBehaviour - coroutine runner.</param>
        /// <param name="condition">The condition to be met while repeating the action</param>
        /// <param name="onTick">The action to be executed repeatedly while the condition is met</param>
        /// <param name="onComplete">The action to be executed when the condition is no longer met</param>
        /// <param name="tickDilation">The minimum time between each action execution</param>
        /// <returns>An IEnumerator for use with the coroutine API</returns>
        public static Coroutine RepeatWhile(this MonoBehaviour owner, Func<bool> condition, Action onTick, Action onComplete, float tickDilation = 0)
        {
            return RepeatWhileCustomTimeScale(owner, condition, onTick, onComplete, () => Time.timeScale, tickDilation);
        }

        /// <summary>
        /// Repeats an action while a condition is met. The time between each execution of the action can be adjusted using a custom time scale. 
        /// The action will stop when the condition is no longer met and an optional completion action is invoked.
        /// </summary>
        /// <param name="owner">The MonoBehaviour - coroutine runner</param>
        /// <param name="condition">The condition to be met while repeating the action</param>
        /// <param name="onTick">The action to be executed repeatedly while the condition is met</param>
        /// <param name="onComplete">The action to be executed when the condition is no longer met</param>
        /// <param name="tickDilation">The minimum time between each action execution</param>
        /// <returns>An IEnumerator for use with the coroutine API</returns>
        public static Coroutine RepeatWhileRealtime(this MonoBehaviour owner, Func<bool> condition, Action onTick, Action onComplete, float tickDilation = 0)
        {
            return RepeatWhileCustomTimeScale(owner, condition, onTick, onComplete, () => 1, tickDilation);
        }




        /// <summary>
        /// Opposite to 'RepeatWhile'
        /// </summary>
        public static Coroutine RepeatUntilCustomTimeScale(this MonoBehaviour owner, Func<bool> condition, Action onTick, Action onComplete, Func<float> timeScaleGetter, float tickDilation = 0)
        {
            Assert.OnwerExist(owner);
            Assert.ConditionExist(condition);
            
            return owner.StartCoroutine(RepeatWhileProcess(() => !condition(), timeScaleGetter, onTick, onComplete, tickDilation));
        }

        /// <summary>
        /// Opposite to 'RepeatWhile'
        /// </summary>
        public static Coroutine RepeatUntil(this MonoBehaviour owner, Func<bool> condition, Action onTick, Action onComplete, float tickDilation = 0)
        {
            Assert.ConditionExist(condition);
            return RepeatWhileCustomTimeScale(owner, () => !condition(), onTick, onComplete, () => Time.timeScale, tickDilation);
        }

        /// <summary>
        /// Opposite to 'RepeatWhileRealtime'
        /// </summary>
        public static Coroutine RepeatUntilRealtime(this MonoBehaviour owner, Func<bool> condition, Action onTick, Action onComplete, float tickDilation = 0)
        {
            Assert.ConditionExist(condition);
            return RepeatWhileCustomTimeScale(owner, () => !condition(), onTick, onComplete, () => 1, tickDilation);
        }
        #endregion

        #region REPEAT FOREVER
        /// <summary>
        /// Repeats an action while the coroutine is running
        /// </summary>
        /// <param name="owner">The MonoBehaviour - coroutine runner</param>
        /// <param name="repeatAction">The action to be executed repeatedly</param>
        /// <param name="skipFrames">The number of frames to skip before executing the action again</param>
        /// <returns></returns>
        public static Coroutine RepeatFramesForever(this MonoBehaviour owner, Action repeatAction, byte skipFrames = 0)
        {
            Assert.OnwerExist(owner);
            return owner.StartCoroutine(RepeatFramesWhileProcess(() => true, repeatAction, null, skipFrames));
        }

        /// <summary>
        /// Repeats an action while the coroutine is running
        /// </summary>
        /// <param name="owner">The MonoBehaviour - coroutine runner</param>
        /// <param name="repeatAction">The action to be executed repeatedly</param>
        /// <param name="tickDilation">Time between ticks</param>
        /// <returns></returns>
        public static Coroutine RepeatForever(this MonoBehaviour owner, Action repeatAction, float tickDilation = 0)
        {
            return RepeatForeverCustomTimeScale(owner, repeatAction, () => Time.timeScale, tickDilation);
        }

        /// <summary>
        /// Repeats an action while the coroutine is running
        /// </summary>
        /// <param name="owner">The MonoBehaviour - coroutine runner</param>
        /// <param name="repeatAction">The action to be executed repeatedly</param>
        /// <param name="tickDilation">Time between ticks</param>
        /// <returns></returns>
        public static Coroutine RepeatForeverRealtime(this MonoBehaviour owner, Action repeatAction, float tickDilation = 0)
        {
            return RepeatForeverCustomTimeScale(owner, repeatAction, () => 1, tickDilation);
        }

        /// <summary>
        /// Repeats an action while the coroutine is running
        /// </summary>
        /// <param name="owner">The MonoBehaviour - coroutine runner</param>
        /// <param name="repeatAction">The action to be executed repeatedly</param>
        /// <param name="timeScaleGetter">Use for pause or your custom time scale in project</param>
        /// <param name="tickDilation">Time between ticks</param>
        /// <returns></returns>
        public static Coroutine RepeatForeverCustomTimeScale(this MonoBehaviour owner, Action repeatAction, Func<float> timeScaleGetter, float tickDilation = 0)
        {
            Assert.OnwerExist(owner);
            return owner.StartCoroutine(RepeatWhileProcess(() => true, timeScaleGetter, repeatAction, null, tickDilation));
        }
        #endregion
    }
}
