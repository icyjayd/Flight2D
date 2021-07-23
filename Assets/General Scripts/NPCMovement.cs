using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CompassGizmo))]
public class NPCMovement : MonoBehaviour
{
    Vector2[] dirs;
    public float[] dirWeights_m;
    public float[] targetWeights_m;
    float[] targetWeightWeights_m;
    public float[] avoidWeights_m;
    float[] avoidWeightWeights_m;

    float[] cumulativeWeights_m;
    public float targetWeightScaleFactor = 1;
    public float avoidWeightScaleFactor = 1;
    public float radius = 1;
    LayerMask mask;
    [SerializeField]
    string[] layers;
    Rigidbody2D rb;
    float velocity;
    public float smoothTime;
    public float steeringThreshold;


    private Vector2 curDir;
    public Vector2 CurDir { get =>curDir; set => curDir = value;}
    
    float targetSwitchGain;
    [SerializeField]
    int dirNum = 16;
    CompassGizmo compassGizmo;
    // Start is called before the first frame update
    void Awake()
    {
        dirWeights_m = new float[dirNum];
        targetWeights_m = new float[dirNum]; 
        targetWeightWeights_m =new float[dirNum];
        avoidWeights_m = new float[dirNum];
        avoidWeightWeights_m = new float[dirNum];
        cumulativeWeights_m = new float[dirNum];
        compassGizmo = GetComponent<CompassGizmo>();
       

        InitDirs();
        compassGizmo.InitGizmo(dirs);

        rb = GetComponent<Rigidbody2D>();
    }
    public void Draw(float[] weights, Vector2[] arrows, Color color, bool flip= false)
    {
        for (int i = 0; i < weights.Length; i++)
        {

            //print(arrows[i]);
            Vector2 dir = arrows[i];
            if (flip)
            {
                dir *= -1;
            }
            Vector2 to = ((Vector2)transform.position + (dir * weights[i]));
            Gizmos.color = color;
            Gizmos.DrawLine(transform.position, to);


        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Draw(targetWeights_m, dirs, Color.green);
            
            Draw(avoidWeights_m, dirs, Color.red, true);
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
            targetWeights_m[i] = (1f/dirs.Length);
            avoidWeights_m[i] = (1f/dirs.Length);
            
            //print(dirs[i]);
        }
        dirWeights_m = targetWeights_m.Zip(avoidWeights_m, (a, b) => a + b).ToArray();
        curDir = dirs[0];
        SetMaxDir();

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
            .Select(d => (d - dataMin) / (range + Mathf.Epsilon))
            .Select(n => (float)((1 - n) * min + n * max))
            .ToArray();
    }

    private void Update()
    {
        //print(targetWeights_m[0]);
    }

    void SetMaxDir()
    {
        float maxVal = dirWeights_m.Max();
        //if the gain in heading is greater than threshold, switch to newest

        int maxIdx = Array.IndexOf(dirWeights_m, maxVal);
        int curDirIdx = Array.IndexOf(dirs, curDir);

        //if (targetWeights_m[maxIdx] - targetWeights_m[curDirIdx] > steeringThreshold)
        //{
        //    curDir = dirs[maxIdx];
        //    print("switching direction");
        //}
        curDir = dirs[maxIdx];
    }

    public void UpdateWeights(Vector2 target)
    {
        for(int i = 0; i<dirs.Length; i++)
        {
            //make layermask
            mask = (1 <<3);
            //for each obstacle, contribute to the weight by optimality (of avoidance)
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius, layerMask: mask);
            //print(cols.Length);
            if (cols.Length > 0)
            {
                
                float[] avoidVals = new float[cols.Length];

                for (int j = 0; j < cols.Length;j++)
                
                {
                    //print(cols[j].name);
                    Vector2 obstacle = cols[j].transform.position;
                    avoidVals[j]=  GetWeight(dirs[i], obstacle, avoidWeightScaleFactor, shapeHorizontal:true);
                    //print(avoidVals[j]);
                }
                float avoidVal = avoidVals.Sum() / avoidVals.Length;
                //print(avoidVal);
                avoidWeights_m[i] = avoidVal;
                //print(avoidVal);
            }
            else
            {
                avoidWeights_m[i] = Mathf.Epsilon;
            }
           
            //targetWeights_m[i] = Mathf.SmoothDamp(targetWeights_m[i], GetWeight(dirs[i], target, targetWeightScaleFactor, shapeHorizontal:true), ref targetWeights_m[i], smoothTime);// - avoidWeights_m[i];
            targetWeights_m[i] = GetWeight(dirs[i], target, targetWeightScaleFactor, shapeHorizontal: false);

            //int j = 0;

            ///
        }

        //print(avoidWeights_m[0]);
        targetWeights_m = NormalizeData(targetWeights_m, 0, 1);
        avoidWeights_m = NormalizeData(avoidWeights_m, 0, 1);
        dirWeights_m = targetWeights_m.Zip(avoidWeights_m, (a, b) => a - b).ToArray();

        for (int i=0; i<targetWeights_m.Length; i++)
        {
            //print((targetWeights_m[i], avoidWeights_m[i], (targetWeights_m[i] - avoidWeights_m[i])));
            //print((i, targetWeights_m[i]));
        }
        //print(curDir);
        SetMaxDir();
        //SetWeights();
    }

    void SetWeights()
    {
        for (int i = 0; i < dirs.Length; i++)
        {
            targetWeights_m[i] = targetWeights_m[i] - avoidWeights_m[i];
        }
        float maxVal = targetWeights_m.Max();
        //if the gain in heading is greater than threshold, switch to newest


        int maxIdx = Array.IndexOf(targetWeights_m, maxVal);
        curDir = dirs[maxIdx];

    }



    float GetWeight(Vector2 dir, Vector2 other, float scaleFactor = 1, bool shapeHorizontal = true, bool avoidWeight = false)
    {
        ///the distance buffer scales the dot product by the distance between 
        float distBuffer =Mathf.Clamp(Vector2.Distance(((Vector2)transform.position + dir), other), 0, 1);

        float dot = Vector2.Dot((other - (Vector2)transform.position).normalized, dir);
        if (shapeHorizontal)
        {
            //dot: 1 (aligned) -1 (opposite) 0 (perpendicular)
            //dot-1
            dot *= (1 - (Mathf.Abs(dot - 0.65f))) * distBuffer; 

        }
        dot = dot * scaleFactor;
        return dot;
    }
    /// <summary>
    ///
    /// for each obstacle within the detection radius,
    ///     get the dot product
    ///     scale it by the distance(maxed as above)
    ///     add it to its corresponding
    /// </summary>
    /// <returns></returns>


    public Vector2 GetDir()
    {
        return CurDir;
    }
}
