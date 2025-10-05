
using Godot;
using System.Threading.Tasks;

using GdUnit4;
using static GdUnit4.Assertions;
// using GdUnit4.Asserts;

using SpaceBallZ;




[TestSuite]
public class TestCameraControl
{
    private Godot.Collections.Dictionary _player1SpawnData = new Godot.Collections.Dictionary()
    {
        {"id", 1},
        {"defaultCoordinates", Vector3.Zero},
        {"invertDirection", false}
    };
    private Godot.Collections.Dictionary _player2SpawnData = new Godot.Collections.Dictionary()
    {
        {"id", 2},
        {"defaultCoordinates", Vector3.Zero},
        {"invertDirection", false}
    };
    [TestCase]
    [RequireGodotRuntime]
    public async Task TestCameraControlOnFirstPlayerJoined()
    {
        // AssertBool(runner.FindChild("Ball") == null).IsFalse();
        ISceneRunner runner = ISceneRunner.Load("res://test/test_environment.tscn");
        PlayerSpawner spawner = (PlayerSpawner)runner.FindChild("PlayerSpawner");
        Manager manager = (Manager)runner.FindChild("Manager");
        AssertBool(runner.Scene().IsNodeReady()).IsTrue();
        AssertBool(manager.IsNodeReady()).IsTrue();
        spawner.DebugMode = true;
        manager.DebugMode = true;
        await runner.SimulateFrames(1);
        // When: first player joins
        _player1SpawnData["defaultCoordinates"] = ((Node3D)runner.FindChild("Player1Spawn")).Position;
        Player player = (Player)spawner.SpawnFunctionOverride(_player1SpawnData);
        runner.Invoke("AddChild", player, false, (int)Node.InternalMode.Disabled);
        Player controlledPlayer = manager.ControlledPlayer;
        AssertBool(manager.ControlledPlayer == null).IsFalse();
        // Check: if player.camera is current
        Camera3D camera = (Camera3D)player.FindChild("PlayerCamera");
        AssertBool(camera.Current).IsTrue();
    }


}
