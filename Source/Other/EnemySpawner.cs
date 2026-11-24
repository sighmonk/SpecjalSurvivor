using Godot;
using System;

public partial class EnemySpawner : Node2D
{
    [Export] private Godot.Collections.Array<PackedScene> _enemyScenes = new(); // Scenes of enemies to pick from
    [Export] private int _enemiesPerWave = 5; // Number of enemies in the first wave
    [Export] private int _extraEnemiesPerWave = 2; // Extra enemies in each following wave
    [Export] private float _timeBetweenWaves = 10f; // Delay between waves (in seconds)
    [Export] private float _timeBetweenSpawns = 1f; // Delay between spawning individual enemies (in seconds)
    [Export] private int _maxWaves = 5; // Maximum number of waves to spawn
    [Export] private float _minSpawnDistance = 200f; // Smallest distance from the player
    [Export] private float _spawnRadius = 400f; // Largest distance from the player

    private int _currentWave = 0;
    private int _enemiesRemainingInWave;
    private float _waveTimer = 0f;
    private float _spawnTimer = 0f;
    private bool _isSpawning = false;
    private Node2D _player;

    public override void _Ready()
    {
        _player = GetTree().GetFirstNodeInGroup("player") as Node2D;

        if (_player == null)
        {
            GD.PrintErr("Player not found!");
            return;
        }

        StartNextWave();
    }

    public override void _Process(double delta)
    {
        if (_currentWave >= _maxWaves)
        {
            GD.Print("All waves completed!");
            return;
        }

        if (_isSpawning)
        {
            _spawnTimer += (float)delta;

            // spawnuj wrogów co określony czas
            if (_spawnTimer >= _timeBetweenSpawns && _enemiesRemainingInWave > 0)
            {
                SpawnEnemy();
                _enemiesRemainingInWave--;
                _spawnTimer = 0f;
            }

            // sprawdź czy skończyliśmy falę
            if (_enemiesRemainingInWave <= 0)
            {
                _isSpawning = false;
                _waveTimer = 0f;
            }
        }
        else
        {
            _waveTimer += (float)delta;

            // zacznij nową falę po upływie czasu
            if (_waveTimer >= _timeBetweenWaves)
            {
                StartNextWave();
            }
        }
    }

    private void StartNextWave()
    {
        _currentWave++;
        _enemiesRemainingInWave = _enemiesPerWave + (_currentWave - 1) * _extraEnemiesPerWave;
        _isSpawning = true;
    }

    private void SpawnEnemy()
    {
        if (_enemyScenes.Count == 0)
        {
            GD.PrintErr("No enemy scenes assigned!");
            return;
        }

        if (_player == null)
        {
            GD.PrintErr("Player not found!");
            return;
        }

        PackedScene enemyScene = _enemyScenes[(int)(GD.Randi() % (uint)_enemyScenes.Count)];
        Node2D enemy = (Node2D)enemyScene.Instantiate();
        AddChild(enemy);

        // losowa pozycja wokół gracza
        Vector2 spawnPosition = GetRandomPositionAroundPlayer();
        enemy.Position = spawnPosition;
    }

    private Vector2 GetRandomPositionAroundPlayer()
    {
        float angle = (float)GD.RandRange(0, 2 * Mathf.Pi);

        float distance = (float)GD.RandRange(_minSpawnDistance, _spawnRadius);

        Vector2 spawnOffset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
        return _player.Position + spawnOffset;
    }
}