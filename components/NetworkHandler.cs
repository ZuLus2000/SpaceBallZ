using Godot;

public partial class NetworkHandler: Node
{

	public static NetworkHandler Instance {get;  private set;}

	protected static string IpAdress = "127.0.0.1";

	protected static int Port = 53135;

	public ENetMultiplayerPeer _peer;


    public override void _Ready()
    {
		Instance = this;
    }

	private void createServer()
	{
		_peer = new ENetMultiplayerPeer();
		_peer.CreateServer(Port);
		Multiplayer.MultiplayerPeer = _peer;
	}

	public static void CreateServer()
	{
		Instance.createServer();
	}

	public static void CreateClient()
	{
		Instance.createClient();
	}

	private void createClient()
	{
		_peer = new ENetMultiplayerPeer();
		_peer.CreateClient(IpAdress, Port);
		Multiplayer.MultiplayerPeer = _peer;
	}

}
