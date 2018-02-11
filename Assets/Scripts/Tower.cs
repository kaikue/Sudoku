using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

	public GameObject ShotPrefab;

	private const int SHOT_TIME = 20;
	private const float SHOT_SPEED = 0.1f;

	private BattleController bc;
	private GameObject player;

	private float health = 100;
	private int shotTimer = SHOT_TIME;

	void Start()
	{
		bc = GameObject.Find("BattleController").GetComponent<BattleController>();
		player = GameObject.Find("Player");
	}
	
	private void FixedUpdate()
	{
		shotTimer--;
		if (shotTimer <= 0)
		{
			shotTimer = SHOT_TIME;
			CreateShot();
		}
	}

	private void CreateShot()
	{
		Vector2 playerDir = player.transform.position - gameObject.transform.position;
		Vector2 shotVel = playerDir.normalized;
		shotVel *= SHOT_SPEED;

		//PlaySound("player_shoot");

		GameObject shot = Instantiate(ShotPrefab);
		shot.transform.position = gameObject.transform.position;
		shot.GetComponent<Rigidbody2D>().velocity = shotVel;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Attack s = collision.gameObject.GetComponent<Attack>();
		if (s != null)
		{
			Damage(s.DAMAGE);
			//set invincible frames
		}
	}
	
	private void Damage(float damage)
	{
		health -= damage;
		if (health <= 0)
		{
			bc.DestroyTower(gameObject);
		}
	}
}
