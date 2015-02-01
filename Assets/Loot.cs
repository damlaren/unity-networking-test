using UnityEngine;
using System.Collections;

public class Loot : MonoBehaviour {

	public bool isAvailable;

	// Use this for initialization
	void Start () {
		isAvailable = true;
	}

	void Cleanup()
	{
		//Network.RemoveRPCs (networkView.viewID);
		//Network.Destroy (networkView.viewID);
		Destroy (gameObject);
	}

	void OnCollisionEnter (Collision col)
	{
		isAvailable = false;
		Cleanup ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isAvailable) {
			Cleanup ();
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.Serialize(ref isAvailable);
		}
		else
		{
			stream.Serialize(ref isAvailable);
		}
	}
}
