using Godot;
using System;
using System.Collections.Generic;

namespace CPark {
	public partial class Gusher : Control {
		private Label label;

		private string[] lines;
		private int currentLineIndex = 0;

		[Signal] public delegate void SectionEndEventHandler(); // The current section is done.
		[Export] public string TextPath { get; set; }
		
		[Export] public int TextPlaySpeed { get; set; } = 5; // number of characters to print every second
		private int textAnimFPS = 10; // Text update frequency
		private bool IsTextPlaying = false;
		private Timer timer;
		private double timerWaitTime;
		private int textProgress;
		private bool TextAnimation = false;

		private bool TextAnimationAuto = false;
		private Timer oneShotTimer;

		public override void _Ready() {
			label = GetNode<Label>("Label");
			timer = GetNode<Timer>("Timer");
			oneShotTimer = GetNode<Timer>("OneShotTimer");
			
			timerWaitTime = 1.0 / textAnimFPS;

			var txtFile = FileAccess.Open(TextPath, FileAccess.ModeFlags.Read);
			var txt = txtFile.GetAsText().StripEdges();
			lines = txt.Split('\n');
		}

		private void TextAnimateSetup() {
			timer.WaitTime = timerWaitTime;
			GD.Print(timer.WaitTime);
			textProgress = 0;
			IsTextPlaying = true;
			timer.Start();
		}

		private void TextAnimationUpdate() {
			textProgress += TextPlaySpeed;
			string line = GetLine();
			if(textProgress >= line.Length) EndCurrentTextAnimation();
			else label.Text = line[..textProgress];
		}
		
		private string GetLine() {
			return $"[{currentLineIndex+1}/{lines.Length}] {lines[currentLineIndex]}";
		}

		private void ShowNextLine() {
			if (currentLineIndex < lines.Length) {
				if (!TextAnimation) {
					label.Text = GetLine();
					currentLineIndex++;
				} else {
					TextAnimateSetup();
				}

			} else {	
				EmitSignal(SignalName.SectionEnd);
				currentLineIndex = 0;
				label.Text = "This Section is Over. \nA signal has been emitted.\nYou are not supposed to see this line.";
			}
		}

		private void EndCurrentTextAnimation() {
			IsTextPlaying = false;
			timer.Stop();
			label.Text = GetLine();
			currentLineIndex++;
			if (TextAnimationAuto && currentLineIndex != lines.Length) { ScheduleNextLine(); }
		}

		private void ScheduleNextLine() {
			oneShotTimer.WaitTime = timerWaitTime * 2;
			oneShotTimer.Start();
		}

		private void _on_full_rect_gui_input(InputEvent @event) {
			// this is called when FullRect receives input event
			if (@event is InputEventMouseButton mbevent && mbevent.ButtonIndex == MouseButton.Left && mbevent.Pressed) {
				if (TextAnimation && IsTextPlaying) { EndCurrentTextAnimation(); } 
				else { ShowNextLine(); }
			}
		}

		private void _on_timer_timeout() {
			TextAnimationUpdate();
		}

		private void _on_check_box_toggled(bool toggled) {
			TextAnimation = toggled;
			if (TextAnimation == false && IsTextPlaying == true) {
				EndCurrentTextAnimation();
			}
		}

		private void _on_check_box_auto_toggled(bool toggled) {
			TextAnimationAuto = toggled;
			if (TextAnimation && !IsTextPlaying) ShowNextLine();
		}

		private void _on_one_shot_timer_timeout() {
			if(TextAnimation && !IsTextPlaying) ShowNextLine();
		}

	}
}


