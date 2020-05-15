using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LineSegment : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Vector2> points;
    public List<float> durations;


    public void CheckLengths()
    {
       int durationCapacity = durations.Capacity;
       if(durations.Capacity != points.Capacity || points.Capacity != durations.Capacity)
        {
            Debug.Log("All durations need a point! Set point number first.");
            List<float> newDurations = new List<float>();
            for (int i=0; i < points.Capacity; i++)
            {
                
                if (i < durationCapacity)
                {
                    Debug.Log(i.ToString() + " Durations: " + durations.Capacity.ToString());
                    float test = durations[i];
                    newDurations.Add(test);

                }
                else
                {
                    Debug.Log(i.ToString() + " above duration capacity");
                    newDurations.Add(0);
                }
            
            }
            durations = newDurations;
              
        }
    }

}
