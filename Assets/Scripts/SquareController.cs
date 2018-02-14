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
	private readonly static Color COLOR_NUMBER_NOT_HINT = new Color (1.0f, 1.0f, 1.0f, 0.75f);
	private readonly static Color COLOR_BACKGROUND_NOT_CONFLICTING = new Color (1.0f, 1.0f, 1.0f, 1.0f);
	private readonly static Color COLOR_BACKGROUND_HARD_CONFLICTING = new Color (1.0f, 0.0f, 0.0f, 1.0f);
	private readonly static Color COLOR_BACKGROUND_SOFT_CONFLICTING = new Color (1.0f, 1.0f, 0.0f, 1.0f);

	// State variables
	public AudioSource audioFail;
	public AudioSource audioPencil;
	public AudioSource audioNote;
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
	private int notesCountLastFrame;

	private SpriteRenderer srNumber;
	private SpriteRenderer srBackground;
	private Sprite[] gameNumberSprites;
	private Sprite[] userNumberSprites;

	// Use this for initialization
	void Start () {
		notes = new bool[9];

		srNumber = GetComponent<SpriteRenderer> ();
		srBackground = transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ();
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

			if (numberVisible) {
				if (!hint && !numberVisibleLastFrame && !hardConflicting) {
					audioPencil.Play ();
				} else {
					if (hint) {
						srNumber.sprite = gameNumberSprites [(int)number];
					} else {
						srNumber.sprite = userNumberSprites [(int)number];
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

			srBackground.color = COLOR_BACKGROUND_HARD_CONFLICTING;
		} else if (softConflicting) {
			srBackground.color = COLOR_BACKGROUND_SOFT_CONFLICTING;
		} else {
			srBackground.color = COLOR_BACKGROUND_NOT_CONFLICTING;
		}

		if (highlighted) {
			srBackground.color = highlightColor(srBackground.color);
		}

		hardConflictingLastFrame = hardConflicting;
		numberVisibleLastFrame = numberVisible;
		notesCountLastFrame = notesCount;

	}

	private static Color highlightColor(Color c) {
		return new Color (c.r * 0.8f, c.g * 0.8f, c.b, c.a);
	}

	private void LoadNumberSprites() {
		for (SudokuNumber number = SudokuNumber.ONE; number <= SudokuNumber.NINE; number++) {
			userNumberSprites [(int)number] = Resources.Load<Sprite> ("user_numbers/" + number.ToString ());
			gameNumberSprites [(int)number] = Resources.Load<Sprite> ("game_numbers/" + number.ToString ());
		}
	}
}
