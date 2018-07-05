using UnityEngine;
using System.Collections;

public class BoxHealth : EnemyHealthBase {
	public int maximumHealth = 200;
	public int currentHealth;
	public bool isDead = false;
	public float screenFadeSpeed = 5f;
	public ParticleSystem explosionEffect;
	public GameObject healthPack;

	private SpriteRenderer spriteRender;
	private Color screenFadeColor = new Color (0f, 1f, 0f, 1f);
	private bool damage = false;
	private Transform waypoint;
	
	protected override void Awake()
	{
		currentHealth = maximumHealth;
		spriteRender = GetComponent<SpriteRenderer> ();
		explosionEffect = gameObject.GetComponentInChildren<ParticleSystem> ();
		waypoint = GameObject.Find ("point1").GetComponent<Transform> ();

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
		if(currentHealth <= 0 && !isDead)
		{
			isDead = true;
			waypoint.localPosition = new Vector2(waypoint.localPosition.x, -4.3f);
			healthPack.SetActive(true);
			explosionEffect.Play ();
			Destroy(gameObject, 1f);
		}
	}		
}
