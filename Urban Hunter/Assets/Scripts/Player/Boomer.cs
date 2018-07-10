using UnityEngine;
using System.Collections;

public class Boomer : MonoBehaviour {
	private int damage = 10;
	private float parm = 0f;
	private Transform boomerTransform;
	private Rigidbody2D boomerRigidBody;
	private WhipMasterHealth bossHealth;
	private BoxHealth boxHealth;
	private Vector2 spawnPosition;
	private Vector2 point1;
	private Vector2 point2;
	private Vector2 point3;
	private Transform playerTransform;
	private ScoreManager playerScore;

	void Awake()
	{
		boomerTransform = GetComponent<Transform> ();
		boomerRigidBody = GetComponent<Rigidbody2D> ();
		playerTransform = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
		playerScore = GameObject.FindGameObjectWithTag ("Score").GetComponent<ScoreManager> ();
		spawnPosition = new Vector2 (boomerTransform.position.x, boomerTransform.position.y);
		if (playerTransform.position.x < spawnPosition.x) {
			point1 = spawnPosition;
			point2 = new Vector2 (spawnPosition.x + 33f, spawnPosition.y + 0.92f); 
			point3 = new Vector2 (spawnPosition.x + 33f, spawnPosition.y - 6.1f); 

		} else {
			point1 = spawnPosition;
			point2 = new Vector2(spawnPosition.x - 33f, spawnPosition.y + 0.92f); 
			point3 = new Vector2(spawnPosition.x - 33f, spawnPosition.y -6.1f); 
		}
		Destroy (gameObject, 3f);
	}

	void Update()
	{
		if (parm <= 1f) {
			Vector2 point = calculateBezierPoint (point1, point2, point3, point1, parm);
			boomerTransform.position = point;
			parm += 0.01f;
		}
		
	}

	//determines path of boomerang
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

	void FixedUpdate()
	{
		boomerRigidBody.AddTorque (100f);
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("Enemy")) {
			EnemyHealthBase enemyHealth = other.GetComponent<EnemyHealthBase> ();
			if (enemyHealth != null)
				enemyHealth.Damage (damage);
			playerScore.IncreaseScore(250);
		} else if (other.CompareTag ("LowerCollider") || other.CompareTag ("UpperCollider")) {
			bossHealth = GameObject.FindGameObjectWithTag ("Boss").GetComponent<WhipMasterHealth> ();
			bossHealth.Damage (damage);
			playerScore.IncreaseScore(550);
		} else if (other.CompareTag ("BoxTwo")) {
			boxHealth =  GameObject.FindGameObjectWithTag("BoxTwo").GetComponent<BoxHealth> ();
			boxHealth.Damage(damage);
			playerScore.IncreaseScore(50);
		}//if-else
	}//OnTriggerEnter2D
}
