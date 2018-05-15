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
		LayerManager.CreateInstance();
        TimerManager.CreateInstance();
		GUIManager.CreateInstance();
        GameStateManager.CreateInstance();
	}

	void Update()
	{

	}
}
