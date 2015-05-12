using UnityEngine;
using System.Collections;

public class ChestScript : MonoBehaviour 
{
	public GameObject player;
	public bool isOpened = false;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey(KeyCode.E) && Vector3.Distance(player.transform.position, transform.position) <= 1.5f && !isOpened)
		{
			Debug.Log("Opened Chest!");
			isOpened = true;
		}
	}
}
