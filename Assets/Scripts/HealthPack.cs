using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthPack : MonoBehaviour {
	public int healthIncrease = 100;
	public LayerMask playerLayerMask;

	private int extrasLayerMask = 20;
	private int enemyLayerMask = 10;
	private PlayerHealth playerHealth;

	private Slider slider;
	private BoxCollider2D healthCollider;
	private BoxCollider2D playerCollider;
	private ScoreManager score;
	private GameObject tempPlayer;
	private GameObject sliderTemp;
	private GameObject scoreTemp;
	private HealthPack pack;

	void Awake()
	{
		playerHealth = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
		healthCollider = GetComponent<BoxCollider2D> ();
		sliderTemp = GameObject.FindGameObjectWithTag ("PlayerHealth");
		if (sliderTemp != null)
			slider = sliderTemp.GetComponent<Slider> ();
		scoreTemp = GameObject.FindGameObjectWithTag ("Score");
		if (scoreTemp != null)
			score = scoreTemp.GetComponent<ScoreManager> ();
		pack = GetComponent<HealthPack> ();
		pack.enabled = true;
	}

	void Update()
	{
		sliderTemp = GameObject.FindGameObjectWithTag ("PlayerHealth");
		scoreTemp = GameObject.FindGameObjectWithTag ("Score");
		if (sliderTemp != null)
			slider = sliderTemp.GetComponent<Slider> ();
		if (scoreTemp != null)
			score = scoreTemp.GetComponent<ScoreManager> ();
		if (Physics2D.IsTouchingLayers (healthCollider, playerLayerMask)) {
            if(playerHealth == null)
            {
                playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
            }
			slider = sliderTemp.GetComponent<Slider> ();
			if(slider.value >= 100)
			{
				score.IncreaseScore(1500);
			}
			playerHealth.IncreaseHealth (healthIncrease);
			Destroy(gameObject);
		}
		Physics2D.IgnoreLayerCollision(extrasLayerMask, enemyLayerMask);
		Physics2D.IgnoreLayerCollision(extrasLayerMask, extrasLayerMask);
	}

	void OnTriggerEnter2D(Collider2D other)
	{ 
		
		if (other.CompareTag ("TopCollider") || other.CompareTag ("BottomCollider")) {
			slider = GameObject.FindGameObjectWithTag ("PlayerHealth").GetComponent<Slider> ();
			if(slider.value >= 100)
			{
				score.IncreaseScore(1500);
			}
            if(playerHealth != null)
			    playerHealth.IncreaseHealth (healthIncrease);
			Destroy(gameObject, 0.2f);
		}
	}//OnTriggerEnter2D
}
