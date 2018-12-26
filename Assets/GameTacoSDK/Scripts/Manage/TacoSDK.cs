using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameTacoSDK
{
  public class TacoSDK : SingletonMono<TacoSDK>
  {
    private IGameTacoUILoader uiLoader;
    private static bool inited = false;

    void Awake ()
    {
      if (inited) {
        Destroy (gameObject);
        return;
      } else
        DontDestroyOnLoad (gameObject);
      inited = true;
        
    }

    public void Init ()
    {
      uiLoader = GameTacoUILoader.Instance;
      uiLoader.createCanvas ();
      uiLoader.createUISignIn ();
    }

    public void openSignInTaco ()
    {
      
    }
  }
}
