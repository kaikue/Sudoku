using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBattle : MonoBehaviour {

	public bool glowEnabled;

	private SudokuController gameController;
	private Image glow;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("SudokuController").GetComponent<SudokuController> ();	

		glow = transform.GetChild (0).gameObject.GetComponent<Image> ();
		GetComponent<Button> ().onClick.AddListener (OnButtonClick);
	}
	
	// Update is called once per frame
	void Update () {
		if (glowEnabled) {
			glow.color = Color.white;	
		} else {
			glow.color = Color.clear;	
		}
	}

	void OnButtonClick() {
		if (glowEnabled) {
			gameController.Battle ();
		}
	}
}
