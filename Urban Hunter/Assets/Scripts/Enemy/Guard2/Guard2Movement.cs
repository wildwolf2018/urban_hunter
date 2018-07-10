using UnityEngine;
using System.Collections;

public class Guard2Movement : MonoBehaviour {

	public float maxSpeed = 4f;
	public float deceleration = 3f;
	public float tweaker = 0.3f;
	public int count = 0;
	public bool faceRight;
	public float groundRadius = 1f;
	
	public LayerMask playerMask;
	public LayerMask groundLayer;
	public Transform groundCheck;
	public Rigidbody2D rdb;
	public Guard2BulletFiring shoot;
	public bool ground = false;
	
	private int ENEMY_LAYER_MASK = 10;
	private int VEHICLES_LAYER_MASK = 14;
	private int BOXES_LAYER_MASK = 15;
	private bool goLeft = true;
	private float MAX_COUNT = 1;
	private int randNumber;

	private Transform player;
	private Animator anim;
	private SteeringBehaviour seek;
	private BoxCollider2D guardCollider;
	private PlayerHealth playerHealth;
	
	private Vector3 playerPivotPos;
	private Vector2 targetPos;
	private Quaternion target;

	void Awake () 
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
		playerHealth = player.GetComponent<PlayerHealth> () as PlayerHealth;
		rdb = GetComponent<Rigidbody2D> ();
		guardCollider = GetComponent<BoxCollider2D>();
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
		randNumber = Random.Range (5, 10);
		targetPos = new Vector2(playerPivotPos.x + randNumber, 0f);
		target = Quaternion.Euler(0f, 0f, 0f);
		if (transform.position.x < targetPos.x)
		{
			goLeft = false;
			faceRight = false;
			ChangeDirection();
		}
		else if (transform.position.x > targetPos.x)
		{
			goLeft = true;
			faceRight = true;
		}
	}//Start

	void Update()
	{
		transform.rotation = Quaternion.Slerp (transform.localRotation, target, Time.deltaTime * 34f);
		Physics2D.IgnoreLayerCollision(VEHICLES_LAYER_MASK, ENEMY_LAYER_MASK, true);
		Physics2D.IgnoreLayerCollision(ENEMY_LAYER_MASK, ENEMY_LAYER_MASK, true);
		Physics2D.IgnoreLayerCollision(BOXES_LAYER_MASK, ENEMY_LAYER_MASK, true);
	}//Update


	void FixedUpdate()
	{
		if (!playerHealth.isDead) {
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
			} else {
				guardCollider.offset = new Vector2 (0f, -0.29f);
				guardCollider.size = new Vector2 (1.41f, 4.48f);
			}

			bool hitLeft = Physics2D.Raycast (new Vector2 (transform.position.x - 1f, transform.position.y), Vector2.left, 9f, playerMask);
			bool hitRight = Physics2D.Raycast (new Vector2 (transform.position.x + 1f, transform.position.y), Vector2.right, 9f, playerMask);
			if ((!hitLeft && !hitRight) && ground) {
				rdb.position += seek.seekAndArrive (targetPos) * Time.deltaTime;
				anim.SetLayerWeight (0, 1f);
				anim.SetBool ("walk", true);
				shoot.enabled = false;
				count = 0;
			} else if ((hitRight && !faceRight) && ground) {
				goLeft = false;
				anim.SetLayerWeight (1, 1f);
				ChangeDirection ();
				shoot.enabled = true;
			} else if ((hitRight && faceRight) && ground) {	
				anim.SetLayerWeight (1, 1f);
				shoot.enabled = true;
			} else if (hitLeft && faceRight) {
				goLeft = true;
				anim.SetLayerWeight (1, 1f);
				ChangeDirection ();
				shoot.enabled = true;
			} else if (hitLeft && !faceRight) {
				anim.SetLayerWeight (1, 1f);
				shoot.enabled = true;
			}
			if (Mathf.Abs (transform.position.x - targetPos.x) <= 0.25f) {
				anim.SetBool ("walk", false);
				if (transform.position.x > player.position.x) {
					goLeft = true;
					faceRight = true;
					ChangeDirection ();
				}

			}
		} else {
			anim.SetLayerWeight(1, 0f);
			anim.SetLayerWeight (0, 1f);
			anim.SetBool("walk", false);
			shoot.enabled = false;
		}

	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("GlockBullet"))
		{
			if(count >= MAX_COUNT)
			{
				anim.SetLayerWeight(1, 1f);
				anim.SetTrigger("duck");
				standUp();
				count = 0;
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

	void ChangeDirection (){
		if (!faceRight && !goLeft) 	
			target = Quaternion.Euler(0f, 180f, 0f);
		else if (faceRight && goLeft) 
			target = Quaternion.Euler(0f, 0f, 0f);
		faceRight = !faceRight;
	}//ChangeDirection
}
