using UnityEngine;
using System.Collections;

public class CannonHealth : EnemyHealthBase {
	public float screenFadeSpeed = 5f;
	public int maximumHealth = 100;
	public ParticleSystem explosionEffect;
	
	private ShootCannon cannonShoot;
	private int currentHealth;
	private bool damage = false;
	public bool isDead = false;
	private Color screenFadeColor = new Color (0f, 1f, 0f, 1f);
	private SpriteRenderer spriteRender;
	private ScoreManager playerScore;
	
	protected override void Awake()
	{
		currentHealth = maximumHealth;
		spriteRender = GetComponent<SpriteRenderer> ();
		cannonShoot = GetComponent<ShootCannon> ();
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
	}
	
	public override void Damage(int damageAmount)
	{
		damage = true;
		currentHealth -= damageAmount;
		playerScore.IncreaseScore(500);
		if(currentHealth <= 0 && !isDead)
		{
			isDead = true;
			playerScore.IncreaseScore(2500);
			cannonShoot.enabled = false;
			explosionEffect.Stop ();
			explosionEffect.Play ();
			Destroy (gameObject, 1f);
		}
	}
}
