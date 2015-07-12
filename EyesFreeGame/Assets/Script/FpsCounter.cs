using UnityEngine;
using System.Collections;

public class FpsCounter : MonoBehaviour {


	int frameCount = 0;
	int fps = -1;
	float nextTime;
	
	//FileManager fileManager;
	
	// Use this for initialization
	void Start () {
		nextTime = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		frameCount++;
		if (Time.time >= nextTime) 
		{
			Debug.Log ("FPS : " + frameCount);
			fps = frameCount;
			frameCount = 0;
			nextTime += 1;
		}
	}
	
	public int getFps()
	{
		return fps;
	}
}
