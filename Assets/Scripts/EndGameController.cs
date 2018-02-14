using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour {
	public KeyCode restartKey;
	public KeyCode quitKey;

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (restartKey)) {
			SceneManager.LoadScene ("Sudoku");
		} else if (Input.GetKey (quitKey)) {
			Application.Quit ();
		}
	}
}
