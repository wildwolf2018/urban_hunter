using UnityEngine;
using System.Collections;

public class SteeringBehaviour:ScriptableObject {
	public float maxSpeed = 0.1f;
	public float deceleration = 3f;
	public float tweaker = 0.3f;
	public Rigidbody2D rdb;
	
	public Vector2 seekAndArrive(Vector2 targetPos)
	{
		targetPos.y = rdb.position.y;
		Vector2 toTarget = targetPos - rdb.position;
		float dist = toTarget.magnitude;
		if (dist > 0) {
			float speed = dist / ((float)deceleration * tweaker);
			speed = Mathf.Min (speed, maxSpeed);
			Vector2 DesiredVelocity = toTarget * speed / dist;
			return (DesiredVelocity - rdb.velocity);
		}
		return Vector2.zero;
	}
	
	public Vector3 seek(Vector2 targetPos)
	{
		targetPos.y = rdb.position.y;
		Vector2 toTarget = targetPos - rdb.position;
		toTarget.Normalize ();
		Vector2 desiredVelocity = toTarget * maxSpeed * Time.deltaTime;
		return (desiredVelocity - rdb.velocity);
	}

}
