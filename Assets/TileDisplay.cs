using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDisplay : MonoBehaviour {

	public Sprite[] sprites;
	public void SetSprite(int bitMask)
	{
		GetComponent<SpriteRenderer>().sprite = sprites[bitMask];
	}
}
