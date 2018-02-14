using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Hurtable {

	public GameObject AttackPrefab;

	private string[] DIRECTIONS = { "Down", "Left", "Up", "Right" };
	private const float SPEED = 3.0f;
	private const float ATTACK_DISTANCE = 0.8f;
	private const float ATTACK_OFFSET = 0.5f;
	private const int MAX_HEALTH = 3;

	private BattleController bc;
	private Rigidbody2D rb;
	private Attack MyAttack = null;
	private bool attackQueued = false;
	private Vector2 facing = new Vector2(0, -1);

	protected override void Start()
	{
		base.Start();
		bc = GameObject.Find("BattleController").GetComponent<BattleController>();
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		faction = Faction.GOOD;
		health = MAX_HEALTH;
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) ||
				Input.GetKeyDown(KeyCode.JoystickButton0))
		{
			attackQueued = true;
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

		if (attackQueued)
		{
			attackQueued = false;
			if (MyAttack == null)
			{
				CreateAttack();
			}
		}

		if (MyAttack != null)
		{
			MyAttack.gameObject.transform.position = transform.position + MyAttack.Offset;
		}
	}

	private void CreateAttack()
	{
		Vector3 attackOffset = facing.normalized;
		attackOffset *= ATTACK_DISTANCE;
		attackOffset += ATTACK_OFFSET * Vector3.down;

		//PlaySound("player_shoot");

		GameObject attack = Instantiate(AttackPrefab);
		attack.transform.position = gameObject.transform.position + attackOffset;
		attack.transform.parent = gameObject.transform;
		//attack.transform.localScale = new Vector3(0.1f, 0.1f, 1);
		MyAttack = attack.GetComponent<Attack>();
		MyAttack.Offset = attackOffset;
		MyAttack.Faction = faction;
		MyAttack.StartDelayDestroy(0.15f);
	}

	protected override void Die()
	{
		bc.Lose();
	}
}
