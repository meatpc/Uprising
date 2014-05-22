using UnityEngine;
using System.Collections.Generic;

public class ObjectController : MonoBehaviour {

	public static GameObject ServerTemplate;
	public static LineRenderer LineTemplate;

	public static List<GameObject> ServerObjects;
	public static List<ServerConnection> ServerConnections;
	public static List<LineRenderer> ServerLines;

	static Color c1;
	static Color c2;

	// Use this for initialization
	void Start () {
		c1 = Color.blue;
		c2 = Color.blue;

		ServerObjects = new List<GameObject>();
		ServerConnections = new List<ServerConnection>();
		ServerLines = new List<LineRenderer>();

		ServerTemplate = Resources.Load<GameObject>("ServerTemplate1");
		LineTemplate = Resources.Load<LineRenderer>("LineRenderer1");
		
		if(ServerTemplate != null)
		{
			Quaternion rot = new Quaternion();
			GameObject server1 = (GameObject)Instantiate(ServerTemplate, new Vector3(0,0,0), rot);
			server1.name = "a";
			ServerObjects.Add(server1);

			GameObject server2 = (GameObject)Instantiate(ServerTemplate, new Vector3(-7,5,0), rot);
			server2.name = "b";
			ServerObjects.Add(server2);

			GameObject server3 = (GameObject)Instantiate(ServerTemplate, new Vector3(5,-6,0), rot);
			server3.name = "c";
			ServerObjects.Add(server3);

			GameObject server4 = (GameObject)Instantiate(ServerTemplate, new Vector3(2.5f,6.35f,0), rot);
			server4.name = "d";
			ServerObjects.Add(server4);

			GameObject server5 = (GameObject)Instantiate(ServerTemplate, new Vector3(2.7f,-11,0), rot);
			server5.name = "e";
			ServerObjects.Add(server5);

			ServerConnections.Add(new ServerConnection(server1, server2));
			ServerConnections.Add(new ServerConnection(server2, server4));
			ServerConnections.Add(new ServerConnection(server2, server5));
			ServerConnections.Add(new ServerConnection(server3, server2));
			ServerConnections.Add(new ServerConnection(server4, server3));
		}
	}
	
	// Update is called once per frame
	void Update () {

		foreach(ServerConnection s in ServerConnections)
		{
			LineRenderer line1 = (LineRenderer)Instantiate(LineTemplate);
			line1.SetVertexCount(2);

			line1.SetPosition(0, s.O1.transform.position);
			line1.SetPosition(1, s.O2.transform.position);

			ServerLines.Add(line1);
		}
	}
}

public class ServerConnection
{
	public GameObject O1;
	public GameObject O2;

	public ServerConnection(GameObject a, GameObject b)
	{
		O1 = a;
		O2 = b;
	}
}