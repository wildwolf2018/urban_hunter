using UnityEngine;
using System.Collections;

public class FireDamge : MonoBehaviour {
	public int damage = 30;
	private PlayerHealth playerHealth;
	private GameObject tempPlayer;

	
	void OnTriggerEnter2D(Collider2D other)
	{
		
		if ((other.CompareTag ("TopCollider") || other.CompareTag ("BottomCollider"))) {
			if(GameObject.FindGameObjectWithTag ("Player") != null){
				playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
				playerHealth.Damage (damage, 0f);
			}
		} else if (other.CompareTag ("Enemy")) {
			EnemyHealthBase enemyHealth = other.GetComponent<EnemyHealthBase>();
			if(enemyHealth != null)
				enemyHealth.Damage(20);
		}
	}//OnTriggerEnter2Df

}
