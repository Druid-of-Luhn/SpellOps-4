using UnityEngine;
using System.Collections;

public class NetworkingScript : MonoBehaviour {

	private string typeName = "Wizard";
	private HostData[] hostList;
	public GameObject playerPrefab;
	public GameObject SpawnPoint;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void StartServer() {
		Network.InitializeServer (4, 7777, !Network.HavePublicAddress ());
		MasterServer.RegisterHost (typeName, "Room1");
		MasterServer.ipAddress = "127.0.0.1";
	}

	void OnServerInitialized() {
		print ("Server initialized");
		SpawnPlayer ();
	}
	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(50, 50, 100, 50), "Start Server"))
				StartServer();
		}
		if (!Network.isClient && !Network.isServer)
		{
			
			if (GUI.Button(new Rect(50, 125, 100, 50), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(200, 50 + (1 * i), 100, 50), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}

	private void RefreshHostList() {
		MasterServer.RequestHostList (typeName);
	}

	void OnMasterServerEvent(MasterServerEvent msEvent) {
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList ();
	}

	private void JoinServer(HostData hostData) {
		Network.Connect (hostData);
	}
	void OnConnectedToServer() {
		print ("Connected to Server");
		SpawnPlayer();
	}

	private void SpawnPlayer() {
		Network.Instantiate (playerPrefab, SpawnPoint.transform.position, Quaternion.identity, 0);
	}
}
