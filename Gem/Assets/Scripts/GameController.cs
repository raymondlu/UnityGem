//#define LOG_TRACE_INFO
//#define LOG_EXTRA_INFO

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//------------------------------------------------------------------------------
// class definition
//------------------------------------------------------------------------------
public class GameController : MonoBehaviour
{
	public GameObject gemPrefab;
	public GameObject board;
	public GameObject scoreText;
	public GameObject levelText;
	public bool shouldUpdate = true;

	private static GameController gameController;

	//--------------------------------------------------------------------------
	// public static methods
	//--------------------------------------------------------------------------
	//--------------------------------------------------------------------------
	// protected mono methods
	//--------------------------------------------------------------------------
	protected void Awake()
	{
		gameController = this;
	}
	
	protected void OnDestroy()
	{
		if(gameController != null)
		{
			gameController = null;
		}
	}
	
	protected void OnDisable()
	{
	}
	
	protected void OnEnable()
	{
	}
	
	protected void Start(){
		shouldUpdate = true;
	}
	
	protected void Update(){
		if (shouldUpdate) {
			shouldUpdate = false;
			UpdateGame ();
		}
	}

	//--------------------------------------------------------------------------
	// private methods
	//--------------------------------------------------------------------------

	private void UpdateGame(){
		var startX = GameConfig.Instance.gemStartX;
		var startY = GameConfig.Instance.gemStartY;
		var gemWidth = GameConfig.Instance.gemWidth;
		var gemHeight = GameConfig.Instance.gemHeight;
		var gemRow = GameConfig.Instance.gemRow;
		var gemColumn = GameConfig.Instance.gemColumn;

		Vector3 pos = new Vector3 (0, 0, board.transform.position.z);

		for (int r = 0; r < gemRow; ++r) {
			for (int c = 0; c < gemColumn; ++c) {
				pos.x = startX + c * gemWidth;
				pos.y = startY - r * gemHeight;
				GameObject gemObj = Instantiate (gemPrefab, pos, board.transform.rotation) as GameObject;
				gemObj.transform.SetParent (board.transform, false);
			}
		}

		Text text = scoreText.GetComponent<Text>();
		if (text != null) {
			text.text = GameState.CurrentScore.ToString();
		}

		text = levelText.GetComponent<Text>();
		if (text != null) {
			text.text = GameState.CurrentLevel.ToString();
		}
	}

	//--------------------------------------------------------------------------
	// public methods
	//--------------------------------------------------------------------------
	public void ShowMenu()
	{
		MainController.SwitchScene ("MenuScene");
	}
}
