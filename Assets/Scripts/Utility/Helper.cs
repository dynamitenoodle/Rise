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

    public bool AABBTest(Vector2 center1, Vector2 bounds1, Vector2 center2, Vector2 bounds2)
    {
        //setup
        float xMax1 = center1.x + (bounds1.x / 2);
        float xMin1 = center1.x - (bounds1.x / 2);
        float yMax1 = center1.y + (bounds1.y / 2);
        float yMin1 = center1.y - (bounds1.y / 2);

        float xMax2 = center2.x + (bounds2.x / 2);
        float xMin2 = center2.x - (bounds2.x / 2);
        float yMax2 = center2.y + (bounds2.y / 2);
        float yMin2 = center2.y - (bounds2.y / 2);

        return (xMin1 < xMax2 && xMax1 > xMin2 && yMin1 < yMax2 && yMax1 > yMin2);
    }

}
