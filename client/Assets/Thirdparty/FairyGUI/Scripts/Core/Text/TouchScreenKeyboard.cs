﻿using UnityEngine;

namespace FairyGUI
{
	/// <summary>
	/// 
	/// </summary>
	public class TouchScreenKeyboard : IKeyboard
	{
		UnityEngine.TouchScreenKeyboard _keyboard;

		public bool done
		{
			get { return _keyboard == null || _keyboard.done; }
		}

		public bool supportsCaret
		{
			get { return false; }
		}

		public string GetInput()
		{
			if (_keyboard != null)
			{
				string s = _keyboard.text;

				if (_keyboard.done)
					_keyboard = null;

				return s;
			}
			else
				return null;
		}

		public void Open(string text, bool autocorrection, bool multiline, bool secure, bool alert, string textPlaceholder, int keyboardType, bool hideInput)
		{
			if (_keyboard != null)
				return;

			UnityEngine.TouchScreenKeyboard.hideInput = hideInput;
			_keyboard = UnityEngine.TouchScreenKeyboard.Open(text, (TouchScreenKeyboardType)keyboardType, autocorrection, multiline, secure, alert, textPlaceholder);
		}

		public void Close()
		{
			if (_keyboard != null)
			{
				_keyboard.active = false;
				_keyboard = null;
			}
		}
	}
}
