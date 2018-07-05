using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour {
	public int score;
	private Slider healthSlider;
	private GameManager gameManager;
	private GameObject sliderUI;
	Text text;
	
	void Awake()
	{
		text = GetComponent<Text> ();
		gameManager = GameObject.Find("GameManager_01").GetComponent<GameManager> ();
		healthSlider = GameObject.FindGameObjectWithTag ("PlayerHealth").GetComponent<Slider> ();
		score = 0;
	}
	
	void Update()
	{   
		if (SceneManager.GetActiveScene().buildIndex == 0) 
			score = 0;
		text.text = "$" + score;
		healthSlider.value = gameManager.playerHealth;
		if(score != 0)
			gameManager.updateScore(score);
	}
	
	public void IncreaseScore(int increase)
	{
		score += increase;
	}
}
