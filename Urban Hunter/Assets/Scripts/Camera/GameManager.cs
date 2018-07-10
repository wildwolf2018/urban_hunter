using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public static GameManager instance = null;
	public int playerScore;// holds player's current score
	public int playerHealth = 100;// holds player's current health
	public int coinCount;// holds player's current number of coins
	public float time;// holds time taken to complete the game in seconds
	public int ammunition = 50;
	public float elapsedTime = 0f;
	private int levelIndex;
	private GameObject hudCanvas;
	private ScoreManager _score;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
		hudCanvas = GameObject.Find ("HUDCanvas");
		_score = GameObject.FindGameObjectWithTag ("Score").GetComponent<ScoreManager> ();
	}

	void Update()
	{
        levelIndex = SceneManager.GetActiveScene().buildIndex;
		if (levelIndex == 0) {
			hudCanvas.GetComponent<Canvas> ().enabled = false;
			playerScore = 0;
			playerHealth = 100;
			ammunition = 50;
		} else {
			hudCanvas.GetComponent<Canvas> ().enabled = true;
            if(_score != null)
                playerScore = _score.score;
			elapsedTime += Time.deltaTime;
		}
	}
		
	public void updateScore(int score)
	{
		playerScore = score;
	}

	public void updateHealth(int currentHealth)
	{
		playerHealth = currentHealth;
	}

	public void updateCoins()
	{
		coinCount += 1;
	}
}
