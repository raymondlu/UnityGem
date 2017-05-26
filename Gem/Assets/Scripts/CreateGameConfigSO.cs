using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName="Create Scriptable Object/Game Config")]
public class CreateGameConfigSO : ScriptableObject {
	public int currentLevel = 1;
	public int currentScore = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
