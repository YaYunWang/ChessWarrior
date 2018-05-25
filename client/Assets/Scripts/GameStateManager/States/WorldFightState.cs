using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KBEngine;

public class WorldFightState : IState
{
	private FightMainUIPanel fightMainUIPanel = null;
	public void Enter()
	{
		Account account = KBEngine.KBEngineApp.app.player() as Account;

		account.baseCall("ClientReady");

		KBEngine.Event.registerOut("OnStartRound", this, "OnStartRound");
		KBEngine.Event.registerOut("AttackChess", this, "OnAttackChess");

		ChessPathManager.CreatePathPoint();

		fightMainUIPanel = GUIManager.Open<FightMainUIPanel>("Fight", "FightMainUIPanel");

		fightMainUIPanel.SetStateInfoString("正在摆盘，请稍候...");
	}

	public void Exit()
	{
	}

	public void Update()
	{
	}

	public void OnAttackChess(int attackChess, int beAttackChess)
	{
		InputManager.ClearSelectChess();

		ChessEntity attack = ChessManager.FindEntityByID(attackChess);
		ChessEntity beAttack = ChessManager.FindEntityByID(beAttackChess);

		if (attack == null || beAttack == null)
			return;

		attack.MoveTo((int)beAttack.chessObj.chess_index_x, (int)beAttack.chessObj.chess_index_z);

		// 开始释放技能
		attack.Target = beAttack;
		beAttack.Target = attack;

		attack.UseNormalSkill();
	}

	public void OnStartRound(int type, int time)
	{
		InputManager.ClearSelectChess();
		if(type == 1)
		{
			Debug.Log("自己回合，开始走棋。");
			InputManager.CanMove = true;

			fightMainUIPanel.MyRound(time);
		}
		else
		{
			Debug.Log("敌人回合，暂时不做处理。");
			InputManager.CanMove = false;
			fightMainUIPanel.SetStateInfoString("敌人正在思考中...");
		}
	}
}
