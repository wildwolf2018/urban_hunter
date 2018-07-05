using UnityEngine;
using System.Collections;

public class CrowMovement : MonoBehaviour {
	public GameObject[] rewards;

	private Transform point0;
	private Transform point1;
	private Transform point2;
	private Transform point3;
	private Vector2 pt0;
	private Vector2 pt1;
	private Vector2 pt2;
	private Vector2 pt3;
	private Transform crowTransform;
	private float parm = 0f;
	private ScoreManager playerScore;

	void Awake()
	{
		crowTransform = GetComponent<Transform> ();
		point0 = GameObject.Find ("CrowPoint1").GetComponent<Transform> ();
		point1 = GameObject.Find ("CrowPoint2").GetComponent<Transform> ();
		point2 = GameObject.Find ("CrowPoint3").GetComponent<Transform> ();
		point3 = GameObject.Find ("CrowPoint4").GetComponent<Transform> ();
		pt0 = point0.position;
		pt1 = point1.position;
		pt2 = point2.position;
		pt3 = point3.position;
		playerScore = GameObject.FindGameObjectWithTag ("Score").GetComponent<ScoreManager> ();
	}

	void Update()
	{
		if (parm <= 1f) {
			Vector2 point = calculateBezierPoint (pt0, pt1, pt2, pt3, parm);
			crowTransform.RotateAround (crowTransform.position, Vector3.forward, 70f * Time.deltaTime);
			crowTransform.position = point;
			parm += 0.004f;
		} else
			Destroy (gameObject);

	}

	//determines path of crow
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
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("GlockBullet")) {
			int result = Random.Range(0, 3);
			Instantiate(rewards[result], crowTransform.position, Quaternion.identity);
			playerScore.IncreaseScore(10);
			Destroy(gameObject);
		}
	}
}
