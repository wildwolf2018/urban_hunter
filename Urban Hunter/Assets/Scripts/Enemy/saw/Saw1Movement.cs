using UnityEngine;
using System.Collections;

public class Saw1Movement : MonoBehaviour {
	public float deceleration = 3f;
	public float tweaker = 0.3f;
	public Movement sawHead;

	private Rigidbody2D rdb2;
	private Transform sawTransform;
	private Transform waypoint6;
	private Vector2 targetPos;
	private Quaternion targetRotation;
	private PlayerHealth playerHealth;
	private SteeringBehaviour seek;
    private float maxSpeed = 0.5f;


    void Awake () 
	{
		sawTransform = GetComponent<Transform> ();
		waypoint6 = GameObject.Find ("Waypoint6").GetComponent<Transform> ();
		targetPos = new Vector2 (waypoint6.position.x + 10f, waypoint6.position.y);
		targetRotation = Quaternion.Euler (0f, 0f, 0f);
		rdb2 = GetComponent<Rigidbody2D> ();
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
		seek = ScriptableObject.CreateInstance ("SteeringBehaviour") as SteeringBehaviour;
		seek.tweaker = tweaker;
		seek.deceleration = deceleration;
		seek.maxSpeed = maxSpeed;
		seek.rdb = rdb2;
	}
	

	void Update () 
	{
		if (!playerHealth.isDead) {
			if (sawTransform.rotation == targetRotation)
				sawHead.enabled = true;
			sawTransform.rotation = Quaternion.Slerp (sawTransform.localRotation, targetRotation, Time.deltaTime * 2f);
			targetPos.y = rdb2.position.y;
			rdb2.position += seek.seekAndArrive (targetPos) * Time.deltaTime;
		}
		else
			Physics2D.IgnoreLayerCollision(9, 17, true);
	}
}
