using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

//using Facebook.Unity;
using System;

public class MainMenu : DefaultMenu
{

  // Use this for initialization

  private GameObject promoDialog_image;
  private CanvasGroup promoDialog_cg;

  private GameObject dialog_image;
  private GameObject ratedialog_image;
  private CanvasGroup dialog_cg;
  private CanvasGroup ratedialog_cg;
  private CanvasGroup panel_cg;
  private GameObject panel;
  private GameObject[] promoIcons_button;
  private Animator[] promoIcons_animator;
  private HoldTest[] promoIcons_ht;

  private bool fadeUp = false;
  private float fadeSpeed = 1.0f;
  private float maxAlpha = 0.65f;
  private bool fadeDown = false;
  private Animator moveAnimator;

  private float maxPromoButtonDelay = 1.0f;
  private float maxPromoDelay = 1.0f;
  private float currentPromoDelay = 0.0f;
  private float currentPromoButtonDelay = 0.0f;
  private int btIndex = 0;

  private bool rateFade = false;
  private bool initializingSoundToggle = true;

  static public bool mustShowRate = false;
  static public bool mustShowPromo = false;
  private bool promoFade = false;
  static public int promoId = 0;

  private string appStore_URL = "https://itunes.apple.com/us/app/solitaire-for-cash/id1089262361?mt=8";
  private string facebook_URL = "https://www.facebook.com/iwingames/";
  private string JQFC_URL = "https://control.kochava.com/v1/cpi/click?campaign_id=kocom-iwin-jewelquest-skillz-ios-skillz-1907-wqzfqx040965df8e521&network_id=3882&device_id=device_id&site_id=1";
  private string BTFC_URL = "https://control.kochava.com/v1/cpi/click?campaign_id=kocom-iwin-bubbletown-skillz-ios-skillz-1969-8hhb45p205b6de41bd28&network_id=3882&device_id=device_id&site_id=1";
  private string CT_URL = "https://control.kochava.com/v1/cpi/click?campaign_id=kocom-freshgames-cubisskill-ios-skillz-742540dfa4271c170e6a375078119&network_id=3882&device_id=device_id&site_id=1";

  void Awake ()
  {
    GameTacoDelegate.Instance.Init ();
    GameTaco.TacoSetup.Instance.TournamentStarted += OnPlayTournamentClicked;
    GameTaco.TacoSetup.Instance.ToggleTacoHeaderFooter (true);
  }

  void Start ()
  {
    currentPromoButtonDelay = maxPromoButtonDelay;

    panel = GameObject.Find ("Black_Panel");
    panel_cg = panel.GetComponent<CanvasGroup> ();
    panel_cg.interactable = false;
    panel_cg.blocksRaycasts = false;
    Color c = Color.black;
    c.a = 0.0f;
    panel.GetComponent<Image> ().color = c;

    dialog_image = MenuManager.Find (gameObject, "Dialog_Image", true);
    dialog_cg = dialog_image.GetComponent<CanvasGroup> ();
    moveAnimator = dialog_image.GetComponent<Animator> ();
    dialog_cg.interactable = false;
    dialog_image.SetActive (false);
    //dialog_cg.alpha = 0.0f;

    ratedialog_image = MenuManager.Find (gameObject, "RateDialog_Image", true);
    ratedialog_cg = ratedialog_image.GetComponent<CanvasGroup> ();
    ratedialog_cg.interactable = false;
    ratedialog_image.SetActive (false);
    ratedialog_cg.alpha = 1.0f;

    promoDialog_image = MenuManager.Find (gameObject, "PromoDialog_Image", true);
    promoDialog_cg = promoDialog_image.GetComponent<CanvasGroup> ();
    promoDialog_cg.interactable = false;
    promoDialog_image.SetActive (false);
    promoDialog_cg.alpha = 1.0f;

    promoIcons_button = new GameObject[3];
    promoIcons_animator = new Animator[3];
    promoIcons_ht = new HoldTest[3];

    promoIcons_button [0] = MenuManager.Find (gameObject, "PromoIconJQFC_Button", true);
    promoIcons_animator [0] = promoIcons_button [0].GetComponent<Animator> ();
    promoIcons_ht [0] = promoIcons_button [0].GetComponent<HoldTest> ();

    promoIcons_button [1] = MenuManager.Find (gameObject, "PromoIconCFC_Button", true);
    promoIcons_animator [1] = promoIcons_button [1].GetComponent<Animator> ();
    promoIcons_ht [1] = promoIcons_button [1].GetComponent<HoldTest> ();

    promoIcons_button [2] = MenuManager.Find (gameObject, "PromoIconBTFC_Button", true);
    promoIcons_animator [2] = promoIcons_button [2].GetComponent<Animator> ();
    promoIcons_ht [2] = promoIcons_button [2].GetComponent<HoldTest> ();

    GameObject st = MenuManager.Find (gameObject, "Sound_Toggle", true);
    Toggle t = st.GetComponent<Toggle> ();
    int val = PlayerData.GetInstance ().GetValueAsInt ("soundVolume");
    t.isOn = val == 0 ? false : true;
    MenuManager.GetInstance ().MuteAll (!t.isOn);

    GameObject vto = MenuManager.Find (gameObject, "Version_Text", true);
    Text vt = vto.GetComponent<Text> ();
    vt.text = "v " + Application.version;

   
  }

  // Update is called once per frame
  void Update ()
  {
    if (rateFade && ratedialog_cg.alpha < 1.0F) {
      ratedialog_cg.alpha += fadeSpeed * Time.deltaTime;

      if (ratedialog_cg.alpha > 1.0F) {
        ratedialog_cg.alpha = 1.0F;
      }
    } else if (!rateFade && ratedialog_cg.alpha > 0.0F) {
      ratedialog_cg.alpha -= fadeSpeed * Time.deltaTime;

      if (ratedialog_cg.alpha < 0.0F) {
        ratedialog_cg.alpha = 0.0F;
      }
    }

    if (promoFade && promoDialog_cg.alpha < 1.0F) {
      promoDialog_cg.alpha += fadeSpeed * Time.deltaTime;

      if (promoDialog_cg.alpha > 1.0F) {
        promoDialog_cg.alpha = 1.0F;
      }
    } else if (!promoFade && promoDialog_cg.alpha > 0.0F) {
      promoDialog_cg.alpha -= fadeSpeed * Time.deltaTime;

      if (promoDialog_cg.alpha < 0.0F) {
        promoDialog_cg.alpha = 0.0F;
      }
    }

    if (fadeUp) {
      Image img = panel.GetComponent<Image> ();
      Color c = img.color;

      c.a += fadeSpeed * Time.deltaTime;

      if (c.a > maxAlpha) {
        c.a = maxAlpha;
        fadeUp = false;
      }

      img.color = c;
    } else if (fadeDown) {
      Image img = panel.GetComponent<Image> ();
      Color c = img.color;

      c.a -= fadeSpeed * Time.deltaTime;

      if (c.a < 0.0f) {
        c.a = 0.0f;
        fadeDown = false;
      }

      img.color = c;
    }

    bool anyPromoPressed = false;
    for (int x = 0; x < 3; x++) {
      if (promoIcons_ht [x].isPressed ()) {
        anyPromoPressed = true;
        break;
      }
    }

    if (currentPromoDelay > 0) {
      currentPromoDelay -= Time.deltaTime;
    }

    if (anyPromoPressed == false && currentPromoDelay <= 0 && currentPromoButtonDelay > 0) {
      currentPromoButtonDelay -= Time.deltaTime;

      if (currentPromoButtonDelay < 0) {
        for (int x = 0; x < 3; x++) {
          if (promoIcons_animator [x].transform.parent.gameObject.activeSelf)
            promoIcons_animator [x].SetBool ("canAnimate", false);
        }

        btIndex++;
        if (btIndex >= 3) {
          btIndex = 0;

        }

        if (btIndex == 2) {
          currentPromoDelay = maxPromoDelay;
        }
        if (promoIcons_animator [btIndex].transform.parent != null && promoIcons_animator [btIndex].transform.parent.gameObject.activeSelf) {
          promoIcons_animator [btIndex].SetBool ("canAnimate", true);
          promoIcons_animator [btIndex].SetTrigger ("Normal");
        }

        currentPromoButtonDelay = maxPromoButtonDelay;
      }
    }
  }

  private void ClosePopup ()
  {
    fadeDown = true;

    panel_cg.interactable = false;
    panel_cg.blocksRaycasts = false;

    if (dialog_cg.interactable) {
      MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.DIALOG_SOUND);
      dialog_image.GetComponent<selfDeactivate> ().deactivate = true;
    }
    dialog_cg.interactable = false;
    moveAnimator.SetTrigger ("OutState");
    //dialog_image.SetActive(false);

    ratedialog_cg.interactable = false;
    ratedialog_image.SetActive (false);
    rateFade = false;

    promoDialog_cg.interactable = false;
    promoDialog_image.SetActive (false);
    promoFade = false;
  }

  private void OpenPopup (int id)
  {
    fadeUp = true;
    panel_cg.interactable = true;
    panel_cg.blocksRaycasts = true;
    Color c = Color.black;
    c.a = 0.0f;
    panel.GetComponent<Image> ().color = c;

    switch (id) {
    case 0:
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.DIALOG_SOUND);

        dialog_image.SetActive (true);
        dialog_cg.interactable = true;
        dialog_cg.alpha = 1.0f;

        moveAnimator.SetTrigger ("InState");
        break;
      }
    case 1:
      {
        ratedialog_image.SetActive (true);
        ratedialog_cg.interactable = true;
        rateFade = true;
        break;
      }
    case 2:
      {
        string name = "";
        switch (promoId) {
        case 0:
          {
            name = "4.3_BTPromoPopup_P";
            break;
          }
        case 1:
          {
            name = "4.3_CFPromoPopup_P";
            break;
          }
        case 2:
          {
            name = "4.3_JQPromoPopup_P";
            break;
          }
        default:
          {
            name = "4.3_BTPromoPopup_P";
            break;
          }
        }
        ReplaceSpriteInAtlas ("PromoDialog_Image", "Sprites/4.3_ui3", name);
        promoDialog_image.SetActive (true);
        promoDialog_cg.interactable = true;
        promoFade = true;
        break;
      }
    default:
      {
        break;
      }
    }
  }

  public override void OnScreenChange (float w, float h)
  {
    if (w > h) {
     
    } else {
      float ar = Util.Round (h / w, 2);
      bool is43 = ar <= Util.Round (4.0f / 3.0f, 2);

      if (is43) {
        // === Portrait 4:3 ===
        ReplaceSprite ("Background_Image", "Sprites/4.3_portrait_bg");

      } else {
        // === Portrait 16:9 ===
        ReplaceSprite ("Background_Image", "Sprites/16.9_portrait_bg");
      }
    }
  }

  private IEnumerator LaunchSkillz ()
  {
    GameTaco.TacoSetup.Instance.ToggleTacoHeaderFooter (false);
    yield return new WaitForSeconds (MenuManager.GetInstance ().GetClipLength (MenuManager.SoundNamesEnum.BUTTON_SOUND));
    MenuManager.GetInstance ().StopAll ();

    //#if UNITY_EDITOR

    if (Util.GetRandomBetween0And (1) == 0) {
      // Test for editor
      //Debug.Log(SceneManager.sceneCountInBuildSettings-(int)MenuManager.SceneNamesEnum.NUM_SCENE);
      int pyramidSceneCount = SceneManager.sceneCountInBuildSettings - (int)MenuManager.SceneNamesEnum.NUM_SCENE - 1;
      int level = Util.GetRandomBetween0And (pyramidSceneCount - 1) + 1;

      SceneManager.LoadScene ("Pyramid" + level + "_Scene");
    } else {
      SceneManager.LoadScene (MenuManager.SceneNames [(int)MenuManager.SceneNamesEnum.KLONDIKE_SCENE]);
    }

    
  }

  public override void OnToggleChange (Toggle tg)
  {
    switch (tg.name) {
    case "Sound_Toggle":
      {
        string val = tg.isOn ? "1" : "0";
        PlayerData.GetInstance ().SetValue ("soundVolume", val);
        PlayerData.GetInstance ().SavePlayerData ();

        MenuManager.GetInstance ().MuteAll (!tg.isOn);
        if (tg.isOn && !initializingSoundToggle) {
          MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.TOGGLE_SOUND);
        }

        initializingSoundToggle = false;

        break;
      }
    default:
      {
        break;
      }
    }
  }

  public override void OnButtonClick (Button bt)
  {
    MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.BUTTON_SOUND);

    switch (bt.name) {
    case "Play_Button":
      {
        GameTaco.TacoSetup.Instance.StartNormalGame ();
        StartCoroutine (LaunchSkillz ());
        break;
      }
    case "Config_Button":
      {
        OpenPopup (0);
        break;
      }
    case "Close_Button":
      {
        ClosePopup ();
        break;
      }
    case "RateClose_Button":
      {
        ClosePopup ();
        break;
      }
    case "RateNow_Button":
      {
        Application.OpenURL (appStore_URL);
        ClosePopup ();
        PlayerData.GetInstance ().SetValue ("canShowRate", "0");
        PlayerData.GetInstance ().SavePlayerData ();
        break;
      }
    case "PromoClose_Button":
      {
        ClosePopup ();
        break;
      }
    case "PromoNow_Button":
      {
        switch (promoId) {
        case 0:
          {
            Application.OpenURL (BTFC_URL);
            break;
          }
        case 1:
          {
            Application.OpenURL (CT_URL);
            break;
          }
        case 2:
          {
            Application.OpenURL (JQFC_URL);
            break;
          }
        default:
          {
            break;
          }
        }

        ClosePopup ();

        break;
      }
    case "FBLike2_Button":
      {
        Application.OpenURL (facebook_URL);
        break;
      }
    case "FBInvite2_Button":
      {
        
        break;
      }
    case "FBLike_Button":
      {
        Application.OpenURL (facebook_URL);
        break;
      }
    case "FBInvite_Button":
      {
       
        break;
      }
    case "PromoIconCFC_Button":
      {
        Application.OpenURL (CT_URL);
        break;
      }
    case "PromoIconJQFC_Button":
      {
        Application.OpenURL (JQFC_URL);
        break;
      }
    case "PromoIconBTFC_Button":
      {
        Application.OpenURL (BTFC_URL);
        break;
      }
    default:
      {
        break;
      }
    }
  }


  public void OnSignUpClicked ()
  {
    GameTaco.TacoSetup.Instance.OpenRegisterPanel ();
  }

  public void OnSignInClicked ()
  {
    GameTaco.TacoSetup.Instance.OpenLoginPanel ();
  }

  public void OnHowToPlayTaco ()
  {
    GameTaco.TacoSetup.Instance.OpenHowToPlayPanel ();
  }

  public void OnOpenCashTournaments ()
  {
    GameTaco.TacoSetup.Instance.OpenCashTournament ();
  }

  public void OnOpenTacoTournaments ()
  {
    GameTaco.TacoSetup.Instance.OpenTacoTournament ();
  }

  private void OnDestroy ()
  {
    GameTaco.TacoSetup.Instance.TournamentStarted -= OnPlayTournamentClicked;
  }

  public void OnPlayTournamentClicked ()
  {
    StartCoroutine (LaunchSkillz ());
  }
}
