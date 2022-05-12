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

    private static float[] coefficients = new float[3];
    public static float[] GetCoefficients(float distance) 
    {
        coefficients[0] = -7.5f / (distance * distance);
        coefficients[1] = 6.5f / distance;
        coefficients[2] = 1f;

        Debug.Log("coeff 1 : " + coefficients[0] + "\n" +
            "coeff 2 : " + coefficients[1] + "\n" +
            "coeff 3 : " + coefficients[2]);

        return coefficients;
    }

    private static Vector3[] points;
    public static Vector3[] GenerateCurve(int nbPoints, float distance) 
    {
        GetCoefficients(distance);

        points = new Vector3[nbPoints];

        for (int i = 0; i < nbPoints; i++) 
        {
            points[i].x = (i * distance / nbPoints);
            points[i].y = coefficients[0] * (i * distance / nbPoints) * (i * distance / nbPoints) + coefficients[1] * (i * distance / nbPoints) + coefficients[2];
            Debug.Log("Point " + i + " : " + points[i]);
        }
        return points;
    }
}
