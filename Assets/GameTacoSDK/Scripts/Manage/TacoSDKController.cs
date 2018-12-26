using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTacoSDK
{
	/// <summary>
	/// This interface define all functions which used in TacoSDK
	/// </summary>
	public interface ITacoSDKController
	{
		IEnumerator login ();

		IEnumerator loginWithGoogle ();
	}

	public class TacoSDKController : ITacoSDKController
	{

		public TacoSDKController ()
		{
	
		}



		#region ITacoSDKController implementation

		public IEnumerator login ()
		{
			return null;
		}

		public IEnumerator loginWithGoogle ()
		{
			return null;
		}

		#endregion
	
	}
}
