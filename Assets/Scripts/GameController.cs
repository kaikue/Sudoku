using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject squarePrefab;
	public GameObject board;
	public Camera camera;
	public float squareSeparationX;
	public float squareSeparationY;

	private GameObject[] squares;

	// Use this for initialization
	void Start () {
		InitializeSquares ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector3 mousePosition = camera.ScreenToWorldPoint (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (new Vector2(mousePosition.x, mousePosition.y), Vector2.zero);
			GameObject hitSquare = hit.collider.gameObject;
			hitSquare.GetComponent<SquareController> ().cleared = !hitSquare.GetComponent<SquareController> ().cleared;
		}
	}

	void InitializeSquares() {
		squares = new GameObject[81];

		Vector3 position = squarePrefab.transform.localPosition;
		for (int y = 0; y < 9; y++) {
			position.x = squarePrefab.transform.localPosition.x;
			for (int x = 0; x < 9; x++) {
				GameObject square = Instantiate (squarePrefab);
				square.transform.SetParent (board.transform);
				square.transform.localPosition = position;
				squares [9 * y + x] = square;

				position.x += squareSeparationX;	
			}
			position.y -= squareSeparationY;
		}

		squarePrefab.SetActive (false);
	}
}
