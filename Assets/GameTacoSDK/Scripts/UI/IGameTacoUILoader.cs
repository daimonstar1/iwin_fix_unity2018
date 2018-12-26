using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTacoSDK
{
  public interface IGameTacoUILoader
  {
    void createCanvas ();

    GameObject createUILoading (string path = "");

    GameObject createUISignIn (string path = "");

    GameObject createUIMenu (string path = "");

    GameObject createUIHeader (string path = "");

  }
}