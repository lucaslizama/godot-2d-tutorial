using System;
using Godot;

public partial class Main : Node
{
  [Export] public PackedScene MobScene;
  public int Score;

  public override void _Ready()
  {
  }

  public void GameOver()
  {
    GetNode<Timer>("MobTimer").Stop();
    GetNode<Timer>("ScoreTimer").Stop();
    GetNode<HUD>("HUD").ShowGameOver();
    GetNode<AudioStreamPlayer2D>("DeathSound").Play();
    GetNode<AudioStreamPlayer2D>("Music").Stop();
  }

  public void NewGame()
  {
    Score = 0;

    var player = GetNode<Player>("Player");
    var startPosition = GetNode<Marker2D>("StartPosition");
    player.Start(startPosition.Position);

    GetNode<Timer>("StartTimer").Start();

    var hud = GetNode<HUD>("HUD");
    hud.UpdateScore(Score);
    hud.ShowMessage("Get Ready!");

    GetTree().CallGroup("mobs", "queue_free");

    GetNode<AudioStreamPlayer2D>("Music").Play();
  }

  public void OnScoreTimerTimeout()
  {
    Score++;
    GetNode<HUD>("HUD").UpdateScore(Score);
  }

  public void OnStartTimerTimeout()
  {
    GetNode<Timer>("MobTimer").Start();
    GetNode<Timer>("ScoreTimer").Start();
  }

  public void OnMobTimerTimeout()
  {
    var mob = MobScene.Instantiate<Mob>();

    var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
    mobSpawnLocation.ProgressRatio = GD.Randf();

    float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

    mob.Position = mobSpawnLocation.Position;

    direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
    mob.Rotation = direction;

    var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
    mob.LinearVelocity = velocity.Rotated(direction);

    AddChild(mob);
  }
}
