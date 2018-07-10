using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameResults : MonoBehaviour {
    private Text _time;
    private Text coinsTotal;
    private Text score;
    private GameManager gameManager;
    private bool display = true;

    public object Math { get; private set; }

    void Start () {
        _time = GameObject.FindGameObjectWithTag("TimeResults").GetComponent<Text>();
        coinsTotal = GameObject.FindGameObjectWithTag("CoinsTotal").GetComponent<Text>();
        score = GameObject.FindGameObjectWithTag("ScoreResults").GetComponent<Text>();
        gameManager = GameObject.Find("GameManager_01").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (display)
        { 
            FindTime();
            display = false;
        }
	}

    private void FindTime()
    {
        float times = gameManager.elapsedTime;
        int timesToInt = Mathf.FloorToInt(times);
        int seconds = timesToInt % 60;
        int minutes = (timesToInt - seconds) / 60;
        float tempVar = times - timesToInt;
        float milliSeconds = Mathf.Ceil(tempVar * 100);
        _time.text = minutes + ":" + seconds + ":" + milliSeconds;
        coinsTotal.text = gameManager.coinCount.ToString();
        score.text = gameManager.playerScore.ToString();
    }

    public void BackToStart()
    {
        SceneManager.LoadScene("main_menu");
    }

}
