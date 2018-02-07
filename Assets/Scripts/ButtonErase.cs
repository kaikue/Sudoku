using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonErase : MonoBehaviour {
	private SudokuController gameController;
	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("SudokuController").GetComponent<SudokuController>();
		GetComponent<Button> ().onClick.AddListener (ButtonOnClick);
	}

	void ButtonOnClick() {
		gameController.Erase ();
	}
}
