using UnityEngine;
using System.Collections;

public class LineCreator : MonoBehaviour {

	LineRenderer lineRenderer;

	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = true;
	}

	int count = 1;
	// Update is called once per frame
	void Update () {
//		lineRenderer.SetVertexCount(count);
//		Vector3 position = Input.mousePosition;
//		position.z = 1;
//		lineRenderer.SetPosition(count-1, Camera.main.ScreenToWorldPoint (position));
//		count++;
	}

	public void AddPoint(Vector3 point)
	{
		count++;

		lineRenderer.SetVertexCount(count);
		Vector3 position = point;
		position.z = 1;
		lineRenderer.SetPosition(count-1, Camera.main.ScreenToWorldPoint (position));
	}

	public void ResetPoint()
	{
		count = 0;
	}
}
