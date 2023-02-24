using SimpleMan.Utilities;
using System;
using UnityEngine;
using static SimpleMan.AsyncOperations.Constants;

namespace SimpleMan.AsyncOperations
{
    internal static class Assert
    {
        public static void TimeNonNegative(float value)
        {
            if (value < 0)
            {
                throw new Exception(
                    PLUGIN_DISPLAYABLE_NAME + " Assertion failed: Delay or tick dilation time is less than zero. Make sure that " +
                    "the argument is positive before calling async operation");
            }
        }

        public static void OnCompleteExist(Action value)
        {
            if (value.NotExist())
            {
                throw new Exception(
                    PLUGIN_DISPLAYABLE_NAME + " Assertion failed: 'OnComplete' callback does not exist. Make sure that " +
                    "the argument is not null before calling async operation");
            }
        }

        public static void ConditionExist(Func<bool> value)
        {
            if (value.NotExist())
            {
                throw new Exception(
                    PLUGIN_DISPLAYABLE_NAME + " Assertion failed: condition does not exist. Make sure that " +
                    "the argument is not null before calling async operation");
            }
        }

        public static void TimeScaleGetterExist(Func<float> value)
        {
            if (value.NotExist())
            {
                throw new Exception(
                    PLUGIN_DISPLAYABLE_NAME + " Assertion failed: time scale getter function does not exist. Make sure that " +
                    "the argument is not null before calling async operation");
            }
        }

        public static void OnwerExist(MonoBehaviour owner)
        {
            if (owner.NotExist())
            {
                throw new ArgumentNullException(nameof(owner),
                    "A coroutine can be started from the existing monobehaviour only. Check your class " +
                    "on 'null' before calling this method");
            }
        }
    }
}
