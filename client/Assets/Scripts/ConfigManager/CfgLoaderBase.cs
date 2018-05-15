using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Xml;

public abstract class CfgLoaderBase
{
    public bool IsLoaded
    {
        get { return m_isLoaded; }
    }
    private bool m_isLoaded = false;

    public void Load()
    {
        if (IsLoaded)
            return;

        OnLoad();
    }

    public void Unload(bool force = false)
    {
        if (!force && !IsLoaded)
            return;

        OnUnload();
    }
    
    protected abstract void OnLoad();

    protected abstract void OnUnload();

    protected static Dictionary<string, FieldInfo> fieldTypeDic = new Dictionary<string, FieldInfo>();


    public void ReadConfig<T>(string path, System.Action<T> rowHandler, string bundleName = "config.bundle") where T : class
    {
		ReadPlainXml<T>(path, rowHandler, bundleName);
    }

	private bool ReadPlainXml<T>(string path, System.Action<T> rowHandler, string bundleName)
	{
		Dictionary<string, FieldInfo> fields = GetFields(typeof(T));

		if (fields == null || fields.Count == 0)
			return false;

		XmlDocument doc = ConfigManager.GetDocument(path, bundleName);

		if (doc == null)
			return false;

		var table = doc.SelectSingleNode("Table");
		if (table == null)
			return false;

		var rows = table.SelectNodes("Row");

		for (int i = 0; i < rows.Count; i++)
		{
			var row = rows[i] as XmlElement;
			if (row == null)
				continue;

			T rowInstance = (T)System.Activator.CreateInstance(typeof(T));
			if (rowInstance == null)
				return false;

			var attribs = row.Attributes;

			for (int j = 0; j < attribs.Count; j++)
			{
				var attribName = attribs[j].Name.ToLower();
				var attribVal = attribs[j].Value;


				FieldInfo field = null;
				if (!fields.TryGetValue(attribName, out field))
					continue;

				SetField(field, rowInstance, attribVal);
			}

			rowHandler(rowInstance);
		}

		return true;
	}

	private void SetField(FieldInfo field, object obj, string strVal)
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

	private Dictionary<string, FieldInfo> GetFields(System.Type type)
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
