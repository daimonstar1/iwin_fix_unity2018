using UnityEngine;
using System.Collections;
using System;

public class Util : MonoBehaviour {

	public static int GetRandomBetween0And(int max) {
		//if (SkillzSDK.Api.IsTournamentInProgress)
		if (Application.platform == RuntimePlatform.IPhonePlayer) {

			//return Mod(Mathf.Abs(SkillzSDK.Api.GetRandomNumber()), max+1);
			return UnityEngine.Random.Range(0, max + 1);
		}
		else {
			//UnityEngine.Random.InitState (1234);
			return UnityEngine.Random.Range(0, max + 1);
		}
	}

	public static float Round(float value, int digits) {
		float mult = Mathf.Pow(10.0f, (float)digits);
		return Mathf.Round(value * mult) / mult;
	}
}
