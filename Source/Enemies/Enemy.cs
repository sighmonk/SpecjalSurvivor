using Godot;

// Wspólna baza wszystkich przeciwników.
// Nowy wróg wystarczy, że dziedziczy po tej klasie (bezpośrednio albo przez Neptun)
// - dostaje wtedy obrażenia kontaktowe i jest liczony do wyniku po śmierci.
public partial class Enemy : Area2D
{
    [Export] public float ContactDamage = 10f;

    public override void _Ready()
    {
        AddToGroup("enemies");
    }

    public virtual void OnAreaEntered(Area2D area)
    {
        if (area is HitboxComponent hitbox)
        {
            hitbox.Damage(ContactDamage);
        }
    }
}
