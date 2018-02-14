using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleNumber : MonoBehaviour {

	private static readonly Color COLOR_HIGHLIGHTED = new Color(0.5f, 0.5f, 0.5f, 1.0f);
	private static readonly Color COLOR_NOT_HIGHLIGHTED = new Color(1.0f, 1.0f, 1.0f, 1.0f);

	public SudokuNumber number;

	private SudokuController gameController;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("SudokuController").GetComponent<SudokuController>();

		transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite> ("game_numbers/" + number.ToString ());
		GetComponent<Toggle> ().onValueChanged.AddListener (ToggleOnClick);
	}

	void Update() {
		if (GetComponent<Toggle> ().isOn) {
			transform.GetChild (0).gameObject.GetComponent<Image> ().color = COLOR_HIGHLIGHTED;
		} else {
			transform.GetChild (0).gameObject.GetComponent<Image> ().color = COLOR_NOT_HIGHLIGHTED;
		}
	}

	void ToggleOnClick(bool selected) {
		if (GetComponent<Toggle> ().isOn) {
			if (gameController.selectedNumber != number) {
				gameController.SelectNumber (number);
			}
		} else {
			if (gameController.selectedNumber == number) {
				gameController.SelectNumber (SudokuNumber.NONE);
			}
		}
	}
}
