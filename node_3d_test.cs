using Godot;
using System;

namespace CPark {
	public partial class node_3d_test : Node3D {

		[Export] public PackedScene GridBox;
		

		public override void _Ready() {
			for(int i = 0; i < 3; i++) {
				for(int j = 0; j < 3; j++) {
					for( int k = 0; k < 3; k++) {
						var gbox = GridBox.Instantiate<GridBox1>();
						gbox.x = i; gbox.y = j; gbox.z = k;
						gbox.Position = new Vector3(i, j , k);
						AddChild(gbox);
					}
				}
			}
		}

		public override void _Process(double delta) {

		}
	}
}
