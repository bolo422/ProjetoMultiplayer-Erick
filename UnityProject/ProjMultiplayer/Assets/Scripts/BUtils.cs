using UnityEditor;
using UnityEngine;


public class BUtils : ScriptableObject
{
    public static GameObject Closest(Vector3 origin, GameObject[] points, out int index)
    {
        float minDistance = Mathf.Infinity;
        GameObject closest = null;
        int i = 0;
        index = 0;

        foreach (GameObject point in points)
        {
            i++;
            if (Vector3.Distance(origin, point.transform.position) < minDistance)
            {
                closest = point;
                minDistance = Vector3.Distance(origin, point.transform.position);
                index = i;
            }
        }

        return closest;
    }
}
