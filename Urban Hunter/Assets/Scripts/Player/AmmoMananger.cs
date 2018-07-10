using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AmmoMananger : MonoBehaviour {
	public static int ammo = 50;
	private GameManager gameManager;

	Text text;

	void Awake()
	{
		text = GetComponent<Text> ();
		gameManager = GameObject.Find("GameManager_01").GetComponent<GameManager> ();
	}

	void Update()
	{   
		if (SceneManager.GetActiveScene().buildIndex == 0) 
			ammo = 50;
		text.text = "" + ammo;
		gameManager.ammunition = ammo;
	}

	public void IncreaseAmmo(int increase)
	{
		ammo += increase;
	}
}
