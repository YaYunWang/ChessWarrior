using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using DG.Tweening;

public class ServerErrorCodeEmitComponent : EmitComponent
{
	private GTextField errorCodeText = null;

	public ServerErrorCodeEmitComponent()
	{
		this.touchable = false;

		errorCodeText = new GTextField();
		errorCodeText.autoSize = AutoSizeType.Both;
		errorCodeText.color = Color.red;

		TextFormat tf = errorCodeText.textFormat;

		tf.color = Color.red;
		tf.size = 30;

		errorCodeText.textFormat = tf;

		AddChild(errorCodeText);
	}

	public override void ShowEmit(params object[] args)
	{
		base.ShowEmit(args);

		string errorStr = GameConvert.StringConvert(args[0]);

		this.SetXY(0, 300);
		DOTween.To(() => new Vector2(0, 300), val => { this.UpdatePosition(val); }, new Vector2(0, 0), 3f)
			.SetTarget(this).OnComplete(this.OnCompleted);

		errorCodeText.text = errorStr;

		EmitNumberManager.AddChild(this);

	}

	void UpdatePosition(Vector2 pos)
	{
		this.SetXY(pos.x, pos.y);
	}

	void OnCompleted()
	{
		EmitNumberManager.RemoveChild(this);
		EmitNumberManager.ReturnComponent(EmitNumberType.EmitNumberTypeServerErrorCode, this);
	}
}
