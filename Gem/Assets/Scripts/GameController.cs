//#define LOG_TRACE_INFO
//#define LOG_EXTRA_INFO

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public enum GemType { Gem_1, Gem_2, Gem_3, Gem_4, Gem_5, Gem_6, Gem_7, Gem_8, Gem_Count };
public enum GameBoardState { Idle, FirstSelection, SecondSelection, Swap, ReverseSwap, DestroyGem, SpawnGem, FallGem, StateCount };
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

    private Dictionary<GameBoardState,GameControllerStateBase> _states = new Dictionary<GameBoardState, GameControllerStateBase>();
    private bool _isSwappingDone = false;
    
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
            gameController.DestroyStateHandlers();
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

        // implementations
        SetupStateHandlers();
    }

    private void ChangeToState(GameBoardState newState)
    {
        if (_states[_currentGameBoardState] != null)
        {
            _states[_currentGameBoardState].Exit();
        }
        _currentGameBoardState = newState;
        if (_states[_currentGameBoardState] != null)
        {
            _states[_currentGameBoardState].Enter();
        }
        Debug.Log(string.Format("Change to new state:{0}", _currentGameBoardState.ToString()));
    }

    private void SetupStateHandlers()
    {
        _states.Add(GameBoardState.Idle, new IdleState(this));
        _states.Add(GameBoardState.FirstSelection, new FirstSelectionState(this));
        _states.Add(GameBoardState.SecondSelection, new SecondSelectionState(this));
        _states.Add(GameBoardState.Swap, new SwapState(this));
        _states.Add(GameBoardState.ReverseSwap, new ReverseSwapState(this));
    }

    private void DestroyStateHandlers()
    {
        foreach (var state in _states.Values)
        {
            state.OnDestroy();
        }
        _states = null;
    }

    private void SwapTwoGems()
    {
        _isSwappingDone = false;
        var mySequence = DOTween.Sequence();
        float moveDuration = 0.5f;
        mySequence.Append(_firstSelectedGemControler.transform.DOLocalMove(_secondSelectedGemControler.transform.localPosition, moveDuration));
        mySequence.Join(_secondSelectedGemControler.transform.DOLocalMove(_firstSelectedGemControler.transform.localPosition, moveDuration));
        mySequence.AppendCallback(() => {
            _isSwappingDone = true;
            _currentGemControllerMatrix[_firstSelectedGemControler.Row,_firstSelectedGemControler.Column] = _secondSelectedGemControler;
            _currentGemControllerMatrix[_secondSelectedGemControler.Row,_secondSelectedGemControler.Column] = _firstSelectedGemControler;
        });
    }

    private bool CalculateSwappedGems()
    {
        /*var dirctions = new Dictionary<int, int> {
            {-1, 0},// left
            {1, 0},// right
            {0, -1},// up
            {0, 1},// down
        };*/
        return false;
    }

    protected void Update(){
		if (shouldUpdate) {
			shouldUpdate = false;
			UpdateGameStatus ();
		}

        if (_states != null && _states[_currentGameBoardState] != null)
        {
            _states[_currentGameBoardState].Update();
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
        if (_states[_currentGameBoardState] != null)
        {
            _states[_currentGameBoardState].OnGemOperation(row, column, operation);
        }
    }

    // state classes
    class GameControllerStateBase : Misc.StateBase
    {
        protected GameController _controller;
        public GameControllerStateBase(GameController controller)
        {
            _controller = controller;
        }
        virtual public void OnGemOperation(int row, int column, GemOperation operation)
        {
            //Implemented in child class
        }
        virtual public void OnDestroy()
        {
            //Implemented in child class
            _controller = null;
        }
    }
    class IdleState : GameControllerStateBase
    {
        public IdleState(GameController controller) : base(controller)
        {
            // Do nothing
        }

        public override void OnGemOperation(int row, int column, GemOperation operation)
        {
            base.OnGemOperation(row, column, operation);
            if (operation == GemOperation.TouchDown)
            {
                _controller._firstSelectedGemControler = _controller._currentGemControllerMatrix[row, column];
                _controller.ChangeToState(GameBoardState.FirstSelection);
            }
        }
    }
    class FirstSelectionState : GameControllerStateBase
    {
        public FirstSelectionState(GameController controller) : base(controller)
        {
            // Do nothing
        }

        public override void OnGemOperation(int row, int column, GemOperation operation)
        {
            base.OnGemOperation(row, column, operation);
            if (operation == GemOperation.TouchDown/* || operation == GemOperation.TouchEnter*/)
            {
                // check if the two selected gem is adjacent
                var selectedGemControler = _controller._currentGemControllerMatrix[row, column];
                bool isAdjacent = GemController.IsAdjacent(_controller._firstSelectedGemControler, selectedGemControler);
                if (isAdjacent)
                {
                    _controller._secondSelectedGemControler = selectedGemControler;
                    _controller.ChangeToState(GameBoardState.SecondSelection);
                }
                else
                {
                    _controller._firstSelectedGemControler = selectedGemControler;
                }
            }
        }
    }
    class SecondSelectionState : GameControllerStateBase
    {
        public SecondSelectionState(GameController controller) : base(controller)
        {
            // Do nothing
        }

        public override void Enter()
        {
            base.Enter();
            _controller.ChangeToState(GameBoardState.Swap);
            //_controller.SwapTwoGems();
        }
    }

    class SwapState : GameControllerStateBase
    {
        public SwapState(GameController controller) : base(controller)
        {
            // Do nothing
        }

        public override void Enter()
        {
            base.Enter();
            _controller.SwapTwoGems();
        }

        public override void Update()
        {
            base.Update();
            if (_controller._isSwappingDone)
            {
                bool hasMatchedGames = _controller.CalculateSwappedGems();
                if (!hasMatchedGames)
                {
                    _controller.ChangeToState(GameBoardState.ReverseSwap);
                    _controller.SwapTwoGems();
                }
                else
                {
                    _controller.ChangeToState(GameBoardState.DestroyGem);
                }
            }
        }
    }

    class ReverseSwapState : GameControllerStateBase
    {
        public ReverseSwapState(GameController controller) : base(controller)
        {
            // Do nothing
        }

        public override void Update()
        {
            base.Update();
            if (_controller._isSwappingDone)
            {
                _controller.ChangeToState(GameBoardState.Idle);
            }
        }
    }
}
