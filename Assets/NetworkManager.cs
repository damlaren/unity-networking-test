using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private const string typeName = "UniqueGameName";
	private const string gameName = "RoomName";

	// List of hosts retrieved from master server.
	private HostData[] hostList;

	// All clients within game will see this object
	public GameObject playerPrefab;

	// Not a callback-- start server on command only
	private void StartServer()
	{
		Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}

	// Interface for connecting to a server
	void OnGUI()
	{
		// TODO: what if master server isn't local server?
		
		// GUI only shows if not already a server or a client
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
			{
				RefreshHostList();
				print (hostList);
			}
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}

	// Called when server is initialized
	void OnServerInitialized()
	{
		SpawnPlayer ();
	}

	// Get host list from master server
	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}

	// Once host list is received, OnMasterServerEvent is triggered.
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	// Join server specified by host data
	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}

	private void SpawnPlayer()
	{
		// Instantiate a prefab on all clients, with a specific position and orientation.
		Network.Instantiate(playerPrefab, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
	}

	// Invoked once connected to server
	void OnConnectedToServer()
	{
		SpawnPlayer ();
	}

	// Use this for initialization
	void Start () {
		// Set local machine as master server.
		// Default master server is run by Unity and could be down.
		// TODO: it is possible to build and run the Unity master server on your own machine.
		// It is desirable to run it locally in case Unity's master goes down.
		// 
		//MasterServer.ipAddress = "127.0.0.1";
	}
}
