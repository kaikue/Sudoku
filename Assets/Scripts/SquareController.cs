using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SudokuNumber {
	ONE,
	TWO,
	THREE,
	FOUR,
	FIVE,
	SIX,
	SEVEN,
	EIGHT,
	NINE,
	NONE
}

public class SquareController : MonoBehaviour {
	private readonly static Color COLOR_NUMBER_HINT = new Color (1.0f, 1.0f, 1.0f, 1.0f);
	private readonly static Color COLOR_NUMBER_NOT_HINT = new Color (0.2f, 0.2f, 0.6f, 1.0f);
	private readonly static Color COLOR_NUMBER_HARD_CONFLICTING = new Color (0.6f, 0.2f, 0.2f, 1.0f);
	private readonly static Color COLOR_NUMBER_SOFT_CONFLICTING = new Color (0.6f, 0.2f, 0.2f, 0.8f);

	// State variables
	public AudioSource audioFail;
	public AudioSource audioPencil;
	public AudioSource audioNote;
	public AudioSource audioErase;
	public SudokuNumber number;
	public bool[] notes;
	public bool numberVisible;
	public bool notesVisible;
	public bool hint; // fixed at start
	public bool highlighted;
	public bool hardConflicting;
	public bool softConflicting;
	public bool lostBattle;

	// Fixed by GameController
	public int index;

	private bool hardConflictingLastFrame;
	private bool numberVisibleLastFrame;
	private SudokuNumber numberLastFrame;
	private int notesCountLastFrame;

	private SpriteRenderer srNumber;
	private SpriteRenderer srLostIndicator;
	private Sprite[] gameNumberSprites;
	private Sprite[] userNumberSprites;

	// Use this for initialization
	void Start () {
		notes = new bool[9];

		srNumber = GetComponent<SpriteRenderer> ();
		srLostIndicator = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer> ();
		gameNumberSprites = new Sprite[9];
		userNumberSprites = new Sprite[9];
		LoadNumberSprites ();
	}

	// Update is called once per frame
	void LateUpdate () {
		int notesCount = 0;
		for (int idx = 0; idx < 9; idx++) {
			if (notes [idx]) {
				notesCount++;
			}
		}

		if (notesVisible) {
			for (int idx = 0; idx < 9; idx++) {
				transform.GetChild (idx + 1).gameObject.GetComponent<SpriteRenderer> ().enabled = notes[idx];						
			}

			if (notesCount > notesCountLastFrame) {
				audioNote.Play();
			}

			srNumber.sprite = null;
		} else {
			for (int idx = 0; idx < 9; idx++) {
				notes [idx] = false;
				transform.GetChild (idx + 1).gameObject.GetComponent<SpriteRenderer> ().enabled = false;						
			}

			if (numberVisible && number != SudokuNumber.NONE) {
				if (!hint && (!numberVisibleLastFrame || number != numberLastFrame) && !hardConflicting) {
					audioPencil.Play ();
				} else {
					if (hint) {
						srNumber.sprite = gameNumberSprites [(int)number];
						srNumber.color = COLOR_NUMBER_HINT;
					} else {
						srNumber.sprite = userNumberSprites [(int)number];
						srNumber.color = COLOR_NUMBER_NOT_HINT;
					}
				}
			} else {
				srNumber.sprite = null;
			}
		}

		if (hardConflicting) {
			if (!hardConflictingLastFrame) {
				audioFail.Play ();
			}
			srNumber.color = COLOR_NUMBER_HARD_CONFLICTING;

		} else if (softConflicting) {
			srNumber.color = COLOR_NUMBER_SOFT_CONFLICTING;
		}

		if (lostBattle) {
			srLostIndicator.color = Color.white;
		} else {
			srLostIndicator.color = Color.clear;
		}


		hardConflictingLastFrame = hardConflicting;
		numberVisibleLastFrame = numberVisible;
		numberLastFrame = number;
		notesCountLastFrame = notesCount;

	}

	private void LoadNumberSprites() {
		for (SudokuNumber number = SudokuNumber.ONE; number <= SudokuNumber.NINE; number++) {
			userNumberSprites [(int)number] = Resources.Load<Sprite> ("user_numbers/" + number.ToString ());
			gameNumberSprites [(int)number] = Resources.Load<Sprite> ("game_numbers/" + number.ToString ());
		}
	}
}
