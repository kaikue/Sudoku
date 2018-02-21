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
	private SudokuController gameController;
	private CursorImage cursorImage;
	private Image glow;
	private Vector3 startPosition;
	private bool planted;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("SudokuController").GetComponent<SudokuController>();
		glow = transform.GetChild(0).gameObject.GetComponent<Image> ();
		glow.color = Color.clear;
		cursorImage = transform.GetChild(1).gameObject.GetComponent<CursorImage> ();
		startPosition = transform.position;

		GetComponent<Button> ().onClick.AddListener (ButtonOnClick);
	}

	void ButtonOnClick() {
		GetComponent<AudioSource>().Play();
		selected = !selected;
		if (selected) {
			gameController.SelectMode (mode);
			glow.color = Color.clear;
			if (planted) {
				RestoreButtonPosition ();	
			}
		}
	}

	public void OnPointerEnter(PointerEventData d) {
		if (!selected) {
			glow.color = Color.white;	
		}
	}

	public void OnPointerExit(PointerEventData d) {
		glow.color = Color.clear;	
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
