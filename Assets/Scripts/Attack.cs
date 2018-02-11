using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	public float DAMAGE = 10;
	public Vector3 Offset;
	private float LIFESPAN = 0.5f;

	void Awake()
	{
		StartCoroutine(DelayDestroy());
	}

	private IEnumerator DelayDestroy()
	{
		yield return new WaitForSeconds(LIFESPAN);
		Destroy(gameObject);
	}
}
