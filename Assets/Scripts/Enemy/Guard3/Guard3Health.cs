using UnityEngine;
using System.Collections;

public class Guard3Health : EnemyHealthBase {
	public float screenFadeSpeed = 5f;
	public int maximumHealth = 20;
	
	private int currentHealth = 20;
	private Guard3BulletFiring guardFire;
	private Guard3Movement guardMovement;
	private bool damage = false;
	private bool isDead = false;
	private Color guardDamageColor = new Color (0f, 1f, 0f, 1f); // gurad color when health decreases
	private SpriteRenderer spriteRender;
	private Animator anim;
	private BoxCollider2D guardCollider;
	private ScoreManager playerScore;
	
	protected override void Awake()
	{
		currentHealth = maximumHealth;
		spriteRender = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
		guardFire = GetComponent<Guard3BulletFiring> ();
		guardMovement = GetComponent<Guard3Movement> ();
		guardCollider = GetComponent<BoxCollider2D>();
		playerScore = GameObject.FindGameObjectWithTag ("Score").GetComponent<ScoreManager> ();
	}

	protected override void Update()
	{
		if (damage) {
			spriteRender.color = guardDamageColor;
		} else {
			spriteRender.color = Color.Lerp (spriteRender.color, Color.white, screenFadeSpeed * Time.deltaTime);
		}
		damage = false;
		if (isDead) {
			guardCollider.size = Vector2.Lerp(guardCollider.size, new Vector2(1.41f, 0.58f), Time.deltaTime * 3f);
		}
	}

	//calculates health after damage 
	public override void Damage(int damageAmount)
	{
		damage = true;
		currentHealth -= damageAmount;
		playerScore.IncreaseScore(100);
		if(currentHealth <= 0 && !isDead)
		{
			guardFire.enabled = false;
			guardMovement.enabled = false;
			anim.SetLayerWeight(1, 0f);
			anim.SetTrigger("die");
			isDead = true;
			playerScore.IncreaseScore(1000);
			Destroy(gameObject, 2f);
		}
	}	
}
