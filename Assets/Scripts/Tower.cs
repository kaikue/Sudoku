using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Hurtable {

	public GameObject EnemyPrefab;
	public Sprite hurt1Sprite;
	public Sprite hurt2Sprite;
	public Sprite hurt3Sprite;

	private const int MAX_HEALTH = 4;
	private const float SPAWN_TIME = 3.0f;
	private const float ENEMY_DISTANCE = 2.0f;

	private BattleController bc;
	
	protected override void Start()
	{
		base.Start();
		bc = GameObject.Find("BattleController").GetComponent<BattleController>();
		faction = Faction.BAD;
		health = MAX_HEALTH;

		StartCoroutine(SpawnTimer());
	}
	
	private IEnumerator SpawnTimer()
	{
		int myNum = 0;
		int.TryParse(gameObject.name.Substring(5), out myNum);
		float startTime = (SPAWN_TIME * myNum) / 9;
		yield return new WaitForSeconds(startTime);
		while (true)
		{
			SpawnEnemy();
			yield return new WaitForSeconds(SPAWN_TIME);
		}
	}

	private void SpawnEnemy()
	{
		GameObject enemy = Instantiate(EnemyPrefab);
		enemy.transform.position = gameObject.transform.position + Vector3.down * ENEMY_DISTANCE;
		enemy.transform.parent = gameObject.transform.parent; //so it'll get destroyed if the battle is
	}
	
	public override void Damage(int damage)
	{
		base.Damage(damage);
		if (health == 3)
		{
			sr.sprite = hurt1Sprite;
		}
		else if (health == 2)
		{
			sr.sprite = hurt2Sprite;
		}
		else if (health == 1)
		{
			sr.sprite = hurt3Sprite;
		}
	}

	protected override void Die()
	{
		bc.DestroyTower(gameObject);
	}
}
