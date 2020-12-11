using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HelperFunctions
{
    public static List<Vector3> return_sphere_cordinates(int NumPoints, float radius)
    {
        List<Vector3> List = new List<Vector3>();

        // Compute the golden ratio for later
        double GLratio = 1 + Math.Pow(5, 0.5);

        // add all the values of the
        for (int i = 0; i < NumPoints; i++)
        {
            double index = (i + 0.5);
            double phi = Math.Acos(1 - ((2 * index) / NumPoints));
            double theta = Math.PI * (GLratio) * index;

            // @Jose polar coordinates are better!!!
            double x = radius * Math.Cos(theta) * Math.Sin(phi);
            double y = radius * Math.Sin(theta) * Math.Sin(phi);
            double z = radius * Math.Cos(phi);

            Vector3 pnt = new Vector3((float)x, (float)y, (float)z);
            List.Add(pnt);
        }

        return List;
    }
}
