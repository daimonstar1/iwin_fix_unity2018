using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameTacoSDK
{
	public class TacoUIButton : Button
	{
		protected override void Start ()
		{
			base.Start ();
			onClick.AddListener (() => _onClick ());
		}

		public void _onClick ()
		{
			Debug.LogError ("auto click");
		}
	}
}
