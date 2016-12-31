using UnityEngine;
using System.Collections;

public class CameraAspactRation : MonoBehaviour {
	public int canvasWidth = 640;
	public int canvasHeight = 1136;
	// Use this for initialization
	void Start () {
		Debug.Log (string.Format("Current screen size:{0}x{1}",Screen.width,Screen.height));
		Camera.main.aspect = (float)canvasWidth / canvasHeight;
		Debug.Log ("Main camera aspact ration:" + Camera.main.aspect);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
