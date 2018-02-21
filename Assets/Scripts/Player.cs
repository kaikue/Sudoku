using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Hurtable {

	public GameObject AttackPrefab;
	public AudioClip attackClip;
	public AudioClip hurtClip;

	private const float SPEED = 3.0f;
	private const float ATTACK_DISTANCE = 0.9f;
	private const float ATTACK_OFFSET_X = -0.4f;
	private const float ATTACK_OFFSET_Y = -0.6f;
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

		if (MyAttack != null || bc.zooming) //can't move while attacking
		{
			horiz = 0;
			vert = 0;
		}

		vel.x = horiz * SPEED;
		vel.y = vert * SPEED;

		if (vert != 0)
		{
			bc.Instructions.SetActive(false);
			facing = new Vector2(0, vert < 0 ? -1 : 1);
		}
		else if (horiz != 0)
		{
			bc.Instructions.SetActive(false);
			facing = new Vector2(horiz < 0 ? -1 : 1, 0);
		}

		rb.velocity = vel;

		if (attackQueued)
		{
			bc.Instructions.SetActive(false);
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
		attackOffset += new Vector3(ATTACK_OFFSET_X, ATTACK_OFFSET_Y, 0);
		
		gameObject.GetComponent<AudioSource>().PlayOneShot(attackClip);

		GameObject attack = Instantiate(AttackPrefab);
		attack.transform.position = gameObject.transform.position + attackOffset;
		attack.transform.parent = gameObject.transform;
		//attack.transform.localScale = new Vector3(0.1f, 0.1f, 1);
		MyAttack = attack.GetComponent<Attack>();
		MyAttack.Offset = attackOffset;
		MyAttack.Faction = faction;
		if (facing.x > 0 || facing.y > 0)
		{
			MyAttack.FlipSprite();
		}
		MyAttack.StartDelayDestroy(0.15f);
	}

	public override void Damage(int damage)
	{
		base.Damage(damage);
		gameObject.GetComponent<AudioSource>().PlayOneShot(hurtClip);
	}

	protected override void Die()
	{
		bc.Lose();
	}
}
