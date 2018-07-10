using UnityEngine;
using System.Collections;

public class DogMovement : MonoBehaviour {
	public float elapsedTime;
	public float runRate;
	public float maxSpeed = 4f;
	public float deceleration = 3f;
	public float tweaker = 0.3f;
	public float goRate = 2f;
	public int damageHit = 10;

	private Transform dogTransform;
	private Vector2 targetLocation;
	private PlayerHealth playerHealth;
	private Rigidbody2D rdb2;
	private SteeringBehaviour seek;
	private float yPos;
	private bool faceRight = false;
	private bool goLeft = true;
	private Quaternion targetRotation;
	private int ENEMY_LAYER_MASK = 10;
	private Transform playerTransform;
	private Animator anim;
	
	void Awake()
	{
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
        dogTransform = GetComponent<Transform> ();
		rdb2 = GetComponent<Rigidbody2D> ();
		seek = ScriptableObject.CreateInstance ("SteeringBehaviour") as SteeringBehaviour;
	}
	
	void Start()
	{
		//targetLocation.y = 0f;
		seek.tweaker = tweaker;
		seek.deceleration = deceleration;
		seek.maxSpeed = maxSpeed;
		seek.rdb = rdb2;
		if (dogTransform.position.x < playerTransform.position.x)
		{
			targetLocation = new Vector2(playerTransform.position.x + 10f, dogTransform.position.y);
			goLeft = false;
			faceRight = false;
			ChangeDirection();
		}
		else if (dogTransform.position.x > playerTransform.position.x)
		{
			targetLocation = new Vector2(playerTransform.position.x - 1f, dogTransform.position.y);
			goLeft = true;
			faceRight = false;
			targetRotation = Quaternion.Euler(0f, 180f, 0f);
		}
		yPos = dogTransform.position.y;
		anim.SetBool ("run", true);
	}
	
	void Update()
	{
		
		if (playerHealth != null && !playerHealth.isDead) {
			dogTransform.rotation = Quaternion.Slerp (dogTransform.localRotation, targetRotation, Time.deltaTime * 10f);
		//	dogTransform.position = new Vector2 (dogTransform.position.x, yPos);
			Physics2D.IgnoreLayerCollision (ENEMY_LAYER_MASK, ENEMY_LAYER_MASK, true);
		} else
			anim.SetBool ("run", false);
	}
	void FixedUpdate () 
	{
		if(playerTransform != null){
			dogTransform.position = new Vector2 (dogTransform.position.x, yPos);
			if (Mathf.Abs (dogTransform.position.x - targetLocation.x) > 0.25f && goLeft) {
				targetLocation = new Vector2 (playerTransform.position.x - 3f, yPos);
				anim.SetBool ("run", true);
			}
			if (Mathf.Abs (dogTransform.position.x - targetLocation.x) > 0.25f && !goLeft) {
				targetLocation = new Vector2 (playerTransform.position.x + 3f, dogTransform.position.y);
				anim.SetBool ("run", true);
			}
			if (Mathf.Abs (dogTransform.position.x - targetLocation.x) <= 0.25f) {
				anim.SetBool ("run", false);
				if (dogTransform.position.x < playerTransform.position.x)
					goLeft = false;
				else 
					goLeft = true;
				elapsedTime += Time.deltaTime;
			if (elapsedTime > runRate && !playerHealth.isDead) {
					run ();
					elapsedTime = 0f;
				} 
			}
			rdb2.position += seek.seekAndArrive (targetLocation) * Time.deltaTime;
		}
	}

	void ChangeDirection (){
        if (!faceRight && !goLeft)
            targetRotation = Quaternion.Euler(0f, 0f, 0f);
		else if (faceRight && goLeft) 
			targetRotation = Quaternion.Euler(0f, 180f, 0f);
		faceRight = !faceRight;
	}//ChangeDirection

	void run()
	{
		if (!goLeft)
			targetLocation = new Vector2 (playerTransform.position.x + 3f, dogTransform.position.y);
		else if (goLeft) 
			targetLocation = new Vector2 (playerTransform.position.x - 3f, dogTransform.position.y);
		ChangeDirection ();
	}//walk


	
	void OnTriggerEnter2D(Collider2D other)
	{ 
		
		if (other.CompareTag ("TopCollider") || other.CompareTag ("BottomCollider")) {
			playerHealth.Damage (damageHit, -1f);
		}
	}//OnTriggerEnter2D
}
