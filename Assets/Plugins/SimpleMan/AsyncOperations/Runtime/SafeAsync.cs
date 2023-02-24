using SimpleMan.Utilities;
using System;
using System.Threading.Tasks;
using UnityEngine;


namespace SimpleMan.AsyncOperations
{
    public static class SafeAsync
    {
        /// <summary>
        /// Waits for the specified number of frames
        /// </summary>
        /// <param name="frames">Frames to skip</param>
        /// <param name="cancelCondition">Use this parameter if you need to stop waiting</param>
        /// <returns></returns>
        public static async Task<EAsyncOperationResult> SkipFrames(byte frames, Func<bool> cancelCondition)
        {
            while (frames > 0)
            {
                if (ShouldBeCanceledBySystem())
                    return EAsyncOperationResult.CanceledBySystem;

                if (cancelCondition.Exist() && cancelCondition())
                    return EAsyncOperationResult.Canceled;

                frames--;
                await Task.Yield();
            }

            return EAsyncOperationResult.Completed;
        }

        /// <summary>
        /// Waits while time is up. Safe for using in playmode (returns 'Canceled by sytem' result on 
        /// switching to editor mode)
        /// </summary>
        /// <param name="seconds">Delay in seconds</param>
        /// <param name="cancelCondition">Use this parameter if you need to stop waiting</param>
        /// <returns></returns>
        public static async Task<EAsyncOperationResult> DelayRealtime(float seconds, Func<bool> cancelCondition)
        {
            Assert.TimeNonNegative(seconds);

            float timeLeft = seconds;
            while(timeLeft > 0)
            {
                if (ShouldBeCanceledBySystem())
                    return EAsyncOperationResult.CanceledBySystem;

                if (cancelCondition.Exist() && cancelCondition())
                    return EAsyncOperationResult.Canceled;

                timeLeft -= Time.unscaledDeltaTime;
                await Task.Yield();
            }

            return EAsyncOperationResult.Completed;
        }

        /// <summary>
        /// Waits while the specified `condition` is true and then calls `onComplete` delegate.
        /// Optionally skips `skipFrames` between each check of the `condition`.
        /// </summary>
        /// <param name="condition">Condition that should return false in order to stop the waiting process</param>
        /// <param name="cancelCondition">Use this parameter if you need to stop waiting</param>
        /// <param name="skipFrames">Optional parameter that indicates how many frames to skip between each check of the `condition`</param>
        /// <returns></returns>
        public static async Task<EAsyncOperationResult> WaitWhile(Func<bool> condition, Func<bool> cancelCondition, byte skipFrames = 0)
        {
            Assert.ConditionExist(condition);

            while (condition())
            {
                EAsyncOperationResult waitFramesResult = await SkipFrames(skipFrames, cancelCondition);
                if (waitFramesResult == EAsyncOperationResult.CanceledBySystem)
                    return EAsyncOperationResult.CanceledBySystem;

                if (ShouldBeCanceledBySystem())
                    return EAsyncOperationResult.CanceledBySystem;

                if (cancelCondition.Exist() && cancelCondition())
                    return EAsyncOperationResult.Canceled;
            }

            return EAsyncOperationResult.Completed;
        }

        /// <summary>
        /// Repeats an action while a condition is met. The time between each execution of the action can be adjusted using a custom time scale. 
        /// The action will stop when the condition is no longer met and an optional completion action is invoked.
        /// </summary>
        /// <param name="condition">The condition to be met while repeating the action</param>
        /// <param name="cancelCondition">Use this parameter if you need to stop waiting</param>
        /// <param name="onTick">The action to be executed repeatedly while the condition is met</param>
        /// <param name="skipFrames">The number of frames to skip before executing the action again</param>
        /// <returns></returns>
        public static async Task<EAsyncOperationResult> RepeatWhile(Func<bool> condition, Func<bool> cancelCondition, Action onTick, byte skipFrames = 0)
        {
            Assert.ConditionExist(condition);

            while (condition())
            {
                onTick?.Invoke();

                EAsyncOperationResult waitFramesResult = await SkipFrames(skipFrames, cancelCondition);
                if (waitFramesResult == EAsyncOperationResult.CanceledBySystem)
                    return EAsyncOperationResult.CanceledBySystem;

                if (ShouldBeCanceledBySystem())
                    return EAsyncOperationResult.CanceledBySystem;

                if (cancelCondition.Exist() && cancelCondition())
                    return EAsyncOperationResult.Canceled;
            }

            return EAsyncOperationResult.Completed;
        }

        private static bool ShouldBeCanceledBySystem()
        {
            return !Application.isPlaying;
        }
    }
}
