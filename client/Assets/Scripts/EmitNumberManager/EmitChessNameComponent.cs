using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class EmitChessNameComponent : EmitComponent
{
	private GTextField nameText = null;

	public EmitChessNameComponent()
	{
		this.touchable = false;

		nameText = new GTextField();
		nameText.autoSize = AutoSizeType.Both;
		nameText.color = Color.red;

		TextFormat tf = nameText.textFormat;

		tf.color = Color.red;
		tf.size = 10;

		nameText.textFormat = tf;

		AddChild(nameText);
	}

	public override void ShowEmit(params object[] args)
	{
		base.ShowEmit(args);

		ChessEntity chess = args[0] as ChessEntity;
		if (chess == null)
		{
			OnCompleted();
			return;
		}

		nameText.text = chess.chessCfg.Name;

		this.container.gameObject.transform.parent = chess.transform;
		//EmitNumberManager.AddChild(this);

		this.container.gameObject.SetActive(true);
	}

	void OnCompleted()
	{
		EmitNumberManager.RemoveChild(this);
		EmitNumberManager.ReturnComponent(EmitNumberType.EmitNumberTypeChessName, this);
	}
}
