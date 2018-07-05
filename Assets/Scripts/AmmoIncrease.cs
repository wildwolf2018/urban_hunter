using UnityEngine;
using System.Collections;

public class AmmoIncrease : MonoBehaviour {
	public int ammoIncrease = 30;
	public LayerMask playerLayerMask;

	private int extrasLayerMask = 20;
	private int enemyLayerMask = 10;

	private AmmoMananger playerAmmo;
	private BoxCollider2D ammoCollider;
	
	void Awake()
	{
		playerAmmo = GameObject.FindGameObjectWithTag ("Ammo").GetComponent<AmmoMananger> ();
		ammoCollider = GetComponent<BoxCollider2D> ();
	}

	void Update()
	{
		if (Physics2D.IsTouchingLayers (ammoCollider, playerLayerMask)) {
			playerAmmo.IncreaseAmmo (ammoIncrease);
			Destroy(gameObject);
		}
		Physics2D.IgnoreLayerCollision(extrasLayerMask, enemyLayerMask);
		Physics2D.IgnoreLayerCollision(extrasLayerMask, extrasLayerMask);
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{ 
		
		if (other.CompareTag ("TopCollider") || other.CompareTag ("BottomCollider")) {
			playerAmmo.IncreaseAmmo (ammoIncrease);
			Destroy(gameObject);
		}
	}//OnTriggerEnter2D
}
