using UnityEngine;
using Chronos;

public class TimeControl : MonoBehaviour 
{	
	// CLOCKS
	Clock playerClock;

	protected bool playerIsSlowed = false;

	void Update () 
	{
		playerClock = Timekeeper.instance.Clock ("Player");

		PlayerSlowDown ();
	}

	// Method to (un)initiate slow down on the Player
	void PlayerSlowDown()
	{
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			if (!playerIsSlowed)
			{
				playerClock.LerpTimeScale(0.5f, 1.0f, false);
				playerIsSlowed = true;
			}
			else
			{
				playerClock.LerpTimeScale(1.0f, 1.0f, false);
				playerIsSlowed = false;
			}
		}
	}
}
