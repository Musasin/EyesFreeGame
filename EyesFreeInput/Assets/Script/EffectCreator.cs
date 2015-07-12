using UnityEngine;
using System.Collections;

public class EffectCreator : MonoBehaviour {
	
	[SerializeField] GameObject trackEffect;
	[SerializeField] GameObject startEffect;
	[SerializeField] GameObject connectEffect;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateTrackEffect(Vector3 screenPoint){
		Vector3 effectPos;
		effectPos = screenPoint;
		effectPos.z = 1;
		Instantiate (trackEffect, Camera.main.ScreenToWorldPoint (effectPos), trackEffect.transform.rotation);
	}
	
	public void CreateStartEffect(Vector3 screenPoint){
		Vector3 effectPos;
		effectPos = screenPoint;
		effectPos.z = 1;
		Instantiate (startEffect, Camera.main.ScreenToWorldPoint (effectPos), startEffect.transform.rotation);
	}
	
	public void CreateConnectEffect(Vector2 pos){
		Vector3 connectEffectPos;
		connectEffectPos = Camera.main.ScreenToWorldPoint (pos);
		connectEffectPos.z = 1;
		Instantiate (connectEffect, connectEffectPos, connectEffect.transform.rotation);
	}
}
