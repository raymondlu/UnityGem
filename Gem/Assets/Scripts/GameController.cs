//#define LOG_TRACE_INFO
//#define LOG_EXTRA_INFO

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum GemType { Gem_1, Gem_2, Gem_3, Gem_4, Gem_5, Gem_6, Gem_7, Gem_8, Gem_Count };

//------------------------------------------------------------------------------
// class definition
//------------------------------------------------------------------------------
public class GameController : MonoBehaviour
{
    public GameObject gemPrefab;
    public List<Sprite> gemSprites;
    public GameObject board;
    public GameObject scoreText;
	public GameObject levelText;

	private GemController[,] currentGemControllerMatrix;

	private bool shouldUpdate = true;
	private static GameController gameController;

    public GameObject Board
    {
        get
        {
            return board;
        }
    }

    //--------------------------------------------------------------------------
    // public static methods
    //--------------------------------------------------------------------------
    public static GameController Instance
    {
        get
        {
            return gameController;
        }
    }

    public GameObject GemPrefab
    {
        get
        {
            return gemPrefab;
        }
    }

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

        Debug.Assert(gemSprites.Count == (int)GemType.Gem_Count, "gem sprite count is not equal to gem type count.");

		var gemRow = GameConfig.Instance.gemRow;
		var gemColumn = GameConfig.Instance.gemColumn;
        currentGemControllerMatrix = new GemController[gemRow, gemColumn];

        for (int row = 0; row < gemRow; row++)
        {
            for (int column = 0; column < gemColumn; column++)
            {
                SpawnGem(row, column, true);
            }
        }
	}
	
	protected void Update(){
		if (shouldUpdate) {
			shouldUpdate = false;
			UpdateGameStatus ();
		}
	}

	//--------------------------------------------------------------------------
	// private methods
	//--------------------------------------------------------------------------
	private void UpdateGameStatus(){
		Text text = scoreText.GetComponent<Text>();
		if (text != null) {
			text.text = GameState.CurrentScore.ToString();
		}

		text = levelText.GetComponent<Text>();
		if (text != null) {
			text.text = GameState.CurrentLevel.ToString();
		}
	}
    private GemController SpawnGem(int row, int column, bool shouldPreventRepeating)
    {
        var candidateGemTypes = new List<GemType>();
        for (int i = 0; i < (int)GemType.Gem_Count; i++)
        {
            candidateGemTypes.Add((GemType)i);
        }

        if (shouldPreventRepeating)
        {
            var leftRow = row - 1;
            if (leftRow >= 0 && currentGemControllerMatrix[leftRow,column] != null)
            {
                candidateGemTypes.Remove(currentGemControllerMatrix[leftRow, column].Type);
            }
            var topColumn = column - 1;
            if (topColumn >= 0 && currentGemControllerMatrix[row, topColumn] != null)
            {
                candidateGemTypes.Remove(currentGemControllerMatrix[row, topColumn].Type);
            }
        }

        var selectedGemType = candidateGemTypes[Random.Range(0, candidateGemTypes.Count)];
        var gemController = GemController.CreateGemObject(row, column, selectedGemType, gemSprites[(int)selectedGemType]);
        var startX = GameConfig.Instance.gemStartX;
        var startY = GameConfig.Instance.gemStartY;
        var gemWidth = GameConfig.Instance.gemWidth;
        var gemHeight = GameConfig.Instance.gemHeight;
        Vector3 pos = new Vector3(0, 0, board.transform.position.z);
        pos.x = startX + column * gemWidth;
        pos.y = startY - row * gemHeight;
        gemController.transform.position = pos;

        currentGemControllerMatrix[row, column] = gemController;

        return gemController;
    }

    private void DestroyGem(int row, int column)
    {
        if (currentGemControllerMatrix[row, column] != null)
        {
            currentGemControllerMatrix[row, column].DestroyGem();
            currentGemControllerMatrix[row, column] = null;
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
