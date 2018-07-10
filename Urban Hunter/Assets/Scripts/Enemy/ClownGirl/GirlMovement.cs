using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GirlMovement : MonoBehaviour {
	public LayerMask playerMask;
	public Transform point1;
	public Transform grenade;
	public float nextThrow = 0f;
	public float throwRate = 1f;
	public float maxSpeed = 4f;
	public float deceleration = 3f;
	public float tweaker = 0.3f;
	public float nextJump = 0f;
	public float jumpRate = 0.5f;
	public class Point
	{
		public int leftPoint;
		public int rightPoint;
		
		public Point(int left, int right)
		{
			leftPoint = left;
			rightPoint = right;
		}
	}
	private PlayerHealth playerHealth;
	private int ENEMY_LAYER_MASK = 10;
	private int PLAYER_LAYER_MASK = 9;
	private SteeringBehaviour seek;
	private Vector2 targetPos;
	private Transform[]pathPoints;
	private Transform[]controlPoints;
	private Dictionary<int, Point> dict = new Dictionary<int, Point>();
	private Transform playerTransform;
	private Transform girlTransform;
	private Rigidbody2D girlRigidBody;
	private Animator anim;
	private Quaternion target = Quaternion.Euler (0f, 0f, 0f);
	private RaycastHit2D hitLeft;
	private RaycastHit2D hitRight;
	private int currentPos = 1;
	public bool faceRight = false;
	private bool insert = true;
	private enum action{ JUMP_LEFT, RUN_LEFT, JUMP_RIGHT, RUN_RIGHT, DO_NOTHING };
	private action doNext = action.DO_NOTHING;
	private Point nextPoint;
	private float parm = 0f;
	private float angle = 0f;



	void Awake () {
		girlTransform = GetComponent<Transform> ();
		girlRigidBody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		playerTransform = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
		playerHealth = playerTransform.GetComponent<PlayerHealth> () as PlayerHealth;
		pathPoints = GameObject.Find ("ClownGirlWayPoints").GetComponentsInChildren<Transform> ();
		controlPoints =  GameObject.Find ("ControlPoints").GetComponentsInChildren<Transform> ();
		seek = ScriptableObject.CreateInstance ("SteeringBehaviour") as SteeringBehaviour;
	}//Awake

	void Start()
	{seek.tweaker = tweaker;
		seek.deceleration = deceleration;
		seek.maxSpeed = maxSpeed;
		seek.rdb = girlRigidBody;
	}
	
	void Update () 
	{
		if (!playerHealth.isDead) {
			initialize ();
            if (doNext == action.DO_NOTHING && Time.time > nextJump)
				ChooseDirection ();
			else {
				ClownMovement ();
			}//if else
		}
	}//Update

	void initialize()
	{
		Physics2D.IgnoreLayerCollision(ENEMY_LAYER_MASK, ENEMY_LAYER_MASK, true);
		Physics2D.IgnoreLayerCollision(PLAYER_LAYER_MASK, ENEMY_LAYER_MASK, true);
		girlTransform.rotation = Quaternion.Slerp (girlTransform.rotation, target, Time.deltaTime * 96f);
		if (insert) {
			dict.Add(1, new Point(1, 2));
			dict.Add(2, new Point(3, 4));
			dict.Add(3, new Point(-1, -1));
			dict.Add(4, new Point(5, 6));
			dict.Add(5, new Point(7, 8));
			dict.Add(6, new Point(-1, -1));
			for(int i = 1;i < pathPoints.Length;i++){
				if(Vector2.Distance(girlTransform.position, pathPoints[i].position) <= 2f)
					currentPos = i;
			}
			insert = false;
		}
	}

	void ClownMovement()
	{
		if(doNext == action.JUMP_RIGHT){
			if(parm <= 1f) {
				rotateAntiClockwise();
			}//if
			else{
				doNext = action.DO_NOTHING;
				anim.SetTrigger("idle");
				girlTransform.position = new Vector3(girlTransform.position.x, girlTransform.position.y, 0.45f);
				parm = 0f;
				angle = 0f;
				currentPos += 1;
			}//else
		}//if
		if(doNext == action.JUMP_LEFT){
			if(parm <= 1f) {
				rotateClockWise();
			}//if
			else{
				doNext = action.DO_NOTHING;
				anim.SetTrigger("idle");
				girlTransform.position = new Vector3(girlTransform.position.x, girlTransform.position.y, 0.45f);
				parm = 0f;
				angle = 0f;
				currentPos -= 1;
			}//if else
		}//if
		if(doNext == action.RUN_RIGHT){
			Vector2 targetPos = pathPoints[4].position;
			if(Mathf.Abs(girlTransform.position.x - targetPos.x) >= 0.5f){
				anim.SetTrigger("run");
				girlRigidBody.position += seek.seekAndArrive (targetPos) * Time.deltaTime;
			}
			else{
				anim.SetTrigger("idle");
				girlTransform.position = new Vector3(girlTransform.position.x, girlTransform.position.y, 0.45f);
				doNext = action.DO_NOTHING;
				currentPos += 1;
			}//if else
		}//if
		if(doNext == action.RUN_LEFT){
			Vector2 targetPos = pathPoints[3].position;
			if(Mathf.Abs(girlTransform.position.x - targetPos.x) >= 0.5f){
				anim.SetTrigger("run");
				girlRigidBody.position += seek.seekAndArrive (targetPos) * Time.deltaTime;
			}
			else{
				anim.SetTrigger("idle");
				girlTransform.position = new Vector3(girlTransform.position.x, girlTransform.position.y, 0.45f);
				doNext = action.DO_NOTHING;
				currentPos -= 1;
			}// if else
		}//if
	}//ClownMovement

	void rotateClockWise()
	{
		Vector2 point = calculateBezierPoint (pathPoints[currentPos].position, controlPoints[nextPoint.rightPoint].position, 
		                                      controlPoints[nextPoint.leftPoint].position, pathPoints[currentPos-1].position, parm);
        girlTransform.position = point;
		if(angle <= 1080f){
			anim.SetTrigger("jump");
			girlTransform.rotation = Quaternion.Euler (0f, 0f, angle);
		}//if
		angle -= 30f;
		parm += 0.025f;
	}

	void rotateAntiClockwise()
	{
		Vector2 point = calculateBezierPoint (pathPoints[currentPos].position, controlPoints[nextPoint.leftPoint].position, 
		                                      controlPoints[nextPoint.rightPoint].position, pathPoints[currentPos+1].position, parm);
        girlTransform.position = point;
        if (angle <= 1080f)
        {
            anim.SetTrigger("jump");
            girlTransform.rotation = Quaternion.Euler(0f, 180f, angle);
        }
        angle += 30f;
        parm += 0.025f;
	}

	Point findNextWayPoint(int pos)
	{
		Point ret = null;
        if (doNext == action.JUMP_RIGHT && pos < 6)
        {
            ret = dict[pos];
            return ret;
        }
        if (doNext == action.JUMP_LEFT && pos > 1)
        {
            ret = dict[pos - 1];
            return ret;
        }
        doNext = action.DO_NOTHING;
        return ret;

	}//findNextWayPoint



	Vector2 calculateBezierPoint(Vector2 p0, Vector2 p1, Vector2 p2,Vector2 p3, float t)
	{
		float a = 1 - t;
		float b = a * a;
		float a0 = b * a;//coefficient 1
		float a1 = 3 * b * t;//coefficient 2
		float a2 = 3 * a * t * t;//coefficient 3
		float a3 = t * t * t;//coefficient 4
		
		Vector2 result = a0 * p0;
		result += a1 * p1;
		result += a2 * p2;
		result += a3 * p3;
		
		return result;
	}//calculateBezierPoint

  	void ChooseDirection()
  	{
		nextJump = Time.time + jumpRate;
		if (playerTransform.position.x > girlTransform.position.x) {
			faceRight = false;
			ChangeDirection();
		} else {
			faceRight = true;
			ChangeDirection();
		}
		float distanceToPlayer = Mathf.Abs(girlTransform.position.x - playerTransform.position.x);
		if(distanceToPlayer <= 10f)
			throwGrenade(distanceToPlayer);
		else jump();
  	}//movement

	void ChangeDirection (){
		if (!faceRight) 	
			target = Quaternion.Euler(0f, 180f, 0f);
		if (faceRight) 
			target = Quaternion.Euler(0f, 0f, 0f);
		faceRight = !faceRight;
	}//ChangeDirection

	void throwGrenade(float distanceToPlayer)
	{
		if (Time.time > nextThrow) {
			nextThrow = Time.time + throwRate;
			if ((distanceToPlayer >= 0f && distanceToPlayer <= 10f) && !playerHealth.isDead) {
				anim.SetTrigger ("throw");
				Instantiate (grenade, point1.position, Quaternion.identity);
			}//if
		}
	}//throwGrenade

	void jump()
	{
		if (faceRight) {
			if (currentPos != 3 || currentPos != 6)
			{
				doNext = action.JUMP_RIGHT;
				nextPoint = findNextWayPoint(currentPos);
			}
			if(currentPos == 3)
			{
				doNext = action.RUN_RIGHT;
				nextPoint = dict[4];
			}
		}
		else {
			if (currentPos != 1 || currentPos != 4)
			{
				doNext = action.JUMP_LEFT;
				nextPoint = findNextWayPoint(currentPos);
			}
			if(currentPos == 4)
			{
				doNext = action.RUN_LEFT;
				nextPoint = dict[3];
            }
		}
	}//jump
}