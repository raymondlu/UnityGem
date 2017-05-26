using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
	private int currentScore = 0;
	private int currentLevel = 1;

	private static GameState gameState;

	public static int CurrentScore {
		get { return gameState.currentScore; }
	}

	public static void AddScore (int add) {
		gameState.currentScore += add;
	}

	public static int CurrentLevel {
		get { return gameState.currentLevel; }
	}

	public static void IncLevel () {
		gameState.currentLevel += 1;
	}

	protected void Awake() {
		//Let's keep this alive between scene changes
		Object.DontDestroyOnLoad (gameObject);

		gameState = this;
	}

	protected void OnDestroy(){
		if (gameState != null) {
			gameState = null;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
