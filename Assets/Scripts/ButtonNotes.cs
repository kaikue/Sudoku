using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNotes : MonoBehaviour {
	private readonly static Color COLOR_SELECTED = new Color (0.5f, 0.5f, 0.5f, 1.0f);
	private readonly static Color COLOR_NOT_SELECTED = new Color (1.0f, 1.0f, 1.0f, 1.0f);

	private bool selected;
	private SudokuController gameController;
	private Image imageBackground;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("SudokuController").GetComponent<SudokuController>();
		imageBackground = transform.GetChild(0).gameObject.GetComponent<Image> ();

		GetComponent<Button> ().onClick.AddListener (ButtonOnClick);
	}

	void ButtonOnClick() {
		selected = !selected;
		gameController.SetNotes (selected);

		if (selected) {
			imageBackground.color = COLOR_SELECTED;	
		} else {
			imageBackground.color = COLOR_NOT_SELECTED;	
		}
	}
}
