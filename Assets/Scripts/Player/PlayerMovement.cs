using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float force = 20f;//force applied to player body
	public float maxSpeed = 5f;//player maximum speed
	public float shootRate = 0.5f;
	public float nextFire = 0.0f;
	public float jumpForce = 700f;
	public float jumpShootForce = 100f;
	public float groundRadius = 0.1f;
	public bool checkForPunch = false;
	public bool checkDownPunch = false;
	public bool faceRight = true; //indicates whether facing right or left
	public bool throwBoomer = false;
	public GameObject playerTopCollider;
	public Transform checkGround;
	public LayerMask groundLayer;

	float moveHorizontal;//shows horizontal direction in which player is moving
	bool canShoot = false;
	bool shootUp = false;
	bool crouch = false;
	bool crouchShoot = false;
	bool shootDiagonal = false;
	public bool ground = true;
	Animator anim;
	
	void Awake () {
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		KeyBoardInput ();
	}

	void FixedUpdate () 
	{
		MoveMent ();
		DoActions ();
	}

	void KeyBoardInput()
	{
		if (ground && Input.GetButtonDown ("Jump")) 
		{
			anim.SetBool("ground", false);
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
		}

		if (Input.GetButtonDown("Shoot")) 
			canShoot = true;
		if (Input.GetButtonUp("Shoot") )
			anim.SetTrigger ("still");
		if (Input.GetButtonDown ("Down"))
		{
			playerTopCollider.SetActive(false);
			crouchShoot = true;
		}

		if (Input.GetButtonUp ("Down")) {
			playerTopCollider.SetActive(true);
			anim.SetTrigger ("still");
		}
		if (Input.GetButtonDown ("shoot_up")) 
		{
			shootUp = true;
		}
		if (Input.GetButtonUp ("shoot_up"))
			anim.SetTrigger ("still");
		if (Input.GetButtonDown ("ShootDiagonal"))
			shootDiagonal = true;
		if (Input.GetButtonUp ("ShootDiagonal"))
			anim.SetTrigger ("still");
		if (Input.GetButtonDown("boomerang") ) {
			throwBoomer = true;
		}
		if (Input.GetButtonDown ("boomerang") && ground) {
			throwBoomer = true;
		} else if (Input.GetButtonDown ("boomerang") && !ground) {
				throwBoomer = false;
		}
		
	}
	
	void MoveMent()
	{
		ground = Physics2D.OverlapCircle (checkGround.position, groundRadius, groundLayer);
		anim.SetBool ("ground", ground);
		anim.SetFloat ("vSpeed", GetComponent<Rigidbody2D>().velocity.y);
		moveHorizontal = Input.GetAxis ("Horizontal");
		anim.SetFloat ("speed", Mathf.Abs (moveHorizontal));
		if (moveHorizontal * GetComponent<Rigidbody2D>().velocity.x < maxSpeed) 
		{//check whether character speed to left or right is below maximum speed
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveHorizontal * force);
		}
		if (Mathf.Abs (GetComponent<Rigidbody2D>().velocity.x) > maxSpeed) 
		{//check whether character speed is above maximum speed
			GetComponent<Rigidbody2D>().velocity = new Vector2 (Mathf.Sign (GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);//if character speed is above maximum speed, clamp it
		}
		if (moveHorizontal > 0 && !faceRight) 
		{ 
			ChangeDirection ();
		} else if (moveHorizontal < 0 && faceRight) {
			ChangeDirection ();
		} 
	}

	//Changes direction player is facing  to left or right
	public void ChangeDirection (){
		faceRight = !faceRight;
		Vector2 xAxis = transform.localScale;
		xAxis.x *= -1;
		transform.localScale = xAxis;
	}

	void DoActions()
	{
		if (crouch) 
		{
			anim.SetTrigger ("crouch");
			crouch = false;
		}
		if (crouchShoot) {
			playerTopCollider.SetActive(false);
			anim.SetTrigger ("crouchShoot");
			crouchShoot = false;

		}
		if (canShoot) 
		{
			anim.SetTrigger ("shoot");
			canShoot = false;
		}
		if (shootUp) 
		{
			anim.SetTrigger ("shootUp");
			shootUp = false;
		}
		if (shootDiagonal) 
		{
			anim.SetTrigger ("shootDiagonal");
			shootDiagonal = false;
		}
	}	
}