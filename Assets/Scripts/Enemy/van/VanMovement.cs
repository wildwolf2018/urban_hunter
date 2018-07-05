using UnityEngine;
using System.Collections;

public class VanMovement : MonoBehaviour {
	public GameObject guard1;
	private Transform waypoint1;
	private Transform waypoint2;
	private Transform vanTransform;
	private Vector2 targetLocation;
	private PlayerHealth playerHealth;
	private Rigidbody2D rdb2;
	private SteeringBehaviour seek;
	private float yPos;
	private bool faceRight = false;
	private bool goLeft;
	private Quaternion target;
	private int ENEMY_LAYER_MASK = 10;
	public float elapsedTime;
	public float maxSpeed = 4f;
	public float deceleration = 3f;
	public float tweaker = 0.3f;
	public float goRate = 2f;
	public int damageHit = 100;
	public BoxCollider2D vanTopCollider;
	private  float VAN = -1f;
	private LevelManager levelManager;


	void Awake()
	{
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
		waypoint1 = GameObject.Find ("Waypoint1").GetComponent<Transform> ();
		waypoint2 = GameObject.Find ("Waypoint2").GetComponent<Transform> ();
		vanTransform = GetComponent<Transform> ();
		rdb2 = GetComponent<Rigidbody2D> ();
		levelManager = GameObject.Find ("GameManager").GetComponent<LevelManager> ();
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
		yPos = vanTransform.position.y;
		target = Quaternion.Euler(0f, 180f, 0f);
		goLeft = false;
		faceRight = false;
	}

	void Update()
	{
		transform.rotation = Quaternion.Slerp (transform.localRotation, target, Time.deltaTime * 10f);
		transform.position = new Vector2(transform.position.x, yPos);
		Physics2D.IgnoreLayerCollision(ENEMY_LAYER_MASK, ENEMY_LAYER_MASK, true);
		if (Mathf.Abs(transform.localRotation.y) >= 1f)
			VAN = -1f;
		else 
			VAN = 1f;
	}
	void FixedUpdate () 
	{
		targetLocation.y = rdb2.position.y;
		rdb2.position += seek.seekAndArrive (targetLocation) * Time.deltaTime;
		if (Vector2.Distance (transform.position, targetLocation) < 0.25) {
		    	ChangeDirection();
			elapsedTime += Time.deltaTime;
			if(elapsedTime > goRate)
			{
				if(faceRight)
				{
					goLeft = true;
					targetLocation = waypoint2.position;
				    levelManager.objects.Add(Instantiate (guard1, new Vector2(waypoint1.position.x, waypoint1.position.y) , Quaternion.identity) as GameObject) ;
				}
				else
				{
					goLeft = false;
					targetLocation = waypoint1.position;
					levelManager.objects.Add(Instantiate (guard1, new Vector2(waypoint2.position.x, waypoint2.position.y), Quaternion.identity) as GameObject);
				}
				elapsedTime = 0f;

			}

		}

	}
	void ChangeDirection (){
		if (!faceRight && !goLeft) 	
			target = Quaternion.Euler(0f, 0f, 0f);
		else if (faceRight && goLeft) 
			target = Quaternion.Euler(0f, 180f, 0f);
		faceRight = !faceRight;
	}//ChangeDirection

	void OnTriggerEnter2D(Collider2D other)
	{ 

		if (other.CompareTag ("TopCollider") || other.CompareTag ("BottomCollider")) {
			playerHealth.Damage (damageHit, VAN);
			Invoke("stop", 0.1f);
		}
	}//OnTriggerEnter2D

	void stop()
	{
		gameObject.GetComponent<VanMovement> ().enabled = false;
	}

}
