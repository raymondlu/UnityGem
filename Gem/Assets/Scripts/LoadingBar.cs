using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingBar : MonoBehaviour {
	public Image barImage;
	public int percent = 0;

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

		EventManager.SafeAddDelegate<GameEventStartLoading> (OnGameEvent);
		EventManager.SafeAddDelegate<GameEventUpdateLoading> (OnGameEvent);
		EventManager.SafeAddDelegate<GameEventFinishLoading> (OnGameEvent);
	}

	// Use this for clean-up
	void OnDisable() {
		EventManager.SafeRemoveDelegate<GameEventStartLoading> (OnGameEvent);
		EventManager.SafeRemoveDelegate<GameEventUpdateLoading> (OnGameEvent);
		EventManager.SafeRemoveDelegate<GameEventFinishLoading> (OnGameEvent);
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnGameEvent(GameEvent eventInstance){
		System.Type eventType = eventInstance.GetType ();
		if (eventType == typeof(GameEventStartLoading)) {
			for (int i = 1; i <= 100; ++i) {
				GameEventUpdateLoading e = new GameEventUpdateLoading ();
				e.percent = i;
				EventManager.SafeQueueEvent (e);
			}
		} else if (eventType == typeof(GameEventUpdateLoading)) {
			GameEventUpdateLoading currentEvent = (GameEventUpdateLoading)eventInstance;
			Debug.Log ("GameEventUpdateLoading:" + currentEvent.percent);
			if (currentEvent.percent == 100) {
				GameEventFinishLoading e = new GameEventFinishLoading ();
				EventManager.SafeQueueEvent (e);
			}
			SetPercent (currentEvent.percent);
		} else if (eventType == typeof(GameEventFinishLoading)) {
			EventManager.SafeQueueEvent(new GameEventGoToMainMenuScene());
		}
	}
}
