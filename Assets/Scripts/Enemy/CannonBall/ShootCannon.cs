using UnityEngine;
using System.Collections;

public class ShootCannon : MonoBehaviour {
	public float fireRate = 0.5f;
	public float nextFire = 0f;
	public float shootForce = 10f;
	public GameObject ball;
	private Vector2 spawnPoint1;
	private PlayerHealth playerHealth;
	
	void Awake()
	{
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
		spawnPoint1 = new Vector2(253.3f, -1.44f);
	}
	
	void Update()
	{
		if (Time.time > nextFire && !playerHealth.isDead) {
			nextFire = Time.time + fireRate;
			Instantiate (ball, spawnPoint1, Quaternion.identity);
		}
	}
}
