using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Hurtable {

	private const int MAX_HEALTH = 1;
	private const float SPEED = 2.0f;
	private Rigidbody2D rb;
	private GameObject player;

	protected override void Start()
	{
		base.Start();
		rb = GetComponent<Rigidbody2D>();
		faction = Faction.BAD;
		health = MAX_HEALTH;
		player = GameObject.Find("Player");
	}
	
	void FixedUpdate()
	{
		Vector2 pos = rb.position;
		Vector2 playerPos = player.transform.position;
		Vector2 posDiff = playerPos - pos;
		pos += posDiff.normalized * SPEED * Time.fixedDeltaTime;
		rb.MovePosition(pos);
	}

	protected override void Die()
	{
		player.GetComponent<Player>().EnemyDieSound();
		Destroy(gameObject);
	}
}
