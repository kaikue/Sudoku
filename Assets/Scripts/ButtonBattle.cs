using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBattle : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler {

	public bool glowEnabled;

	private SudokuController gameController;
	private Image glow;
	private bool showInstructions;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("SudokuController").GetComponent<SudokuController> ();	

		glow = transform.GetChild (0).gameObject.GetComponent<Image> ();
		showInstructions = true;
		transform.GetChild (2).gameObject.SetActive (false);
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
			showInstructions = false;
			transform.GetChild (2).gameObject.SetActive (false);
			gameController.Battle ();
		}
	}

	public void OnPointerEnter(PointerEventData d) {
		if (showInstructions) {
			transform.GetChild (2).gameObject.SetActive (true);
		}		
	}

	public void OnPointerExit(PointerEventData d) {
		transform.GetChild (2).gameObject.SetActive (false);
	}
}
