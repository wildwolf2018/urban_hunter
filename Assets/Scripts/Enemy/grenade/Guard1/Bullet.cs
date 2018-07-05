using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public int damage = 10;
	private PlayerHealth playerHealth;
	private CircleCollider2D bulletCollider;
	private Camera cam;

	void Awake () {
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
		bulletCollider = GetComponent<CircleCollider2D> ();
		cam = Camera.main;
	}//Awake

	void Update()
	{
		if (!CameraUtility.IsRendererInFrustum (bulletCollider, cam))
			Destroy (gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("TopCollider") || other.CompareTag ("BottomCollider")) {
			playerHealth.Damage (damage, 0f);
			Destroy(gameObject);
		}
	}//OnTriggerEnter2D
}
