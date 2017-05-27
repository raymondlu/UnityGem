using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName="Create Scriptable Object/Game Config")]
public class GameConfig : ScriptableObject {
	public float gemStartX = 0.0f;
	public float gemStartY = 0.0f;
	public float gemWidth = 1.0f;
	public float gemHeight = 1.0f;
	public int gemRow = 8;
	public int gemColumn = 8;

	private static GameConfig _instance;

	public static GameConfig Instance {
		get {
			Debug.Assert (_instance != null, "game config is not initialized.");
			return _instance;
		}
	}

	public static void Initialize(GameConfig template){
		_instance = Instantiate (template) as GameConfig;
	}

	protected void OnEnable() {
		_instance = this;
	}

	protected void OnDestroy() {
		_instance = null;
	}
}
