using UnityEngine;
using System.Collections;

public abstract class EnemyHealthBase : MonoBehaviour {
	protected abstract void Awake();
	protected abstract void Update();
	public abstract void Damage(int amount);
}
