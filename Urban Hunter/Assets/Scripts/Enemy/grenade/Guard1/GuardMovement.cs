using UnityEngine;
using System.Collections;

public class GuardMovement : MonoBehaviour {
	public float maxSpeed = 4f;
	public float deceleration = 3f;
	public float tweaker = 0.3f;
	public int count = 0;
	public float groundRadius = 1f;
	public bool faceRight;
	public bool ground = false;

	public LayerMask playerMask;
	public LayerMask groundLayer;
	public Transform groundCheck;
	public Rigidbody2D rdb;
	public GuardBulletFiring shoot;
    public BoxCollider2D guardHitDetector;

	private int ENEMY_LAYER_MASK = 10;
	private int BOXES_LAYER_MASK = 15;
	private int VEHICLES_LAYER_MASK = 14;
	private bool goLeft = true;
	private int MAX_COUNT = 1;

	private Transform player;
	private Animator anim;
	private SteeringBehaviour seek;
	private BoxCollider2D guardCollider;
	private PlayerHealth playerHealth;

	private Vector3 playerPivotPos;
	private Vector2 targetPos;
	private float yPos;
	private Quaternion target;
	
	void Awake () 
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
		playerHealth =  GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
		rdb = GetComponent<Rigidbody2D> ();
		guardCollider = GetComponent<BoxCollider2D>();
		yPos = rdb.position.y;
		anim = GetComponent<Animator> ();
		seek = ScriptableObject.CreateInstance ("SteeringBehaviour") as SteeringBehaviour;
	}//Awake
	
	void Start()
	{
		playerPivotPos = player.position;
		seek.tweaker = tweaker;
		seek.deceleration = deceleration;
		seek.maxSpeed = maxSpeed;
		seek.rdb = rdb;
		target = Quaternion.Euler(0f, 0f, 0f);;
		if (transform.position.x < playerPivotPos.x)
		{
			targetPos = new Vector2(playerPivotPos.x + 10f, 0f);
			goLeft = false;
			faceRight = false;
			ChangeDirection();
		}
		else if (transform.position.x > playerPivotPos.x)
		{
			targetPos = new Vector2(playerPivotPos.x - 10f, 0f);
			goLeft = true;
			faceRight = false;
		}
	}//Start

	void Update()
	{
		transform.rotation = Quaternion.Slerp (transform.localRotation, target, Time.deltaTime * 24f);
		Physics2D.IgnoreLayerCollision(ENEMY_LAYER_MASK, ENEMY_LAYER_MASK, true);
		Physics2D.IgnoreLayerCollision(VEHICLES_LAYER_MASK, ENEMY_LAYER_MASK, true);
		Physics2D.IgnoreLayerCollision(BOXES_LAYER_MASK, ENEMY_LAYER_MASK, true);
	}//Update

	void FixedUpdate () 
	{   
		if (playerHealth != null && !playerHealth.isDead) {
			//check if on the ground
			ground = Physics2D.OverlapCircle (groundCheck.position, groundRadius, groundLayer);
			if (ground) {
				targetPos.y = rdb.position.y;
			} else
				anim.SetLayerWeight (0, 1f);
			anim.SetLayerWeight (0, 1f);
			anim.SetLayerWeight (1, 0f);
			anim.SetBool ("walk", false);
			if (anim.GetCurrentAnimatorStateInfo (1).IsTag ("duck_shoot") && ground) {
				guardCollider.offset = new Vector2 (0f, -1.96f);
				guardCollider.size = new Vector2 (1.41f, 0.75f);
                guardHitDetector.offset = new Vector2(0f, -1.96f);
                guardHitDetector.size = new Vector2(1.41f, 0.75f);
            } else {
				guardCollider.offset = new Vector2 (0f, -0.29f);
				guardCollider.size = new Vector2 (1.41f, 4.48f);
                guardHitDetector.offset = new Vector2(0.05f, -0.25f);
                guardHitDetector.size = new Vector2(0.87f, 3.87f);
            }
			//test whether player is in front or at the back
			bool hitLeft = Physics2D.Raycast (new Vector2 (transform.position.x - 1f, transform.position.y), Vector2.left, 12f, playerMask);
			bool hitRight = Physics2D.Raycast (new Vector2 (transform.position.x + 1f, transform.position.y), Vector2.right, 12f, playerMask);
			if ((!hitLeft && !hitRight) && ground) {//patrol if player cannot be detected
				anim.SetLayerWeight (0, 1f);
				anim.SetLayerWeight (1, 0f);
				anim.SetBool ("walk", false);
				shoot.enabled = false;
				count = 0;
				patrol ();
			} else if ((hitRight && !faceRight) && ground) {
				targetPos = new Vector2 (playerPivotPos.x - 10f, 0f);
				goLeft = false;
				anim.SetLayerWeight (1, 1f);
				ChangeDirection ();
				shoot.enabled = true;
			} else if ((hitRight && faceRight) && ground) {	
				anim.SetLayerWeight (1, 1f);
				shoot.enabled = true;
			} else if (hitLeft && faceRight) {
				targetPos = new Vector2 (playerPivotPos.x - 10f, yPos);
				goLeft = true;
				anim.SetLayerWeight (1, 1f);
				ChangeDirection ();
				shoot.enabled = true;
			} else if (hitLeft && !faceRight) {
				anim.SetLayerWeight (1, 1f);
				shoot.enabled = true;
			}
		} else {
				anim.SetLayerWeight(1, 0f);
				anim.SetLayerWeight (0, 1f);
				anim.SetBool("walk", false);
				shoot.enabled = false;
		}
	}//FixedUpdate
	
	void patrol()
	{
		if( Mathf.Abs(transform.position.x - targetPos.x) > 0.25f && goLeft)
		{
			targetPos = new Vector2(playerPivotPos.x - 10f, yPos);
			anim.SetBool ("walk", true);
		}
		else if (Mathf.Abs(transform.position.x - targetPos.x) > 0.25f && !goLeft)
		{
			targetPos = new Vector2(playerPivotPos.x + 10f, 0f);
			anim.SetBool ("walk", true);
		}
		else if(Mathf.Abs(transform.position.x - targetPos.x) <= 0.25f)
		{
			anim.SetBool ("walk", false);
			if(goLeft)
				goLeft = false;
			else if(!goLeft)
				goLeft = true;
			Invoke("walk", 2f);
		}
		rdb.position += seek.seekAndArrive (targetPos) * Time.deltaTime;
	}//patrol
	
	void ChangeDirection (){
		if (!faceRight && !goLeft) 	
			target = Quaternion.Euler(0f, 180f, 0f);
		 else if (faceRight && goLeft) 
			target = Quaternion.Euler(0f, 0f, 0f);
		faceRight = !faceRight;
	}//ChangeDirection


	void walk()
	{
		 if (!goLeft)
			targetPos = new Vector2 (playerPivotPos.x + 10f, 0f);
		else if (goLeft) 
			targetPos = new Vector2 (playerPivotPos.x - 10f, 0f);
		 ChangeDirection ();
	}//walk

	void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("GlockBullet"))
		{
			if(count >= MAX_COUNT)
			{
				anim.SetLayerWeight(1, 1f);
				anim.SetTrigger("duck");
				standUp();
			}
			count += 1;
		}//if
	}//OnTriggerEnter2D

	IEnumerator standUp()
	{
		yield return new WaitForSeconds(3f);
		anim.SetLayerWeight(0, 1f);
		anim.SetBool ("walk", true);

	}

}
