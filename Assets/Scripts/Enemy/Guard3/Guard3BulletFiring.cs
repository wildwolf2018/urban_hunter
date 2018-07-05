using UnityEngine;
using System.Collections;

public class Guard3BulletFiring : MonoBehaviour {
	public float fireRate = 0.5f;
	public float nextFire = 0f;
	public float shootForce = 10f;
	public Rigidbody2D bullet;
	public Transform spawnPoint1;
	public Transform spawnPoint2;
	
	private Guard3Movement guardMovement;
	private Rigidbody2D rdb;
	private Animator anim;
	private float right;
	private PlayerMovement playerMovement;
	
	void Awake()
	{
		guardMovement = GetComponent<Guard3Movement> ();
		anim = GetComponent<Animator> ();
		playerMovement = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();
	}
	
	void Start()
	{
		spawnPoint1.localRotation = Quaternion.Euler (0f, 180f, 0f);
		spawnPoint2.localRotation = Quaternion.Euler (0f, 180f, 0f);
	}
	
	void Update()
	{
		if (Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			if(playerMovement.ground)
				shoot ();
		}
	}
	
	void shoot()
	{
		if (anim.GetCurrentAnimatorStateInfo (1).IsTag ("shoot") && guardMovement.ground) {
			rdb = Instantiate (bullet, spawnPoint1.position, Quaternion.identity) as Rigidbody2D;
			rdb.velocity = shootForce * spawnPoint1.right;
		}
		if (anim.GetCurrentAnimatorStateInfo (1).IsTag ("duck_shoot")&& guardMovement.ground) {
			rdb = Instantiate (bullet, spawnPoint2.position, Quaternion.identity) as Rigidbody2D;
			rdb.velocity = shootForce * spawnPoint2.right;
		}
	}//shoot
}
