using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager{
	public delegate void EventDelegate(GameEvent eventInstance);
	private Dictionary<System.Type,EventDelegate> _delegateDictionary;
	private Queue _eventQueue;

	private static EventManager _instance;
	public static EventManager instance{
		get{
			if(_instance == null){
				_instance = new EventManager();
				_instance.Init ();
			}
			return _instance;
		}
	}

	public static void DestroyInstance(){
		_instance = null;
	}

	void Init(){
		Debug.Log ("Event manager is initialized.");
		_delegateDictionary = new Dictionary<System.Type, EventDelegate>();
		_eventQueue = new Queue ();
	}

	public bool AddDelegate<T>(EventDelegate delegateInstance) where T:GameEvent{
		EventDelegate existedDelegates;
		if(_instance._delegateDictionary.TryGetValue(typeof(T),out existedDelegates)){
			existedDelegates += delegateInstance;
		} else {
			_delegateDictionary [typeof(T)] = delegateInstance;
		}

		return true;
	}

	public static bool SafeAddDelegate<T>(EventDelegate delegateInstance) where T:GameEvent{
		if (_instance != null) {
			return _instance.AddDelegate<T> (delegateInstance);
		}

		return false;
	}

	public void RemoveDelegate<T> (EventDelegate delegateInstance) where T:GameEvent{
		EventDelegate existedDelegates;
		if(_delegateDictionary.TryGetValue(typeof(T),out existedDelegates)){
			existedDelegates -= delegateInstance;
			if (existedDelegates != null) {
				_delegateDictionary [typeof(T)] = existedDelegates;
			} else {
				_delegateDictionary.Remove (typeof(T));
			}
		} else {
			Debug.LogError ("Error in RemoveDelegate.");
		}
	}

	public static void SafeRemoveDelegate<T> (EventDelegate delegateInstance) where T:GameEvent{
		if (_instance != null) {
			_instance.RemoveDelegate<T> (delegateInstance);
		}
	}

	public void TriggerEvent(GameEvent eventInstance){
		Debug.Log ("Trigger event:" + eventInstance.GetType ());
		EventDelegate existedDelegates;
		if(_delegateDictionary.TryGetValue(eventInstance.GetType(),out existedDelegates)){
			existedDelegates.Invoke (eventInstance);
		}
	}

	public void QueueEvent(GameEvent eventInstance){
		Debug.Log ("Queue event:" + eventInstance.GetType ());
		_eventQueue.Enqueue (eventInstance);
	}
		
	// Update is called once per frame
	public void Update (){
		while(_eventQueue.Count > 0){
			GameEvent eventInstance = _eventQueue.Dequeue () as GameEvent;
			TriggerEvent (eventInstance);
		}
	}
}

