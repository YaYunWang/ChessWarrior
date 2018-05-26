using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
	private void Awake()
	{
		GameObject.DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		GameEventManager.CreateInstance();
		LayerManager.CreateInstance();
        TimerManager.CreateInstance();
		GUIManager.CreateInstance();
        GameStateManager.CreateInstance();
		EmitNumberManager.CreateInstance();

		InputManager.CreateInstance();
		ChessManager.CreateInstance();
		ChessPathManager.CreateInstance();
	}

	void Update()
	{

	}
}
