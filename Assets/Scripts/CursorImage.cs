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
			rectTransform.position = new Vector3 (Input.mousePosition.x + cursorOffset.x, Input.mousePosition.y + cursorOffset.y, Input.mousePosition.z);
		} else {
			rectTransform.position = transform.parent.transform.position;
		}
	}
}
