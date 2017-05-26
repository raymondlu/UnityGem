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
	
	protected void Start()
	{
		for (int i = 0; i < 64; ++i) {
			GameObject gemObj = Instantiate (gemPrefab);
			gemObj.transform.SetParent (board.transform, false);
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
	
	protected void Update()
	{
	}

	//--------------------------------------------------------------------------
	// private methods
	//--------------------------------------------------------------------------

	//--------------------------------------------------------------------------
	// public methods
	//--------------------------------------------------------------------------
	public void ShowMenu()
	{
		MainController.SwitchScene ("MenuScene");
	}
}
