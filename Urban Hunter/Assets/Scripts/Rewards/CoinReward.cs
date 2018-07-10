using UnityEngine;
using System.Collections;

public class CoinReward : MonoBehaviour {
	public int scoreIncrease = 1000;
	public LayerMask playerLayerMask;

	private int extrasLayerMask = 20;
	private int enemyLayerMask = 10;

	private BoxCollider2D coinCollider;
	private ScoreManager playerScore;
	private GameManager gameManager;
	
	void Awake()
	{
		playerScore = GameObject.FindGameObjectWithTag ("Score").GetComponent<ScoreManager> ();
		gameManager = GameObject.Find("GameManager_01").GetComponent<GameManager> ();
		coinCollider = GetComponent<BoxCollider2D> ();
	}

	void Update(){
		if (Physics2D.IsTouchingLayers (coinCollider, playerLayerMask)) {
			playerScore.IncreaseScore (scoreIncrease);
			gameManager.updateCoins ();
			Destroy (gameObject);
		}
		Physics2D.IgnoreLayerCollision(extrasLayerMask, enemyLayerMask);
		Physics2D.IgnoreLayerCollision(extrasLayerMask, extrasLayerMask);
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{ 
		
		if (other.CompareTag ("TopCollider") || other.CompareTag ("BottomCollider")) {
			playerScore.IncreaseScore (scoreIncrease);
			gameManager.updateCoins();
			Destroy(gameObject);
		}
	}//OnTriggerEnter2D
}
