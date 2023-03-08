using Godot;

public partial class Mob : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animSprite2D.Play();
		var mobTypes = animSprite2D.SpriteFrames.GetAnimationNames();
		animSprite2D.Animation = mobTypes[GD.Randi() % mobTypes.Length];
	}

	public void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}
