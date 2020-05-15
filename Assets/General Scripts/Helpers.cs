using System.Collections;
using System.Collections.Generic;
//using UnityEngine;


    public static class HelperFunctions : object
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

    }



