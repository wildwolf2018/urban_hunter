using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour {
	public Canvas exitMenu;
	public Button startButton;
	public Button exitButton;
	private Canvas thisCnavas;
	private bool sceneEnding = false;
	private ScreenFader fader;//fades the screen in and out

	void Awake()
	{
		exitMenu = exitMenu.GetComponent<Canvas> ();
		startButton = startButton.GetComponent<Button> ();
		exitButton = startButton.GetComponent<Button> ();
		thisCnavas = GetComponent<Canvas> ();
		fader = GameObject.Find ("Fader").GetComponent<ScreenFader> ();
	}

	void Update()
	{
		if (sceneEnding)
			fader.SceneEnding (0);
	}
	
	//executes when exit button is pressed
	public void Exit()
	{
		exitMenu.enabled = true;
		thisCnavas.enabled = false;

	}

	//executes when no button is pressed
	public void NoButton()
	{
		exitMenu.enabled = false;
		thisCnavas.enabled = true;
	}

	//executes when yes button is pressed
	public void YesButton()
	{
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit ();
        #endif
    }

    //executes when game starts
    public void StartGame()
	{
		thisCnavas.enabled = false;
		sceneEnding = true;
	}

}
