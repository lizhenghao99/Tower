using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectTower
{
    public static class Utils
    {
        // Execute after delay
        public static IEnumerator Timeout(Action action, float time)
        {
            yield return new WaitForSeconds(time);
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

        // Calculate Projectile
        public static Vector3 getProjectileVelocity(Vector3 source, Vector3 target, float angle)
        {
            Vector3 projectileXZPos = Vector3.ProjectOnPlane(
                source, new Vector3(0, 1, 0));
            Vector3 targetXZPos = Vector3.ProjectOnPlane(
                target, new Vector3(0, 1, 0));


            // shorthands for the formula
            float R = Vector3.Distance(projectileXZPos, targetXZPos);
            float G = Physics.gravity.y;
            float tanAlpha = Mathf.Tan(angle * Mathf.Deg2Rad);
            float H = target.y - source.y;

            // calculate the local space components of the velocity 
            // required to land the projectile on the target object 
            float Vx = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
            float Vy = tanAlpha * Vx;

            Vector3 direction = (targetXZPos - projectileXZPos).normalized;
            return direction * Vx + new Vector3(0f, Vy, 0f);
        }

        public static IEnumerator LoadAsync(int sceneIndex, GameObject loadingScreen)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
            loadingScreen.SetActive(true);
            Slider loadingSlider = loadingScreen.GetComponentInChildren<Slider>();

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                loadingSlider.value = progress;

                yield return null;
            }
        }
    }
}

