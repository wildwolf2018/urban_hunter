using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager3 : MonoBehaviour {
	public LayerMask playerLayerMask;
	public BoxCollider2D wakeBoss;
	public Transform[] helpObjects;
	public Transform helpStart;
	public GameObject dog;
	public Transform startPos;
	public float sendDogTime;
	public float dogRate;
	public float floatRate = 5f;
	public float elapsedFloatTime = 0f;
	public LayerMask extrasLayer;
	public GameObject player;
	public bool goNextRound = false;
	public Transform playerFinder;
	private WhipBossMovement boss;
	private WhipMasterHealth bossHealth;
	private bool moveBoss = false;
	private Transform helpTransform;
	private float angle = 0f;
	private int count = 0;
	private bool goLeft = false;
	private TextManager textManager;
	private GameObject temp;
	private ScreenFader fader;
	private PlayerHealth playerHealth;
    private List<GameObject> objects = new List<GameObject>();

    void Awake()
	{
		fader = GameObject.Find ("Fader").GetComponent<ScreenFader> ();
		temp = GameObject.Find ("Player");
		bossHealth = GameObject.FindGameObjectWithTag ("Boss").GetComponent<WhipMasterHealth> ();
		textManager = GameObject.Find ("TextCanvas").GetComponent<TextManager> ();
		textManager.RoundStarting ();
		fader.gameStarting = true;
	}
	
	void Update () 
	{
		Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(playerFinder.position, 50f, playerLayerMask);
		if (hitPlayer.Length == 0 && temp == null) {
            if (objects.Count != 0)
            {
                for (int i = 0; i < objects.Count; i++)
                    Destroy(objects[i]);
            }
            objects.Clear();
            temp = Instantiate (player, new Vector2(-44.4f, 0.3f), Quaternion.identity) as GameObject;
			playerHealth = temp.GetComponent<PlayerHealth>();
			playerHealth.currentHealth = 100;
		}
		if (bossHealth.isDead) {
			goNextRound = true;
			textManager.RoundEnding ();
			Invoke ("EndLevel", 3f);
		}
		spawnExtras ();
		if(wakeBoss.enabled && moveBoss)
		{
			boss = GameObject.FindGameObjectWithTag ("Boss").GetComponent<WhipBossMovement> ();
			boss.enabled = true;
			wakeBoss.enabled = false;
		}
		sendDogTime += Time.deltaTime;
		if (sendDogTime > dogRate && !bossHealth.isDead) {
            objects.Add(Instantiate(dog, startPos.position, Quaternion.identity));
			sendDogTime = 0f;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Player") || other.CompareTag("GlockBullet"))
		 moveBoss = true;
	}

	void spawnExtras()
	{
		Collider2D[] col = Physics2D.OverlapCircleAll (new Vector2 (helpStart.position.x + 42f, helpStart.position.y), 42f, extrasLayer);
		if (col.Length == 0 && count == 0 && elapsedFloatTime > floatRate) {
			int index = Random.Range(0, 3);
			helpTransform = Instantiate (helpObjects[index], helpStart.position, Quaternion.identity) as Transform;
			helpTransform.GetComponent<Rigidbody2D>().isKinematic = true;
			angle = 0f;
			count = 1;
			elapsedFloatTime = 0f;
		} else if (col.Length == 0 && count == 1)
			count = 0;
		if (angle >= 84f)
			goLeft = true;
		else if(angle < 0f)
			goLeft = false;
		if (angle <= 84f && col.Length == 1 && !goLeft) {
			helpTransform.position = new Vector2 (angle - 50f, 2.5f * Mathf.Cos (angle * 16f * Mathf.Deg2Rad) + 4f);
			angle += 0.1f;
		} else if (angle > 0f && col.Length == 1 && goLeft) {
			helpTransform.position = new Vector2 (angle - 50f, 2.5f * Mathf.Cos (angle * 16f * Mathf.Deg2Rad) + 4f);
			angle -= 0.1f;
		} else if (col.Length == 0) {
			elapsedFloatTime += Time.deltaTime;
			goLeft = false;
		}
	}
	
	void EndLevel()
	{
		textManager.ClearText();
        SceneManager.LoadScene("results");
    }
}
