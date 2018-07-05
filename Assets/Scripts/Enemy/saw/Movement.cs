using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	public CircleCollider2D sawCollider;
	private PlayerHealth playerHealth;
	private Transform sawTransform;
	private int damage = 1;
	public float angle = 0f;

	void Awake () 
	{
		sawTransform = GetComponent<Transform> ();
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
	}

	void OnEnable()
	{
		sawCollider.enabled = true;
	}
	
	void OnTriggerStay2D(Collider2D other)
	{
		
		if ((other.CompareTag ("TopCollider") || other.CompareTag ("BottomCollider")) ) {
			playerHealth.Damage (damage, 0f);
		}
	}//OnTriggerEnter2Df

	void FixedUpdate () 
	{
		sawTransform.rotation = Quaternion.Euler (0f, 0f, angle);
		angle -= 10f;
	}
}
