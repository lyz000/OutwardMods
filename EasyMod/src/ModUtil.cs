using BepInEx.Logging;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace EasyMod
{
    class ModUtil
    {
        public static ManualLogSource Log;

        public static float CalculateValue(float configValue, float currenValue, float maxValue, float updateDeltaTime)
        {
            var updateValue = currenValue + configValue * updateDeltaTime;
            return Mathf.Clamp(updateValue, 0f, maxValue);
        }
    }
   
}
