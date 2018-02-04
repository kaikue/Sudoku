using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject squarePrefab;
	public GameObject board;
	public Camera camera;
	public float squareSeparationX;
	public float squareSeparationY;

	private int[] testNumbers = 
	{0, 6, 0, 3, 0, 0, 8, 0, 4,
     5, 3, 7, 0, 9, 0, 0, 0, 0,
	 0, 4, 0, 0, 0, 6, 3, 0, 7,
	 0, 9, 0, 0, 5, 1, 2, 3, 8,
	 0, 0, 0, 0, 0, 0, 0, 0, 0,
	 7, 1, 3, 6, 2, 0, 0, 4, 0,
	 3, 0, 6, 4, 0, 0, 0, 1, 0,
	 0, 0, 0, 0, 6, 0, 5, 2, 3,
     1, 0, 2, 0, 0, 9, 0, 8, 0};

	private GameObject[] squares;

	// Use this for initialization
	void Start () {
		squares = new GameObject[81];

		InstantiateSquares ();
		InitializeSquares ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector3 mousePosition = camera.ScreenToWorldPoint (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (new Vector2(mousePosition.x, mousePosition.y), Vector2.zero);
			SquareController hitSquareController = hit.collider.gameObject.GetComponent<SquareController>();
			if (!hitSquareController.hint) {
				hitSquareController.cleared = !hitSquareController.cleared;
			}
		}
	}

	private void InstantiateSquares() {
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

	private void InitializeSquares() {
		for (int i = 0; i < 81; i++) {
			SquareController squareController = squares [i].GetComponent<SquareController> ();
			if (testNumbers [i] == 0) {
				squareController.cleared = true;
				squareController.hint = false;
			} else {
				squareController.cleared = false;
				squareController.hint = true;
				squareController.number = (SudokuNumber)(testNumbers [i] - 1);
			}
		}
	}
}
