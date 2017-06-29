//#define LOG_TRACE_INFO
//#define LOG_EXTRA_INFO

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum GemType { Gem_1, Gem_2, Gem_3, Gem_4, Gem_5, Gem_6, Gem_7, Gem_8, Gem_Count };
public enum GameBoardState { Idle, FirstSelection, SecondSelection, Swap, Calculation, ReverseSwap, DestroyGem, SpawnGem, FallGem, StateCount };
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

	private GemController[,] _currentGemControllerMatrix;

    private GameBoardState _currentGameBoardState = GameBoardState.Idle;

    private delegate void GameBoardStateOperationHandler(int row, int column, GemOperation operation);
    private GameBoardStateOperationHandler[] _gameBoardStateOperationHandlers;

    private delegate void GameBoardStateUpdate();
    private GameBoardStateUpdate[] _gameBoardStateUpdates;
    
    private bool shouldUpdate = false;
	private static GameController gameController;

    private GemController _firstSelectedGemControler;
    private GemController _secondSelectedGemControler;

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
	
	protected void Start()
    {
        shouldUpdate = true;

        Debug.Assert(gemSprites.Count == (int)GemType.Gem_Count, "gem sprite count is not equal to gem type count.");

        var gemRow = GameConfig.Instance.gemRow;
        var gemColumn = GameConfig.Instance.gemColumn;
        _currentGemControllerMatrix = new GemController[gemRow, gemColumn];

        for (int row = 0; row < gemRow; row++)
        {
            for (int column = 0; column < gemColumn; column++)
            {
                SpawnGem(row, column, true);
            }
        }

        _currentGameBoardState = GameBoardState.Idle;
        _firstSelectedGemControler = null;
        _secondSelectedGemControler = null;
        _gameBoardStateOperationHandlers = new GameBoardStateOperationHandler[(int)GameBoardState.StateCount];
        _gameBoardStateUpdates = new GameBoardStateUpdate[(int)GameBoardState.StateCount];

        // implementations
        SetupStateHandlers();
    }

    private void ChangeToState(GameBoardState newState)
    {
        _currentGameBoardState = newState;
        Debug.Log(string.Format("Change to new state:{0}", _currentGameBoardState.ToString()));
    }

    private void SetupStateHandlers()
    {
        _gameBoardStateUpdates[(int)GameBoardState.Idle] = () =>
        {

        };
        _gameBoardStateOperationHandlers[(int)GameBoardState.Idle] = (int row, int column, GemOperation operation) =>
        {
            if (operation == GemOperation.TouchDown)
            {
                _firstSelectedGemControler = _currentGemControllerMatrix[row, column];
                ChangeToState(GameBoardState.FirstSelection);
            }
        };

        _gameBoardStateUpdates[(int)GameBoardState.FirstSelection] = () =>
        {

        };
        _gameBoardStateOperationHandlers[(int)GameBoardState.FirstSelection] = (int row, int column, GemOperation operation) =>
        {
            if (operation == GemOperation.TouchDown/* || operation == GemOperation.TouchEnter*/)
            {
                // TODO:
                // check if the two selected gem is adjacent
                bool isAdjacent = true;
                if (isAdjacent)
                {
                    _secondSelectedGemControler = _currentGemControllerMatrix[row, column];
                    ChangeToState(GameBoardState.SecondSelection);
                }
                else
                {
                    _firstSelectedGemControler = _currentGemControllerMatrix[row, column];
                }
            }
        };

        _gameBoardStateUpdates[(int)GameBoardState.SecondSelection] = () =>
        {

        };
        _gameBoardStateOperationHandlers[(int)GameBoardState.SecondSelection] = (int row, int column, GemOperation operation) =>
        {

        };

        _gameBoardStateUpdates[(int)GameBoardState.Swap] = () =>
        {

        };
        _gameBoardStateOperationHandlers[(int)GameBoardState.Swap] = (int row, int column, GemOperation operation) =>
        {

        };

        _gameBoardStateUpdates[(int)GameBoardState.Calculation] = () =>
        {

        };
        _gameBoardStateOperationHandlers[(int)GameBoardState.Calculation] = (int row, int column, GemOperation operation) =>
        {

        };

        _gameBoardStateUpdates[(int)GameBoardState.ReverseSwap] = () =>
        {

        };
        _gameBoardStateOperationHandlers[(int)GameBoardState.ReverseSwap] = (int row, int column, GemOperation operation) =>
        {

        };

        _gameBoardStateUpdates[(int)GameBoardState.DestroyGem] = () =>
        {

        };
        _gameBoardStateOperationHandlers[(int)GameBoardState.DestroyGem] = (int row, int column, GemOperation operation) =>
        {

        };

        _gameBoardStateUpdates[(int)GameBoardState.SpawnGem] = () =>
        {

        };
        _gameBoardStateOperationHandlers[(int)GameBoardState.SpawnGem] = (int row, int column, GemOperation operation) =>
        {

        };

        _gameBoardStateUpdates[(int)GameBoardState.FallGem] = () =>
        {

        };
        _gameBoardStateOperationHandlers[(int)GameBoardState.FallGem] = (int row, int column, GemOperation operation) =>
        {

        };
    }

    protected void Update(){
		if (shouldUpdate) {
			shouldUpdate = false;
			UpdateGameStatus ();
		}

        if (_gameBoardStateUpdates != null && _gameBoardStateUpdates[(int)_currentGameBoardState] != null)
        {
            _gameBoardStateUpdates[(int)_currentGameBoardState]();
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
            if (leftRow >= 0 && _currentGemControllerMatrix[leftRow,column] != null)
            {
                candidateGemTypes.Remove(_currentGemControllerMatrix[leftRow, column].Type);
            }
            var topColumn = column - 1;
            if (topColumn >= 0 && _currentGemControllerMatrix[row, topColumn] != null)
            {
                candidateGemTypes.Remove(_currentGemControllerMatrix[row, topColumn].Type);
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

        _currentGemControllerMatrix[row, column] = gemController;
        gemController.GemOperationEvent += OnGemOperation;

        return gemController;
    }

    private void DestroyGem(int row, int column)
    {
        if (_currentGemControllerMatrix[row, column] != null)
        {
            _currentGemControllerMatrix[row, column].GemOperationEvent -= OnGemOperation;
            _currentGemControllerMatrix[row, column].DestroyGem();
            _currentGemControllerMatrix[row, column] = null;
        }
    }

	//--------------------------------------------------------------------------
	// public methods
	//--------------------------------------------------------------------------
	public void ShowMenu()
	{
		MainController.SwitchScene ("MenuScene");
	}
    public void OnGemOperation(int row, int column, GemOperation operation)
    {
        //Debug.Log(string.Format("Gem:{0},{1} is {2}.", row, column, operation.ToString()));
        if (_gameBoardStateOperationHandlers[(int)_currentGameBoardState] != null)
        {
            _gameBoardStateOperationHandlers[(int)_currentGameBoardState](row, column, operation);
        }
    }
}
