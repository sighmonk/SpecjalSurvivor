using Godot;
using System;

public partial class KillCounter : Label
{
    private int killCount = 0;

    public override void _Ready()
    {
		var gameEvents = GetNode<GameEvents>("/root/GameEvents");

		// Podłącz metodę IncrementKillCount do zdarzenia EnemyKilled
		gameEvents.EnemyKilled += IncrementKillCount;

        // Zaktualizuj tekst Label na początku
        UpdateKillCounter();
    }

    public override void _ExitTree()
    {
        // GameEvents jest autoloadem, więc przeżywa restart sceny - trzeba się odpiąć
        var gameEvents = GetNodeOrNull<GameEvents>("/root/GameEvents");
        if (gameEvents != null)
        {
            gameEvents.EnemyKilled -= IncrementKillCount;
        }
    }

    public void IncrementKillCount()
    {
        killCount++;
        UpdateKillCounter();
    }

    private void UpdateKillCounter()
    {
        // Ustaw tekst Label na aktualną liczbę zabitych przeciwników
        Text = "piwo: " + killCount.ToString();
    }
}
