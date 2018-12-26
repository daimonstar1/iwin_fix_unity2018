using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KlondikeMenu : DefaultMenu
{

  public static KlondikeMenu instance = null;
//this script

  private GameObject panel;
  private CanvasGroup panel_cg;

  private Animator confirmationDialog_moveAnimator;
  private GameObject confirmationDialog_image;
  private CanvasGroup confirmationDialog_cg;

  private Animator pauseDialog_moveAnimator;
  private GameObject pauseDialog_image;
  public CanvasGroup pauseDialog_cg;

  private Animator dialog_moveAnimator;
  private GameObject dialog_image;
  private CanvasGroup dialog_cg;

  public Animator endDialog_moveAnimator;
  private GameObject endDialog_image;
  private CanvasGroup endDialog_cg;

  public Animator tutorialDialog_moveAnimator;
  private GameObject tutorialDialog_image;
  private CanvasGroup tutorialDialog_cg;

  public Animator tipDialog_moveAnimator;
  private GameObject tipDialog_image;
  private CanvasGroup tipDialog_cg;


  private bool fadeUp = false;
  private float fadeSpeed = 1.0f;
  private float maxAlpha = 0.45f;
  private bool fadeDown = false;
  private bool initializingSoundToggle = true;

  public static KlondikeMenu GetInstance ()
  {
    return instance;
  }

  void Awake ()
  {
    panel = GameObject.Find ("Black_Panel");
    panel_cg = panel.GetComponent<CanvasGroup> ();
    panel_cg.interactable = false;
    panel_cg.blocksRaycasts = false;
    Color c = Color.black;
    c.a = 0.0f;
    panel.GetComponent<Image> ().color = c;

    GameObject st = MenuManager.Find (gameObject, "Sound_Toggle", true);
    Toggle t = st.GetComponent<Toggle> ();
    int val = PlayerData.GetInstance ().GetValueAsInt ("soundVolume");
    t.isOn = val == 0 ? false : true;
    MenuManager.GetInstance ().MuteAll (!t.isOn);

    dialog_image = MenuManager.Find (gameObject, "Dialog_Image", true);
    dialog_cg = dialog_image.GetComponent<CanvasGroup> ();
    dialog_moveAnimator = dialog_image.GetComponent<Animator> ();
    dialog_cg.interactable = false;
    dialog_image.SetActive (false);

    confirmationDialog_image = MenuManager.Find (gameObject, "Confirmation_Image", true);
    confirmationDialog_cg = confirmationDialog_image.GetComponent<CanvasGroup> ();
    confirmationDialog_moveAnimator = confirmationDialog_image.GetComponent<Animator> ();
    confirmationDialog_cg.interactable = false;
    confirmationDialog_image.SetActive (false);

    pauseDialog_image = MenuManager.Find (gameObject, "Pause_Image", true);
    pauseDialog_cg = pauseDialog_image.GetComponent<CanvasGroup> ();
    pauseDialog_moveAnimator = pauseDialog_image.GetComponent<Animator> ();
    pauseDialog_cg.interactable = false;
    pauseDialog_image.SetActive (false);

    endDialog_image = MenuManager.Find (gameObject, "GameOver_Image", true);
    endDialog_cg = endDialog_image.GetComponent<CanvasGroup> ();
    endDialog_moveAnimator = endDialog_image.GetComponent<Animator> ();
    endDialog_cg.interactable = false;
    endDialog_image.SetActive (false);

    tutorialDialog_image = MenuManager.Find (gameObject, "HowToPlay_Image", true);
    tutorialDialog_cg = tutorialDialog_image.GetComponent<CanvasGroup> ();
    tutorialDialog_moveAnimator = tutorialDialog_image.GetComponent<Animator> ();
    tutorialDialog_cg.interactable = false;
    tutorialDialog_image.SetActive (false);

    tipDialog_image = MenuManager.Find (gameObject, "Tip_Image", true);
    tipDialog_cg = tipDialog_image.GetComponent<CanvasGroup> ();
    tipDialog_moveAnimator = tipDialog_image.GetComponent<Animator> ();
    tipDialog_cg.interactable = false;
    tipDialog_image.SetActive (false);
  }

  // Use this for initialization
  void Start ()
  {
    instance = this;
  }

  // Update is called once per frame
  void Update ()
  {
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
  }

  public void OpenPopup (int p)
  {
    // Show black background
    if (p == 0 || p == 1 || p == 2 || p == 4 || p == 5) {
      if (!panel_cg.blocksRaycasts) {
        fadeUp = true;
        panel_cg.interactable = true;
        panel_cg.blocksRaycasts = true;
        Color c = Color.black;
        c.a = 0.0f;
        panel.GetComponent<Image> ().color = c;
      } else {
        panel_cg.interactable = true;
        panel_cg.blocksRaycasts = true;
      }

      kdeck.GetInstance ().player_can_move = false;
    }

    switch (p) {
    //Options
    case 0:
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.DIALOG_SOUND);

        dialog_image.SetActive (true);
        dialog_cg.interactable = true;
        dialog_cg.alpha = 1.0f;

        dialog_moveAnimator.SetTrigger ("InState");
        break;
      }
    // Game over
    case 1:
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.DIALOG_SOUND);

        endDialog_image.SetActive (true);
        endDialog_cg.interactable = true;
        endDialog_cg.alpha = 1.0f;

        endDialog_moveAnimator.SetTrigger ("InState");
        break;
      }
    case 2:
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.DIALOG_SOUND);

        tutorialDialog_image.SetActive (true);
        tutorialDialog_cg.interactable = true;
        tutorialDialog_cg.alpha = 1.0f;

        tutorialDialog_moveAnimator.SetTrigger ("InState");
        //tutorialDialog_moveAnimator.SetBool("TutoAnim", true);

        break;
      }
    // Tip
    case 3:
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.DIALOG_SOUND);
        tipDialog_image.SetActive (true);
        tipDialog_cg.interactable = true;
        tipDialog_cg.alpha = 1.0f;

        tipDialog_moveAnimator.SetTrigger ("InState");
        kdeck.GetInstance ().player_can_move = false;
        kdeck.GetInstance ().timer_paused = true;
        panel_cg.interactable = true;
        panel_cg.blocksRaycasts = true;
        break;
      }
    // PauseDialog
    case 4:
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.DIALOG_SOUND);

        pauseDialog_image.SetActive (true);
        pauseDialog_cg.interactable = true;
        pauseDialog_cg.alpha = 1.0f;

        pauseDialog_moveAnimator.SetTrigger ("InState");
        break;
      }
    case 5: // Confirmation dialog
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.DIALOG_SOUND);

        confirmationDialog_image.SetActive (true);
        confirmationDialog_cg.interactable = true;
        confirmationDialog_cg.alpha = 1.0f;

        confirmationDialog_moveAnimator.SetTrigger ("InState");
        break;
      }
    default:
      {
        break;
      }
    }
  }

  public void OpenBlackFade ()
  {
    //deck.this_deck.enableCards(true);
    kdeck.GetInstance ().player_can_move = false;

    fadeUp = true;
    fadeDown = false;
    panel_cg.interactable = false;
    panel_cg.blocksRaycasts = true;
    /*Color c = Color.black;
		c.a = 0.0f;
		panel.GetComponent<Image>().color = c;*/

  }

  public void ClosePopups ()
  {
    fadeDown = true;

    panel_cg.interactable = false;
    panel_cg.blocksRaycasts = false;


    kdeck.GetInstance ().CheckPlayerCanMove ();

    if (dialog_cg.interactable) {
      MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.DIALOG_SOUND);
      dialog_image.GetComponent<selfDeactivate> ().deactivate = true;
    }
    dialog_cg.interactable = false;
    if (dialog_moveAnimator.gameObject.activeSelf)
      dialog_moveAnimator.SetTrigger ("OutState");
    //dialog_image.SetActive(false);

    if (pauseDialog_cg.interactable) {
      MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.DIALOG_SOUND);
      pauseDialog_image.GetComponent<selfDeactivate> ().deactivate = true;
    }
    pauseDialog_cg.interactable = false;
    if (pauseDialog_moveAnimator.gameObject.activeSelf)
      pauseDialog_moveAnimator.SetTrigger ("OutState");

    if (confirmationDialog_cg.interactable) {
      MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.DIALOG_SOUND);
      confirmationDialog_image.GetComponent<selfDeactivate> ().deactivate = true;
    }
    confirmationDialog_cg.interactable = false;
    if (confirmationDialog_moveAnimator.gameObject.activeSelf)
      confirmationDialog_moveAnimator.SetTrigger ("OutState");

    if (endDialog_cg.interactable) {
      MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.DIALOG_SOUND);
      endDialog_image.GetComponent<selfDeactivate> ().deactivate = true;
    }
    endDialog_cg.interactable = false;
    if (endDialog_moveAnimator.gameObject.activeSelf)
      endDialog_moveAnimator.SetTrigger ("OutState");

    if (tutorialDialog_cg.interactable) {
      MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.DIALOG_SOUND);
      tutorialDialog_image.GetComponent<selfDeactivate> ().deactivate = true;
    }
    tutorialDialog_cg.interactable = false;
    if (tutorialDialog_moveAnimator.gameObject.activeSelf)
      tutorialDialog_moveAnimator.SetTrigger ("OutState");

    if (tipDialog_cg.interactable) {
      MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.DIALOG_SOUND);
      tipDialog_image.GetComponent<selfDeactivate> ().deactivate = true;
    }
    tipDialog_cg.interactable = false;
    if (tipDialog_moveAnimator.gameObject.activeSelf)
      tipDialog_moveAnimator.SetTrigger ("OutState");
  }


  public override void OnButtonClick (Button bt)
  {
    switch (bt.name) {
    case "Undo_Button":
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.TOGGLE_SOUND);
        //bt.gameObject.SetActive(false);
        kdeck.GetInstance ().UndoLastMove ();


        break;
      }
    case "GPEnd_Button":
    case "End_Button":
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.BUTTON_SOUND);
        if (GameTaco.TacoSetup.Instance.IsTournamentPlayed ()) {
          GameTaco.TacoSetup.Instance.TacoOpenEndPlayGame ();
        } else {
          OpenPopup (5);
        }

        break;
      }
    case "Config_Button":
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.BUTTON_SOUND);
        OpenPopup (0);
        break;
      }
    case "Close_Button":
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.BUTTON_SOUND);
        ClosePopups ();
        break;
      }
    case "CloseTuto_Button":
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.BUTTON_SOUND);
        ClosePopups ();
        StartCoroutine (kdeck.GetInstance ().StartGame (tutorialDialog_moveAnimator.GetCurrentAnimatorClipInfo (0).Length));
        break;
      }
    case "DontShowAgain_Button":
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.BUTTON_SOUND);
        PlayerData.GetInstance ().SetValue ("klondikeHTP", "1");
        PlayerData.GetInstance ().SavePlayerData ();
        ClosePopups ();
        StartCoroutine (kdeck.GetInstance ().StartGame (tutorialDialog_moveAnimator.GetCurrentAnimatorClipInfo (0).Length));
        break;
      }
    case "CloseTip_Button":
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.BUTTON_SOUND);
        PlayerData.GetInstance ().SetValue ("klondikeTips0", "1");
        PlayerData.GetInstance ().SavePlayerData ();
        kdeck.GetInstance ().timer_paused = false;
        ClosePopups ();
        break;
      }
    case "PauseResume_Button":
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.BUTTON_SOUND);
        kdeck.GetInstance ().timer_paused = false;
        ClosePopups ();

        break;
      }
    case "Confirmation_Button":
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.BUTTON_SOUND);
        StartCoroutine (kdeck.GetInstance ().GameOver (2));
        break;
      }
    case "CloseConfirmation_Button":
      {
        MenuManager.GetInstance ().PlaySound (MenuManager.SoundNamesEnum.BUTTON_SOUND);
        ClosePopups ();
        break;
      }
    default:
      {
        break;
      }
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

  public override void OnScreenChange (float w, float h)
  {
    if (w > h) {
      /*float ar = Util.Round(w / h,2);
			bool is43 = ar <= Util.Round(4.0f/3.0f, 2);

			if( is43 )
			{
					// === Landscape 4:3 ===
					replaceSprite("Background_Image", "Sprites/4.3_landscape_bg");
			}
			else
			{
					// === Landscape 16:9 ===
					replaceSprite("Background_Image", "Sprites/16.9_landscape_bg");
			}*/
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

  void OnApplicationPause (bool pauseStatus)
  {
    if (pauseStatus && pauseDialog_cg.interactable == false && tutorialDialog_cg.interactable == false && !kdeck.GetInstance ().game_end) {
      kdeck.GetInstance ().timer_paused = true;
      OpenPopup (4);
    }
  }
}
