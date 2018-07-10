using UnityEngine;
using System.Collections;

public class HellicopterHealth : EnemyHealthBase {
	public float screenFadeSpeed = 5f;
	public int maximumHealth = 10;
	public HelicopterMovement  movement;
	public LayerMask groundLayer;
	public ParticleSystem explosionEffect;
	public Vector2 velocity = new Vector2(0.01f, -0.01f);

	private BoxCollider2D heliCollider;
	private int currentHealth;
	private bool damage = false;
	public bool isDead = false;
	private Color screenFadeColor = new Color (0.5f, 0.5f, 0.5f, 1f);
	private SpriteRenderer spriteRender;
	private Rigidbody2D rdb2;
	private ScoreManager playerScore;
	
	protected override void Awake()
	{
		currentHealth = maximumHealth;
		spriteRender = GetComponent<SpriteRenderer> ();
		movement = GetComponent<HelicopterMovement> ();
		rdb2 = GetComponent<Rigidbody2D> ();
		heliCollider = GetComponent<BoxCollider2D> ();
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
			movement.enabled = false;
		}
		if (heliCollider.IsTouchingLayers (groundLayer)) {
				explosionEffect.Stop ();
				explosionEffect.Play ();
				Destroy (gameObject,0.5f);
		}
	}
	
	 void FixedUpdate()
	{
		if (isDead) {
			rdb2.MovePosition (rdb2.position + velocity * Time.fixedDeltaTime);
		}
	}
	
	public override void Damage(int damageAmount)
	{
		damage = true;
		currentHealth -= damageAmount;
		playerScore.IncreaseScore(200);
		if(currentHealth <= 0 && !isDead)
		{
			playerScore.IncreaseScore(2000);
			isDead = true;
		}
	}
}
