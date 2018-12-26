using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameTacoSDK;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{

  // Use this for initialization
  void Start ()
  {
  }
	
  // Update is called once per frame
  void Update ()
  {
		
  }

  void OnEnable ()
  {
    SceneManager.sceneLoaded += OnLevelFinishedLoading;
  }

  void OnLevelFinishedLoading (Scene scene, LoadSceneMode mode)
  {
    TacoSDK.Instance.Init ();
    SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    Debug.Log ("Level Loaded");
    Debug.Log (scene.name);
    Debug.Log (mode);
  }
}
