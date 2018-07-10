using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LevelManager2 : MonoBehaviour {
	public float nextShoot = 0f;
	public float fireRate = 1f;
	public BoxCollider2D[] trigger;
	public GameObject[] fireFountain;
	public GameObject[] enemies;
	public Transform spawnPoint1;
	public Transform spawnPoint2;
	public LayerMask enemyMask;
	public LayerMask playerLayerMask;
	public Transform playerFinder;
	public GameObject player;
	public GameObject bombs;
	public List<GameObject> objects = new List<GameObject>();
	public float elapsedTime = 0f;
	public float spawnRate = 1.5f;

	private BoxCollider2D playerCollider;
	private bool shootFire = false;
	private int index = 0;
	private int activeFountains = 0;
	private int enemyCount = 0;
	private int ENEMY_LAYER_MASK = 10;
	private TextManager textManager;
	private ScreenFader fader;
	public bool goNextRound = false;
	private GameObject temp;
	private PlayerHealth playerHealth;
	public int numOfBombs = 0;
	private GameObject tempBombs;
	private HealthPack pack;

	void Awake () 
	{
		temp = GameObject.FindGameObjectWithTag("Player");
		playerCollider = temp.GetComponent<BoxCollider2D> ();
		playerHealth = temp.GetComponent<PlayerHealth> ();
		pack = GameObject.Find ("pack1").GetComponent<HealthPack> ();
		pack.enabled = true;
		fader = GameObject.Find ("Fader").GetComponent<ScreenFader> ();
		textManager = GameObject.Find ("TextCanvas").GetComponent<TextManager> ();
		textManager.RoundStarting ();
		fader.gameStarting = true;
	}

	void Update () 
	{
		Physics2D.IgnoreLayerCollision(ENEMY_LAYER_MASK, ENEMY_LAYER_MASK, true);
		Physics2D.IgnoreLayerCollision(ENEMY_LAYER_MASK, ENEMY_LAYER_MASK, true);
		if(tempBombs == null)
			tempBombs = Instantiate(bombs, new Vector2(-14.7f, -10.6f), Quaternion.identity) as GameObject;
		if (playerHealth != null && tempBombs != null) {
			if(playerHealth.isDead)
			{
				Destroy(tempBombs, 1f);
			}
		}
		RaycastHit2D[] hitPlayer = Physics2D.BoxCastAll(playerFinder.position, new Vector2 (30f, 30f), 0f, playerFinder.right, 400f, playerLayerMask);
		if (hitPlayer.Length == 0) {
			if(objects.Count != 0){
				for(int i = 0; i < objects.Count;i++)
					Destroy (objects[i]);
			}
			objects.Clear();
			temp = Instantiate (player, new Vector2 (-34.5f, -9.91f), Quaternion.identity) as GameObject;
			playerCollider = temp.GetComponent<BoxCollider2D> ();
			playerHealth = temp.GetComponent<PlayerHealth> ();
			playerHealth.currentHealth = 100;
			trigger [0].enabled = true;
			trigger [1].enabled = true;
			enemyCount = 0;
		} else {
			if(!trigger[1].enabled){
                for(int i = 0; i < objects.Count; i++)
                {
                    Destroy(objects[i]);
                }
				goNextRound = true;
				textManager.RoundEnding ();
                Invoke("RoundEnded", 3f);
			}
		}
		scene1 ();
	}

	void scene1()
	{
		elapsedTime += Time.deltaTime;
		if (!trigger [0].enabled && elapsedTime > spawnRate) {
			objects.Add(Instantiate (enemies[0], spawnPoint1.position, Quaternion.identity) as GameObject);
			objects.Add (Instantiate (enemies[1], spawnPoint1.position, Quaternion.identity) as GameObject);
			enemyCount += 2;
			elapsedTime = 0f;
		}
		if (shootFire && Time.time > nextShoot) {
			nextShoot = Time.time + fireRate;
			if (activeFountains == 0) {
				index = (int)Random.Range (0, 5);
				fireFountain [index].SetActive (true);
				activeFountains += 1;
			}
			Invoke ("fireDuration", 0.5f);
		}
		if(trigger[0].IsTouching(playerCollider) && trigger[0].enabled)
		{
			shootFire = true;
			trigger[0].enabled = false;
		}

		if(trigger[1].IsTouching(playerCollider) && trigger[1].enabled)
		{
			trigger[1].enabled = false;
		}
	}

	void fireDuration()
	{
		fireFountain [index].SetActive (false);
		activeFountains = 0;
	}

    public void RoundEnded()
    {
        fader.SceneEnding(2);
    }
}
