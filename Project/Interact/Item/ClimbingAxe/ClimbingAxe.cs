using Godot;
using BelowTheHorizon;

public partial class ClimbingAxe : Item
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