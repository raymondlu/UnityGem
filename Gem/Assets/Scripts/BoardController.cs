using UnityEngine;
using System.Collections;

public class BoardController : MonoBehaviour {
	public GameObject gemPrefab;

	private RectTransform thisTransform;

	// Use this for initialization
	void Start () {
		thisTransform = (RectTransform)transform;

		for( int i = 0; i < 64; ++i){
			GameObject obj =  Instantiate (gemPrefab);
			obj.transform.SetParent (thisTransform, false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
