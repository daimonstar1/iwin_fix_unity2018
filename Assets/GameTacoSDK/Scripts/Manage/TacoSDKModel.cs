using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTacoSDK
{
	/// <summary>
	/// This interface implements the functions of the Login screen
	/// </summary>
	public interface ITacoSDKLoginModel
	{
		/// <summary>
		///  Attach the result view where display the returned value
		/// </summary>
		void attachResult ();

		/// <summary>
		/// Attach the loading view
		/// </summary>
		void attachLoading ();

		/// <summary>
		/// Implement the login function
		/// </summary>
		IEnumerator login ();

		/// <summary>
		/// Sets the user email.
		/// </summary>
		/// <param name="email">Email.</param>
		void setEmail (string email);

		/// <summary>
		/// Sets the user password.
		/// </summary>
		/// <param name="password">Password.</param>
		void setPassword (string password);

		/// <summary>
		/// Remember this session or not. Set 1 if the user wants to remember this session otherwise set 0
		/// </summary>
		/// <param name="remember_login">Set 1 if the user wants to remember this session otherwise set 0</param>
		void setRememberLogin (string remember_login);

	}

	public class TacoSDKModel:ITacoSDKLoginModel
	{
		#region ITacoSDKLoginModel implementation

		public void attachResult ()
		{
			throw new System.NotImplementedException ();
		}

		public void attachLoading ()
		{
			throw new System.NotImplementedException ();
		}

		public IEnumerator login ()
		{
			throw new System.NotImplementedException ();
		}

		public void setEmail (string email)
		{
			throw new System.NotImplementedException ();
		}

		public void setPassword (string password)
		{
			throw new System.NotImplementedException ();
		}

		public void setRememberLogin (string remember_login)
		{
			throw new System.NotImplementedException ();
		}

		#endregion

	}
}
