using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameTacoSDK
{
  public class GameTacoUILoader : SingletonMono<GameTacoUILoader>,IGameTacoUILoader
  {
    #region IGameTacoUILoader implementation

    private static bool exist = false;

    void Awake ()
    {
      if (exist)
        Destroy (gameObject);
      else
        DontDestroyOnLoad (gameObject);
      exist = true;
     
    }

    public GameObject createUILoading (string path = "")
    {
      GameObject clone;
      if (path.Equals (""))
        clone = Instantiate (Resources.Load<GameObject> ("Prefabs/loading"));
      else
        clone = Instantiate (Resources.Load<GameObject> (path));
      clone.name = "Loading";
      clone.transform.SetParent (transform);
      RectTransform rect = clone.GetComponent<RectTransform> ();
      rect.anchorMax = Vector2.one * 0.5f;
      rect.anchorMin = Vector2.one * 0.5f;
      rect.sizeDelta = Vector2.zero;
      rect.localPosition = Vector2.zero;
      clone.AddComponent<TacoManageLoadingView> ();
      return clone;
    }

    public GameObject createUISignIn (string path = "")
    {
      GameObject clone;
      if (path.Equals (""))
        clone = Instantiate (Resources.Load<GameObject> ("Prefabs/login"));
      else
        clone = Instantiate (Resources.Load<GameObject> (path));
      clone.transform.SetParent (transform);

      clone.name = "Login";
      RectTransform rect = clone.GetComponent<RectTransform> ();
      rect.anchorMax = Vector2.one;
      rect.anchorMin = Vector2.zero;
      rect.sizeDelta = Vector2.zero;
      rect.localPosition = Vector2.zero;
      return clone;
    }

    public GameObject createUIMenu (string path = "")
    {
      return null;
    }

    public void createCanvas ()
    {
      gameObject.AddComponent<RectTransform> ();
      gameObject.AddComponent<Canvas> ();
      gameObject.AddComponent<GraphicRaycaster> ();
      gameObject.GetComponent<Canvas> ().renderMode = RenderMode.ScreenSpaceOverlay;
      gameObject.GetComponent<Canvas> ().sortingOrder = 999;
      gameObject.AddComponent<CanvasScaler> ();
      gameObject.GetComponent<CanvasScaler> ().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
      gameObject.GetComponent<CanvasScaler> ().referenceResolution = new Vector2 (1080, 1920);
      gameObject.GetComponent<CanvasScaler> ().matchWidthOrHeight = 0.5f;
      transform.SetParent (null);
    }

    public GameObject createUIHeader (string path = "")
    {
      GameObject clone;
      if (path.Equals (""))
        clone = Instantiate (Resources.Load<GameObject> ("Prefabs/header"));
      else
        clone = Instantiate (Resources.Load<GameObject> (path));
      clone.transform.SetParent (transform);
      clone.name = "header";
      RectTransform rect = clone.GetComponent<RectTransform> ();
      rect.anchorMax = Vector2.one;
      rect.anchorMin = Vector2.zero;
      rect.sizeDelta = Vector2.zero;
      rect.localPosition = Vector2.zero;
      return clone;
    }

    #endregion



  }
}
