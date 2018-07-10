using UnityEngine;

public class Grenade : MonoBehaviour {
	public int damage = 20;
	public ParticleSystem explosionEffect;
	private PlayerHealth playerHealth;
	private Transform grenadeTransform;
	private Vector2 point1;
	private Vector2 point2;
	private Vector2 point3;
	private Vector2 point4;
	private Vector2 spawnPosition;
	private Transform playerTransform;
	private Vector2 playerPos;
	private Transform ground;
	private float parm = 0f;
	private int ENEMY_LAYER_MASK = 10;

    void Start () {
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
		playerTransform = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
        ground = GameObject.FindGameObjectWithTag ("Feet").GetComponent<Transform> ();
		grenadeTransform = GetComponent<Transform> ();
		spawnPosition = new Vector2 (grenadeTransform.position.x, grenadeTransform.position.y);
		if (playerTransform.position.x > spawnPosition.x) {
			point1 = spawnPosition;
			point2 = new Vector2 (spawnPosition.x + 1.67f, spawnPosition.y + 0.5f); 
			point3 = new Vector2 (spawnPosition.x + 2.54f, spawnPosition.y + 0.42f); 
			point4 = new Vector2 (ground.position.x, ground.position.y);
			if(Mathf.Abs(playerTransform.position.x - spawnPosition.x) <= 3f)
			{
				point2 = point1;
				point3 = point4;
			}
		} else {
			point1 = spawnPosition;
			point2 = new Vector2(spawnPosition.x - 1.67f, spawnPosition.y + 0.57f); 
			point3 = new Vector2(spawnPosition.x - 2.54f, spawnPosition.y + 0.42f); 
			point4 = new Vector2 (ground.position.x, ground.position.y);
			if(Mathf.Abs(playerTransform.position.x - spawnPosition.x) <= 3f)
			{
				point2 = point1;
				point3 = point4;
			}
		}
		Invoke ("Explode", 3f);
	}//Awake

	void Explode()
	{
		if (gameObject.activeSelf) {
			explosionEffect.Stop ();
			explosionEffect.Play ();
			Destroy (gameObject, 0.1f);
		}
	}
	void Update()
	{
		Physics2D.IgnoreLayerCollision(ENEMY_LAYER_MASK, ENEMY_LAYER_MASK, true);
        if (parm <= 1f) {
			Vector2 point = calculateBezierPoint (point1, point2, point3, point4, parm);
			grenadeTransform.position = point;
			parm += 0.04f;
        }

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "TopCollider" || coll.gameObject.tag == "BottomCollider")
        {
            playerHealth.Damage(damage, 0f);
            explosionEffect.Stop();
            explosionEffect.Play();
            Destroy(gameObject, 0.1f);
        }
    }

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
}
