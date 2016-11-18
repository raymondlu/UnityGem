using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingBar : MonoBehaviour {
	public Image barImage;
	public int percent = 50;

	void InitReferences(){
		Transform imageBarTransform = transform.Find ("ImageProgressBar");
		if (imageBarTransform) {
			barImage = imageBarTransform.gameObject.GetComponent<Image> ();
		}

		if (!barImage) {
			Debug.LogError ("Failed to locate image bar component");	
		}
	}

	void UpdateProgress(){
		float currentAmount = percent / 100f;
		barImage.fillAmount = currentAmount;
	}

	public void SetPercent(int newPercent) {
		percent = Mathf.Clamp (newPercent, 0, 100);
		UpdateProgress ();
	}

	void Reset() {
		InitReferences ();
		SetPercent (percent);
	}

	// Use this for initialization
	void Start () {
		InitReferences ();
		UpdateProgress ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
