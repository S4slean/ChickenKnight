using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable  
{
	void ReceiveDamage(int dmg);
	void Heal(int heal);
	void Die();
}
