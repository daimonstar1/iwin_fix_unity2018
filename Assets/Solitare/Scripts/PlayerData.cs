using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerData : MonoBehaviour
{

	/// //////////////////////////
	/// Functions for iOS - Android
	/// //////////////////////////

	#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR

		private JSONObject json = null;
		private string enc_password = "5kWs07js3";

		private void _SaveBinary()
		{
			if(json != null)
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Create(Application.persistentDataPath + "/"+this.localKey+".dat");
				var desEncryption = new DESEncryption();
				string encryptedValue = desEncryption.Encrypt(json.ToString(), enc_password);
				bf.Serialize(file,encryptedValue);
				file.Close();
			}

		}

		private void _LoadBinary()
		{
			if(File.Exists(Application.persistentDataPath + "/"+this.localKey+".dat"))
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(Application.persistentDataPath + "/"+this.localKey+".dat", FileMode.Open);
				string str_json = bf.Deserialize(file) as string;
				var desEncryption = new DESEncryption();
				string decryptedValue;
				if(desEncryption.TryDecrypt(str_json, enc_password, out decryptedValue))
				{
					json = new JSONObject(decryptedValue);

                   // check version and add any new fields, also add them in resetdata
				}
				else
				{
					resetData();
					SavePlayerData();
				}
				file.Close();
			}
			else
			{
				resetData();
				SavePlayerData();

			}
		}

	#endif

	/// /////////////////////////////////
	/// Global functions to all platforms
	/// /////////////////////////////////

	private string localKey = "ST_BinData";
	private static PlayerData instance = null;

	public static PlayerData GetInstance()
	{
		return instance;
	}

	void Awake()
	{
		if(instance == null)
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
			_LoadBinary();
		}
		else if(instance != this)
		{
			Destroy(gameObject);
		}
	}


	public void SetValue(string path, string value)
	{
		#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
			if(json)
			{
				JSONObject.SetValue(json,path,value);
			}
			else
			{
				Debug.Log("PlayerData: json is null");
			}
		#endif


	}

	public string GetValueAsString(string path)
	{
		#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
			if(json)
			{
				return JSONObject.GetValueAsString(json,path);
			}
			else
			{
				Debug.Log("PlayerData: json is null");
				return "";
			}
		#endif


	}

	public int GetValueAsInt(string path)
	{
		#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
			if(json)
			{
				return JSONObject.GetValueAsInt(json,path);
			}
			else
			{
				Debug.Log("PlayerData: json is null");
				return 0;
			}
		#endif


	}

    public void SavePlayerData()
    {
		#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
			_SaveBinary();
		#endif
    }

	public void resetData()
	{
		#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
			json = new JSONObject(JSONObject.Type.OBJECT);
			json.AddField("soundVolume", "1");
			json.AddField("rateStep", "0");
            json.AddField("canShowRate", "1");
            json.AddField("promoStep", "0");
            json.AddField("promoId", "0");
            json.AddField("klondikeHTP", "0");
            json.AddField("pyramidHTP", "0");
            json.AddField("pyramidTips0", "0");
            json.AddField("klondikeTips0", "0");
			json.AddField("version", "1");

			
			/*JSONObject scores = new JSONObject(JSONObject.Type.ARRAY);
			json.AddField("scores", scores);
			JSONObject seenTutorials = new JSONObject(JSONObject.Type.ARRAY);
			json.AddField("scores", seenTutorials);
			JSONObject seenStory = new JSONObject(JSONObject.Type.ARRAY);
			json.AddField("scores", seenStory);
			JSONObject wildCards = new JSONObject(JSONObject.Type.ARRAY);
			json.AddField("scores", wildCards);*/

			

		#endif


	}

	public void printData()
	{
		Debug.Log(json.Print());
	}
}

