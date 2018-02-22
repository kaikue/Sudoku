using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorImage : MonoBehaviour {

	public bool selected;
	public Vector2 cursorOffset;
	public Material transparentMaterial;

	private RectTransform rectTransform;
	private Material normalMaterial;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<Image> ().rectTransform;
		normalMaterial = GetComponent<Image>().material;
	}
	
	// Update is called once per frame
	void Update ()
	{
		SudokuController sc = GameObject.Find("SudokuController").GetComponent<SudokuController>();
		if (selected) {
			GetComponent<Image>().material = transparentMaterial;

			float xSepWorld = sc.squareSeparationX;
			float ySepWorld = sc.squareSeparationY;
			Vector3 sep = Camera.main.WorldToScreenPoint(new Vector3(xSepWorld, ySepWorld, 10)); //TODO: fix this
			float xSep = sep.x;
			float ySep = sep.y;
			//print(xSepWorld + " " + xSep);

			float baseX = Input.mousePosition.x + cursorOffset.x;
			float clampedX = Mathf.Round(baseX / xSep) * xSep;
			float baseY = Input.mousePosition.y + cursorOffset.y;
			float clampedY = Mathf.Round(baseY / ySep) * ySep;
			//rectTransform.position = new Vector3(clampedX, clampedY, Input.mousePosition.z); //TODO uncomment when it works
			rectTransform.position = new Vector3(baseX, baseY, Input.mousePosition.z); //TODO remove
		} else {
			GetComponent<Image>().material = normalMaterial;
			rectTransform.position = transform.parent.transform.position;
			sc.RefreshButtonLabels();
		}
	}
}
