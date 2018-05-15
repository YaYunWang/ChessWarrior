using UnityEngine;
using System.Collections;

public static class ConfigParseUtil
{
    public const char DEFAULT_SPLITER = ',';
    public const char DEFAULT_APLITER2 = ';';

    public static int[] ParseIntArray(string str, char spliter = DEFAULT_SPLITER)
    {
        if (string.IsNullOrEmpty(str))
            return null;

        string[] strArray = str.Split(spliter);
        int[] intArray = new int[strArray.Length];

        for (int i = 0; i < strArray.Length; i++)
        {
            intArray[i] = int.Parse(strArray[i]);
        }

        return intArray;
    }

    public static float[] ParseFloatArray(string str, char spliter = DEFAULT_SPLITER)
    {
        if (string.IsNullOrEmpty(str))
            return null;

        string[] strArray = str.Split(spliter);
        float[] floatArray = new float[strArray.Length];

        for (int i = 0; i < strArray.Length; i++)
        {
            floatArray[i] = float.Parse(strArray[i]);
        }

        return floatArray;
    }

    public static Vector3 ParseVec3(string str, char spliter = DEFAULT_SPLITER)
    {
        if (string.IsNullOrEmpty(str))
            return Vector3.zero;

        string[] strArray = str.Split(spliter);
        Vector3 vec3 = Vector3.zero;

        for (int i = 0; i < strArray.Length; i++)
        {
            switch (i)
            {
                case 0:
                    vec3.x = float.Parse(strArray[i]);
                    break;
                case 1:
                    vec3.y = float.Parse(strArray[i]);
                    break;
                case 2:
                    vec3.z = float.Parse(strArray[i]);
                    break;
                default:
                    break;
            }
        }

        return vec3;
    }

    public static Vector3[] ParseVec3Array(string str, char spliter = DEFAULT_SPLITER, char spliter2 = DEFAULT_APLITER2)
    {
        if (string.IsNullOrEmpty(str))
            return null;

        string[] strArray = str.Split(spliter2);
        Vector3[] vec3 = new Vector3[strArray.Length];

        for (int i = 0; i < strArray.Length; i++)
        {
            vec3[i] = ParseVec3(strArray[i]);
        }

        return vec3;
    }
}

