
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

    ISceneRunner runner;

    [BeforeTest]
    public void BeforeEach()
    {
        runner = ISceneRunner.Load("res://test/test_environment.tscn");
    }

    [TestCase]
    [RequireGodotRuntime]
    public async Task TestCameraControlOnFirstPlayerJoined()
    {
		// Setup
        PlayerSpawner spawner = (PlayerSpawner)runner.FindChild("PlayerSpawner");
        Manager manager = (Manager)runner.FindChild("Manager");
        AssertBool(runner.Scene().IsNodeReady()).IsTrue();
        AssertBool(manager.IsNodeReady()).IsTrue();
        spawner.DebugMode = true;
        manager.DebugMode = true;
        await runner.SimulateFrames(1);
		//
        // When: first player joins
        _player1SpawnData["defaultCoordinates"] = ((Node3D)runner.FindChild("Player1Spawn")).Position;
        Player player = (Player)spawner.SpawnFunctionOverride(_player1SpawnData);
        runner.Invoke("AddChild", player, false, (int)Node.InternalMode.Disabled);
        Player controlledPlayer = manager.ControlledPlayer;
        // Check: if player.camera is current
        Camera3D camera = (Camera3D)player.FindChild("PlayerCamera");
        AssertBool(camera.Current).IsTrue();
		// And
		// When: the second player joins
        _player2SpawnData["id"] = -1;
        _player2SpawnData["defaultCoordinates"] = ((Node3D)runner.FindChild("Player2Spawn")).Position;
        Player player2 = (Player)spawner.SpawnFunctionOverride(_player2SpawnData);
        runner.Invoke("AddChild", player2, false, (int)Node.InternalMode.Disabled);
		// Check: camera focus did not change
        AssertBool(camera.Current).IsTrue();
		Camera3D camera2 = (Camera3D) player2.FindChild("PlayerCamera");
        AssertBool(camera2.Current).IsFalse();
	
    }
}
