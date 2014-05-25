using UnityEngine;
using System.Collections.Generic;

public class ObjectController : MonoBehaviour {

	public static GameObject ServerTemplate;
	public static LineRenderer LineTemplate;

	public static List<GameSystemAbstract> AllSystemObjects;
	public static List<ServerConnection> ServerConnections;
	public static List<LineRenderer> ServerLines;

	static Color c1;
	static Color c2;

	// Use this for initialization
	void Start () {
		c1 = Color.blue;
		c2 = Color.blue;

		AllSystemObjects = new List<GameSystemAbstract>();
		ServerConnections = new List<ServerConnection>();
		ServerLines = new List<LineRenderer>();

		ServerTemplate = Resources.Load<GameObject>("ServerTemplate1");
		LineTemplate = Resources.Load<LineRenderer>("LineRenderer1");
		
		if(ServerTemplate != null)
		{
			Quaternion rot = new Quaternion();

			Server server1 = new Server((GameObject)Instantiate(ServerTemplate, new Vector3(0,0,0), rot));
			server1.InternalGameObject.name = "server1";
			AllSystemObjects.Add(server1);

			Server server2 = new Server((GameObject)Instantiate(ServerTemplate, new Vector3(-7,5,0), rot));
			server2.InternalGameObject.name = "server2";
			AllSystemObjects.Add(server2);

            Server server3 = new Server((GameObject)Instantiate(ServerTemplate, new Vector3(5,-6,0), rot));
			server3.InternalGameObject.name = "server3";
			AllSystemObjects.Add(server3);

			Server server4 = new Server((GameObject)Instantiate(ServerTemplate, new Vector3(2.5f,6.35f,0), rot));
			server4.InternalGameObject.name = "server4";
			AllSystemObjects.Add(server4);

			Server server5 = new Server((GameObject)Instantiate(ServerTemplate, new Vector3(2.7f,-11,0), rot));
			server5.InternalGameObject.name = "server5";
			AllSystemObjects.Add(server5);

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

			line1.SetPosition(0, s.O1.InternalGameObject.transform.position);
			line1.SetPosition(1, s.O2.InternalGameObject.transform.position);

			ServerLines.Add(line1);
		}
	}
}

public class ServerConnection
{
	public GameSystemAbstract O1;
	public GameSystemAbstract O2;

	public ServerConnection(GameSystemAbstract a, GameSystemAbstract b)
	{
		O1 = a;
		O2 = b;
	}
}

public abstract class GameSystemAbstract
{
	public GameObject InternalGameObject;

	public GameSystemAbstract()
	{
	}

	public GameSystemAbstract(GameObject internalObject) : base()
	{
		InternalGameObject = internalObject;
	}
}

public class Server : GameSystemAbstract
{
	public GameObject ProgressBar;

	public Server(GameObject gameObject) : base(gameObject)
	{
	    DecoratorController.AddProgressBarToObject(this);
	}
}