using UnityEngine;
using UnityEngine.SceneManagement;
public class ScreenFader : MonoBehaviour {
	public float screenFadeSpeed = 1.5f;
	private SpriteRenderer sprite;
	public bool gameStarting = true;

	void Awake()
	{
		DontDestroyOnLoad (gameObject);
		sprite = GetComponent<SpriteRenderer> ();
	}
	void Update () 
	{
		if (gameStarting) 
		{
			SceneStart();
		}
	}

	public void SceneStart()
	{
		ClearScreen ();
		if (sprite.color.a <= 0.05f) 
		{
			sprite.color = Color.clear;
			gameStarting = false;
		}
	}

	public void SceneEnding(int level)
	{
		DarkScreen ();
		if (sprite.color.a >= 0.95f) 
		{
            sprite.color = Color.black;
            switch (level)
            {
                case 0:
                    SceneManager.LoadScene("circus_outside_level");
                    break;
                case 1:
                    SceneManager.LoadScene("inside_tent_level");
                    break;
                case 2:
                    SceneManager.LoadScene("boss_level");
                    break;
                default:
                    SceneManager.LoadScene("main_menu");
                    break;
            }
		}
	}
	
	public void FadeToClear()
	{
			sprite.color = Color.clear;
	}

	public void FadeToDark()
	{
		DarkScreen ();
		if (sprite.color.a >= 0.95f) 
		{
			sprite.color = Color.black;
		}
	}


	void ClearScreen()
	{
		sprite.color = Color.Lerp (sprite.color, Color.clear, screenFadeSpeed * Time.deltaTime);
	}
	
	void DarkScreen()
	{
		sprite.color = Color.Lerp (sprite.color, Color.black, screenFadeSpeed * Time.deltaTime);
	}
}
