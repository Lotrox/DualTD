#if ENABLE_UNET

namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkManUI")]
	[RequireComponent(typeof(NetworkManager))]
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public class NetworkManUI : MonoBehaviour
	{
		private NetworkManager manager;
		[SerializeField] public bool showGUI = true;
		[SerializeField] public int offsetX;
		[SerializeField] public int offsetY;

		// Runtime variable
		bool showServer = false;
		bool creditos = false;

		void Awake()
		{
			manager = GetComponent<NetworkManager>();
		}

		void Update()
		{
			if (!showGUI)
				return;

			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{
				if (Input.GetKeyDown(KeyCode.S))
				{
					manager.StartServer();
				}
				if (Input.GetKeyDown(KeyCode.H))
				{
					manager.StartHost();
				}
				if (Input.GetKeyDown(KeyCode.C))
				{
					manager.StartClient();
				}
			}
			if (NetworkServer.active && NetworkClient.active)
			{
				if (Input.GetKeyDown(KeyCode.X))
				{
					manager.StopHost();
				}
			}
		}

		void OnGUI()
		{
			if (!showGUI)
				return;

			int xpos = 10 + offsetX;
			int ypos = 40 + offsetY;
			int spacing = 80;

			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
			{

				if (GUI.Button(new Rect(Screen.width - 300, Screen.height - 100, 200, 40), "CREDITOS"))
				{
					creditos = !creditos;
				}
				if (creditos) {
					GUIStyle myStyle = new GUIStyle(GUI.skin.label);
					myStyle.fontSize = 18;
					//myStyle.normal.textColor = Color.red;
					GUI.Label(new Rect(Screen.width - 400, 50, 500,400), "<b><size=25>Desarrollo de Videojuegos</size> \n <i>Curso:2015/2016</i></b>\n\n\n <size=20><b>Autores:</b></size> \n\nJosé Ángel Pastrana Padilla. \nDaniel Martínez Caballero. \n\n\n\n <size=20><b>Arte 2D:</b></size>\n\n Francisco Manuel Molina.", myStyle);
				}
				if (GUI.Button(new Rect(xpos, ypos, 200, 40), "CREAR PARTIDA"))
				{
					manager.StartHost();
				}
				ypos += spacing;

				if (GUI.Button(new Rect(xpos, ypos, 200, 40), "UNIRSE A PARTIDA"))
				{
					manager.StartClient();
				}
				manager.networkAddress = GUI.TextField(new Rect(xpos, ypos - 20, 200, 20), manager.networkAddress);
				ypos += spacing;

				if (GUI.Button(new Rect(xpos, ypos - 20, 200, 40), "CREAR SERVIDOR"))
				{
					manager.StartServer();
				}
				ypos += spacing;
			}
			else
			{
				xpos -= 40;
				if (NetworkServer.active)
				{
					ypos -= 30;
					GUI.Label(new Rect(xpos, ypos, 300, 20), "Server Port: " + manager.networkPort);
					ypos += 20;
				}
				if (NetworkClient.active)
				{
					GUI.Label(new Rect(xpos, ypos, 300, 20), "Cliente: " + manager.networkAddress);
					ypos += spacing;
				}
			}

			if (NetworkClient.active && !ClientScene.ready)
			{
				if (GUI.Button(new Rect(xpos, ypos - 50, 200, 20), "Esperando al servidor..."))
				{
					ClientScene.Ready(manager.client.connection);

					if (ClientScene.localPlayers.Count == 0)
					{
						ClientScene.AddPlayer(0);
					}
				}
				ypos += 20;
			}

			if (NetworkServer.active || NetworkClient.active)
			{
				if (GUI.Button(new Rect(xpos, ypos - 50, 100, 20), "Salir"))
				{
					manager.StopHost();
				}
				ypos += spacing;
			}

//			if (!NetworkServer.active && !NetworkClient.active)
//			{
//				ypos += 10;
//
//				if (manager.matchMaker == null)
//				{
//					if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Enable Match Maker (M)"))
//					{
//						manager.StartMatchMaker();
//					}
//					ypos += spacing;
//				}
//				else
//				{
//					if (manager.matchInfo == null)
//					{
//						if (manager.matches == null)
//						{
//							if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Create Internet Match"))
//							{
//								manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
//							}
//							ypos += spacing;
//
//							GUI.Label(new Rect(xpos, ypos, 100, 20), "Room Name:");
//							manager.matchName = GUI.TextField(new Rect(xpos+100, ypos, 100, 20), manager.matchName);
//							ypos += spacing;
//
//							ypos += 10;
//
//							if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Find Internet Match"))
//							{
//								manager.matchMaker.ListMatches(0,20, "", manager.OnMatchList);
//							}
//							ypos += spacing;
//						}
//						else
//						{
//							foreach (var match in manager.matches)
//							{
//								if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join Match:" + match.name))
//								{
//									manager.matchName = match.name;
//									manager.matchSize = (uint)match.currentSize;
//									manager.matchMaker.JoinMatch(match.networkId, "", manager.OnMatchJoined);
//								}
//								ypos += spacing;
//							}
//						}
//					}
//
//					if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Change MM server"))
//					{
//						showServer = !showServer;
//					}
//					if (showServer)
//					{
//						ypos += spacing;
//						if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Local"))
//						{
//							manager.SetMatchHost("localhost", 1337, false);
//							showServer = false;
//						}
//						ypos += spacing;
//						if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Internet"))
//						{
//							manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
//							showServer = false;
//						}
//						ypos += spacing;
//						if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Staging"))
//						{
//							manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
//							showServer = false;
//						}
//					}
//
//					ypos += spacing;
//
//					GUI.Label(new Rect(xpos, ypos, 300, 20), "MM Uri: " + manager.matchMaker.baseUri);
//					ypos += spacing;
//
//					if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Disable Match Maker"))
//					{
//						manager.StopMatchMaker();
//					}
//					ypos += spacing;
//				}
//			}
		}
	}
};
#endif //ENABLE_UNET
