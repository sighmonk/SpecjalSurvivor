using Godot;
using System;

public partial class HealthComponent : Node2D
{

	[Export] public float MaxHealth = 100;
	public float CurrentHealth;
	private ProgressBar _healthBar;

	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
		_healthBar = GetChildOrNull<ProgressBar>(0);
		if (_healthBar != null)
		{
			_healthBar.MaxValue = MaxHealth;
			_healthBar.Value = CurrentHealth;
		}
	}

	public void Damage(float damage)
	{
		CurrentHealth -= damage;
		if (_healthBar != null)
		{
			_healthBar.Value = CurrentHealth;
		}
		if (CurrentHealth <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		var gameEvents = GetNode<GameEvents>("/root/GameEvents");
		Node owner = GetParent();

		// o tym, kto to jest, decydują grupy - nie trzeba znać konkretnych klas
		if (owner.IsInGroup("enemies"))
		{
			gameEvents.EmitSignal(GameEvents.SignalName.EnemyKilled);
			owner.QueueFree();
		}
		else if (owner.IsInGroup("player"))
		{
			gameEvents.PlayerDied();
		}
	}
}
