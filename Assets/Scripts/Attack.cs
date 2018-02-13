using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
	
	public int Damage = 1;
	public Hurtable.Faction Faction;
	public Vector3 Offset;
	
	public void StartDelayDestroy(float lifespan)
	{
		StartCoroutine(DelayDestroy(lifespan));
	}

	private IEnumerator DelayDestroy(float lifespan)
	{
		yield return new WaitForSeconds(lifespan);
		Destroy(gameObject);
	}
}
