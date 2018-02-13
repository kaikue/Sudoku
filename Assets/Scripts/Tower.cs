using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Hurtable {

	public GameObject EnemyPrefab;

	private const int MAX_HEALTH = 5;
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
		while (true)
		{
			yield return new WaitForSeconds(SPAWN_TIME);
			SpawnEnemy();
		}
	}

	private void SpawnEnemy()
	{
		GameObject enemy = Instantiate(EnemyPrefab);
		enemy.transform.position = gameObject.transform.position + Vector3.down * ENEMY_DISTANCE;
	}
	
	protected override void Die()
	{
		bc.DestroyTower(gameObject);
	}
}
