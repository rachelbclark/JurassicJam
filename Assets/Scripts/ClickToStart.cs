using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToStart : MonoBehaviour {


	public void OnClick()
	{
		GameManager.instance.ClickToStart();
	}
}