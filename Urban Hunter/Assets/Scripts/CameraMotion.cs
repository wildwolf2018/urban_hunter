using UnityEngine;
using System.Collections;

public class CameraMotion : MonoBehaviour {
	public float xMin;
	public float xMax;
	public float yMin;
	public float yMax;
	private Transform playerTransform;
	private PlayerHealth playerHealth;
	private GameObject playerObject;
	
	void Awake()
	{
		playerTransform = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
	}

	void Update()
	{
		playerObject = GameObject.FindGameObjectWithTag ("Player");
		if (playerObject != null) {
			playerTransform = playerObject.GetComponent<Transform> ();
			playerHealth = playerObject.GetComponent<PlayerHealth> ();
		}
	}

	void LateUpdate () 
	{
		if (playerObject != null && !playerHealth.isDead) {	
		transform.position = new Vector3 (Mathf.Clamp (playerTransform.position.x, xMin, xMax), Mathf.Clamp (playerTransform.position.y, yMin, yMax), 
		                                  transform.position.z);
			playerTransform.position = new Vector3 (Mathf.Clamp (playerTransform.position.x, xMin - 14f, xMax + 14f),
	                                        playerTransform.position.y, playerTransform.position.z);
		}
	}
}
