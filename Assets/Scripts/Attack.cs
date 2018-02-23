using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
	
	public int Damage = 1;
	public Hurtable.Faction Faction;
	public Vector3 Offset;
	public Sprite flippedSprite;
	public Sprite upSprite;

	public void SetSprite(Sprite sp)
	{
		gameObject.GetComponent<SpriteRenderer>().sprite = sp;
	}

	public void StartDelayDestroy(float lifespan)
	{
		StartCoroutine(DelayDestroy(lifespan));
	}

	private IEnumerator DelayDestroy(float lifespan)
	{
		yield return new WaitForSeconds(lifespan);
		Destroy(gameObject);
	}
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Hitbox h = collision.gameObject.GetComponent<Hitbox>();
		if (h != null && Faction != h.parent.faction)
		{
			h.parent.Damage(Damage);
		}
	}
}
