using UnityEngine;
using System.Collections;

public class ThrowGrenade : MonoBehaviour {
	/*private Transform p1;
	private Transform p2;
	private Transform p3;
	private Vector2 point1;
	private Vector2 point2;
	private Vector2 point3;
	private Vector2 point4;
	private Transform playerTransform;
	private PlayerMovement playerMovement;
	private Girl2Movement girlMovement;
	private float parm = 0f;

	void Awake()
	{
		playerTransform = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
		playerMovement = playerTransform.GetComponent<PlayerMovement> () as PlayerMovement;
		girlMovement = GetComponent<Girl2Movement> ();
		p1 = GameObject.Find ("point1_2").GetComponent<Transform> ();
		p2 = GameObject.Find ("point2_2").GetComponent<Transform> ();
		p3 = GameObject.Find ("point3_2").GetComponent<Transform> ();
		point1 = p1.position;
		point2 = p2.position;
		point3 = p3.position;
		point4 = new Vector2(playerTransform.position.x + 0.1f, playerTransform.position.y - 0.5f);
	}

	void Update () 
	{
	
			if (playerMovement.faceRight)
				point4 = new Vector2 (playerTransform.position.x + 0.5f, playerTransform.position.y - 2.5f);
			else
				point4 = new Vector2 (playerTransform.position.x - 0.5f, playerTransform.position.y - 2.5f);
			if (parm <= 1f) {
			//	Vector2 point = calculateBezierPoint (point1, point2, point3, point4, parm);
				//girlMovement.grenadeTransForm.position = point;
				parm += 0.04f;
			}
	}//Update
	

/*	Vector2 calculateBezierPoint(Vector2 p0, Vector2 p1, Vector2 p2,Vector2 p3, float t)
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
	}**/
}
