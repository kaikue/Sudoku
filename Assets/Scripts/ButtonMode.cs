using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public enum Mode
	{
		NUMBER,
		NOTES,
		ERASE,
		NONE
	}

	public bool selected;
	public Mode mode;
	public Vector3 snapPosition;
	public bool snapped;
	private SudokuController gameController;
	private CursorImage cursorImage;
	private Image glow;
	private Vector3 startPosition;
	private bool planted;
	private GameObject instructions;
	private bool showInstructions;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("SudokuController").GetComponent<SudokuController>();
		glow = transform.GetChild(0).gameObject.GetComponent<Image> ();
		glow.color = Color.clear;
		cursorImage = transform.GetChild(1).gameObject.GetComponent<CursorImage> ();
		startPosition = transform.position;
		instructions = transform.GetChild (2).gameObject;
		showInstructions = true;
		instructions.SetActive (false);

		GetComponent<Button> ().onClick.AddListener (ButtonOnClick);
	}

	void ButtonOnClick() {
		showInstructions = false;
		instructions.SetActive (false);

		GetComponent<AudioSource>().Play();
		selected = !selected;
		if (selected) {
			gameController.SelectMode (mode);
			glow.color = Color.clear;
			if (planted) {
				RestoreButtonPosition ();	
			}
		} else {
			gameController.SelectMode (Mode.NONE);
		}
	}

	public void OnPointerEnter(PointerEventData d) {
		if (!selected) {
			glow.color = Color.white;	
		}

		if (showInstructions) {
			instructions.SetActive (true);
		}
	}

	public void OnPointerExit(PointerEventData d) {
		glow.color = Color.clear;	
		instructions.SetActive (false);
	}

	void Update() {
		cursorImage.selected = selected;
	}

	public void PlantButtonOnImage() {
		transform.position = cursorImage.transform.position;
		selected = false;
		planted = true;
	}

	public void RestoreButtonPosition() {
		transform.position = startPosition;
		planted = false;
	}

}
