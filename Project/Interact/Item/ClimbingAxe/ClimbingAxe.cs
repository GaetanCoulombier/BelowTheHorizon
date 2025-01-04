using Godot;

public partial class IceAxe : BTHItem
{
    public override void UsePrimary(PlayerController player)
    {
        GD.Print("Using Ice Axe primary");
    }

    public override void UseSecondary(PlayerController player)
    {
        GD.Print("Using Ice Axe secondary");
    }
}