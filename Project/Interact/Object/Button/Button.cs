using Godot;

namespace BelowTheHorizon
{
	public partial class Button : Object
	{
		public override void Interact(PlayerController player)
		{
			GD.Print($"Pressing {Name}");
		}

		public override bool CanInteract(PlayerController player)
		{
			return true;
		}
	}
}
