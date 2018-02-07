using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public GameObject ShotPrefab;

	private string[] DIRECTIONS = { "Down", "Left", "Up", "Right" };
	private float SPEED = 6.0f;
	private float SHOT_SPEED = 10.0f;

	private BattleController bc;
	private Rigidbody2D rb;
	private SpriteRenderer sr;
	private bool shotQueued = false;
	private Vector2 facing = new Vector2(0, -1);
	private float health = 100;

	void Start()
	{
		bc = GameObject.Find("BattleController").GetComponent<BattleController>();
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) ||
				Input.GetKeyDown(KeyCode.JoystickButton0))
		{
			shotQueued = true;
		}
	}


	private int GetFacingIndex()
	{
		if (facing[0] == -1)
		{
			return 1; //left
		}
		else if (facing[0] == 1)
		{
			return 3; //right
		}
		else if (facing[1] == -1)
		{
			return 0; //down
		}
		else if (facing[1] == 1)
		{
			return 2; //up
		}
		return 0; //should never happen
	}

	void FixedUpdate()
	{
		
		Vector2 vel = new Vector2();

		float horiz = Input.GetAxisRaw("Horizontal");
		float vert = Input.GetAxisRaw("Vertical");

		vel.x = horiz * SPEED;
		vel.y = vert * SPEED;

		if (vert != 0)
		{
			facing = new Vector2(0, vert < 0 ? -1 : 1);
		}
		else if (horiz != 0)
		{
			facing = new Vector2(horiz < 0 ? -1 : 1, 0);
		}

		rb.velocity = vel;

		if (shotQueued)
		{
			shotQueued = false;
			CreateShot();
		}
	}

	private void CreateShot()
	{
		Vector2 shotVel = facing.normalized;
		shotVel *= SHOT_SPEED;
		shotVel += rb.velocity;
		
		//PlaySound("player_shoot");

		GameObject shot = Instantiate(ShotPrefab);
		shot.transform.position = gameObject.transform.position;
		//shot.transform.parent = gameObject.transform;
		shot.GetComponent<Rigidbody2D>().velocity = shotVel;
		//Shot = shot.GetComponent<Shot>();
		//Cough.cougher = this;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		TowerShot t = collision.gameObject.GetComponent<TowerShot>();
		if (t != null)
		{
			Damage(t.DAMAGE);
			Destroy(collision.gameObject);
		}
	}

	private void Damage(float damage)
	{
		health -= damage;
		if (health <= 0)
		{
			bc.Lose();
		}
	}
}
