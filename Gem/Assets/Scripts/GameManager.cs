﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{
	private EventManager _eventManager;

	private static GameManager _gameManager;
	public static GameManager instance{
		get{
			if(!_gameManager){
				_gameManager = FindObjectOfType (typeof(GameManager)) as GameManager;
				if(!_gameManager){
					Debug.LogError ("Game manager is missing.");
				} else {
					_gameManager.Init ();
				}
			}
			return _gameManager;
		}
	}

	void Init(){
		Debug.Log ("Game manager is initialized.");
		_eventManager = EventManager.instance;
	}

	void Awake(){
		if (_gameManager == null) {
			_gameManager = GameManager.instance;
		}
		DontDestroyOnLoad (transform.gameObject);
	}

	void OnEnable(){
		EventManager.SafeAddDelegate<GameEventGoToMainMenuScene> (OnGameEvent);
	}

	void OnDisable(){
		EventManager.SafeRemoveDelegate<GameEventGoToMainMenuScene> (OnGameEvent);
	}

	// Use this for initialization
	void Start (){
		_eventManager.QueueEvent (new GameEventStartLoading());
	}
	
	// Update is called once per frame
	void Update (){
		if (_eventManager != null) {
			_eventManager.Update ();
		}
	}

	void OnGameEvent(GameEvent eventInstance){
		System.Type eventType = eventInstance.GetType ();
		if (eventType == typeof(GameEventGoToMainMenuScene)) {
			SceneManager.LoadScene ("MainMenuScene");
		}
	}

	void OnDestroy(){
		Debug.Log ("Game manager:OnDestroy");
		EventManager.DestroyInstance ();
	}
}

