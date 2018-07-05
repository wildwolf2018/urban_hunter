using UnityEngine;
using System.Collections;

public class DogHealth : EnemyHealthBase {
	public float screenFadeSpeed = 5f;
	public int maximumHealth = 10;
	public bool isDead = false;

	private DogMovement dogMovement;
	private int currentHealth;
	private bool damage = false;
	private Color screenFadeColor = new Color (0f, 1f, 0f, 1f);
	private SpriteRenderer spriteRender;
	private Animator anim;
	private Transform dogTransform;
	private Rigidbody2D rdb2;
	private ScoreManager playerScore;
	
	protected override void Awake()
	{
		currentHealth = maximumHealth;
		spriteRender = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
		rdb2 = GetComponent<Rigidbody2D> ();
		dogMovement = GetComponent<DogMovement> ();
		dogTransform = GetComponent<Transform> ();
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
			dogMovement.enabled = false;
			spriteRender.color = Color.Lerp (spriteRender.color, Color.red, screenFadeSpeed * Time.deltaTime);
			dogTransform.rotation = Quaternion.Slerp(dogTransform.rotation, Quaternion.Euler(270f, 0f, 0f),2f * Time.deltaTime);
		}
	}

	
	void FixedUpdate()
	{
		if (isDead) {
			rdb2.MovePosition (rdb2.position);
		}
	}
	
	public override void Damage(int damageAmount)
	{
		damage = true;
		currentHealth -= damageAmount;
		playerScore.IncreaseScore(50);
		if(currentHealth <= 0 && !isDead)
		{
			isDead = true;
			playerScore.IncreaseScore(200);
			anim.SetBool("run", false);
			Destroy(gameObject, 2f);
		}
	}
}
