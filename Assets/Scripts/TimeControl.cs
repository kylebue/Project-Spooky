using UnityEngine;
using Chronos;

public class TimeControl : MonoBehaviour 
{	
	// Update is called once per frame
	void Update () 
	{
		Clock playerClock = Timekeeper.instance.Clock ("Player");

		if (Input.GetKey(KeyCode.LeftControl))
		{
			playerClock.localTimeScale = 0.5f;
		}
		else
		{
			playerClock.localTimeScale = 1.0f;
		}
	}
}
