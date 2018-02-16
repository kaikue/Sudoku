using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SudokuController : MonoBehaviour {

	public GameObject squarePrefab;
	public GameObject battlePrefab;
	public GameObject board;
	public GameObject winText;
	public GameObject parent;
	public GameObject canvas;
	public GameObject buttonNumber;
	public GameObject buttonNotes;
	public GameObject buttonErase;
	public GameObject buttonBattle;
	public GameObject musicObj;
	public Camera cam;
	public float squareSeparationX;
	public float squareSeparationY;

	public SquareController selectedSquare;
	public SudokuNumber selectedNumber;


	// Battle args here
	public ButtonMode.Mode selectedMode;


	private int[] solution = 
	{4, -6, -8, -9, 3, -1, -5, -2, -7,
         -7, -5, -1, 6, -2, -4, 8, -3, -9,
	 -3, -9, -2, -5, -7, -8, -4, -6, 1,
	 -1, -3, -4, -7, 5, -6, -2, 9, -8,
	 -2, 8, -9, -4, -1, -3, 6, -7, -5,
	 -6, 7, -5, 2, -8, -9, -3, -1, -4,
	 -8, -4, -6, 1, -9, 2, 7, -5, -3,
	 5, -1, 3, -8, -6, -7, -9, 4, -2,
         9, -2, -7, -3, -4, -5, -1, -8, -6};

	/*private int[] testNumbers = 
	{2, 6, 1, 3, 7, 5, 8, 9, 4,
     5, 3, 7, 8, 9, 4, 1, 6, 2,
	 9, 4, 8, 2, 1, 6, 3, 5, 7,
	 6, 9, 4, 7, 5, 1, 2, 3, 8,
	 8, 2, 5, 9, 4, 3, 6, 7, 1,
	 7, 1, 3, 6, 2, 8, 9, 4, 5,
	 3, 5, 6, 4, 8, 2, 7, 1, 9,
	 4, 8, 9, 1, 6, 7, 5, 2, 3,
     1, 7, 2, 5, 3, 9, 4, 8, -6};*/

	//private int[] solution;
	private bool generatedBoard;
	private SquareController[] squares;
	private bool notes;

	private const float BATTLE_SCALE = 0.03f;
	private const float ZOOM_TIME = 1.0f;
	private Vector3 cameraGoalPos;
	private float cameraGoalSize;
	private float cameraNormalSize = 5;

	// Use this for initialization
	void Start () {
		solution = new int[81];
		GenerateBoard ();

		squares = new SquareController[81];

		InstantiateSquares ();
		InitializeSquares ();

		SelectMode (ButtonMode.Mode.NONE);

		winText.GetComponent<Text> ().text = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (!generatedBoard) {
			return;
		}
		ProcessClick ();
		ProcessKeyPress ();
		UpdateConflicts ();
		UpdateSelected ();
		CheckForWin ();
	}

	public void SetLostBattle()
	{
		print ("lost");
		selectedSquare.lostBattle = true;
	}
	
	public void ExitGame() {
		//TODO: Implement
		Destroy(Parent.parent);
		SceneManager.LoadScene("Title");
	}

	public void SelectMode(ButtonMode.Mode mode) {
		selectedMode = mode;
		selectedSquare = null;
		selectedNumber = SudokuNumber.NONE;
		buttonNotes.GetComponent<ButtonMode> ().RestoreButtonPosition ();
		buttonNumber.GetComponent<ButtonMode> ().RestoreButtonPosition ();

		switch (mode) {
		case ButtonMode.Mode.NUMBER:
			buttonNumber.GetComponent<ButtonMode> ().selected = true;	
			buttonNotes.GetComponent<ButtonMode> ().selected = false;	
			buttonErase.GetComponent<ButtonMode> ().selected = false;	
			break;
		case ButtonMode.Mode.NOTES:
			buttonNumber.GetComponent<ButtonMode> ().selected = false;	
			buttonNotes.GetComponent<ButtonMode> ().selected = true;	
			buttonErase.GetComponent<ButtonMode> ().selected = false;	
			break;
		case ButtonMode.Mode.ERASE:
			buttonNumber.GetComponent<ButtonMode> ().selected = false;	
			buttonNotes.GetComponent<ButtonMode> ().selected = false;	
			buttonErase.GetComponent<ButtonMode> ().selected = true;	
			break;
		case ButtonMode.Mode.NONE:
			buttonNumber.GetComponent<ButtonMode> ().selected = false;	
			buttonNotes.GetComponent<ButtonMode> ().selected = false;	
			buttonErase.GetComponent<ButtonMode> ().selected = false;	
			break;
		}
	}

	public void SelectNumber(SudokuNumber number) {
		selectedNumber = number;
		ApplySelectedAction ();
	}

	public void SelectSquare(SquareController square) {
		selectedNumber = SudokuNumber.NONE;
		selectedSquare = square;
		ApplySelectedAction ();
	}

	private void ProcessClick() {
		if (Input.GetMouseButtonDown (0)) {
			Vector3 mousePosition = cam.ScreenToWorldPoint (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (new Vector2(mousePosition.x, mousePosition.y), Vector2.zero);
			if (hit != null && hit.collider != null) {
				SquareController newSelectedSquare = hit.collider.gameObject.GetComponent<SquareController>();
				if (!newSelectedSquare.hint) {
					SelectSquare (newSelectedSquare);
				}
			}
		}
	}

	private void ProcessKeyPress() {
		if (Input.anyKeyDown) {
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				SelectNumber(SudokuNumber.ONE);
			} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
				SelectNumber(SudokuNumber.TWO);
			} else if (Input.GetKeyDown(KeyCode.Alpha3)) {
				SelectNumber(SudokuNumber.THREE);
			} else if (Input.GetKeyDown(KeyCode.Alpha4)) {
				SelectNumber(SudokuNumber.FOUR);
			} else if (Input.GetKeyDown(KeyCode.Alpha5)) {
				SelectNumber(SudokuNumber.FIVE);
			} else if (Input.GetKeyDown(KeyCode.Alpha6)) {
				SelectNumber(SudokuNumber.SIX);
			} else if (Input.GetKeyDown(KeyCode.Alpha7)) {
				SelectNumber(SudokuNumber.SEVEN);
			} else if (Input.GetKeyDown(KeyCode.Alpha8)) {
				SelectNumber(SudokuNumber.EIGHT);
			} else if (Input.GetKeyDown(KeyCode.Alpha9)) {
				SelectNumber(SudokuNumber.NINE);
			}
		}
	}
	
	public void Battle()
	{
		musicObj.GetComponent<AudioSource>().Stop();
		GameObject battle = Instantiate(battlePrefab);
		BattleController bc = battle.GetComponentInChildren<BattleController>();
		bc.InitializeGame(selectedSquare, this);
		StartCoroutine(ZoomIn(battle));
	}

	public void ReturnToNormal(BattleController bc)
	{
		UpdateBattleAvailability ();
		StartCoroutine(ZoomOut(bc));
	}

	private IEnumerator ZoomIn(GameObject battle)
	{
		selectedSquare.gameObject.SetActive(false);
		canvas.SetActive(false);
		battle.transform.position = selectedSquare.transform.position;
		battle.transform.localScale = new Vector3(BATTLE_SCALE, BATTLE_SCALE, 1);
		cameraGoalPos = new Vector3(selectedSquare.transform.position.x, selectedSquare.transform.position.y, -10);
		float cameraBattleSize = cameraNormalSize * 1.3f; //a little bigger so it fits
		cameraGoalSize = cameraBattleSize * BATTLE_SCALE;
		for (float t = 0; t < ZOOM_TIME; t += Time.deltaTime)
		{
			cam.transform.position = Vector3.Lerp(cam.transform.position, cameraGoalPos, t);
			cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cameraGoalSize, t);
			yield return new WaitForEndOfFrame();
		}
		parent.SetActive(false);
		cam.transform.position = cameraGoalPos;
		cam.orthographicSize = cameraBattleSize;
		battle.transform.localScale = new Vector3(1, 1, 1);
		battle.GetComponentInChildren<BattleController>().zooming = false;
	}

	private IEnumerator ZoomOut(BattleController bc)
	{
		bc.zooming = true;
		GameObject battleParent = bc.gameObject.transform.parent.gameObject;
		battleParent.transform.localScale = new Vector3(BATTLE_SCALE, BATTLE_SCALE, 1);
		cam.orthographicSize = cameraNormalSize * 1.3f * BATTLE_SCALE;
		cameraGoalPos = new Vector3(0, 0, -10);
		cameraGoalSize = cameraNormalSize;
		for (float t = 0; t < ZOOM_TIME; t += Time.deltaTime)
		{
			cam.transform.position = Vector3.Lerp(cam.transform.position, cameraGoalPos, t);
			cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cameraGoalSize, t);
			yield return new WaitForEndOfFrame();
		}
		cam.transform.position = cameraGoalPos;
		cam.orthographicSize = cameraNormalSize;
		Destroy(battleParent);
		canvas.SetActive(true);
		selectedSquare.gameObject.SetActive(true);
		//musicObj.GetComponent<AudioSource>().Play();
	}

	private void ApplySelectedAction() {
		switch (selectedMode) {
		case ButtonMode.Mode.NUMBER:
			if (selectedSquare != null) {
				selectedSquare.notesVisible = false;
				if (selectedNumber != SudokuNumber.NONE) {
					selectedSquare.number = selectedNumber;
					selectedSquare.numberVisible = true;
				} else {
					buttonNumber.GetComponent<ButtonMode> ().PlantButtonOnImage ();
				}
			}
				
			break;
		case ButtonMode.Mode.NOTES:
			if (selectedSquare != null) {
				selectedSquare.numberVisible = false;
				if (selectedNumber != SudokuNumber.NONE) {
					selectedSquare.notes [(int)selectedNumber] = true;
					selectedSquare.notesVisible = true;
				} else {
					buttonNotes.GetComponent<ButtonMode> ().PlantButtonOnImage ();
				}
			}
			break;
		case ButtonMode.Mode.ERASE:
			if (selectedSquare != null) {
				selectedSquare.notesVisible = false;
				selectedSquare.numberVisible = false;
			}
			break;
		}
		UpdateBattleAvailability ();
	}	

	public SudokuNumber GetCorrectNumber() {
		return (SudokuNumber)((-1 * solution[selectedSquare.index]) - 1);
	}

	public void SetCorrectNumber() {
		print ("setting correct number");
		selectedSquare.number = GetCorrectNumber ();
		selectedSquare.hint = true;
		selectedSquare.numberVisible = true;
		selectedSquare.notesVisible = false;
	}

	private void CheckForWin() {
		bool won = true;
		foreach (SquareController square in squares) {
			if (!square.numberVisible || square.notesVisible || square.softConflicting || square.hardConflicting) {
				won = false;
			}
		}

		if (won) {
			Win ();
		}
	}

	private void Win() {
		
		Destroy (Parent.parent);
		SceneManager.LoadScene ("EndGame");
	}

	private void UpdateBattleAvailability() {
		if (selectedMode == ButtonMode.Mode.NOTES && selectedSquare != null) {
			int notesCount = 0;
			for (int idx = 0; idx < 9; idx++) {
				if (selectedSquare.notes [idx]) {
					notesCount += 1;
				}
			}

			if (!selectedSquare.lostBattle && !selectedSquare.hint && notesCount >= 2) {
				buttonBattle.GetComponent<ButtonBattle> ().glowEnabled = true;
				return;
			}
		}

		buttonBattle.GetComponent<ButtonBattle> ().glowEnabled = false;
	}

	private void UpdateConflicts() {
		foreach (SquareController square in squares) {
			square.softConflicting = false;
			square.hardConflicting = false;
		}

		foreach (SquareController square1 in squares) {
			foreach (SquareController square2 in GetConflicting(square1)) {
				if (square1 != square2 && square1.numberVisible && square2.numberVisible && square1.number.Equals (square2.number)) {
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

	private void UpdateSelected() {
		foreach (SquareController square in squares) {
			if (square == selectedSquare) {
				square.highlighted = true;
			} else {
				square.highlighted = false;
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
			if (solution [i] < 0) {
				square.numberVisible = false;
			} else {
				square.hint = true;
				square.numberVisible = true;
				square.lostBattle = false;
				square.number = (SudokuNumber)(solution [i] - 1);
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

	private void GenerateBoard() {
		bool success = false;
		generatedBoard = false;
		try {
			int random = Mathf.FloorToInt (Random.value * 1464.0f);
			char[] board = Resources.Load<TextAsset>("puzzles/" + random.ToString()).text.ToCharArray();
			print(board.Length);
			for (int i = 0; i < 81; i++) {
				char c = board[i];
				solution [i] = System.Int32.Parse (new string (c, 1));
			} 

			for (int i = 0; i < 81; i++) {
				print(solution[i]);
			}

			print("middle");

			for (int i = 82; i < 163; i++) {
				char c = board[i];
				if (solution[i - 82] == 0) {
					solution [i - 82] = -1 * System.Int32.Parse (new string (c, 1));
				}
			} 

			for (int i = 0; i < 81; i++) {
				print(solution[i]);
			}

			success = true;
		} catch (System.Exception e) {
			print (e.StackTrace);
		}

		generatedBoard = true;
	}

}
