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
	NINE
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
	public bool cleared;
	public bool noted;
	public bool hint; // fixed at start
	public bool highlighted;
	public bool hardConflicting;
	public bool softConflicting;

	// Fixed by GameController
	public int index;

	private bool hardConflictingLastFrame;
	private bool clearedLastFrame;
	private int notesCountLastFrame;

	private SpriteRenderer srNumber;
	private SpriteRenderer srBackground;
	private Sprite[] inkNumberSprites;
	private Sprite[] pencilNumberSprites;

	// Use this for initialization
	void Start () {
		notes = new bool[9];

		srNumber = GetComponent<SpriteRenderer> ();
		srBackground = transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ();
		inkNumberSprites = new Sprite[9];
		pencilNumberSprites = new Sprite[9];
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

		if (noted) {
			for (int idx = 0; idx < 9; idx++) {
				if (notes [idx]) {
					transform.GetChild (idx + 1).gameObject.GetComponent<SpriteRenderer> ().enabled = true;						
				} else {
					transform.GetChild (idx + 1).gameObject.GetComponent<SpriteRenderer> ().enabled = false;						
				}
			}

			if (notesCount > notesCountLastFrame) {
				audioNote.Play();
				print ("note");
			}

			srNumber.sprite = null;
		} else if (cleared) {
			for (int idx = 0; idx < 9; idx++) {
				transform.GetChild (idx + 1).gameObject.GetComponent<SpriteRenderer> ().enabled = false;						
			}

			srNumber.sprite = null;
		} else {
			if (clearedLastFrame && !hardConflicting) {
				audioPencil.Play ();
				print ("pencil");
			}

			for (int idx = 0; idx < 9; idx++) {
				transform.GetChild (idx + 1).gameObject.GetComponent<SpriteRenderer> ().enabled = false;						
			}

			if (hint) {
				srNumber.sprite = inkNumberSprites [(int)number];
			} else {
				srNumber.sprite = pencilNumberSprites [(int)number];
			}
		}

		if (hardConflicting) {
			if (!hardConflictingLastFrame) {
				audioFail.Play ();
				print ("fail");
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
		clearedLastFrame = cleared;
		notesCountLastFrame = notesCount;

	}

	private static Color highlightColor(Color c) {
		return new Color (c.r * 0.5f, c.g * 0.5f, c.b, c.a);
	}

	private void LoadNumberSprites() {
		for (SudokuNumber number = SudokuNumber.ONE; number <= SudokuNumber.NINE; number++) {
			pencilNumberSprites [(int)number] = Resources.Load<Sprite> ("pencil_numbers/" + number.ToString ());
			inkNumberSprites [(int)number] = Resources.Load<Sprite> ("ink_numbers/" + number.ToString ());
		}
	}
}
