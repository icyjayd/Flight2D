using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CompassGizmo : MonoBehaviour
{
    public float[] weights;
    //float[16] weights;

    //this class draws a gizmo with 16 directions indicating the possible directions that the object can move in
   void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

         
    }
}
