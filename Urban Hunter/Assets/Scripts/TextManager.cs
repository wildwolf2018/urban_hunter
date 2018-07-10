using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextManager : MonoBehaviour {
	public Text text;
	private PlayerMovement player;
  
	void Awake()
	{
		DontDestroyOnLoad (gameObject);
    }

	public void RoundStarting()
	{
		text.text = "ROUND " + SceneManager.GetActiveScene().buildIndex;
		Invoke ("ClearText", 4f);
	}

	public void RoundEnding()	
	{
		text.text = "   ROUND COMPLETED";
	}

	public void ClearText()
	{
		text.text = "";
	}
}
