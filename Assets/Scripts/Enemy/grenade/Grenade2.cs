using UnityEngine;
using System.Collections;

public class Grenade2 : MonoBehaviour {
	public int damage = 100;
	public ParticleSystem explosionEffect;
	private PlayerHealth playerHealth;
	private GameObject temp;
	
	void Update()
	{
		temp = GameObject.FindGameObjectWithTag ("Player");
		if (temp != null) {
			playerHealth = temp.GetComponent<PlayerHealth> ();
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		
		if ((other.CompareTag ("TopCollider") || other.CompareTag ("BottomCollider")) ) {
			if(playerHealth != null){
				playerHealth.Damage (damage, 0f);
				explosionEffect.Stop ();
				explosionEffect.Play ();
				Destroy(gameObject, 0.1f);
			}
		}
	}//OnTriggerEnter2Df

}
