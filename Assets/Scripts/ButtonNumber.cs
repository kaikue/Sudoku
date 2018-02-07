using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNumber : MonoBehaviour {

	public SudokuNumber number;
	private GameController gameController;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("SudokuController").GetComponent<GameController>();

		transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite> ("number_" + number.ToString () + "_temp");
		GetComponent<Button> ().onClick.AddListener (ButtonOnClick);
	}

	void ButtonOnClick() {
		gameController.SetNumber (number);	
	}
}
