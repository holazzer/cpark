using Godot;
using System;

namespace CPark {
	public partial class GridBox1 : Node3D {

		private MeshInstance3D meshBox;

		[Export] public int x, y, z;
		[Export] public Material WhiteMaterial;
		[Export] public Material GreenMaterial;

		[Signal] public delegate void BoxMouseEnterSelectedEventHandler(GridBox1 self);

		public override void _Ready() {
			meshBox = GetNode<MeshInstance3D>("Mesh");
			
		}

		private void _on_box_area_mouse_entered() {
			GD.Print("Mouse Enter");
			meshBox.MaterialOverride = GreenMaterial;
			EmitSignal(SignalName.BoxMouseEnterSelected, this);
		}

		private void _on_box_area_mouse_exited() {
			GD.Print("Mouse Exit");
			meshBox.MaterialOverride = WhiteMaterial;
		}

	}


}
