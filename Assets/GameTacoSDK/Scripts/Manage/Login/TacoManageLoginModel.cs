using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
namespace GameTacoSDK
{
	public interface ITacoLogin
	{
		void attachResultView (ITacoLoginModelObserver _result);

		void attachLoadingView (ITacoLoadingState loading);

		IEnumerator login ();

		IEnumerator loginWithGoogle ();

		void setEmail (string email);

		void setPassword (string password);

		void setRememberLogin (string is_remember_login);
	}

	public class UserInfo
	{
		public string id, name, email;
		public int score, cash, token;

		public UserInfo (string _id, string _name, string _email, int _score, int _cash, int _token)
		{
			id = _id;
			name = _name;
			email = _email;
			score = _score;
			cash = _cash;
			token = _token;
		}
	}

	public class LoginSuccessModelEventArgs : EventArgs
	{
		public UserInfo data;
		public string message;

		public LoginSuccessModelEventArgs (UserInfo _data, string _message)
		{
			data = _data;
			message = _message;
		}

	}

	public class LoginFailedModelEventArgs : EventArgs
	{
		public string message;
		public int error_code;

		public LoginFailedModelEventArgs (string _message, int _error_code)
		{
			message = _message;
			error_code = _error_code;
		}

	}

	public interface ITacoLoginModelObserver
	{
		void loginSuccess (TacoManageLoginModel model, LoginSuccessModelEventArgs e);

		void loginFailed (TacoManageLoginModel model, LoginFailedModelEventArgs e);
	}

	public class TacoManageLoginModel : ITacoLogin
	{


		#region ITacoLogin implementation

		public void attachResultView (ITacoLoginModelObserver _result)
		{
			this.result_view = _result;
		}

		public void attachLoadingView (ITacoLoadingState _loading)
		{
			loading = _loading;
		}

		public IEnumerator login ()
		{
			if (loading != null)
				loading.showLoading ("Loading...");
			else
				Debug.LogError ("No loading view...loading...");
#if !UNITY_2018

            WWW www = new WWW ("www.google.com");
			yield return www;
			if (loading != null)
				loading.hideLoading ("hideloading!");
			else
				Debug.LogError ("No loading view...finishloading!!");
			if (www.error != null) {
				if (result_view != null)
					result_view.loginFailed (this, new LoginFailedModelEventArgs ("login failed--message=" + www.error, 500));
				else
					Debug.LogError ("No view receive result---email=" + email + ",password=" + password + ",isremember=" + remember_login);
			} else {
				if (result_view != null)
					result_view.loginSuccess (this, new LoginSuccessModelEventArgs (null, "Login sucessfully!"));
				else
					Debug.LogError ("No view receive result---email=" + email + ",password=" + password + ",isremember=" + remember_login);
			}
#else
            UnityWebRequest www = UnityWebRequest.Get("www.google.com");
            yield return www.SendWebRequest();
            if (loading != null)
                loading.hideLoading("hideloading!");
            else
                Debug.LogError("No loading view...finishloading!!");
            if (!www.isNetworkError)
            {
                if (result_view != null)
                    result_view.loginFailed(this, new LoginFailedModelEventArgs("login failed--message=" + www.error, 500));
                else
                    Debug.LogError("No view receive result---email=" + email + ",password=" + password + ",isremember=" + remember_login);
            }
            else
            {
                if (result_view != null)
                    result_view.loginSuccess(this, new LoginSuccessModelEventArgs(null, "Login sucessfully!"));
                else
                    Debug.LogError("No view receive result---email=" + email + ",password=" + password + ",isremember=" + remember_login);
            }
#endif



        }

		public IEnumerator loginWithGoogle ()
		{
			yield return null;
		}

		public void setEmail (string email)
		{
			this.email = email;
		}

		public void setPassword (string password)
		{
			this.password = password;
		}

		public void setRememberLogin (string is_remember_login)
		{
			this.remember_login = is_remember_login;
		}

#endregion

		private ITacoLoginModelObserver result_view;
		private ITacoLoadingState loading;
		private string email, password, remember_login;
	
	}
}
