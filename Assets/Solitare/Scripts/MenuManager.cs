using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using Facebook.Unity;

public class MenuManager : MonoBehaviour {


	public enum SceneNamesEnum
	{
		MAIN_SCENE = 0,
		SKILLZ_SCENE,
		KLONDIKE_SCENE,
		NUM_SCENE
	}

	public static string[] SceneNames = new string[(int)SceneNamesEnum.NUM_SCENE] {"Main_Scene", "Skillz_Scene", "Klondike_Scene"};

	public enum SoundNamesEnum
	{
		BUTTON_SOUND = 0,
		DIALOG_SOUND,
		TOGGLE_SOUND,
        ERROR_SOUND,
        NEW_CARD_SOUND,
        TAKE_CARD_SOUND,
        WIN_SOUND,
        NEW_THREE_CARD_SOUND,
        DECK_SOUND,
		CARD_TO_FOUNDATION_SOUND,
		SCORE_TALLY_SOUND,
		NUM_SOUND
	}

	private static MenuManager instance = null;
	private float _width;
	private float _height;
	private CanvasScaler canvasScalerComponent;
	private DefaultMenu currentMenu;
	private AudioSource[] soundSources;
	public AudioClip[] audioClips;// = new AudioClip[(int)SoundNamesEnum.NUM_SOUND];

	// This methond find inactive objects too (if using search in children)
	public static GameObject Find(GameObject go, string nameToFind, bool bSearchInChildren)
	{
		if (bSearchInChildren)
		{
			var transform = go.transform;
			var childCount = transform.childCount;
			for (int i = 0; i < childCount; ++i)
			{
				var child = transform.GetChild(i);
				if (child.gameObject.name == nameToFind)
					return child.gameObject;
				GameObject result = MenuManager.Find(child.gameObject, nameToFind, bSearchInChildren);
				if (result != null) return result;
			}
			return null;
		}
		else
		{
			return GameObject.Find(nameToFind);
		}
	}

	public static MenuManager GetInstance()
	{
		return instance;
	}


		
	void Start()
	{

		_width = Screen.width;
		_height = Screen.height;
		OnScreenChange();
	}

	void Awake()
	{
		if(instance == null)
		{
			// Enable 60 fps!! It drains more battery but animations looks much better. Default 30.
			Application.targetFrameRate = 60;
				

            DontDestroyOnLoad(gameObject);
			instance = this;

			soundSources = gameObject.GetComponents<AudioSource> ();

            // Rate
            int rateStep = PlayerData.GetInstance().GetValueAsInt("rateStep");
            bool canShowRate = PlayerData.GetInstance().GetValueAsInt("canShowRate") == 0 ? false : true;

            if(canShowRate)
            {
                rateStep++;
                if(rateStep == 3)
                {
                    MainMenu.mustShowRate = true;
                    rateStep = 0;
                }

                PlayerData.GetInstance().SetValue("rateStep",rateStep.ToString());
            }

            // Promo
            int promoStep = PlayerData.GetInstance().GetValueAsInt("promoStep");
            int promoId = PlayerData.GetInstance().GetValueAsInt("promoId");

            promoStep++;
            if (promoStep == 2)
            {
                if (!MainMenu.mustShowRate)
                {
                    MainMenu.mustShowPromo = true;
                    MainMenu.promoId = promoId;
                
                    promoId++;
                    if (promoId == 3)
                    {
                        promoId = 0;
                    }
                }
                promoStep = 0;
            }

            PlayerData.GetInstance().SetValue("promoStep",promoStep.ToString());
            PlayerData.GetInstance().SetValue("promoId", promoId.ToString());

            PlayerData.GetInstance().SavePlayerData();

           // FB.Init(this.OnInitComplete, this.OnHideUnity);
            //Debug.Log("FB.Init() called with " + FB.AppId);

			_InitCanvas();
		}
		else if(instance != this)
		{
			instance._InitCanvas();
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (_width != Screen.width || _height != Screen.height)
		{
			_width = Screen.width;
			_height = Screen.height;
			OnScreenChange();
		}
	}

	private void OnScreenChange()
	{
		//Debug.Log("onScreenChage() " + _width + "x" + _height);

		if(_width > _height)
		{
			canvasScalerComponent.matchWidthOrHeight = 0.3f;
		}
		else
		{
			canvasScalerComponent.matchWidthOrHeight = 0.0f;
		}

		if(currentMenu)
		{
			currentMenu.OnScreenChange(_width,_height);
		}
	}

	private void _InitCanvas()
	{
		// If needed, change scale mode.
		GameObject canvasGameObject = GameObject.Find("Canvas");
		currentMenu = canvasGameObject.GetComponent<DefaultMenu>() as DefaultMenu;
		canvasScalerComponent = canvasGameObject.GetComponent<CanvasScaler>();
		
		Button[] buttons = GameObject.FindObjectsOfType<Button>();
		for (int x = 0; x < buttons.Length ; x++)
		{
			Button bt = buttons[x];
			// Very important call from GetInstance() for get the reference.
			buttons[x].onClick.AddListener(delegate() { OnButtonClick(bt); });
		}

		Toggle[] toggles = GameObject.FindObjectsOfType<Toggle>();
		for(int x = 0; x < toggles.Length ; x++)
		{
			Toggle tg = toggles[x];
			tg.onValueChanged.AddListener(delegate(bool val) { OnToggleChange(tg); });
		}
	}

	// Call functions below after get the instance: MenuManager.GetInstance().functionName() or from other functions of this class.

	public void PlaySound(SoundNamesEnum sn, bool loop = false, int channel = -1)
	{
		AudioClip clip = audioClips[(int)sn];

		if (channel == -1)
		{
			bool played = false;

			for (int x = 0; x < soundSources.Length; x++)
			{
				if (!soundSources [x].isPlaying) {
					soundSources [x].clip = clip;
					soundSources [x].loop = loop;
					soundSources [x].Play ();
					played = true;
					break;
				}
			}

			if (!played)
			{
				soundSources [0].clip = clip;
				soundSources [0].loop = loop;
				soundSources [0].Play ();
			}
		}
		else
		{
			soundSources [channel].clip = clip;
			soundSources [channel].loop = loop;
			soundSources [channel].Play ();
		}

	}

    public float GetClipLength(SoundNamesEnum sn)
    {
        AudioClip clip = audioClips[(int)sn];
        return clip.length;
    }

    public void StopAll()
    {
		for (int x = 0; x < soundSources.Length; x++)
		{
			soundSources [x].Stop ();
		}
    }

	public void StopSound(int channel)
	{
		soundSources [channel].Stop ();
	}

	public void MuteAll(bool m)
	{
		for (int x = 0; x < soundSources.Length; x++)
		{
			soundSources [x].mute = m;
		}
	}

	private void OnButtonClick(Button bt)
	{
		if(currentMenu)
		{
			currentMenu.OnButtonClick(bt);
		}
	}

	private void OnToggleChange(Toggle tg)
	{
		if(currentMenu)
		{
			currentMenu.OnToggleChange(tg);
		}
	}

    private void OnHideUnity(bool isGameShown)
    {
        Debug.Log("Is game shown: " + isGameShown);
    }
        

	void OnApplicationPause (bool pauseStatus)
	{
		
	}
}
