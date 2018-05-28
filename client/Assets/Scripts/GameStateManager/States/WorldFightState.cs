using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KBEngine;
using System;

public class WorldFightState : IState
{
	private FightMainUIPanel fightMainUIPanel = null;

	private ChessEntity currentAttackChess = null;
	public void Enter()
	{
		KBEngine.Event.registerOut("OnStartRound", this, "OnStartRound");
		KBEngine.Event.registerOut("AttackChess", this, "OnAttackChess");
		KBEngine.Event.registerOut("OnExitFb", this, "OnExitFb");
		GameEventManager.RegisterEvent(GameEventTypes.KillChess, OnKillChessEvent);

		fightMainUIPanel = GUIManager.Open<FightMainUIPanel>("Fight", "FightMainUIPanel");
		fightMainUIPanel.SetStateInfoString("正在摆盘，请稍候...");

		GUIManager.Open<ChessInfoUIPanel>("ChessInfo", "ChessInfoUIPanel");

		ChessPathManager.CreatePathPoint();
        ChessPathManager.ShowChessBoard();

        Account account = KBEngine.KBEngineApp.app.player() as Account;
		account.baseCall("ClientReady");
	}

	private void OnKillChessEvent(GameEventTypes eventType, object[] args)
	{
		ChessEntity attack = args[0] as ChessEntity;
		ChessEntity beAttack = args[1] as ChessEntity;
		if (attack == null || beAttack == null)
			return;

		if(beAttack.GetChessType() == ChessType.ChessTypeJiang)
		{
			fightMainUIPanel.SetStateInfoString("");

			if(beAttack.chessObj.chess_owner_player == 1)
			{
				GUIManager.Open<FightLostUIPanel>("Fight", "FightLostUIPanel");
			}
			else
			{
				GUIManager.Open<FightWinUIPanel>("Fight", "FightWinUIPanel");
			}
		}
		else
		{
			Account account = KBEngine.KBEngineApp.app.player() as Account;
			account.baseCall("KillChess", beAttack.chessObj.id);

			if (currentAttackChess != null && currentAttackChess == attack)
			{
				currentAttackChess.MoveTo((int)beAttack.chessObj.chess_index_x, (int)beAttack.chessObj.chess_index_z);
			}
		}

		currentAttackChess = null;
	}

	public void Exit()
	{
		GameEventManager.UnregisterEvent(GameEventTypes.KillChess, OnKillChessEvent);

		KBEngine.Event.deregisterOut("OnStartRound", this, "OnStartRound");
		KBEngine.Event.deregisterOut("AttackChess", this, "OnAttackChess");
		KBEngine.Event.deregisterOut("OnExitFb", this, "OnExitFb");

		currentAttackChess = null;
	}

	public void Update()
	{
	}

	public void OnExitFb()
	{
		SceneManager.ChangeScene(1);
	}

	public void OnAttackChess(int attackChess, int beAttackChess)
	{
		InputManager.ClearSelectChess();

		ChessEntity attack = ChessManager.FindEntityByID(attackChess);
		ChessEntity beAttack = ChessManager.FindEntityByID(beAttackChess);

		if (attack == null || beAttack == null)
			return;

		Vector3 selfPos = attack.transform.localPosition;
		Vector3 beAttackpos = new Vector3(ChessEntity.ChessInterval * (int)beAttack.chessObj.chess_index_x, 0, ChessEntity.ChessInterval * (int)beAttack.chessObj.chess_index_z);

		// 刚开始不能直接移动到要攻击的棋子旁边
		Vector3 pos = (beAttackpos - selfPos).normalized;
		pos *= Vector3.Distance(beAttackpos, selfPos) - ChessEntity.ChessInterval;
		pos += selfPos;
		attack.MoveTo(pos);

		// 开始释放技能
		attack.Target = beAttack;
		beAttack.Target = attack;

		attack.UseNormalSkill();

		currentAttackChess = attack;

		fightMainUIPanel.SetStateInfoString("战斗中...");
	}

	public void OnStartRound(int type, int time)
	{
		InputManager.ClearSelectChess();
		if(type == 1)
		{
			InputManager.CanMove = true;
			fightMainUIPanel.MyRound(time);
		}
		else
		{
			InputManager.CanMove = false;
			fightMainUIPanel.SetStateInfoString("敌人正在思考中...");
		}
	}
}
