using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TouchSystem : MonoBehaviour {

	Text GuiText;
	EffectCreator effectCreator;
	FileManager fileManager;
	FpsCounter fpsCounter;

	Vector3 cursorPos;
	Vector3 acceleration;
	Vector3 firstPos;
	Vector3 lastPos;
	
	List<Vector3> tracksList = new List<Vector3>();
	List<Direction> directionList = new List<Direction>();

	int holdFrame = 0;
	int waitFrame = 0;
	int rightAngleCount = 0;
	int quarterAngleCount = 0;
	float radian;

	enum Direction{
		CENTER = -1,
		EAST,
		NORTHEAST,
		NORTH,
		NORTHWEST,
		WEST,
		SOUTHWEST,
		SOUTH,
		SOUTHEAST
	};
	Direction dir = Direction.CENTER;
	
	enum Mode { 
		NOTHING,
		TOUCH,
		SWIPE,
		SEPARATE
	};
	Mode nowMode = Mode.NOTHING;


	// Use this for initialization
	void Start () {
		GuiText = GameObject.Find ("GUIText").GetComponent<Text> ();
		effectCreator = gameObject.GetComponent<EffectCreator> ();
		fileManager = gameObject.GetComponent<FileManager> ();
		fpsCounter = gameObject.GetComponent<FpsCounter>();
	}


	// Update is called once per frame
	void FixedUpdate () {
		nowMode = modeCheck ();

		switch (nowMode) {

		case Mode.NOTHING:
			waitFrame++;

			break;

		case Mode.TOUCH:

		case Mode.SWIPE:

			holdFrame++;
			cursorPos = updateNowPos ();
			
			effectCreator.CreateTrackEffect (cursorPos);
			tracksList.Add (cursorPos);

			if(nowMode == Mode.TOUCH)
			{
				fileManager.stringOutput(nowMode.ToString () + "   waitFrame = " + waitFrame.ToString());
				waitFrame = 0;

				firstPos = cursorPos;
				effectCreator.CreateStartEffect(firstPos);
			} else
				acceleration = tracksList[tracksList.Count - 1] - tracksList[tracksList.Count - 2];

			
			if(isMoving (acceleration))
			{
				radian = Mathf.Atan2 ( acceleration.y , acceleration.x ) * Mathf.Rad2Deg;
				radian = (radian < 0) ? radian + 360 : radian;
				dir = directionCheck (radian);
				directionList.Add(dir);
				
				if(howChangedAngle(directionList) == 2)
					rightAngleCount++;
				if(howChangedAngle(directionList) == 1)
					quarterAngleCount++;
			}
			
			fileManager.dataOutput(cursorPos, acceleration, dir.ToString(), radian, nowMode.ToString());

			break;

		case Mode.SEPARATE:

			fileManager.stringOutput(nowMode.ToString () + "   holdFrame = " + holdFrame.ToString());

			lastPos = cursorPos;
			if(isNearTwoPoint(firstPos,lastPos))
			   effectCreator.CreateConnectEffect(lastPos);
			holdFrame = 0;
			rightAngleCount = 0;
			quarterAngleCount = 0;
			tracksList.Clear ();
			directionList.Clear ();
			acceleration = new Vector3(0,0,0);


			break;

		}

		GuiText.text = 
			"fps: " + fpsCounter.getFps () + 
			"\ncursorPos: " + cursorPos + 
			"\nacceleration: " + acceleration + 
			"\ndirection: " + dir +
//				"\nChangeAngle: " + howChangedAngle(directionList) +
//				"\nrightAngleCount: " + rightAngleCount + 
//				"\nquartetAngleCount: " + quarterAngleCount +
			"\nradian: " + radian;
//				"\n datapath: \n" + Application.dataPath;
	}

	
	Mode modeCheck()
	{
		if (isTouching ()) {
			if (holdFrame == 0)
				return Mode.TOUCH;
			else
				return Mode.SWIPE;
		} else {
			if (holdFrame == 0)
				return Mode.NOTHING;
			else
				return Mode.SEPARATE;
		}
	}

	Direction directionCheck(float radian)
	{
		
		//方向判定
		if (radian <= 22.5f || radian > 337.5f)
			return Direction.EAST;
		else if(radian <= 67.5f && radian > 22.5f)
			return Direction.NORTHEAST;
		else if(radian <= 112.5f && radian > 67.5f)
			return Direction.NORTH;
		else if(radian <= 157.5f && radian > 112.5f)
			return Direction.NORTHWEST;
		else if(radian <= 202.5f && radian > 157.5f)
			return Direction.WEST;
		else if(radian <= 247.5f && radian > 202.5f)
			return Direction.SOUTHWEST;
		else if(radian <= 292.5f && radian > 247.5f)
			return Direction.SOUTH;
		else if(radian <= 337.5f && radian > 292.5f)
			return Direction.SOUTHEAST;
		else 
			return Direction.CENTER;
	}

	Vector3 updateNowPos()
	{
		//UpdatePos (Mouse)
		if(Input.GetMouseButton (0))
			return Input.mousePosition;
		
		//UpdatePos (Touch)
		if(Input.touchCount > 0)
			return Input.GetTouch(0).position;
		
		return new Vector3(0,0,0);
	}
	
	int howChangedAngle(List<Direction> dirList)
	{
		if (dirList.Count < 2)
			return -1;
		else
			return (System.Math.Abs (dirList[dirList.Count - 1] - dirList[dirList.Count - 2]) % 7);
	}

	bool isTouching(){
		return Input.GetMouseButton (0) || Input.touchCount > 0;
	}

	bool isMoving(Vector3 acceleration)
	{
		return System.Math.Abs (acceleration.x + acceleration.y) > 1;
	}
	
	bool isNearTwoPoint(Vector3 begin, Vector3 end)
	{
		double diffX = System.Math.Abs(begin.x - end.x);
		double diffY = System.Math.Abs(begin.y - end.y);
		
		if (diffX + diffY <= 100)
			return true;
		else
			return false;
	}

}
