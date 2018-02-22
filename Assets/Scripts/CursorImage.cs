using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorImage : MonoBehaviour {

	public bool selected;
	public Vector2 cursorOffset;
	public Material transparentMaterial;

	private RectTransform rectTransform;
	private ButtonMode parent;
	private Material normalMaterial;
	private Vector2 actualCursorOffset;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<Image> ().rectTransform;
		normalMaterial = GetComponent<Image>().material;
		parent = transform.parent.gameObject.GetComponent<ButtonMode> ();
		actualCursorOffset = new Vector2 (cursorOffset.x * GetComponent<Image> ().flexibleWidth, cursorOffset.y * GetComponent<Image>().flexibleHeight);
	}
	
	// Update is called once per frame
	void Update ()
	{
		SudokuController sc = GameObject.Find("SudokuController").GetComponent<SudokuController>();
		if (selected) {
			GetComponent<Image>().material = transparentMaterial;
			Camera cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();

			Vector3 targetPositionWS;
			if (parent.snapped) {
				targetPositionWS = cam.ScreenToWorldPoint(parent.snapPosition);
			} else {
				targetPositionWS = cam.ScreenToWorldPoint(Input.mousePosition);
			}
			Vector3 newPosWS = new Vector3 (targetPositionWS.x + actualCursorOffset.x, targetPositionWS.y + actualCursorOffset.y, transform.parent.transform.position.z);
			rectTransform.position = cam.WorldToScreenPoint (newPosWS);
		} else {
			GetComponent<Image>().material = normalMaterial;
			rectTransform.position = transform.parent.transform.position;
			sc.RefreshButtonLabels ();
		}
	}
}
