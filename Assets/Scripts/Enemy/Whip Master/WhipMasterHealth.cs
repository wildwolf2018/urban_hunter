using UnityEngine;
using System.Collections;

public class WhipMasterHealth : EnemyHealthBase {
	public int maximumHealth = 100;
	public int currentHealth = 60;
	public bool isDead = false;
	public float screenFadeSpeed = 5f;

	private SpriteRenderer spriteRender;
	private Color screenFadeColor = new Color (0f, 1f, 0f, 1f);
	private Animator anim;
	private bool damage = false;
	private ScoreManager playerScore;

	protected override void Awake()
	{
		currentHealth = maximumHealth;
		spriteRender = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
		playerScore = GameObject.FindGameObjectWithTag ("Score").GetComponent<ScoreManager> ();
	}
	protected override void Update()
	{
		if (damage) 
			spriteRender.color = screenFadeColor;	
		damage = false;
	}
	
	public override void Damage(int damageAmount)
	{
		damage = true;
		currentHealth -= damageAmount;
		playerScore.IncreaseScore(100);
		if(currentHealth <= 0 && !isDead)
		{
			anim.SetTrigger("die");
			playerScore.IncreaseScore(10000);
			isDead = true;
		}
	}	
}
