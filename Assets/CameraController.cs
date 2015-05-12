using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public GameObject player = null;
	public Vector3 playerPos = new Vector3 (0, 0, 0);

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		playerPos = player.gameObject.transform.position;
		transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);
	}
}
