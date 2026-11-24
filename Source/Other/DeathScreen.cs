using Godot;

public partial class DeathScreen : CanvasLayer
{
    public override void _Ready()
    {
        // Connect the button's pressed signal to the restart method
        GetNode<Button>("Button").Pressed += OnRestartButtonPressed;
        GetTree().Paused = true;

    }

    private void OnRestartButtonPressed()
    {
        GetTree().Paused = false;
        GetTree().ReloadCurrentScene();
    }
}