using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CompassGizmo : MonoBehaviour
{
    //public float[] weights;
    Vector2[] arrows;
    float radius = 2;
    //float[16] weights;


    //this class draws a gizmo with 16 directions indicating the possible directions that the object can move in
    public void InitGizmo(Vector2[] dirs)
    {
        arrows = dirs;
    }

    public void Draw(float[] weights)
    {
        for(int i =0; i<weights.Length; i++)
        {
            //print(arrows[i]);
            Vector3 dir = new Vector3(arrows[i].x, arrows[i].y);
            Vector3 to =  (transform.position +( dir * weights[i]));
            Gizmos.DrawLine(transform.position, to);

        }
    }
}
