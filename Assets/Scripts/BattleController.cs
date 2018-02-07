using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour {

	public GameObject player;
	
	public void DestroyTower(GameObject tower)
	{
		Destroy(tower);
		if (GameObject.FindGameObjectsWithTag("Tower").Length == 0)
		{
			Win();
		}
	}

	public void Win()
	{
		print("You win!");
	}

	public void Lose()
	{
		print("You lose!");
	}
}
