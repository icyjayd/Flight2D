using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CompassGizmo))]
public class NPCMovement : MonoBehaviour
{
    Vector2[] dirs;
    float[] targetWeights_m;
    float[] targetWeightWeights_m;
    float[] avoidWeight_m;
    float[] avoidWeightWeights_m;
    public float targetWeightScaleFactor = 1;
    public float avoidWeightScaleFactor = 1;
    public float radius = 1;
    Rigidbody2D rb;
    int curDirIdx;

    private Vector2 curDir;
    public Vector2 CurDir { get =>curDir; set => curDir = value;}

    float targetSwitchGain;
    [SerializeField]
    int dirNum = 16;
    CompassGizmo compassGizmo;
    // Start is called before the first frame update
    void Awake()
    {
        targetWeights_m = new float[dirNum];
        targetWeightWeights_m =new float[dirNum];
        avoidWeight_m = new float[dirNum];
        avoidWeightWeights_m = new float[dirNum];
        compassGizmo = GetComponent<CompassGizmo>();

        InitDirs();
        compassGizmo.InitGizmo(dirs);

        rb = GetComponent<Rigidbody2D>();
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            compassGizmo.Draw(targetWeights_m);
        }
    }
#endif
    private void InitDirs()
    {        //initialize sixteen directions
        //1 x with each magnitude and each sign
        //1 y with each magnitude
        dirs = new Vector2[dirNum];
        for(int i = 0; i<dirs.Length; i++)
        {
            float frac = (i * 1f) / dirNum;
            //print(frac);
            float x = radius * Mathf.Cos(frac * 2 * Mathf.PI);
            float y = radius * Mathf.Sin(frac * 2 * Mathf.PI);
            dirs[i] = new Vector2(x, y);
            //print(dirs[i]);
        }


        //float[] mags = new float[4] { -1, -0.5f, 0.5f, 1 };
        //int i = 0;
        //foreach (float mag_x in mags)
        //{

        //    foreach (float mag_y in mags)
        //    {
        //        Vector2 vec = new Vector2(mag_x, mag_y);
                
        //        dirs[i] = vec;

        //        i++;

        //    }
        //}

    }
    private static float[] NormalizeData(IEnumerable<float> data, int min, int max)
    {
        float dataMax = data.Max();
        float dataMin = data.Min();
        float range = dataMax - dataMin;

        return data
            .Select(d => (d - dataMin) / range)
            .Select(n => (float)((1 - n) * min + n * max))
            .ToArray();
    }
    public void UpdateTargetWeights(Vector2 target)
    {
        //get the d
        Vector2 pos = transform.position;
        Vector2 targetDir = (target - pos).normalized;
      
        ///
        for (int i = 0; i < dirs.Length; i++)
        {   //TODO: implement distance-based weights of weights
            float distBuffer = Mathf.Max(0, (radius - Vector2.Distance(new Vector2(transform.position.x, transform.position.y) + dirs[i], target)));
            print(distBuffer);
            float dot = Vector2.Dot(targetDir, dirs[i]);
            float horizontal = 1 - Mathf.Abs(dot);
            targetWeights_m[i] = dot* targetWeightScaleFactor * horizontal -distBuffer;//* targetWeightWeights_m[i];// * targetWeightScaleFactor;
            //print((i, Vector2.Dot(targetDir, dirs[i])));
        }
        targetWeights_m = NormalizeData(targetWeights_m, 0, 1);
        //int j = 0;
        float maxVal = targetWeights_m.Max();
        //if the gain in heading is greater than threshold, switch to newest


        int maxIdx = Array.IndexOf(targetWeights_m, maxVal);
        curDir = 


        //foreach(float weight in targetWeights_m)
        //{
        //    print((j, weight));
        //    j++;
        //}
    }
    public Vector2 GetDir()
    {
    }
}
