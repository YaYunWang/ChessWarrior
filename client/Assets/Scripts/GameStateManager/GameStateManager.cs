using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StateEnum
{
	UPDATE,
	LOADCONFIG,
    LOGIN,
    WORLD,
    WORLDFight,
}

public class GameStateManager : ManagerTemplateBase<GameStateManager>
{
    private static Dictionary<StateEnum, IState> m_stateDic = new Dictionary<StateEnum, IState>();

    private static IState m_activeState;

    public static IState CurrentState
    {
        get { return m_activeState; }
    }

    private static StateEnum m_stateType = StateEnum.UPDATE;

    public static StateEnum CurrentStateType
    {
        get { return m_stateType; }
    }

    protected override void InitManager()
    {
        m_stateDic.Add(StateEnum.UPDATE, new UpdateState());
        m_stateDic.Add(StateEnum.LOADCONFIG, new LoadConfigState());
        m_stateDic.Add(StateEnum.LOGIN, new LoginState());
        m_stateDic.Add(StateEnum.WORLD, new WorldState());
        m_stateDic.Add(StateEnum.WORLDFight, new WorldFightState());

		KBEngine.Event.registerOut("onLoginFailed", this, "OnLoginFailed");
		KBEngine.Event.registerOut("onDisconnected", this, "OnDisconnected");
		KBEngine.Event.registerOut("EntryFb", this, "EntryFb");

		GameEventManager.RegisterEvent(GameEventTypes.ExitScene, OnExitScene);
		GameEventManager.RegisterEvent(GameEventTypes.EnterScene, OnEntryScene);
    }

	public void EntryFb()
	{
		SceneManager.ChangeScene(2);
	}

	private void OnEntryScene(GameEventTypes eventType, object[] args)
	{
	}

	private void OnExitScene(GameEventTypes eventType, object[] args)
	{
	}

	public void OnDisconnected()
	{
		GameStateManager.ChangeState(StateEnum.LOGIN);
	}

	public void OnLoginFailed(UInt16 failedcode)
	{
		ServerErrorCodeCfg cfg = ConfigManager.Get<ServerErrorCodeLoader>().GetConfig((int)failedcode);
		if (cfg == null)
			return;

		EmitNumberManager.Emit(EmitNumberType.EmitNumberTypeServerErrorCode, cfg.Error);
	}

	void Update()
    {
        if (m_activeState != null)
            m_activeState.Update();
    }

    void Start()
    {
		ChangeState(m_stateType);
    }

    protected override void OnDestroy()
    {
        m_stateDic.Clear();
    }

    public static IState ChangeState(StateEnum se)
    {
        if (m_activeState != null)
        {
            m_activeState.Exit();
        }
        m_stateType = se;
        m_activeState = m_stateDic[se];
        m_activeState.Enter();
        return m_activeState;
    }
}
