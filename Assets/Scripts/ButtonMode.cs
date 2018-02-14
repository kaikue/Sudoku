using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMode : MonoBehaviour {

	public enum Mode
	{
		NUMBER,
		NOTES,
		ERASE,
		BATTLE,
		NONE
	}

	public bool selected;
	public Mode mode;
	private SudokuController gameController;
	private CursorImage cursorImage;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("SudokuController").GetComponent<SudokuController>();
		cursorImage = transform.GetChild(0).gameObject.GetComponent<CursorImage> ();

		GetComponent<Button> ().onClick.AddListener (ButtonOnClick);
	}

	void ButtonOnClick() {
		selected = !selected;
		if (selected) {
			gameController.SelectMode (mode);
		}
	}

	void Update() {
		cursorImage.selected = selected;
	}

}
