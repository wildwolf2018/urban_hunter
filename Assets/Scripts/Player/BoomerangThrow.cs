using UnityEngine;
using System.Collections;

public class BoomerangThrow : MonoBehaviour {
	public GameObject boomerang;
	public Transform spawnPoint;
	public PlayerMovement playerMovement;
	public float throwRate = 4f;
	public float elapsedThrowTime = 0f;

	GameObject boomerangClone;
	Animator anim;
	
	
	void Awake()
	{
		anim = GetComponent<Animator> ();
	}
	

	void Update () 
	{ 
		elapsedThrowTime += Time.deltaTime;
		if (playerMovement.throwBoomer && elapsedThrowTime > throwRate) 
		{  
		 	anim.SetTrigger("throw");
			boomerangClone = Instantiate (boomerang, spawnPoint.position, Quaternion.identity) as GameObject;
			playerMovement.throwBoomer = false;
			elapsedThrowTime = 0f;
		} 
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("Boomerang"))
		{
			anim.SetTrigger("catch");
			Destroy(boomerangClone);
		}
	}
	
}
