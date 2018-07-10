using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {
	public int damage = 10;
	public LayerMask groundLayer;
	public BoxCollider2D missileCollider;
	private PlayerHealth playerHealth;
	private Transform missileTranform;
	public ParticleSystem explosionEffect;
	
	void Awake () {
		explosionEffect = gameObject.GetComponentInChildren<ParticleSystem> ();
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
	}

	void Update () {
		if (missileCollider.IsTouchingLayers (groundLayer)) {
			explosionEffect.Stop ();
			explosionEffect.Play ();
			Destroy (gameObject,0.1f);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("TopCollider") || other.CompareTag ("BottomCollider")) {
			playerHealth.Damage (damage, 0f);
			explosionEffect.Stop ();
			explosionEffect.Play ();
			Destroy (gameObject,0.1f);
		}
	}//OnTrig
}
