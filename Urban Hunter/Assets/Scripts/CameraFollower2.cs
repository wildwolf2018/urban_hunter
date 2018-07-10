using UnityEngine;
using System.Collections;

public class CameraFollower2 : MonoBehaviour {
	public float xMin;
	public float xMax;
	public float yMin;
	public float yMax;
	private Transform player;
	private PlayerHealth playerHealth;
	private GameObject temp;
	

	void Update()
	{
		temp = GameObject.FindGameObjectWithTag ("Player");
		if (temp != null) {
			player = temp.GetComponent<Transform> ();
			playerHealth = temp.GetComponent<PlayerHealth> ();
		}
	}
	void LateUpdate () 
	{
		if (!playerHealth.isDead) {
			transform.position = new Vector3 (Mathf.Clamp (player.position.x, xMin, xMax), Mathf.Clamp (player.position.y, yMin, yMax), transform.position.z);
			player.position = new Vector3 (Mathf.Clamp (player.position.x, xMin - 15f, xMax + 15f), player.position.y, player.position.z);
		}
	}
}
