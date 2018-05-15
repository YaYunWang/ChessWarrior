using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class LoginUIPanel : GUIBase
{
	protected override void OnStart()
	{
        SetEventListener("login", EventListenerType.onClick, OnLoginClick);

		if (PlayerPrefs.HasKey(LoginState.PlayerPrefs_Account) && PlayerPrefs.HasKey(LoginState.PlayerPrefs_Password))
		{
			SetTextInput("user_name", PlayerPrefs.GetString(LoginState.PlayerPrefs_Account));
			SetTextInput("pass_word", PlayerPrefs.GetString(LoginState.PlayerPrefs_Password));
		}
	}

	private void OnLoginClick(EventContext ec)
	{
		string usename = GetTextInput("user_name");
		string password = GetTextInput("pass_word");

		if (string.IsNullOrEmpty(usename) || string.IsNullOrEmpty(password))
		{
			DebugLogger.Log("帐号密码为空，不能登录.");
			return;
		}

		PlayerPrefs.SetString(LoginState.PlayerPrefs_Account, usename);
		PlayerPrefs.SetString(LoginState.PlayerPrefs_Password, password);

		KBEngine.Event.fireIn("login", usename, password, System.Text.Encoding.UTF8.GetBytes("ChessWarrior"));
	}
}
