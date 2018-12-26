using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class DefaultMenu : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{

	}

	// Update is called once per frame
	void Update () 
	{

	}

	public void ReplaceSprite(string objectName, string spriteName)
	{
		
		// gameObject should be the Canvas object.

		GameObject cobject = MenuManager.Find(gameObject, objectName,true);
		Image cimg = cobject.GetComponent<Image>();
		cimg.sprite = Resources.Load<Sprite>(spriteName);
	}

	public void ReplaceSpriteInAtlas(string objectName, string atlasName, string spriteName)
	{
		GameObject cobject = MenuManager.Find(gameObject, objectName,true);
		Image cimg = cobject.GetComponent<Image>();
		cimg.sprite = FindSpriteInAtlas(atlasName,spriteName);
	}

	public void ReplaceToggleSpriteInAtlas(string objectName, string atlasName, string spriteNameOn, string spriteNameOff)
	{
		GameObject cobject = MenuManager.Find(gameObject,objectName,true);
		//Toggle toggle = cobject.GetComponent<Toggle>();
		ToggleSpriteSwap toggleSS = cobject.GetComponent<ToggleSpriteSwap>();

		Image targetImage = toggleSS.targetToggle.targetGraphic as Image;
		// Off
		targetImage.sprite = FindSpriteInAtlas(atlasName,spriteNameOff);
		// On (overrideSprite)
		toggleSS.selectedSprite = FindSpriteInAtlas(atlasName,spriteNameOn);
	}

	public Sprite FindSpriteInAtlas(string atlasPath, string spriteName)
	{
		Sprite[] sprites = Resources.LoadAll<Sprite>(atlasPath);
		foreach (Sprite asprite in sprites)
		{
			if(asprite.name == spriteName)
			{
				return asprite;
			}
		}

		return null;
	}

	abstract public void OnScreenChange(float w, float h);
	abstract public void OnButtonClick(Button bt);
	abstract public void OnToggleChange(Toggle tg);
}
