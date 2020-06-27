using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerUtils
{
    public class Utils
    {
        public static IEnumerator Timeout(System.Action action, float time)
        {
            yield return new WaitForSecondsRealtime(time);
            action();
        }
    }
}

