using UnityEngine;
using System.Collections;

public class Girl2Health : EnemyHealthBase {
	public float screenFadeSpeed = 5f;
	public int maximumHealth = 20;
	
	private Girl2Movement girlMovement;
	private int currentHealth;
	private bool damage = false;
	public bool isDead = false;
	private Color screenFadeColor = new Color (0f, 1f, 0f, 1f);
	private SpriteRenderer spriteRender;
	private Transform girlTransform;
	private ScoreManager playerScore;
	
	protected override void Awake()
	{
		currentHealth = maximumHealth;
		spriteRender = GetComponent<SpriteRenderer> ();
		girlMovement = GetComponent<Girl2Movement> ();
		girlTransform = GetComponent<Transform> ();
		playerScore = GameObject.FindGameObjectWithTag ("Score").GetComponent<ScoreManager> ();
	}
	protected override void Update()
	{
		bool temp = girlMovement.faceRight;
		if (damage) {
			spriteRender.color = screenFadeColor;
		} else {
			spriteRender.color = Color.Lerp (spriteRender.color, Color.white, screenFadeSpeed * Time.deltaTime);
		}
		damage = false;
		if (isDead) {
			if(temp)
				girlTransform.rotation = Quaternion.Slerp(girlTransform.rotation, Quaternion.Euler(0f, 180f, -90f), Time.deltaTime * 4f);
			else
				girlTransform.rotation = Quaternion.Slerp(girlTransform.rotation, Quaternion.Euler(0f, 0f, -90f), Time.deltaTime * 4f);
			
			//GameManager.numberOfDefeatedEnemies++;
		}
	}
	
	public override void Damage(int damageAmount)
	{
		damage = true;
		currentHealth -= damageAmount;
		playerScore.IncreaseScore(250);
		if(currentHealth <= 0 && !isDead)
		{
			playerScore.IncreaseScore(3000);
			girlMovement.enabled = false;
			isDead = true;
			Destroy(gameObject, 3f);
		}
	}
}
