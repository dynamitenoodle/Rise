using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    
    public int getLarger(int a, int b)
    {
        return (a > b) ? a : b;
    }

    public float getLarger(float a, float b)
    {
        return (a > b) ? a : b;
    }

    public float getDistance(Vector2 v1, Vector2 v2)
    {
        return (v1 - v2).magnitude;
    }

}
