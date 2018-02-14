using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

	public Hurtable parent;

	void Start()
	{
		parent = gameObject.transform.parent.gameObject.GetComponent<Hurtable>();
	}

}
