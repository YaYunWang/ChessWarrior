using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class FieldMisc
{

    public static void SetField(FieldInfo field, object obj, string strVal)
    {
        var fieldType = field.FieldType;

        if (fieldType == typeof(System.String))
        {
            field.SetValue(obj, strVal);
        }
        else if (fieldType == typeof(System.Int32))
        {
            int intVal = 0;
            System.Int32.TryParse(strVal, out intVal);
            field.SetValue(obj, intVal);
        }
        else if (fieldType == typeof(System.Int64))
        {
            long longVal = 0;
            System.Int64.TryParse(strVal, out longVal);
            field.SetValue(obj, longVal);
        }
        else if (fieldType == typeof(System.Single))
        {
            float floatVal = 0;
            System.Single.TryParse(strVal, out floatVal);
            field.SetValue(obj, floatVal);
        }
        else if (fieldType == typeof(System.Double))
        {
            double doubleVal = 0;
            System.Double.TryParse(strVal, out doubleVal);
            field.SetValue(obj, doubleVal);
        }
        else if (fieldType == typeof(System.UInt32))
        {
            uint uintVal = 0;
            System.UInt32.TryParse(strVal, out uintVal);
            field.SetValue(obj, uintVal);
        }
        else if (fieldType == typeof(System.UInt64))
        {
            ulong ulongVal = 0;
            System.UInt64.TryParse(strVal, out ulongVal);
            field.SetValue(obj, ulongVal);
        }
        else if (fieldType == typeof(System.Boolean))
        {
            bool boolVal = false;
            System.Boolean.TryParse(strVal, out boolVal);
            field.SetValue(obj, boolVal);
        }
        else
        {
            Debug.LogErrorFormat("Unsupported config type {0}", fieldType.ToString());
        }
    }

    public static Dictionary<string, FieldInfo> GetFields(System.Type type)
    {
        Dictionary<string, FieldInfo> result = new Dictionary<string, FieldInfo>();

        var fields = type.GetFields();
        for (int i = 0; i < fields.Length; i++)
        {
            var field = fields[i];

            string fieldName = field.Name.ToLower();
            result[fieldName] = field;

        }

        return result;
    }
}
