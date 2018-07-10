using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
	public LayerMask enemyMask;
	public BoxCollider2D[] triggers;
	public Transform[] waypoints;
	public GameObject guard1;
	public GameObject van;
	public GameObject clownGirl;
	public GameObject clownGir2;
	public GameObject clownGir3;
	public GameObject clownGir4;
	public GameObject helicopter;
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
	public GameObject player;
	public GameObject clown1;
	public GameObject clown2;
	public GameObject dog;
	public GameObject cannon;
	public Transform start;
	public Transform end;
	public GameObject saw1;
	public GameObject saw2;
	public Dictionary<int, Point> dict = new Dictionary<int, Point>();
	public Transform[] playerPoints;
	
	public LayerMask playerLayerMask;

	private bool scrollScreen = false;
	private bool scrollScreen2 = false;

	private PlayerHealth playerHealth;
	private Saw1Movement saw1Movement;
	private Saw2Movement saw2Movement;
	private Transform startPos, endPos;
    private Transform spawnpoint1;
    private Transform spawnpoint2;
	private HellicopterHealth heliHealth;
	private BoxCollider2D playerCollider;
	private CameraFollower cameraBoundries;
	private RaycastHit hitInfo;
	private ScreenFader fader;
	private TextManager textManager;
	private Transform[]pathPoints;
	private Transform[]controlPoints;
	private Transform playerFinder;
	public List<GameObject> objects = new List<GameObject>();
	private GameObject temp;
	private GameObject temp1;
	private GameObject temp2;
	private GameObject temp3;
	private GameObject temp4;
	private GameObject temp5;
	private GameObject temp6;
	private Transform trigger4;
	private CircleCollider2D startCollider;
    private bool enemyCheck = false;
	public bool goNextRound = false;

	void Awake () 
	{
		fader = GameObject.Find ("Fader").GetComponent<ScreenFader> ();
		temp = GameObject.Find ("Player");
		playerCollider = temp.GetComponent<BoxCollider2D> ();
		playerHealth = temp.GetComponent<PlayerHealth> ();
		cameraBoundries = GameObject.FindWithTag ("MainCamera").GetComponent<CameraFollower> ();
		pathPoints = GameObject.Find ("ClownGirlWayPoints").GetComponentsInChildren<Transform> ();
        startPos = GameObject.Find("LineCastStart").GetComponent<Transform>();
        endPos = GameObject.Find ("LineCastEnd").GetComponent<Transform> ();
		textManager = GameObject.Find ("TextCanvas").GetComponent<TextManager> ();
		playerFinder =  GameObject.Find ("PlayerFinder").GetComponent<Transform> ();
		trigger4 = GameObject.Find ("Trigger4").GetComponent<Transform> ();
        spawnpoint2 = GameObject.FindGameObjectWithTag("HelicopterSpawnPoint").GetComponent<Transform>();
		textManager.RoundStarting ();
		fader.gameStarting = true;
	}
	
	void Update()
	{
		SpawnPlayer ();
		Scene1 ();
		Scene2 ();
		Scene3 ();
		Scene4 ();
    }

	void SpawnPlayer ()
	{
		RaycastHit2D[] hitPlayer = Physics2D.BoxCastAll(playerFinder.position, new Vector2 (300f, 300f), 0f, playerFinder.right, 400f, playerLayerMask);
		if (hitPlayer.Length == 0) {
			for (int i = 0; i < objects.Count; i++)
				Destroy (objects [i]);
			objects.Clear ();
            AmmoMananger.ammo = 50;
            if (cameraBoundries.xMin == -5.0f || cameraBoundries.xMin == 17f) {
				cameraBoundries.xMin = -5.0f;
				temp = Instantiate (player, playerPoints [0].position, Quaternion.identity) as GameObject;
				playerCollider = temp.GetComponent<BoxCollider2D> ();
				playerHealth = temp.GetComponent<PlayerHealth>();
				playerHealth.currentHealth = 100;
				triggers [0].enabled = true;
				triggers [1].enabled = true;
			} else if (cameraBoundries.xMin == 75f) {
				cameraBoundries.xMin = 75f;
				temp = Instantiate (player, playerPoints [1].position, Quaternion.identity) as GameObject;
				playerCollider = temp.GetComponent<BoxCollider2D> ();
				playerHealth = temp.GetComponent<PlayerHealth>();
				playerHealth.currentHealth = 100;
				triggers [2].enabled = true;
				triggers [3].enabled = true;
			} else if (cameraBoundries.xMin == 116f) {
				cameraBoundries.xMin = 75f;
				cameraBoundries.xMax = 113f;
				temp = Instantiate (player, playerPoints [1].position, Quaternion.identity) as GameObject;
				playerCollider = temp.GetComponent<BoxCollider2D> ();
				playerHealth = temp.GetComponent<PlayerHealth>();
				playerHealth.currentHealth = 100;
				triggers [2].enabled = true;
				triggers [3].enabled = true;
				triggers [4].enabled = true;
				scrollScreen = false;
				scrollScreen2 = false;
			} else if (cameraBoundries.xMin == 200f) {
				cameraBoundries.xMin = 200f;
				triggers [5].enabled = true;
				temp = Instantiate (player, playerPoints [2].position, Quaternion.identity) as GameObject;
				playerCollider = temp.GetComponent<BoxCollider2D> ();
				goNextRound = false;
			}//if-else
		}//if
        int enemyCount = Physics2D.BoxCastAll(start.position, new Vector2(300f, 300f), 0f, playerFinder.right, 0f, enemyMask).Length;
         if (hitPlayer.Length > 0 && !triggers [5].enabled && enemyCount == 0) {
				goNextRound = true;
                textManager.RoundEnding();
            Invoke("RoundEnded", 3f);
			}
		}

	void Scene1() 
	{
		if(triggers[0].IsTouching(playerCollider) && triggers[0].enabled)
		{
			objects.Add(Instantiate (guard1,waypoints[0].position, Quaternion.identity) as GameObject) ;
			triggers[0].enabled = false;
		}
		if(triggers[1].IsTouching(playerCollider) && triggers[1].enabled)
		{
			objects.Add(Instantiate (van,waypoints[1].position, Quaternion.identity) as GameObject);
			triggers[1].enabled = false;
            enemyCheck = true;
			cameraBoundries.xMin = 17f;
			cameraBoundries.xMax = 35f;
		}
        if(enemyCheck && !triggers[1].enabled)
        {
            RaycastHit2D hitInfo = Physics2D.Linecast(startPos.position, endPos.position, enemyMask);
            if (hitInfo.collider == null)
            {
                triggers[6].enabled = true;
                enemyCheck = false;
            }
        }
		if (triggers[6].IsTouching(playerCollider) && triggers [6].enabled) {
			triggers[6].enabled = false;
			cameraBoundries.xMin = 17f;
			cameraBoundries.xMax = 80f;
		}
	}//scene1

	void Scene2()
	{
		if (triggers [2].IsTouching (playerCollider) && triggers [2].enabled) {
			cameraBoundries.xMin = 75f;
			cameraBoundries.xMax = 112f;
			objects.Add(Instantiate (clownGirl,pathPoints[4].position, Quaternion.identity) as GameObject);
			objects.Add(Instantiate (clownGir2,pathPoints[6].position, Quaternion.identity) as GameObject);
			triggers [2].enabled = false;
		}

		if (triggers [3].IsTouching (playerCollider) && triggers [3].enabled) {
			objects.Add(Instantiate (clownGir3,pathPoints[1].position, Quaternion.identity) as GameObject);
			objects.Add(Instantiate (clownGir4,pathPoints[6].position, Quaternion.identity) as GameObject);
			temp1 = Instantiate(saw1, new Vector2(128.7f, -4.54f) , Quaternion.identity) as GameObject;
			temp1.GetComponent<Transform> ().rotation = Quaternion.Euler (65.966f, 0f, 0f);
			objects.Add(temp1);
			temp2 = Instantiate(saw2, new Vector2(157.75f, -4.52f) , Quaternion.identity) as GameObject;
			temp2.GetComponent<Transform> ().rotation = Quaternion.Euler (65.966f, 0f, 0f);
			objects.Add(temp2);
			triggers [3].enabled = false;
			scrollScreen = true;
		}
		if(!triggers [3].enabled && scrollScreen)
		{
			Collider2D[] cols = Physics2D.OverlapCircleAll (trigger4.position, 30f, enemyMask);
			if(cols.Length == 0)
			{
			  cameraBoundries.xMax = 160f;
			  scrollScreen = false;
			}
		}
	}//scene2

	void Scene3()
	{
		if (triggers [4].IsTouching (playerCollider) && triggers [4].enabled) {
			cameraBoundries.xMin = 116f;
			cameraBoundries.xMax = 150f ;
			temp1.GetComponent<Saw1Movement>().enabled = true;
			temp2.GetComponent<Saw2Movement>().enabled = true;
			objects.Add(Instantiate(helicopter, spawnpoint2.position, Quaternion.identity) as GameObject);
			triggers [4].enabled = false;
		}

		if (!triggers [4].enabled && !scrollScreen2) {
			if(!Physics2D.Linecast(new Vector2(spawnpoint2.position.x - 100f, spawnpoint2.position.y),
			                   new Vector2(spawnpoint2.position.x , spawnpoint2.position.y), enemyMask))
			{
				cameraBoundries.xMin = 116f;
				cameraBoundries.xMax = 240f;
				scrollScreen2 = true;
			}
		}
	}//scene3

	void Scene4()
	{

		if (triggers [5].IsTouching (playerCollider) && triggers [5].enabled) {
			cameraBoundries.xMin = 200f;
			objects.Add(Instantiate (clown1, new Vector2(245.8f, -2.8f), Quaternion.identity) as GameObject);
			objects.Add(Instantiate (clown2, new Vector2(229.5f, -2.3f), Quaternion.identity) as GameObject);
			objects.Add(Instantiate (dog, new Vector2(241.91f, -1.78f), Quaternion.identity) as GameObject);
			objects.Add(Instantiate (cannon, new Vector2(255f, -3.17f), Quaternion.identity) as GameObject);
			triggers [5].enabled = false;
		}
	}//scene4

    public void RoundEnded()
    {
        fader.SceneEnding(1);
    }
}
