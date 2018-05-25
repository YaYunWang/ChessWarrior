using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class SkillBase
{
    protected int id = 0;

    public int ID
    {
        get
        {
            return id;
        }
    }

    protected SkillBase parent = null;
    public SkillBase Parent
    {
        get
        {
            return parent;
        }

        set
        {
            parent = value;
        }
    }

    public SkillBase Root()
    {
		SkillBase root = this;
        while(true)
        {
            if(root.Parent != null)
            {
                root = root.Parent;
            }
            else
            {
                break;
            }
        }

        return root;
    }

    private Dictionary<string, string> m_prop = new Dictionary<string, string>();

    public virtual bool OnLoad(XmlNode node)
    {
        // 设置属性
        XmlElement element = node as XmlElement;
        string id_string = element.GetAttribute("ID");
        if(!string.IsNullOrEmpty(id_string))
        {
            id = int.Parse(id_string);
        }

        var attributes = element.Attributes;
        for (int idx = 0; idx < attributes.Count; idx++)
        {
            var attribute = attributes[idx];
            if (attribute == null)
                continue;

            string key = attribute.Name;
            string value = attribute.Value;

            SetProp(key, value);
        }

        return true;
    }

    public virtual bool OnInit(SkillArgs args)
    {
		if (args == null)
			return false;

		return true;
    }

    public virtual bool OnEntry(SkillArgs args)
    {
		if (args == null)
			return false;

        return true;
    }

    public virtual bool OnLeave(SkillArgs args)
    {
		if (args == null)
			return false;

		return true;
    }

	public virtual bool OnStop(SkillArgs args)
	{
		if (args == null)
			return false;

		OnLeave(args);

		return true;
	}

	public string GetProp(string key)
    {
        if (m_prop == null)
            return string.Empty;

        return m_prop.ContainsKey(key) ? m_prop[key] : string.Empty;
    }

    private void SetProp(string key, string value)
    {
        if (m_prop == null)
        {
            m_prop = new Dictionary<string, string>();
        }

        if (m_prop.ContainsKey(key))
        {
            return;
        }

        m_prop.Add(key, value);
    }
}
