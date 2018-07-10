using UnityEngine;
using System.Collections;

public class BulletFiring : MonoBehaviour {
	public float fireRate = 0.5f;
	public float nextFire = 0f;
	public float shootForce = 10f;
	public Rigidbody2D bullet;
	public Transform fireHorizontal;
	public Transform fireDiagonal;
	public Transform fireUp;
	public Transform fireCrouching;
	public PlayerMovement playerMovement;

	Animator anim;



	void Awake()
	{
		anim = GetComponent<Animator> ();
	}

	void Update () 
	{ 
		if ((Input.GetButton ("Fire1") || Input.GetKey(KeyCode.I)) && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			if(!AmmoEmpty())
				shoot();
		}
	}

	void shoot()
	{
		Rigidbody2D rigidbody;
		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("shoot_horizontal")) 
		{
			if (playerMovement.faceRight) {
				fireHorizontal.rotation = Quaternion.Euler (0f, 0f, 270f);
				rigidbody = Instantiate (bullet, fireHorizontal.position, fireHorizontal.rotation) as Rigidbody2D;
			} else {
					fireHorizontal.rotation = Quaternion.Euler (0f, 0f, 90f);
					rigidbody = Instantiate (bullet, fireHorizontal.position, fireHorizontal.rotation) as Rigidbody2D;
			}
			rigidbody.velocity = shootForce * fireHorizontal.up;
			AmmoMananger.ammo -= 1;
		} 
		else if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("crouch_shoot")) 
		{
			if (playerMovement.faceRight) {
				fireCrouching.rotation = Quaternion.Euler (0f, 0f, 270f);
				rigidbody = Instantiate (bullet, fireCrouching.position, fireCrouching.rotation) as Rigidbody2D;
				rigidbody.velocity = shootForce * fireCrouching.up;
			} else {
				fireCrouching.rotation = Quaternion.Euler (0f, 0f, 90f);
				rigidbody = Instantiate (bullet, fireCrouching.position, fireCrouching.rotation) as Rigidbody2D;
			}
			rigidbody.velocity = shootForce * fireCrouching.up;
			AmmoMananger.ammo -= 1;
		}
		checkUpAndDiagonal();
	}

	bool AmmoEmpty()
	{
		return AmmoMananger.ammo <= 0;
	}

	void checkUpAndDiagonal()
	{
		Rigidbody2D rigidbody = null;
		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("shoot_up")) 
		{
			fireUp.rotation = Quaternion.Euler(0f, 0f, 0f);
			rigidbody = Instantiate (bullet, fireUp.position, fireUp.rotation) as Rigidbody2D;
			rigidbody.velocity = shootForce * fireUp.up;
			AmmoMananger.ammo -= 1;
		}
		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("shoot_diagonal")) 
		{
			if (playerMovement.faceRight)
			{
				fireDiagonal.rotation = Quaternion.Euler(0f, 0f, -53f);
				rigidbody = Instantiate (bullet, fireDiagonal.position, fireDiagonal.rotation) as Rigidbody2D;
			}
			else if(!playerMovement.faceRight)
			{
                fireDiagonal.rotation = Quaternion.Euler(0f, 0f, 53f);
              //  bullet.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, 53f);
                rigidbody = Instantiate (bullet, fireDiagonal.position, fireDiagonal.rotation) as Rigidbody2D;
			}
			rigidbody.velocity = shootForce * fireDiagonal.up;
			AmmoMananger.ammo -= 1;
		}
	}
	
}
