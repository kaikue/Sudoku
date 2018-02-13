using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

	private Hurtable parent;

	void Start()
	{
		parent = gameObject.transform.parent.gameObject.GetComponent<Hurtable>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Attack s = collision.gameObject.GetComponent<Attack>();
		if (s != null && s.Faction != parent.faction)
		{
			parent.Damage(s.Damage);
		}
	}

}
