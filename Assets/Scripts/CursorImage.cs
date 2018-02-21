using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorImage : MonoBehaviour {

	public bool selected;
	public Vector2 cursorOffset;

	private RectTransform rectTransform;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<Image> ().rectTransform;
	}
	
	// Update is called once per frame
	void Update () {
		if (selected) {
			float xSep = GameObject.Find("SudokuController").GetComponent<SudokuController>().squareSeparationX;
			float ySep = GameObject.Find("SudokuController").GetComponent<SudokuController>().squareSeparationY;

			float baseX = Input.mousePosition.x + cursorOffset.x;
			float clampedX = ((int)(baseX / xSep)) * xSep; //TODO: center?
			float baseY = Input.mousePosition.y + cursorOffset.y;
			float clampedY = ((int)(baseY / ySep)) * ySep; //TODO: center?
			rectTransform.position = new Vector3(clampedX, clampedY, Input.mousePosition.z);
		} else {
			rectTransform.position = transform.parent.transform.position;
		}
	}
}
