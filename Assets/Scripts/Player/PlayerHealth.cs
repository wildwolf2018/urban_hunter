using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour 
{
	public float screenFadeSpeed = 5f;
	public int maximumHealth = 100;
	public int currentHealth = 100;
	public bool isDead = false;
	public bool isDestroyed = false;

	public BoxCollider2D boxCollider;
	
	bool damage = false;
	private int ENEMY_LAYER_MASK = 10;
	private int PLAYER_LAYER_MASK = 9;
	Color screenFadeColor = new Color (1f, 0f, 0f, 1f);

	private Slider healthSlider;
	private  Transform playerTransform;
	SpriteRenderer spriteRender;
	Animator anim;
	PlayerMovement playerMovement;
	Vector2 targetPos;
	Rigidbody2D playerRdb2;
	private GameManager gameManager;
	private LevelManager level1Manager;
	private LevelManager2 level2Manager;
	private LevelManager3 level3Manager;
	private Camera cam;
	private ScreenFader fader;
	private GameObject temp1;
	private GameObject temp2;
	private GameObject temp3;

	void Awake()
	{
		playerTransform = GetComponent<Transform> ();
		playerRdb2 = playerTransform.GetComponent<Rigidbody2D> ();
		healthSlider = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<Slider> ();
		anim = GetComponent<Animator> ();
		spriteRender = GetComponent<SpriteRenderer> ();
		playerMovement = GetComponent<PlayerMovement> ();
		gameManager = GameObject.Find("GameManager_01").GetComponent<GameManager> ();
		temp1 = GameObject.Find ("GameManager");
		if(temp1 != null)level1Manager = temp1.GetComponent<LevelManager> ();
		temp2 = GameObject.Find ("LevelManager2");
		if (temp2 != null)
			level2Manager = temp2.GetComponent<LevelManager2> ();
		temp3 = GameObject.Find ("LevelManager");
		if(temp3 != null)level3Manager = temp3.GetComponent<LevelManager3> ();
		fader = GameObject.Find ("Fader").GetComponent<ScreenFader> ();
		cam = Camera.main;
	}


	void Update()
	{
		gameManager.updateHealth(currentHealth);
		if (damage) {
			spriteRender.color = screenFadeColor;
		} 
		else 
		{
			spriteRender.color = Color.Lerp(spriteRender.color, Color.white, screenFadeSpeed * Time.deltaTime);
		}
		damage = false;
		if (isDead) {
			fader.FadeToDark ();
			Physics2D.IgnoreLayerCollision (PLAYER_LAYER_MASK, ENEMY_LAYER_MASK, true);
			playerTransform.position = Vector2.Lerp (playerTransform.position, 
			                                        new Vector2 (playerTransform.position.x, playerTransform.position.y - 100f), 0.01f * Time.deltaTime);
			playerRdb2.isKinematic = true;
			if (!CameraUtility.IsRendererInFrustum (boxCollider, cam)) {
				isDestroyed = true;
				Destroy (gameObject);
			}
		} 
		if(!isDead){
			if(temp1 != null && !level1Manager.goNextRound)
				fader.FadeToClear ();
			if(temp2 != null && !level2Manager.goNextRound)
				fader.FadeToClear ();
			if(temp3 != null && !level3Manager.goNextRound)
				fader.FadeToClear ();
		}
	}

	public void Damage(int damageAmount,float cause)
	{
		damage = true;
		currentHealth -= damageAmount;
        healthSlider = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<Slider> ();
			healthSlider.value = currentHealth;
			if (currentHealth <= 0 && !isDead) {
				anim.SetTrigger ("die");
				isDead = true;
				playerMovement.enabled = false;
			}
	}

	public void IncreaseHealth(int increaseAmount)
	{ 
		currentHealth = Mathf.Clamp (currentHealth + increaseAmount, 0, maximumHealth);
		healthSlider = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<Slider> ();
		healthSlider.value = currentHealth;
	}

}
