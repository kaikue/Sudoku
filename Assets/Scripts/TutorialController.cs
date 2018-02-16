using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour {

	public KeyCode continueKey;
	public GameObject loadingScreen;
	
	void Update ()
	{
		if (Input.GetKey (continueKey))
		{
			loadingScreen.SetActive(true);
			gameObject.GetComponent<AudioSource>().Play();
			SceneManager.LoadScene("Sudoku");
		}
	}
}
