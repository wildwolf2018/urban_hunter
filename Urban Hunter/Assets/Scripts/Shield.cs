using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
	public bool protection = false;
	public float fireRate = 0.5f;
	public float nextFire = 0f;

	void OnTriggerEnter2D(Collider2D other)
	{ 
		
		if (other.CompareTag ("TopCollider") || other.CompareTag ("BottomCollider")) {

			Destroy(gameObject);
		}
	}

	void Update () {
		if (Time.time > nextFire) {
			nextFire = Time.time + fireRate;

		}
	}
}
