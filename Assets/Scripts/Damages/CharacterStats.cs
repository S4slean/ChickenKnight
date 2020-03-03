using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour, IDamageable
{
	[Header("References")]
	public Character character;

	[Header("Stats")]
	public int health;
	public int maxHealth;

	private void Start()
	{
		health = Mathf.Clamp(health, 0, maxHealth);
		if (health <= 0) Die();
	}

	public void Die()
	{
		Debug.Log("Death anim not implemented yet for " + gameObject.name);
		Destroy(gameObject);
	}

	public void Heal(int heal)
	{
		health = Mathf.Clamp(health + heal, 0, maxHealth);
	}

	public void ReceiveDamage(int dmg)
	{
		health = Mathf.Clamp(health - dmg, 0, maxHealth);
		if (health <= 0) Die();
	}
}
