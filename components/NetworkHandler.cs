using Godot;

public partial class NetworkHandler: Node
{

	public enum NetworkSate {UNDEFINED = -1, HOST = 0, SERVER = 1, CLIENT = 2};

	public static NetworkHandler Instance {get;  private set;}


	public NetworkSate CurrentNetworkSate;

	protected static string IpAdress = "127.0.0.1";

	protected static int Port = 53135;

	public ENetMultiplayerPeer _peer;


    public override void _Ready()
    {
		Instance = this;
		CurrentNetworkSate = NetworkSate.CLIENT;
    }

	public static bool IsServer() { return Instance.CurrentNetworkSate == NetworkSate.SERVER; }

	public static bool IsHost() { return Instance.CurrentNetworkSate == NetworkSate.HOST; }

	public static void CreateServer() { Instance.createServer(); }

	public static void CreateClient() { Instance.createClient(); }

	public static void CreateHost() { Instance.createHost(); }
	
	private void clearState() {
		CurrentNetworkSate = NetworkSate.UNDEFINED;
		_peer = null;
		_peer = new ENetMultiplayerPeer();
	}

	private void createServer()
	{
		clearState();
		_peer.CreateServer(Port);
		Multiplayer.MultiplayerPeer = _peer;
		CurrentNetworkSate = NetworkSate.SERVER;
	}

	private void createClient()
	{
		clearState();
		_peer.CreateClient(IpAdress, Port);
		Multiplayer.MultiplayerPeer = _peer;
		CurrentNetworkSate = NetworkSate.CLIENT;
	}

	private void createHost()
	{
		clearState();
		_peer.CreateServer(Port);
		Multiplayer.MultiplayerPeer = _peer;
		CurrentNetworkSate = NetworkSate.HOST;
		EmitSignal(MultiplayerApi.SignalName.PeerConnected, 1);
	}

}
