using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SudokuNumber {
	ONE,
	TWO,
	THREE,
	FOUR,
	FIVE,
	SIX,
	SEVEN,
	EIGHT,
	NINE
}

public class SquareController : MonoBehaviour {
	public SudokuNumber number;
	public bool cleared;
	public bool hint;

	private SpriteRenderer sr;
	private Sprite[] numberSprites;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		if (numberSprites == null) {
			numberSprites = new Sprite[9];
			LoadNumberSprites ();
		}
	}

	// Update is called once per frame
	void LateUpdate () {
		if (cleared) {
			sr.sprite = null;
			//Debug.Log ("Cleared");
		} else {
			if (sr.sprite == null) {
    			//Debug.Log ("Setting sprite");
				sr.sprite = numberSprites [(int)number];
			}
		}
	}


	private void LoadNumberSprites() {
		for (SudokuNumber number = SudokuNumber.ONE; number <= SudokuNumber.NINE; number++) {
			string path = "number_" + number.ToString () + "_temp";
			numberSprites [(int)number] = Resources.Load<Sprite> (path);
			//Debug.Log (path);
		}
	}
}
