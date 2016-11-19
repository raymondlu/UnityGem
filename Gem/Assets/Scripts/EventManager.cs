using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour{
	public delegate bool EventDelegate(GameEvent eventInstance);
	Dictionary<System.Type,EventDelegate> _delegateDictionary;
	HashSet<EventDelegate> _delegateSet;

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
		_delegateSet = new HashSet<EventDelegate>();
	}

	public bool AddDelegate<T>(EventDelegate delegateInstance) where T:GameEvent{
		if (_delegateSet.Contains (delegateInstance)) {
			return false;
		}

		EventDelegate existedDelegates;
		if(_delegateDictionary.TryGetValue(typeof(T),out existedDelegates)){
			existedDelegates += delegateInstance;
		} else {
			_delegateDictionary [typeof(T)] = delegateInstance;
		}
		_delegateSet.Add (delegateInstance);

		return true;
	}

	public bool RemoveDelegate<T> (EventDelegate delegateInstance) where T:GameEvent{
		if (!_delegateSet.Contains (delegateInstance)) {
			return true;
		}
		EventDelegate existedDelegates;
		if(_delegateDictionary.TryGetValue(typeof(T),out existedDelegates)){
			existedDelegates -= delegateInstance;
			if (existedDelegates != null) {
				_delegateDictionary [typeof(T)] = existedDelegates;
			} else {
				_delegateDictionary.Remove (typeof(T));
			}
			_delegateSet.Remove (delegateInstance);
		} else {
			Debug.LogError ("Error in RemoveDelegate.");
		}

		return true;
	}

	public bool TriggerEvent(GameEvent eventInstance){
		EventDelegate existedDelegates;
		if(_delegateDictionary.TryGetValue(eventInstance.GetType(),out existedDelegates)){
			existedDelegates.Invoke (eventInstance);
		}
		return false;
	}

	// Use this for initialization
	void Start (){
	
	}
	
	// Update is called once per frame
	void Update (){
	
	}
}

