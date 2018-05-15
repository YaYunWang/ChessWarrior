using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using UnityEngine;

public class ConfigManager : ManagerTemplateBase<ConfigManager>
{
    private static Dictionary<System.Type, CfgLoaderBase> m_configLoaders = new Dictionary<System.Type, CfgLoaderBase>();

    private static float totalnum = 0;
    private static float loadednum = 0;

    private static Stopwatch taskStopwatch = new Stopwatch();

    public static T Get<T>() where T : CfgLoaderBase
    {
        CfgLoaderBase loader = null;
        if (!m_configLoaders.TryGetValue(typeof(T), out loader))
            return null;

        return loader as T;
    }

    protected override void InitManager()
    {
        var types = typeof(CfgLoaderBase).Assembly.GetTypes();

        for (int i = 0; i < types.Length; i++)
        {
            var type = types[i];
            if (type.IsSubclassOf(typeof(CfgLoaderBase)))
            {
                var loader = System.Activator.CreateInstance(type) as CfgLoaderBase;
                if (loader == null)
                    continue;

                m_configLoaders.Add(types[i], loader);
            }
        }
        totalnum = m_configLoaders.Count;

        StartCoroutine(LoadConfig());
    }

    public static bool isDone
    {
        get
        {
            return (loadednum > totalnum) ? true : false;
        }
    }

    public static float progress
    {
        get
        {
            return loadednum / totalnum;
        }
    }

    IEnumerator LoadConfig()
    {
        taskStopwatch.Reset();
        taskStopwatch.Start();

        var ienumer = m_configLoaders.GetEnumerator();
        while (ienumer.MoveNext())
        {
            ienumer.Current.Value.Load();
            loadednum++;
            yield return null;
        }
        loadednum++;
        yield return null;

        taskStopwatch.Stop();
        DebugLogger.LogFormat("[ConfigManager]:配置文件加载总时间 totalTime: {0} ms",taskStopwatch.ElapsedMilliseconds);
	}

	public static XmlDocument GetDocument(string path, string bundleName)
	{
		TextAsset data = AssetLoadManager.LoadAsset<TextAsset>(bundleName, System.IO.Path.GetFileNameWithoutExtension(path));

		if (data == null)
		{
			return null;
		}

		XmlDocument doc = new XmlDocument();
		try
		{
			doc.LoadXml(data.text);
		}
		catch (System.Exception ex)
		{
			return null;
		}

		return doc;
	}
}
