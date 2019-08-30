using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueEyesManager : MonoBehaviour
{
    public bool isTracked;
	public bool justSummoned;

	public float summonedTimer = 1f;

    private void Update()
	{
        if(isTracked)
		{
			justSummoned = true;
            Debug.Log("You Summoned Blue-Eyes White Dragon");
			Invoke("SummonedTimer", summonedTimer);
		}
	} 

    public void Tracked()
    {
        isTracked = true;
    }

    void SummonedTimer()
	{
		justSummoned = false;
	}

    public void NotTracked()
    {
        isTracked = false;
    }
}
