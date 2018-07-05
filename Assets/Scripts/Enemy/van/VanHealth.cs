using UnityEngine;
using System.Collections;

public class VanHealth : EnemyHealthBase {
	public float screenFadeSpeed = 5f;
	public int maximumHealth = 20;
	public BoxCollider2D top;
    public BoxCollider2D bottom;

	private int ENEMY_LAYER_MASK = 10;
	private int PLAYER_LAYER_MASK = 9;
	private int TOP_COLLIDER_MASK = 12;
	private int currentHealth = 20;
	private bool damage = false;
	private bool isDead = false;
	private Color screenFadeColor = new Color (0f, 1f, 0f, 1f);
	private SpriteRenderer spriteRender;
	private VanMovement vanMovement;
	private ScoreManager playerScore;

	protected override void Awake()
	{
		currentHealth = maximumHealth;
		spriteRender = GetComponent<SpriteRenderer> ();
		vanMovement = GetComponent<VanMovement> ();
		playerScore = GameObject.FindGameObjectWithTag ("Score").GetComponent<ScoreManager> ();
	}

	protected override void Update()
	{
		if (damage) {
			spriteRender.color = screenFadeColor;
		} else {
			spriteRender.color = Color.Lerp (spriteRender.color, Color.white, screenFadeSpeed * Time.deltaTime);
		}
		damage = false;
		if (isDead) {
			Physics2D.IgnoreLayerCollision(ENEMY_LAYER_MASK, PLAYER_LAYER_MASK, true);
			Physics2D.IgnoreLayerCollision(ENEMY_LAYER_MASK, TOP_COLLIDER_MASK, true);
		}
	}
	
	public override void Damage(int damageAmount)
	{
		damage = true;
		currentHealth -= damageAmount;
		playerScore.IncreaseScore(100);
		if(currentHealth <= 0 && !isDead)
		{
			playerScore.IncreaseScore(500);
			vanMovement.enabled = false;
			isDead = true;
            if (top == null)
                top = GameObject.Find("vanTop").GetComponent<BoxCollider2D>();
			top.enabled = false;
            bottom.enabled = false;
            spriteRender.color = new Color(0.5f, 0.5f, 0.5f);
            gameObject.GetComponent<VanHealth>().enabled = false;
        }
	}
}
