using UnityEngine;
using System.Collections;

public class GlockBullet : MonoBehaviour {
	public LayerMask bossLayerMask;
	public float radius = 0.5f;
	public ParticleSystem explosionEffect;

	private BoxCollider2D bulletCollider;
	private WhipMasterHealth bossHealth;
	private BoxHealth boxHealth;
	int damagePerShot = 10;
	private Camera cam;

	void Awake()
	{
	    explosionEffect = gameObject.GetComponentInChildren<ParticleSystem> ();
		bulletCollider = GetComponent<BoxCollider2D> ();
		cam = Camera.main;
	}

	void Update()
	{
		Physics2D.IgnoreLayerCollision (11, 16);
		if (!CameraUtility.IsRendererInFrustum (bulletCollider, cam))
			Destroy (gameObject);
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("Enemy")) {
            EnemyHealthBase enemyHealth = other.GetComponent<EnemyHealthBase> ();
			if (enemyHealth != null)
				enemyHealth.Damage (damagePerShot);
			explosionEffect.Play ();
			Destroy (gameObject, 0.1f);
		} else if (other.CompareTag ("LowerCollider")) {
			bossHealth = GameObject.FindGameObjectWithTag ("Boss").GetComponent<WhipMasterHealth> ();
			bossHealth.Damage (damagePerShot);
			explosionEffect.Play ();
			Destroy (gameObject, 0.1f);
		} else if (other.CompareTag ("UpperCollider")) {
			explosionEffect.Play ();
			Destroy (gameObject, 0.1f);
		} else if (other.CompareTag ("BoxTwo")) {
			boxHealth =  GameObject.FindGameObjectWithTag("BoxTwo").GetComponent<BoxHealth> ();
			boxHealth.Damage(damagePerShot);
			explosionEffect.Play ();
			Destroy (gameObject, 0.1f);
		}
	}
}
