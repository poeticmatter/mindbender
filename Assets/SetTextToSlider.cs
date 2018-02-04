using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTextToSlider : MonoBehaviour {

	public void SetMazeSize(Slider slider)
	{
		GetComponent<Text>().text = ""+slider.value;
	}
}
