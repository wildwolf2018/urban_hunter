using UnityEngine;
using System.Collections;

public class HelicopterMovement : MonoBehaviour {
	public GameObject helliMissile;
	public Transform spawnpoint1;
	private Transform waypoint1;
	private Transform waypoint2;
	private Transform helliTransform;
	private Vector2 targetLocation;
	private PlayerHealth playerHealth;
	private Rigidbody2D rdb2;
	private SteeringBehaviour seek;
	private float yPos;
	private bool faceRight = false;
	private bool goLeft = true;
	private Quaternion target;
	public float elapsedTime;
	public float maxSpeed = 4f;
	public float deceleration = 3f;
	public float tweaker = 0.3f;
	public float goRate = 2f;
	public int damageHit = 100;
	public float fireRate = 1f;
	public float nextFire = 0f;
	
	void Awake()
	{
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
		waypoint1 = GameObject.Find ("Waypoint7").GetComponent<Transform> ();
		waypoint2 = GameObject.Find ("Waypoint8").GetComponent<Transform> ();
		helliTransform = GetComponent<Transform> ();
		rdb2 = GetComponent<Rigidbody2D> ();
		seek = ScriptableObject.CreateInstance ("SteeringBehaviour") as SteeringBehaviour;
	}
	
	void Start()
	{
		targetLocation = waypoint1.position;
		targetLocation.y = 0f;
		seek.tweaker = tweaker;
		seek.deceleration = deceleration;
		seek.maxSpeed = maxSpeed;
		seek.rdb = rdb2;
		yPos = helliTransform.position.y;
		target = Quaternion.Euler(0f, 0f, 0f);
		goLeft = false;
		faceRight = false;
	}
	
	void Update()
	{
		helliTransform.rotation = Quaternion.Slerp (helliTransform.localRotation, target, Time.deltaTime * 10f);
		helliTransform.position = new Vector2(helliTransform.position.x, yPos);
	}
	void FixedUpdate () 
	{
		targetLocation.y = rdb2.position.y;
		rdb2.position += seek.seekAndArrive (targetLocation) * Time.deltaTime;
		if (Time.time > nextFire && Vector2.Distance (helliTransform.position, targetLocation) > 1f) {
			nextFire = Time.time + fireRate;
			Instantiate (helliMissile, spawnpoint1.position , Quaternion.identity) ;
		}
		if (Vector2.Distance (helliTransform.position, targetLocation) < 0.25f) {
			elapsedTime += Time.deltaTime;
			if(elapsedTime > goRate)
			{
				if(faceRight)
				{
					goLeft = true;
					targetLocation = waypoint1.position;
					ChangeDirection();
				}
				else
				{
					goLeft = false;
					targetLocation = waypoint2.position;
					ChangeDirection();
				}
				elapsedTime = 0f;		
			}
			
		}
		
	}
	void ChangeDirection (){
		if (!faceRight && !goLeft) 	
			target = Quaternion.Euler(0f, 180f, 0f);
		else if (faceRight && goLeft) 
			target = Quaternion.Euler(0f, 0f, 0f);
		faceRight = !faceRight;
	}//ChangeDirection
	
	void OnTriggerEnter2D(Collider2D other)
	{ 
		if (other.CompareTag ("TopCollider") || other.CompareTag ("BottomCollider")) {
			{
				playerHealth.Damage (damageHit, 0);
			}
		}
	}//OnTriggerEnter2D

}
