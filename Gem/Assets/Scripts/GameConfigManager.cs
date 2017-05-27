using UnityEngine;
using System.Collections;

public class GameConfigManager : MonoBehaviour{
	public GameConfig gameConfigTemplate;

	private static GameConfigManager _instance;

	protected void Awake() {
		//Let's keep this alive between scene changes
		Object.DontDestroyOnLoad (gameObject);

		_instance = this;
		GameConfig.Initialize (gameConfigTemplate);
	}

	protected void OnDestroy(){
		if (_instance != null) {
			_instance = null;
		}
	}

	// Use this for initialization
	protected void Start (){
	
	}
	
	// Update is called once per frame
	protected void Update (){
	
	}
}

