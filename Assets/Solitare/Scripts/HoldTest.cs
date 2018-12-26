using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HoldTest : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	bool buttonHeld;

	public void OnPointerDown(PointerEventData eventData)
	{
		buttonHeld = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		buttonHeld = false;
	}

	public bool isPressed()
	{
		return buttonHeld;
	}

	void Update()
	{
		
	}
}