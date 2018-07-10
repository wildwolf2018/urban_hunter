using UnityEngine;
using System.Collections;

public class GrenadeMove : MonoBehaviour {
	public ParticleSystem explosionEffect;
	public int damage = 20;
	public float throwForce = 5f;

	private Rigidbody2D rdb2;
	private Transform bossTransform;
	private PlayerHealth playerHealth;
	private Vector2 velocity;
	private BoxCollider2D grenadeCollider;
	private Camera cam;

	void Awake () 
	{
		rdb2 = GetComponent<Rigidbody2D> ();
		bossTransform = GameObject.FindGameObjectWithTag ("Boss").GetComponent<Transform> ();
		velocity = -1 * bossTransform.right;
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
		grenadeCollider = GetComponent<BoxCollider2D>();
		cam = Camera.main;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		
		if ((other.CompareTag ("TopCollider") || other.CompareTag ("BottomCollider")) ) {
			playerHealth.Damage (damage, 0f);
			explosionEffect.Stop ();
			explosionEffect.Play ();
			Destroy(gameObject, 0.1f);
		}
	}//OnTriggerEnter2Df

	void Update()
	{
		if (!CameraUtility.IsRendererInFrustum (grenadeCollider, cam))
			Destroy (gameObject);
	}
	void FixedUpdate()
	{
		rdb2.MovePosition (rdb2.position + velocity * 0.8f);
	}
}
