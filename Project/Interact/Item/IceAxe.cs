using Godot;

public partial class IceAxe : BTHItem
{
    public override void Interact(PlayerController player)
    {
        base.Interact(player);
        GD.Print("Picking up Ice Axe");
    }
}