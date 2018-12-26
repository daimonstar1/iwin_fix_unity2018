using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameTacoSDK
{
	/// <summary>
	/// Implement login function
	/// </summary>
	public interface ITacoManageLoginController
	{
		IEnumerator login ();

		IEnumerator loginWithGoogle ();
	}

	public class TacoManageLoginController:ITacoManageLoginController
	{
		ITacoUIInputView email_view;
		ITacoUIInputView password_view;
		ITacoUIButtonView btn_login;
		ITacoUIButtonView btn_login_with_google;
		ITacoUIToggleView remember_login_toggle;
		ITacoLoadingState loading;
		ITacoLogin handler;
		ITacoLoginModelObserver result_view;

		public TacoManageLoginController (ITacoUIInputView _email_view, ITacoUIInputView _password_view,
		                                  ITacoUIButtonView _btn_login, ITacoUIButtonView _btn_login_with_google, ITacoUIToggleView _remember_login_toggle,
		                                  ITacoLoadingState _loading, ITacoLoginModelObserver _result_view, ITacoLogin _handler)
		{
			email_view = _email_view;
			password_view = _password_view;
			btn_login = _btn_login;
			btn_login_with_google = _btn_login_with_google;
			remember_login_toggle = _remember_login_toggle;
			loading = _loading;
			handler = _handler;
			result_view = _result_view;

			email_view.initEvent ();
			email_view.data_changed += text_changed;
			password_view.initEvent ();
			password_view.data_changed += text_changed;
			btn_login.initEvent ();
			btn_login.on_click += button_clicked;
			btn_login_with_google.initEvent ();
			btn_login_with_google.on_click += button_clicked;
			remember_login_toggle.initEvent ();
			remember_login_toggle.check_changed += check_changed;
			handler.attachLoadingView (loading);
			handler.attachResultView (result_view);
		}

		private void text_changed (InputField sender, TacoUIInputEventArgs args)
		{
			if (args.type == eNumComponentType.INPUT_EMAIL)
				handler.setEmail (args._text);
			else if (args.type == eNumComponentType.INPUT_PASSWORD)
				handler.setPassword (args._text);
		}

		private void button_clicked (Button sender, TacoUIButtonEventArgs args)
		{
			if (args.type == eNumComponentType.BUTTON_LOGIN)
				sender.StartCoroutine (login ());
			else if (args.type == eNumComponentType.BUTTON_LOGIN_WITH_GOOGLE)
				sender.StopCoroutine (loginWithGoogle ());
		}

		private void check_changed (Toggle sender, TacoUIToggleEventArgs args)
		{
			if (args.type == eNumComponentType.TOGGLE_REMEMBER_LOGIN)
				handler.setRememberLogin (args.is_checked);
		}

		#region ITacoManageLoginController implementation

		public IEnumerator login ()
		{
			yield return handler.login ();
		}

		public IEnumerator loginWithGoogle ()
		{
			yield return handler.loginWithGoogle ();
		}

		#endregion





	}
}
