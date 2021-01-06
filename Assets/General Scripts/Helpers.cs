using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public static class Helpers: object
    {
        public static float Sign(float num)
        {
            ///This function expands Mathf.Sign to catch 0s
            ///

            if (num == 0)
            {
                return 0;
            }
            else
            {
                return System.Math.Sign(num);
            }

        }
        public static bool CheckBoundary(float num, float boundary = 10)
        {
            num = System.Math.Abs(num);
            if (num > boundary)
            {
                return false;

            }
            else
            {
                return true;
            }
            ///This function returns 0 if within a given boundary
            ///

        }

        public static IEnumerator SpriteFlicker(SpriteRenderer sp, Color normalColor, Color flickerColor, float duration, float flickerSpeed = 1f)
        {
            float elapsedTime = 0;
            float flickerTime = 0;
            int i = 1;
            Color[] colors = { normalColor, flickerColor };
            sp.color = flickerColor;
            while (elapsedTime < duration) {
                //Debug.Log("flicker time: " + flickerTime + ", i: " + i);

                elapsedTime += Time.deltaTime;
                flickerTime += Time.deltaTime;
                if (flickerTime >= flickerSpeed)
                {
                    flickerTime = 0;
                    i += 1;
                    sp.color = colors[i%colors.Length];


                }
                yield return null;
            }
            sp.color = normalColor;
        }
    }




