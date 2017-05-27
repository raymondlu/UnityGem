//#define LOG_TRACE_INFO
//#define LOG_EXTRA_INFO

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


//------------------------------------------------------------------------------
// class definition
//------------------------------------------------------------------------------
public class GameController : MonoBehaviour
{
	public GameObject gemPrefab;
	public GameObject board;
	public GameObject scoreText;
	public GameObject levelText;
	public List<Sprite> gems = new List<Sprite> ();

	private GameObject[,] gemObjectMatrix;
	private bool shouldUpdate = true;
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

		var gemRow = GameConfig.Instance.gemRow;
		var gemColumn = GameConfig.Instance.gemColumn;
		gemObjectMatrix = new GameObject[gemRow, gemColumn];

		for (int r = 0; r < gemRow; ++r) {
			for (int c = 0; c < gemColumn; ++c) {
				gemObjectMatrix[r,c] = CreateGemObject (r, c);
			}
		}
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
	private GameObject CreateGemObject(int row, int column, bool preventRepeat = true){
		var startX = GameConfig.Instance.gemStartX;
		var startY = GameConfig.Instance.gemStartY;
		var gemWidth = GameConfig.Instance.gemWidth;
		var gemHeight = GameConfig.Instance.gemHeight;
		Vector3 pos = new Vector3 (0, 0, board.transform.position.z);
		pos.x = startX + column * gemWidth;
		pos.y = startY - row * gemHeight;
		GameObject gemObj = Instantiate (gemPrefab, pos, board.transform.rotation) as GameObject;
		gemObj.transform.SetParent (board.transform, false);

		List<Sprite> candidateGemSprites = new List<Sprite> ();
		candidateGemSprites.AddRange (gems);

		if (preventRepeat) {
			if (row - 1 >= 0 && gemObjectMatrix [row - 1, column] != null) {
				Sprite leftGemSprite = gemObjectMatrix [row - 1, column].GetComponent<SpriteRenderer>().sprite;
				candidateGemSprites.Remove (leftGemSprite);
			}

			if (column - 1 >=0 && gemObjectMatrix [row, column - 1] != null) {
				Sprite uppperGemSprite = gemObjectMatrix [row, column - 1].GetComponent<SpriteRenderer>().sprite;
				candidateGemSprites.Remove (uppperGemSprite);
			}
		}

		gemObj.GetComponent<SpriteRenderer> ().sprite = candidateGemSprites [Random.Range (0, candidateGemSprites.Count)];

		return gemObj;
	}
	private void UpdateGame(){
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
