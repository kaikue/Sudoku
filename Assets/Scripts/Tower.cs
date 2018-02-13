using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Hurtable {

	public GameObject ShotPrefab;

	private const int SHOT_TIME = 20;
	private const float SHOT_SPEED = 0.1f;
	private const float MAX_HEALTH = 5.0f;

	private BattleController bc;
	private GameObject player;
	
	private int shotTimer = SHOT_TIME;

	void Start()
	{
		bc = GameObject.Find("BattleController").GetComponent<BattleController>();
		player = GameObject.Find("Player");
		faction = Faction.BAD;
		health = MAX_HEALTH;
	}
	
	private void FixedUpdate()
	{
		shotTimer--;
		if (shotTimer <= 0)
		{
			shotTimer = SHOT_TIME;
			//CreateShot();
		}
	}

	/*private void CreateShot()
	{
		Vector2 playerDir = player.transform.position - gameObject.transform.position;
		Vector2 shotVel = playerDir.normalized;
		shotVel *= SHOT_SPEED;

		//PlaySound("player_shoot");

		GameObject shot = Instantiate(ShotPrefab);
		shot.transform.position = gameObject.transform.position;
		shot.GetComponent<Rigidbody2D>().velocity = shotVel;
	}*/
	
	protected override void Die()
	{
		bc.DestroyTower(gameObject);
	}
}
