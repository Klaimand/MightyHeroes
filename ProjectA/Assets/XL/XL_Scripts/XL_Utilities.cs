using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class XL_Utilities
{
    private static float g = 9.81f;

    public static float[] GetVelocity(float startingHeight, float distance, float projectileTravelTime) 
    {
        float vy = (g * projectileTravelTime * projectileTravelTime - 2 * startingHeight) / (2 * projectileTravelTime);
        float vx = (g * distance) / (vy + Mathf.Sqrt(vy * vy + 2 * g * startingHeight));
        float[] velocity = { vx, vy };
        return velocity;
    }
}
