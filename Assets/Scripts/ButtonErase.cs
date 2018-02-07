using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonErase : MonoBehaviour {
	private GameController gameController;
	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("SudokuController").GetComponent<GameController>();
		GetComponent<Button> ().onClick.AddListener (ButtonOnClick);
	}

	void ButtonOnClick() {
		gameController.Erase ();
	}
}
