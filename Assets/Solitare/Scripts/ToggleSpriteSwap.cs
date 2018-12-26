using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToggleSpriteSwap : MonoBehaviour {

	public Toggle targetToggle;
	public Sprite selectedSprite;
	public Sprite offSprite;

	// Use this for initialization
	void Start () 
	{
		targetToggle.toggleTransition = Toggle.ToggleTransition.None; 
		targetToggle.onValueChanged.AddListener(OnTargetToggleValueChanged);

        Image targetImage = targetToggle.targetGraphic as Image;

        if (targetToggle.isOn) 
        {
            targetImage.sprite = selectedSprite;
        }
        else
        {
            targetImage.sprite = offSprite;
        }
	}

	void OnTargetToggleValueChanged(bool newValue) 
	{
		Image targetImage = targetToggle.targetGraphic as Image;

		if (targetImage != null) 
		{
			if (newValue) 
			{
				targetImage.sprite = selectedSprite;
			}
			else
			{
				targetImage.sprite = offSprite;
			}
		}
	}
}