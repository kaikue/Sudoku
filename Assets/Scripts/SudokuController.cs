using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SudokuController : MonoBehaviour {

	public GameObject squarePrefab;
	public GameObject battlePrefab;
	public GameObject board;
	public GameObject winText;
	public GameObject parent;
	public Camera cam;
	public float squareSeparationX;
	public float squareSeparationY;

	// Battle args here
	public SquareController selectedSquare;

	private int[] testNumbers = 
	{-2, 6, -1, 3, -7, -5, 8, -9, 4,
     5, 3, 7, -8, 9, -4, -1, -6, -2,
	 -9, 4, -8, -2, -1, 6, 3, -5, 7,
	 -6, 9, -4, -7, 5, 1, 2, 3, 8,
	 -8, -2, -5, -9, -4, -3, -6, -7, -1,
	 7, 1, 3, 6, 2, -8, -9, 4, -5,
	 3, -5, 6, 4, -8, -2, -7, 1, -9,
	 -4, -8, -9, -1, 6, -7, 5, 2, 3,
     1, -7, 2, -5, -3, 9, -4, 8, -6};

	private int[] solution;

	private SquareController[] squares;
	private bool notes;
	// Use this for initialization
	void Start () {
		squares = new SquareController[81];

		InstantiateSquares ();
		InitializeSquares ();

		winText.GetComponent<Text> ().text = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector3 mousePosition = cam.ScreenToWorldPoint (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (new Vector2(mousePosition.x, mousePosition.y), Vector2.zero);
			if (hit != null && hit.collider != null) {
				SquareController newSelectedSquare = hit.collider.gameObject.GetComponent<SquareController>();
				if (!newSelectedSquare.hint) {
					if (selectedSquare != null) {
						selectedSquare.highlighted = false;
					}

					if (selectedSquare == newSelectedSquare && selectedSquare.noted) {
						Battle();
					}

					selectedSquare = newSelectedSquare;

					selectedSquare.highlighted = true;
					selectedSquare.noted = notes;
					selectedSquare.notes = new bool[9];
				}
			}
		}

		if (Input.anyKeyDown) {
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				SetNumber (SudokuNumber.ONE);
			} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
				SetNumber (SudokuNumber.TWO);
			} else if (Input.GetKeyDown(KeyCode.Alpha3)) {
				SetNumber (SudokuNumber.THREE);
			} else if (Input.GetKeyDown(KeyCode.Alpha4)) {
				SetNumber (SudokuNumber.FOUR);
			} else if (Input.GetKeyDown(KeyCode.Alpha5)) {
				SetNumber (SudokuNumber.FIVE);
			} else if (Input.GetKeyDown(KeyCode.Alpha6)) {
				SetNumber (SudokuNumber.SIX);
			} else if (Input.GetKeyDown(KeyCode.Alpha7)) {
				SetNumber (SudokuNumber.SEVEN);
			} else if (Input.GetKeyDown(KeyCode.Alpha8)) {
				SetNumber (SudokuNumber.EIGHT);
			} else if (Input.GetKeyDown(KeyCode.Alpha9)) {
				SetNumber (SudokuNumber.NINE);
			}
		}

		CheckForWin ();
	}

	private void Battle()
	{
		GameObject battle = Instantiate(battlePrefab);
		BattleController bc = battle.GetComponentInChildren<BattleController>();
		bc.InitializeGame(selectedSquare, this);
		//TODO: set battle.position and scale
		//TODO: zoom in on battle
		parent.SetActive(false);
	}

	public void ReturnToNormal(BattleController battle)
	{
		//TODO: zoom out
		Destroy(battle.gameObject.transform.parent.gameObject);
	}

	public void SetNumber(SudokuNumber number) {
		if (selectedSquare.noted) {
			selectedSquare.notes [(int)number] = true;
		} else {
			selectedSquare.number = number;
			selectedSquare.cleared = false;
			CheckForConflicts ();
		}
	}

	public void SetNotes(bool notes) {
		this.notes = notes;
		if (selectedSquare != null) {
			selectedSquare.noted = notes;
			selectedSquare.notes = new bool[9];
		}
		CheckForConflicts ();
	}	

	public void Erase() {
		if (selectedSquare != null) {
			selectedSquare.cleared = true;
			selectedSquare.notes = new bool[9];
		}
		CheckForConflicts ();
	}	

	public void SetCorrectNumber() {
		SetNotes (false);
		SetNumber ((SudokuNumber)((-1 * testNumbers [selectedSquare.index]) - 1));
	}

	public void SetLostBattle()
	{
		//TODO: can't battle on selected square anymore
	}
	
	public void ExitGame() {
		//TODO: Implement
	}

	private void CheckForWin() {
		CheckForConflicts ();
		bool won = true;
		foreach (SquareController square in squares) {
			if (square.cleared || square.noted || square.softConflicting || square.hardConflicting) {
				won = false;
			}
		}

		if (won) {
			winText.GetComponent<Text> ().text = "YOU WIN";
		}
	}

	private void CheckForConflicts() {
		foreach (SquareController square in squares) {
			square.softConflicting = false;
			square.hardConflicting = false;
		}

		foreach (SquareController square1 in squares) {
			foreach (SquareController square2 in GetConflicting(square1)) {
				if (square1 != square2 && 
						!square1.cleared && !square2.cleared && 
						!square1.noted && !square2.noted &&
						square1.number.Equals (square2.number)) {
					if (square1.hint && !square2.hint) {
						square2.hardConflicting = true;
					} else if (!square1.hint && square2.hint) {
						square1.hardConflicting = true;
					} else if (!square1.hint && !square2.hint) {
						square1.softConflicting = true;
						square2.softConflicting = true;
					}
				}
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
				squares [9 * y + x] = square.GetComponent<SquareController>();

				position.x += squareSeparationX;	
			}
			position.y -= squareSeparationY;
		}

		squarePrefab.SetActive (false);
	}

	private void InitializeSquares() {
		for (int i = 0; i < 81; i++) {
			SquareController square = squares [i];
			square.index = i;
			if (testNumbers [i] < 0) {
				square.cleared = true;
			} else {
				square.hint = true;
				square.number = (SudokuNumber)(testNumbers [i] - 1);
			}
		}
	}

	private HashSet<SquareController> GetRow(SquareController square) {
		HashSet<SquareController> row = new HashSet<SquareController> ();

		int start = (square.index / 9) * 9;
		int end = start + 9;
		int inc = 1;

		for (int idx = start; idx < end; idx += inc) {
			row.Add (squares [idx].GetComponent<SquareController> ());	
		}	

		return row;
	}

	private HashSet<SquareController> GetColumn(SquareController square) {
		HashSet<SquareController> column = new HashSet<SquareController> ();

		int start = square.index % 9;
		int end = 9 * 9 + start;
		int inc = 9;

		for (int idx = start; idx < end; idx += inc) {
			column.Add (squares [idx].GetComponent<SquareController> ());	
		}	

		return column;
	}

	private HashSet<SquareController> GetGroup(SquareController square) {
		HashSet<SquareController> group = new HashSet<SquareController> ();

		int startX = ((square.index % 9) / 3) * 3;
		int endX = startX + 3;

		int startY = ((square.index / 9) / 3) * 3;
		int endY = startY + 3;

		for (int y = startY; y < endY; y++) {
			for (int x = startX; x < endX; x++) {
				group.Add (squares [9 * startY + startX].GetComponent<SquareController> ());
			}	
		}

		return group;
	}

	private HashSet<SquareController> GetConflicting(SquareController square) {
		HashSet<SquareController> conflicting = new HashSet<SquareController> (); 
		conflicting.UnionWith (GetRow (square));
		conflicting.UnionWith (GetColumn (square));
		conflicting.UnionWith (GetGroup (square));

		return conflicting;
	}
}
