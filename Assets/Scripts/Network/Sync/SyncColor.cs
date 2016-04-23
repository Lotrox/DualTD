using System;
using UnityEngine;
using UnityEngine.Networking;


class SyncColor : NetworkBehaviour
{
	[SyncVar]
	public Color myColor;

	public override void OnStartClient()
	{
		GetComponent<Renderer>().material.color = myColor;
	}
}