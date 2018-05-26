using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class DamageNormal : ExecuteBase
{
	private string damageTarget = "";
	private string beDamageTarget = "";

	public override bool OnLoad(XmlNode node)
	{
		if (!base.OnLoad(node))
			return false;

		damageTarget = GetProp("DamageTarget");
		beDamageTarget = GetProp("BeDamageTarget");

		return true;
	}

	public override bool OnEntry(SkillArgs args)
	{
		if (!base.OnEntry(args))
			return false;

		ChessEntity attack = GetTarget(args, damageTarget);
		ChessEntity beAttack = GetTarget(args, beDamageTarget);
		if (attack == null || beAttack == null)
			return false;

		int damage = (int)attack.chessObj.chess_attack;
		damage -= (int)beAttack.chessObj.chess_defense;

		if (damage <= 0)
			damage = 1;

		beAttack.BeAttack(damage);

		return true;
	}
}
