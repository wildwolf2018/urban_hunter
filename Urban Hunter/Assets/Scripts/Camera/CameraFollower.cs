using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {
	public float xMin;
	public float xMax;
	public float yMin;
	public float yMax;
	public float flyRate = 5f;
	public float elapsedTime = 0f;
	public Transform spawnpoint;
	public GameObject crow;
	private Transform player;
	private PlayerHealth playerHealth;
	private GameObject temp;

	void Awake()
	{
		temp = GameObject.FindGameObjectWithTag ("Player");
		player = temp.GetComponent<Transform> ();
		playerHealth = temp.GetComponent<PlayerHealth> ();
	}

	void Update()
	{
		temp = GameObject.FindGameObjectWithTag ("Player");
		if (temp != null) {
			player = temp.GetComponent<Transform> ();
			playerHealth = temp.GetComponent<PlayerHealth> ();
			elapsedTime += Time.deltaTime;
			if (player != null && playerHealth != null) {
				if (elapsedTime > flyRate && !playerHealth.isDead) {
					Instantiate (crow, spawnpoint.position, Quaternion.identity);
					Invoke ("Bird", 0.5f);
					Invoke ("Bird", 1f);
					elapsedTime = 0f;
				}
			}
		}

	}

	void Bird()
	{
		Instantiate(crow, spawnpoint.position, Quaternion.identity);
	}
	
	void LateUpdate () 
	{
		if (player != null && playerHealth != null) {
			if (!playerHealth.isDead && !playerHealth.isDestroyed) {
				transform.position = new Vector3 (Mathf.Clamp (player.position.x, xMin, xMax), Mathf.Clamp (player.position.y, yMin, yMax), transform.position.z);
				player.position = new Vector3 (Mathf.Clamp (player.position.x, xMin - 10f, xMax + 12f), player.position.y, player.position.z);
			}
		}
	}
}
