using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour{
	public delegate bool EventDelegate(GameEvent eventInstance);
	private Dictionary<System.Type,EventDelegate> _delegateDictionary;
	private Queue _eventQueue;

	private static EventManager _eventManager;

	public static EventManager instance{
		get{
			if(!_eventManager){
				_eventManager = FindObjectOfType (typeof(EventManager)) as EventManager;
				if(!_eventManager){
					Debug.LogError ("Event manager is missing.");
				} else {
					_eventManager.Init ();
				}
			}
			return _eventManager;
		}
	}

	void Init(){
		Debug.Log ("Event manager is initialized.");
		_delegateDictionary = new Dictionary<System.Type, EventDelegate>();
		_eventQueue = new Queue ();
	}

	public bool AddDelegate<T>(EventDelegate delegateInstance) where T:GameEvent{
		EventDelegate existedDelegates;
		if(_delegateDictionary.TryGetValue(typeof(T),out existedDelegates)){
			existedDelegates += delegateInstance;
		} else {
			_delegateDictionary [typeof(T)] = delegateInstance;
		}

		return true;
	}

	public bool RemoveDelegate<T> (EventDelegate delegateInstance) where T:GameEvent{
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

		return true;
	}

	public bool TriggerEvent(GameEvent eventInstance){
		Debug.Log ("Trigger event:" + eventInstance.GetType ());
		EventDelegate existedDelegates;
		if(_delegateDictionary.TryGetValue(eventInstance.GetType(),out existedDelegates)){
			existedDelegates.Invoke (eventInstance);
		}
		return false;
	}

	public void QueueEvent(GameEvent eventInstance){
		Debug.Log ("Queue event:" + eventInstance.GetType ());
		_eventQueue.Enqueue (eventInstance);
	}

	// Use this for initialization
	void Start (){
	
	}

	void OnDisable() {
	}
	
	// Update is called once per frame
	void Update (){
		while(_eventQueue.Count > 0){
			GameEvent eventInstance = _eventQueue.Dequeue () as GameEvent;
			TriggerEvent (eventInstance);
		}
	}
}

