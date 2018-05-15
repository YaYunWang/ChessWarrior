using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateEnum
{
	UPDATE,
	LOADCONFIG,
    LOGIN,
    WORLD,
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
