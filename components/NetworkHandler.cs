using Godot;

public partial class NetworkHandler : Node
{

    public enum NetworkSate { UNDEFINED = -1, HOST = 0, SERVER = 1, CLIENT = 2 };

    public static NetworkHandler Instance { get; private set; }


    public NetworkSate CurrentNetworkSate;

    protected static string IpAdress = "127.0.0.1";

    protected static int Port = 53135;

    public ENetMultiplayerPeer _peer;

    [Signal]
    public delegate void PeerConnectedEventHandler(long id);

	[Signal]
	public delegate void StateChangedEventHandler(int newState);

    public override void _Ready()
    {
        Instance = this;
        CurrentNetworkSate = NetworkSate.CLIENT;
        Multiplayer.PeerConnected += (id) => EmitSignal(NetworkHandler.SignalName.PeerConnected, id);
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
		EmitSignal(NetworkHandler.SignalName.StateChanged, (int) CurrentNetworkSate);
	}

	private void createServer()
	{
		clearState();
		_peer.CreateServer(Port);
		Multiplayer.MultiplayerPeer = _peer;
		CurrentNetworkSate = NetworkSate.SERVER;
		EmitSignal(NetworkHandler.SignalName.StateChanged, (int) CurrentNetworkSate);
	}

	private void createClient()
	{
		clearState();
		_peer.CreateClient(IpAdress, Port);
		Multiplayer.MultiplayerPeer = _peer;
		CurrentNetworkSate = NetworkSate.CLIENT;
		EmitSignal(NetworkHandler.SignalName.StateChanged, (int) CurrentNetworkSate);
	}

	private void createHost()
	{
		clearState();
		_peer.CreateServer(Port);
		Multiplayer.MultiplayerPeer = _peer;
		CurrentNetworkSate = NetworkSate.HOST;
		EmitSignal(NetworkHandler.SignalName.StateChanged, (int) CurrentNetworkSate);
		EmitSignal(NetworkHandler.SignalName.PeerConnected, 1);
	}

}
