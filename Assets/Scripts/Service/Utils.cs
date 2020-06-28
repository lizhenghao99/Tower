using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TowerUtils
{
    public static class Utils
    {
        // Execute after delay
        public static IEnumerator Timeout(Action action, float time)
        {
            yield return new WaitForSecondsRealtime(time);
            action();
        }

        // The normal distribution function.
        public static float BellCurve(float x, float mean, float var)
        {
            return (float)(1 / (2 * Mathf.PI) *
                Math.Exp(-(x - mean) * (x - mean) / (2 * var)));
        }

        // Shuffle list
        private static System.Random rng = new System.Random();
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }
}

