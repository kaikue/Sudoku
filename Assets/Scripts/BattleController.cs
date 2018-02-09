using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleController : MonoBehaviour {

	public GameObject player;

	private GameObject parent;

	void Start() {
	}

	public void DestroyTower(GameObject tower)
	{
		Destroy(tower);
		//TODO: Fix
		if (true || GameObject.FindGameObjectsWithTag("Tower").Length == 0)
		{
			Win();
		}
	}

	public void Win()
	{
		print("You win!");
		GameObject parent = Parent.parent;
		parent.SetActive (true);
		SudokuController sudoku = parent.transform.GetChild (0).gameObject.GetComponent<SudokuController> ();
		sudoku.SetCorrectNumber ();
		SceneManager.UnloadSceneAsync ("Battle");
	}

	public void Lose()
	{
		print("You lose!");
		GameObject parent = Parent.parent;
		parent.SetActive (true);
		SceneManager.UnloadSceneAsync ("Battle");
	}
}
