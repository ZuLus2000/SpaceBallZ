using Godot;

partial class NetworkButtons : Node
{
	private void OnServerBtnPressed() { NetworkHandler.CreateServer(); }
	private void OnClientBtnPressed() { NetworkHandler.CreateClient(); }

}
