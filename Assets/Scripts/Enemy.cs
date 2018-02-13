using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Hurtable {

	private const int MAX_HEALTH = 1;
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
		//move towards player...
		rb.velocity = Vector2.down;
	}

	protected override void Die()
	{
		Destroy(gameObject);
	}
}
