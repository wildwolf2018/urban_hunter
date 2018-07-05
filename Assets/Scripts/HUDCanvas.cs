using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDCanvas : MonoBehaviour {
	private Text score;
	private Text ammo;

	void Awake()
	{
		DontDestroyOnLoad (gameObject);
		score = GameObject.FindGameObjectWithTag ("Score").GetComponent<Text> ();
		ammo = GameObject.FindGameObjectWithTag ("Ammo").GetComponent<Text> ();
	}

	void Update()
	{
		if(SceneManager.GetActiveScene().buildIndex == 0)
		{
			score.text = "";
			ammo.text = "";
		}
	}

	
}
