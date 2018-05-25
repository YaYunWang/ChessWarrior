using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class FlowBase : ExecuteBase
{
	protected List<SkillBase> childList = new List<SkillBase>();
	protected Dictionary<string, List<SkillBase>> childDic = new Dictionary<string, List<SkillBase>>();

	public override bool OnLoad(XmlNode node)
	{
		if (!base.OnLoad(node))
			return false;

		XmlNodeList nodeList = node.ChildNodes;
		foreach (XmlNode child in nodeList)
		{
			string nodeName = child.Name;

			System.Type type = SkillCfgLoader.GetSkillType(nodeName);
			if (type == null)
			{
				continue;
			}

			SkillBase combat = System.Activator.CreateInstance(type) as SkillBase;
			if (combat == null)
			{
				continue;
			}

			if (!combat.OnLoad(child))
			{
				continue;
			}

			combat.Parent = this;

			childList.Add(combat);
			if (!childDic.ContainsKey(nodeName))
			{
				childDic.Add(nodeName, new List<SkillBase>());
			}

			childDic[nodeName].Add(combat);
		}

		return true;
	}

	protected bool RunningChild(SkillArgs args)
	{
		for(int idx = 0; idx < childList.Count; idx++)
		{
			if (!childList[idx].OnEntry(args))
				return false;
		}

		return true;
	}

	protected bool RunningChild(SkillArgs args, int index)
	{
		if (childList == null || childList.Count <= index)
			return false;

		return childList[index].OnEntry(args);
	}

	protected bool RunningChild(SkillArgs args, string childName, int index = 0)
	{
		SkillBase combat = FindChild(childName, index);

		if (combat == null)
			return false;

		return combat.OnEntry(args);
	}

	protected SkillBase FindChild(string childName, int index)
	{
		if (childDic == null || !childDic.ContainsKey(childName))
			return null;

		List<SkillBase> list = childDic[childName];
		if (list == null || list.Count <= index)
			return null;

		return list[index];
	}

	public override bool OnLeave(SkillArgs args)
	{
		if (!base.OnLeave(args))
			return false;

		for (int idx = 0; idx < childList.Count; idx++)
		{
			if (!childList[idx].OnLeave(args))
				return false;
		}

		return true;
	}
}
