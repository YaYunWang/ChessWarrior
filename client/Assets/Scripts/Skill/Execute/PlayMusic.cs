using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class PlayMusic : ExecuteBase
{
	private int musicId = 0;

	public override bool OnLoad(XmlNode node)
	{
		if (!base.OnLoad(node))
			return false;

		musicId = GameConvert.IntConvert(GetProp("MusicID"));

		return true;
	}

	public override bool OnEntry(SkillArgs args)
	{
		if (!base.OnEntry(args))
			return false;

		ChessEntity target = GetTarget(args);
		if (target == null)
			return false;

		AudioManager.PlayAudio(musicId, target.transform);

		return true;
	}
}
