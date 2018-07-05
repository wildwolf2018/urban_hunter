using UnityEngine;
using System.Collections;

public class BallMovement : MonoBehaviour {
	public int damage = 20;
	public ParticleSystem explosionEffect;
	private PlayerHealth playerHealth;
	private Transform ballTransform;
	private Vector2 point1;
	private Vector2 point2;
	private Vector2 point3;
	private Vector2 point4;
	public Transform p1;
	public Transform p2;
	public Transform p3;
	private Vector2 playerPos;
	private Transform ground;
	private float parm = 0f;
	private int ENEMY_LAYER_MASK = 10;
	
	void Awake () {
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
		ground = GameObject.FindGameObjectWithTag ("Feet").GetComponent<Transform> ();
		ballTransform = GetComponent<Transform> ();
		point1 = p1.position;// spwanPosition;
		point2 = p2.position;//new Vector2 (spwanPosition.x - 3.67f, spwanPosition.y + 0.5f); 
		point3 = p2.position;//new Vector2 (spwanPosition.x - 2.54f, spwanPosition.y + 0.42f); 
		point4 = new Vector2 (ground.position.x, ground.position.y);
		Invoke ("Explode", 3f);
	}//Awake
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if ((other.CompareTag ("TopCollider") || other.CompareTag ("BottomCollider")) ) {
			playerHealth.Damage (damage, 0f);
			explosionEffect.Stop ();
			explosionEffect.Play ();
			Destroy(gameObject, 0.2f);
		}
	}//OnTriggerEnter2Df
	
	void Explode()
	{
		if (gameObject.activeSelf) {
			explosionEffect.Stop ();
			explosionEffect.Play ();
			Destroy (gameObject, 0.2f);
		}
	}
	void Update()
	{
		Physics2D.IgnoreLayerCollision(ENEMY_LAYER_MASK, ENEMY_LAYER_MASK, true);
		if (parm <= 1f) {
			Vector2 point = calculateBezierPoint (point1, point2, point3, point4, parm);
			ballTransform.position = point;
			parm += 0.01f;
		}
		
	}
	
	Vector2 calculateBezierPoint(Vector2 p0, Vector2 p1, Vector2 p2,Vector2 p3, float t)
	{
		float a = 1 - t;
		float b = a * a;
		float a0 = b * a;//coefficient 1
		float a1 = 3 * b * t;//coefficient 2
		float a2 = 3 * a * t * t;//coefficient 3
		float a3 = t * t * t;//coefficient 4
		
		Vector2 result = a0 * p0;
		result += a1 * p1;
		result += a2 * p2;
		result += a3 * p3;
		
		return result;
	}
}
