using Godot;

public partial class BTHButton : BTHInteractable
{
	public override void Interact(PlayerController player)
	{
		GD.Print($"Pressing {Name}");
	}
}