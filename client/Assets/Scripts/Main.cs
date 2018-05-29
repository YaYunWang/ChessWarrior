using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
	public bool HasReporter = false;

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

		GameObject reporter = transform.Find("Reporter").gameObject;
		reporter.SetActive(HasReporter);
	}

	void Update()
	{

	}
}
