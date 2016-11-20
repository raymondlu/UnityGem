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

		EventManager.instance.AddDelegate<GameEventStartLoading> (OnGameEvent);
		EventManager.instance.AddDelegate<GameEventUpdateLoading> (OnGameEvent);
		EventManager.instance.AddDelegate<GameEventFinishLoading> (OnGameEvent);
	}

	// Use this for clean-up
	void OnDisable() {
		EventManager.instance.RemoveDelegate<GameEventStartLoading> (OnGameEvent);
		EventManager.instance.RemoveDelegate<GameEventUpdateLoading> (OnGameEvent);
		EventManager.instance.RemoveDelegate<GameEventFinishLoading> (OnGameEvent);
	}

	// Update is called once per frame
	void Update () {
	
	}

	bool OnGameEvent(GameEvent eventInstance){
		System.Type eventType = eventInstance.GetType ();
		if (eventType == typeof(GameEventStartLoading)) {
			GameEventUpdateLoading e = new GameEventUpdateLoading ();
			e.percent = 1;
			EventManager.instance.QueueEvent (e);
		} else if (eventType == typeof(GameEventUpdateLoading)) {
			GameEventUpdateLoading currentEvent = (GameEventUpdateLoading)eventInstance;
			if (currentEvent.percent > 100) {
				GameEventFinishLoading e = new GameEventFinishLoading ();
				EventManager.instance.QueueEvent (e);
			} else {
				GameEventUpdateLoading e = new GameEventUpdateLoading ();
				e.percent = currentEvent.percent + 1;
				EventManager.instance.QueueEvent (e);
				SetPercent (currentEvent.percent);
			}
		} else if (eventType == typeof(GameEventFinishLoading)) {
			Destroy (gameObject);
		}

		return false;
	}
}
