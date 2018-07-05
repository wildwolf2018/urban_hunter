using UnityEngine;
using System.Collections;

public class Saw1Health : EnemyHealthBase {
	public float screenFadeSpeed = 5f;
	public int maximumHealth = 200;
	public Saw1Movement sawMovement;
	public Movement sawHeadMovement;

	private int currentHealth;
	private bool damage = false;
	public bool isDead = false;
	private Color screenFadeColor = new Color (0.5f, 0.5f, 0.5f, 1f);
	private SpriteRenderer spriteRender;
	private CircleCollider2D sawCollider;
	private ScoreManager playerScore;
	
	protected override void Awake()
	{
		currentHealth = maximumHealth;
		spriteRender = GetComponent<SpriteRenderer> ();
		sawHeadMovement = GetComponent<Movement> ();
		sawCollider = GetComponent<CircleCollider2D> (); 
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
			spriteRender.color = Color.Lerp (spriteRender.color, Color.black, screenFadeSpeed * Time.deltaTime);
			sawCollider.enabled = false;
		}
	}
	
	public override void Damage(int damageAmount)
	{
		damage = true;
		currentHealth -= damageAmount;
		playerScore.IncreaseScore(500);
		if(currentHealth <= 0 && !isDead)
		{
		    playerScore.IncreaseScore(5000);
			sawMovement.enabled = false;
			sawHeadMovement.enabled = false;
			isDead = true;
		}
	}
}
