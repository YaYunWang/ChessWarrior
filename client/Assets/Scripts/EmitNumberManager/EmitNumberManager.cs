using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmitNumberType
{
	EmitNumberTypeServerErrorCode,
	EmitNumberTypeChessName,
}

public class EmitNumberManager : ManagerTemplateBase<EmitNumberManager>
{
	public static GComponent view { get; private set; }

	private static Dictionary<EmitNumberType, List<EmitComponent>> _componentPool = new Dictionary<EmitNumberType, List<EmitComponent>>();

	protected override void InitManager()
	{
	}

	public static void Emit(EmitNumberType type, params object[] args)
	{
		if (view == null)
		{
			view = new GComponent();
			GRoot.inst.AddChild(view);
		}

		EmitComponent ec = FindEmit(type);

		if (ec == null)
		{
			switch (type)
			{
				case EmitNumberType.EmitNumberTypeServerErrorCode:
					ec = new ServerErrorCodeEmitComponent();
					break;
				case EmitNumberType.EmitNumberTypeChessName:
					ec = new EmitChessNameComponent();
					break;
			}
		}

		ec.ShowEmit(args);
	}

	private static EmitComponent FindEmit(EmitNumberType type)
	{
		if (!_componentPool.ContainsKey(type))
			return null;

		if (_componentPool[type].Count <= 0)
			return null;

		EmitComponent component = _componentPool[type][0];

		_componentPool[type].Remove(component);

		return component;
	}

	public static void RemoveChild(EmitComponent com)
	{
		view.RemoveChild(com);
	}

	public static void AddChild(EmitComponent com)
	{
		view.AddChild(com);
	}

	public static void ReturnComponent(EmitNumberType type, EmitComponent com)
	{
		if(!_componentPool.ContainsKey(type))
		{
			_componentPool.Add(type, new List<EmitComponent>());
		}

		_componentPool[type].Add(com);
	}
}
