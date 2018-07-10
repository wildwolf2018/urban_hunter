using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WhipBossMovement : MonoBehaviour {
	public GameObject grenade;
	public Transform spawnPoint;
	public Image fader;
	public float maxSpeed = 4f;
	public float deceleration = 3f;
	public float tweaker = 0.3f;
	public float screenFadeSpeed = 1f;
	public float teleportRate = 2f;
	public float elapsedRegenTime = 0f;
	public float regenRate = 3f;
	public Vector2 velocity = new Vector2(0.2f, 0f);
	public float elapsedTime = 0f;
	public float elapsedGrenadeTime = 0f;
	public float grenadeRate = 2f;
	public float elapsedWhipTime = 0f;
	public float  whipRate = 2f;
	public int damage  = 40;

	private Transform[] waypoints;
	private Rigidbody2D rdb2;
	private WhipMasterHealth health;
	private Transform bossTransform;
	private SpriteRenderer spriteRender;
	private Animator anim;
	private PlayerHealth playerHealth;
	private Transform playerTransform;
	private SteeringBehaviour seek;
	private bool increaseHealth = false;
	private BoxHealth boxHealth;
	private int numGrenades = 3;
	private bool faceRight = false;
	private bool goLeft = true;
	private int currentPos;
	private Quaternion targetRotation;
	private bool changeToRed = false;
	private GameObject temp;

	void Awake () {
		spriteRender = GetComponent<SpriteRenderer> ();
		rdb2 = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		health = GetComponent<WhipMasterHealth> ();
		bossTransform = GetComponent<Transform> ();
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
		boxHealth = GameObject.FindGameObjectWithTag ("BoxTwo").GetComponent<BoxHealth> ();
		playerTransform =  GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
		waypoints = GameObject.Find ("TeleportPoints").GetComponentsInChildren<Transform> ();
		seek = ScriptableObject.CreateInstance ("SteeringBehaviour") as SteeringBehaviour;
		seek.tweaker = tweaker;
		seek.deceleration = deceleration;
		seek.maxSpeed = maxSpeed;
		seek.rdb = rdb2;
		fader.color = Color.clear;
		targetRotation = Quaternion.Euler(0f, 0f, 0f);
		bossLocation ();
	}

	void Update () 
	{
		temp = GameObject.FindGameObjectWithTag ("Player");
		if (temp != null) {
			playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
			playerTransform =  GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
		}
		if (!health.isDead && !playerHealth.isDead) {
			Physics2D.IgnoreLayerCollision (9, 10);
			//Debug.Log ("current = " + currentPos);
			facePlayer ();
			checkHealth ();
			if (!changeToRed)
				changeState ();
			else {
				spriteRender.color = Color.Lerp (spriteRender.color, Color.red, screenFadeSpeed * Time.deltaTime * 0.5f);
				fader.color = Color.Lerp (fader.color, new Color (1f, 0f, 0f, 0.6f), screenFadeSpeed * Time.deltaTime * 0.5f);
				if (fader.color.a >= 0.5f) {
					fader.color = Color.Lerp (fader.color, Color.clear, screenFadeSpeed * Time.deltaTime * 10f);
					elapsedRegenTime += Time.deltaTime;
				}
				if (elapsedRegenTime > regenRate) {
					elapsedRegenTime = 0f;
					changeToRed = false;
				}
			}
		}
	}

	void facePlayer()
	{
		if (bossTransform.position.x < playerTransform.position.x) {
			faceRight = true;
			goLeft = false;
			ChangeDirection ();
		} else {
			faceRight = false;
			goLeft = true;
			ChangeDirection ();
		}
		bossTransform.rotation = Quaternion.Slerp (transform.localRotation, targetRotation, Time.deltaTime * 40f);
	}//facePlayer
	
	void changeState()
	{
		float xDist = Vector2.Distance(bossTransform.position,playerTransform.position);
		if (xDist >= 30f) {

			numGrenades = 3;
			spriteRender.color = Color.Lerp(spriteRender.color, Color.black, screenFadeSpeed * Time.deltaTime);
			moveToTarget ();
		}
		if (xDist > 11f && xDist < 30f)
			grenadeAttack ();
		if (xDist >= 0f && xDist <= 11f) {
			numGrenades = 3;
			whipAttack ();
		}
	}
	void moveToTarget()
	{
		if (spriteRender.color.r < 0.0034f) {
			if (!increaseHealth) 
			{
				bossTransform.position = findLocation ();
				spriteRender.color = Color.white;//.Lerp(spriteRender.color, Color.white, screenFadeSpeed * Time.deltaTime * 10000f);
			}
			else
			{
				bossTransform.position = new Vector2 (waypoints [1].position.x, waypoints [1].position.y);
				regenerate();
				increaseHealth = false;

			}
		}
	}//moveToTarget

	Vector2 findLocation()
	{
		if (bossTransform.position.x > playerTransform.position.x) {
			if (currentPos != 8)
				currentPos += 1;
		} else {
			if (currentPos != 1)
				currentPos -= 1;
		} 

		return waypoints[currentPos].position;
	}

	void bossLocation()
	{
		float xPoz = bossTransform.position.x;
		//Debug.Log ("1 = " + waypoints [1].localPosition.y);
		for(int i = 1; i < waypoints.Length;i++)
		{
			if(Mathf.Abs(waypoints[i].position.x - xPoz) <= 2f){
				currentPos = i;
				break;
			}
		}
	}

	void checkHealth()
	{
		if (!boxHealth.isDead && health.currentHealth <= 350)
			increaseHealth = true;
	}
	void whipAttack()
	{
		elapsedWhipTime += Time.deltaTime;
		if (health.currentHealth <= 60)
			whipRate = 2f;
		else
			whipRate = 4f;
		if (increaseHealth) {
			spriteRender.color = Color.Lerp (spriteRender.color, Color.black, screenFadeSpeed * Time.deltaTime);
			moveToTarget();
		}
		if (elapsedWhipTime > whipRate && !increaseHealth) {
			anim.SetTrigger ("whip");
			elapsedWhipTime = 0f;
		}
	}//whipAttack()

	void decreaseHealth()
	{
		if (playerTransform.position.y  < 10f)
			playerHealth.Damage (damage, 1f);
	}

	void grenadeAttack()
	{
		elapsedGrenadeTime += Time.deltaTime;
		if (elapsedGrenadeTime > grenadeRate && numGrenades > 0) 
		{
			anim.SetTrigger("grenade");
			Instantiate (grenade, spawnPoint.position, Quaternion.identity);
			elapsedGrenadeTime = 0f;
			numGrenades -= 1;
		}
		if (numGrenades == 0) {
			spriteRender.color = Color.Lerp(spriteRender.color, Color.black, screenFadeSpeed * Time.deltaTime);
			elapsedTime += Time.deltaTime;
			if(elapsedTime > teleportRate){
			moveToTarget ();
				elapsedTime = 0f;
			}
		}
	}//grenadeAttack()

	void regenerate()
	{
		health.currentHealth += 30;
		changeToRed = true;
	}//idle
	
	void ChangeDirection (){
		if (!faceRight && goLeft) 	
			targetRotation = Quaternion.Euler(0f, 0f, 0f);
		else if (faceRight && !goLeft) 
			targetRotation = Quaternion.Euler(0f, 180f, 0f);
		faceRight = !faceRight;
	}//ChangeDirection

	public override string ToString ()
	{
		return string.Format ("r = {0}, g = {1}, b = {2}", spriteRender.color.r, spriteRender.color.g, spriteRender.color.b);
	}
}
