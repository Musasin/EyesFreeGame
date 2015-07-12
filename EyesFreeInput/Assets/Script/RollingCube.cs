using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RollingCube : MonoBehaviour {

	enum Mode { 
		NOTHING,
		TOUCH,
		SWIPE,
		SEPARATE
	};

	Mode nowMode = Mode.NOTHING;

	private GameObject GuiObj;
	Text GuiText;
	[SerializeField] GameObject effect;
	[SerializeField] GameObject startEffect;
	[SerializeField] GameObject niceEffect;

	// Use this for initialization
	void Start () {
		GuiObj = GameObject.Find ("GUIText");
		GuiText = GuiObj.GetComponent<Text> ();
	}

	Vector2 beforePos;
	Vector2 nowPos;
	Vector2 afterPos;
	Vector2 acceleration;
	Vector3 effectPos;

	Vector2 startPoint;

	Vector2 totalMovement;

	float beforeAngle = 0;
	float afterAngle = 0;
	int edgeCount = 0;

	string result = "default";

	int holdFrame = 0;

	int duplicationFrame = 0;

	// Update is called once per frame
	void Update () {

		duplicationFrame--;

		GuiText.text = result + 
					 "\nXa: " + (acceleration.x).ToString() + 
					 "\nYa: " + (acceleration.y).ToString() + 
				   "\n\nX:  " + (afterPos.x).ToString() +
				"\nY:  " + (afterPos.y).ToString() + 
				"\nholdFrame: " + holdFrame +
				"\nedgeCount : " + edgeCount +
				"\nnowMode: " + nowMode +
				"\nstartPoint: " + startPoint + 
				"\ntotalMove: " + totalMovement + 
				"\nbeforeAngle: " + beforeAngle + 
				"\nafterAngle: " + afterAngle;

		if (isTouching ()) {

			beforePos = afterPos;
			nowPos = updateNowPos();
			afterPos = nowPos;
			if(holdFrame == 0)
				beforePos = afterPos;

			if(System.Math.Abs(acceleration.x) + System.Math.Abs(acceleration.y) > 5){
				beforeAngle = afterAngle;

			if(GetAim(beforePos,afterPos) < 0)
				afterAngle = GetAim(beforePos,afterPos) + 360;
			else
				afterAngle = GetAim(beforePos,afterPos);
				

				if(System.Math.Abs(afterAngle - beforeAngle) > 45 && duplicationFrame <= 0)
				{
					edgeCount++;
					duplicationFrame = 10;
				}
			}

			effectPos.x = afterPos.x;
			effectPos.y = afterPos.y;
			effectPos.z = 1;

			if(holdFrame == 0)
				Instantiate (startEffect, Camera.main.ScreenToWorldPoint(effectPos), startEffect.transform.rotation);
			else
			{
				totalMovement += (afterPos - beforePos);
				Instantiate (effect, Camera.main.ScreenToWorldPoint(effectPos), effect.transform.rotation);
			}

		}

		nowMode = modeCheck ();

		if (nowMode == Mode.TOUCH) {
			startPoint = nowPos;
			holdFrame++;
		}
		if (nowMode == Mode.SWIPE)
			holdFrame++;

		if (nowMode == Mode.SEPARATE) {
			
			if(isNearTwoPoint(startPoint, afterPos))
			   createYellowEffect();
		}

		acceleration = afterPos - beforePos;

		transform.Rotate(acceleration.x, 0, acceleration.y);
	}

	bool isTouching(){
		return Input.GetMouseButton (0) || Input.touchCount > 0;
	}

	Vector2 updateNowPos()
	{
		//UpdatePos (Mouse)
		if(Input.GetMouseButton (0))
			return Input.mousePosition;
		
		//UpdatePos (Touch)
		if(Input.touchCount > 0)
			return Input.GetTouch(0).position;

		return new Vector2(0,0);
	}

	void createYellowEffect()
	{
		Vector3 niceEffectPos;
		niceEffectPos = Camera.main.ScreenToWorldPoint (effectPos);
		niceEffectPos.z = 1;
		Instantiate (niceEffect, niceEffectPos, niceEffect.transform.rotation);
	}

	Mode modeCheck()
	{
		if (!isTouching ()) {
			if (holdFrame != 0) {
				if(System.Math.Abs(acceleration.x) + System.Math.Abs(acceleration.x) > 0)
					result = "Flick!";
				else
					result = "Stop!";
				holdFrame = 0;
				edgeCount = 0;
				totalMovement = new Vector2(0,0);
				return Mode.SEPARATE;
			} else {
				return Mode.NOTHING;
			}
		} else {
			if (holdFrame != 0) {
				return Mode.SWIPE;
			} else {
				return Mode.TOUCH;
			}
		}
	}

	bool isNearTwoPoint(Vector2 begin, Vector2 end)
	{
		double diffX = System.Math.Abs(begin.x - end.x);
		double diffY = System.Math.Abs(begin.y - end.y);

		if (diffX + diffY <= 100)
			return true;
		else
			return false;
	}

	// p2からp1への角度を求める
	// @param p1 自分の座標
	// @param p2 相手の座標
	// @return 2点の角度(Degree)
	public float GetAim(Vector2 p1, Vector2 p2) {
		float dx = p2.x - p1.x;
		float dy = p2.y - p1.y;
		float rad = Mathf.Atan2(dy, dx);
		return rad * Mathf.Rad2Deg;
	}
}
