using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

#if UNITY_IOS || UNITY_ANDROID
//using Facebook.Unity;
#endif
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Security.Cryptography;

namespace GameTaco
{
  public class LoginPanel : MonoBehaviour
  {
    public static LoginPanel Instance;

    public GameObject EmailInput = null;
    public GameObject PasswordInput = null;
    //public GameObject TacoStatusText = null;

    public GameObject AutoLoginToggle = null;
    public GameObject FacebookBtn = null;
    public GameObject GoogleBtn = null;
    public InputField resetAccount;
    public InputField ReferenceCodeInput;
    public Button submitResetBtn;
    public GameObject CodeDisplayPanel;
    public Button copyCode;
    public Button goToGooglePage;
    private GoogleDeviceInfo deviceInfo;

    // client configuration
    const string clientID = "701274653877-p7gh14u6k37hqrob520oh172fq4856m9.apps.googleusercontent.com";
    const string clientSecret = "fsE-_7GJN-XmdLkszyikuWMO";
    //const string authorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
    const string authorizationDeviceEndpoint = "https://accounts.google.com/o/oauth2/device/code";
    const string tokenEndpoint = "https://www.googleapis.com/oauth2/v4/token";
    const string userInfoEndpoint = "https://www.googleapis.com/oauth2/v3/userinfo";

    void Awake ()
    {
      Instance = this;
#if UNITY_IOS_ || UNITY_ANDROID_
			if (!FB.IsInitialized)
			{
				// Initialize the Facebook SDK
				//FB.Init(InitCallback, OnHideUnity);
			}
			else
			{
				// Already initialized, signal an app activation App Event
				//FB.ActivateApp();
			}

#endif
      Init ();

      copyCode.onClick.AddListener (() => {
        CodeDisplayPanel.transform.Find ("Main/Code").GetComponent<InputField> ().text = deviceInfo.user_code;
      });

      goToGooglePage.onClick.AddListener (() => {
        TacoManager.OpenMessage (TacoConfig.Processing);
        CodeDisplayPanel.SetActive (false);
        Application.OpenURL (deviceInfo.verification_url);
        StartCoroutine (PollRequestToGoogle (deviceInfo));
      });

      submitResetBtn.interactable = false;
      resetAccount.onValueChanged.AddListener ((string value) => {
        submitResetBtn.interactable = !string.IsNullOrEmpty (value);
      });

    }

    // Use this for initialization
    public void Init ()
    {
#if UNITY_IOS || UNITY_ANDROID
      //FacebookBtn.SetActive(false);
      //GoogleBtn.SetActive(false);
#else
			//FacebookBtn.SetActive (false);
			//var googleBtnRect = GoogleBtn.GetComponent (typeof(RectTransform)) as RectTransform;
			//var facebookBtnRect = FacebookBtn.GetComponent (typeof(RectTransform)) as RectTransform;
			//googleBtnRect.sizeDelta = new Vector2 (772, 93);

			//googleBtnRect.anchoredPosition3D = new Vector3 (400f, -660f, 0);
#endif


      //TacoStatusText.GetComponent<Text> ().text = TacoConfig.TacoLoginMessage;

      PasswordInput.SetActive (true);
      EmailInput.SetActive (true);

      PasswordInput.GetComponent<InputField> ().placeholder.GetComponent<Text> ().text = TacoConfig.TacoLoginPassword;
      EmailInput.GetComponent<InputField> ().placeholder.GetComponent<Text> ().text = TacoConfig.TacoLoginEmail;

      PasswordInput.GetComponent<InputField> ().text = string.Empty;
      EmailInput.GetComponent<InputField> ().text = string.Empty;
    }
    #if UNITY_IOS || UNITY_ANDROID
    private void OnHideUnity (bool isGameShown)
    {
      if (!isGameShown) {
        // Pause the game - we will need to hide
        Time.timeScale = 0;
      } else {
        // Resume the game - we're getting focus again
        Time.timeScale = 1;
      }
    }
    #endif
    // Update is called once per frame
    void Update ()
    {
      if (this.isActiveAndEnabled & !TacoManager.CheckModalsOpen ()) {
        if (Input.GetKeyDown (KeyCode.Tab)) {
          if (EmailInput.GetComponent<InputField> ().isFocused) {
            PasswordInput.GetComponent<InputField> ().ActivateInputField ();
          }

          if (PasswordInput.GetComponent<InputField> ().isFocused) {
            EmailInput.GetComponent<InputField> ().ActivateInputField ();
          } else if (!EmailInput.GetComponent<InputField> ().isFocused && !PasswordInput.GetComponent<InputField> ().isFocused) {
            EmailInput.GetComponent<InputField> ().ActivateInputField ();
          }
        } else if (Input.GetKeyDown (KeyCode.Return)) {
          Login ();
        }
      }
    }

    public void SetCredential (string email, string password)
    {
      EmailInput.GetComponent<InputField> ().text = email;
      PasswordInput.GetComponent<InputField> ().text = password;
    }

    public void Reset ()
    {
      TacoManager.OpenMessage (TacoConfig.Processing);
      Action<string> success = (string data) => {
        TacoManager.CloseMessage ();
        GeneralResult r = JsonUtility.FromJson<GeneralResult> (data);
        string notice = r.success ? r.msg : r.err;
        string header = r.success ? TacoConfig.EmailSentHeader : TacoConfig.ErrorHeader;
        TacoManager.OpenModalGeneralResultPanel (r.success, header, notice);
      };
      Action<string, string> fail = (string data, string error) => {
        TacoManager.CloseMessage ();
        GeneralResult r = JsonUtility.FromJson<GeneralResult> (data);
        TacoManager.OpenModalGeneralResultPanel (false, TacoConfig.ErrorHeader, r.err);
      };
      StartCoroutine (ApiManager.Instance.ResetPassword (resetAccount.text, success, fail));
      resetAccount.text = string.Empty;
    }

    public void Login ()
    {
      var email = EmailInput.GetComponent<InputField> ().text;
      var password = PasswordInput.GetComponent<InputField> ().text;
      // toggle returns a bool
      bool autoLoginBool = AutoLoginToggle.GetComponent<Toggle> ().isOn;

      // Unity doesn't allow Bool as a preference type, using int
      int autoLoginInt = 0;

      if (autoLoginBool) {
        autoLoginInt = 1;
      }

      TacoManager.OpenMessage (TacoConfig.TacoLoginStatusMessage00);

      //TODO: Verify valid email syntax
      if (email == string.Empty || password == string.Empty) {
        TacoManager.CloseMessage ();
        TacoManager.OpenModalLoginFailedPanel (TacoConfig.TacoLoginErrorMessage00);
      } else {
        Action<string> success = (string data) => {
          LoginResult r = JsonUtility.FromJson<LoginResult> (data);
          if (r.success) {
            BalanceManager.Instance.SetRemainingValue (r.remainingClaim, r.login_count);
            TacoManager.SetPreference (UserPreferences.autoLogin, autoLoginInt);
            TacoManager.SetPreferenceString (UserPreferences.userToken, r.token);

            TacoManager.OpenMessage (TacoConfig.TacoLoginStatusMessage01);
            TacoManager.CreateUser (r);

            if (r.msg == "in") {
              if (r.free == "free") {
                //Debug.LogError("No longer implemented");
                //TacoManager.OpenModal("Login success", "Welcome back to game taco! You get " + r.score_tokens + " taco token from free play!");
              } else {
                if (r.free == "enough") {
                  Debug.LogError ("No longer implemented");
                  //TacoManager.OpenModal("Login success", "Welcome back to game taco! You've got too many taco tokens today! Please try again tomorrow.");
                } else {
                  TacoManager.OpenHowToPlayPanel ();
                }
              }
            } else {
              if (r.free == "free") {
                Debug.LogError ("No longer implemented");
                //TacoManager.OpenModal("Login success", "You get " + r.value + " taco token for login today! And " + r.score_tokens + " taco token from free play!");
              } else {
                if (!TacoManager.GetPanel ("SuccessRegister").activeSelf)
                  TacoManager.OpenModalDailyTokenPanel (r.value);
                else
                  TacoManager.OpenModalRegisterPanel (TacoManager.User.name);
              }

            }
            //ApiManager.Instance.OpenConnection();
            //ApiManager.Instance.SendData(r.userName + string.Empty);
            Init ();

          } else {
            TacoManager.CloseMessage ();
            TacoManager.OpenModalLoginFailedPanel (TacoConfig.TacoLoginErrorEmailPassword);
          }
        };

        Action<string, string> fail = (string data, string error) => {
          if (!string.IsNullOrEmpty (data)) {
            SystemError r = JsonUtility.FromJson<SystemError> (data);
            if (r.verErr) {
              //version error
              TacoManager.CloseMessage ();
              TacoManager.OpenModalIncorrectVersionPanel (r.message);
            } else {
              TacoManager.CloseMessage ();
              string msg = r.message;
              if (string.IsNullOrEmpty (msg)) {
                msg = TacoConfig.TacoLoginErrorEmailPassword;
              }
              TacoManager.OpenModalLoginFailedPanel (msg);
            }
          } else {
            TacoManager.CloseMessage ();
            TacoManager.OpenModalLoginFailedPanel (TacoConfig.TacoLoginErrorEmailPassword);
          }

          if (!string.IsNullOrEmpty (error)) {
            Debug.Log ("Error : " + error);
          }
        };

        StartCoroutine (ApiManager.Instance.Login (email, password, success, fail));
      }
    }

    public void LoginWithGoogle ()
    {
      TacoManager.LoginPanelObject.SetActive (true);//if from register panel
      TacoManager.OpenMessage (TacoConfig.TacoLoginStatusMessage00);
      Debug.Log ("internet: " + Application.internetReachability);
      if (Application.internetReachability == NetworkReachability.NotReachable) {
        TacoManager.CloseMessage ();
        TacoManager.OpenModalLoginFailedPanel (TacoConfig.TacoLoginStatusNoInternet);
        return;
      }

      Action<string> onSuccess = (string data) => {
        TacoManager.CloseMessage ();
        GoogleDeviceInfo r = JsonUtility.FromJson<GoogleDeviceInfo> (data);
        CodeDisplayPanel.SetActive (true);
        CodeDisplayPanel.transform.Find ("Main/Code").GetComponent<InputField> ().text = r.user_code;
        deviceInfo = r;
        //testing
        TextEditor te = new TextEditor ();
        te.text = deviceInfo.user_code;
        te.SelectAll ();
        te.Copy ();
      };

      Action<string, string> onFail = (string data, string error) => {
        TacoManager.CloseMessage ();
        TacoManager.OpenModalLoginFailedPanel (error);
      };
      StartCoroutine (ApiManager.Instance.RequestDeviceAdnUserCode (authorizationDeviceEndpoint, clientID, onSuccess, onFail));
    }

    private IEnumerator PollRequestToGoogle (GoogleDeviceInfo deviceData)
    {
      bool polling = true;
      int nbOfPoll = 9;
      while (polling) {
        Action<string> onSuccess = (string accessData) => {
          Debug.Log ("access data: " + accessData);
          AccessData result = JsonUtility.FromJson<AccessData> (accessData);
          if (!string.IsNullOrEmpty (result.access_token)) {
            polling = false;

            Debug.Log ("login google");
            Action<string> successUserInfo = (string userInfo) => {
              TacoManager.CloseMessage ();

              Debug.Log (userInfo);
              LoginResult r = JsonUtility.FromJson<LoginResult> (userInfo);
              if (r.success) {
                TacoManager.OpenMessage (TacoConfig.TacoLoginStatusMessage01);
                TacoManager.CreateUser (r);
                BalanceManager.Instance.SetRemainingValue (r.remainingClaim, r.login_count);
                if (r.msg == "in") {
                  if (r.free == "free") {
                    //TacoManager.OpenModal("Login success", "Welcome back to game taco! You get " + r.score_tokens + " taco token from free play!");
                  } else {
                    if (r.free == "enough") {
                      //TacoManager.OpenModal("Login success", "Welcome back to game taco! You've got too many taco tokens today! Please try again tomorrow.");
                    } else {
                      TacoManager.OpenHowToPlayPanel ();
                    }
                  }
                } else {
                  if (r.msg == "anotherSocial") {
                    if (r.free == "free") {
                      //TacoManager.OpenModal("Login success", "You get " + "6" + " taco token for login today! And " + r.score_tokens + " taco token from free play!");
                    } else {
                      TacoManager.OpenModalRegisterPanel (TacoManager.User.name);
                    }
                  } else {
                    if (r.free == "free") {
                      //TacoManager.OpenModal("Login success", "You get " + r.value + " taco token for login today! And " + r.score_tokens + " taco token from free play!");
                    } else {
                      if (!TacoManager.GetPanel ("SuccessRegister").activeSelf)
                        TacoManager.OpenModalDailyTokenPanel (r.value);
                    }
                  }
                }

                // clean up the login panel
                Init ();

              } else {
                TacoManager.CloseMessage ();
                TacoManager.OpenModalLoginFailedPanel (TacoConfig.Error);
              }

            };
            Action<string, string> failUserInfo = (string userInfo, string error) => {
              //TacoManager.CloseMessage();
              //TacoManager.OpenModalLoginFailedPanel(error);
              Debug.Log (userInfo);
              if (!string.IsNullOrEmpty (userInfo)) {
                SystemError r = JsonUtility.FromJson<SystemError> (userInfo);
                if (r.verErr) {
                  //version error
                  TacoManager.CloseMessage ();
                  TacoManager.OpenModalIncorrectVersionPanel (r.message);
                } else {
                  TacoManager.CloseMessage ();
                  string msg = r.message;
                  if (string.IsNullOrEmpty (msg)) {
                    msg = TacoConfig.TacoLoginErrorEmailPassword;
                  }
                  TacoManager.OpenModalLoginFailedPanel (msg);
                }
              } else {
                TacoManager.CloseMessage ();
                TacoManager.OpenModalLoginFailedPanel (TacoConfig.TacoLoginErrorEmailPassword);
              }
            };
            StartCoroutine (ApiManager.Instance.LoginGoogle (result.access_token, result.id_token, ReferenceCodeInput.text, successUserInfo, failUserInfo));
          } else if (nbOfPoll < 0) {
            polling = false;
            TacoManager.CloseMessage ();
            TacoManager.OpenModalLoginFailedPanel (TacoConfig.Error);
          }
          nbOfPoll--;
        };
        Action<string, string> onFail = (string data, string error) => {
          nbOfPoll--;
          if (nbOfPoll < 0) {
            polling = false;
            TacoManager.CloseMessage ();
            TacoManager.OpenModalLoginFailedPanel (error);
          }
        };
        StartCoroutine (ApiManager.Instance.PollGoogleAuthorization ("https://www.googleapis.com/oauth2/v4/token", clientID, clientSecret, deviceData.device_code, "http://oauth.net/grant_type/device/1.0", deviceData.interval, onSuccess, onFail));
        yield return new WaitForSeconds (deviceData.interval);
      }
    }

  }
}
