using System.Collections.Generic;
using UnityEngine;

public enum GemOperation {TouchEnter, TouchDown, TouchUp, TouchExit};

public class GemController : MonoBehaviour
{
    [SerializeField]
    private int _row = 0;
    [SerializeField]
    private int _column = 0;
    [SerializeField]
    private GemType _type = GemType.Gem_Count;
    [SerializeField]
    private bool _isTagged = false;

    public delegate void GemOperationHandler(int row, int column, GemOperation operation);
    public event GemOperationHandler GemOperationEvent;

    private Renderer _renderer;
    private static List<GemController> _gemControllers;

    public GemType Type
    {
        get
        {
            return _type;
        }
    }

    public int Column
    {
        get
        {
            return _column;
        }
        set
        {
            _column = value;
        }
    }

    public int Row
    {
        get
        {
            return _row;
        }
        set
        {
            _row = value;
        }
    }

    public bool IsTagged
    {
        get
        {
            return _isTagged;
        }

        set
        {
            _isTagged = value;

            if (_isTagged)
            {
                _renderer.material.color = Color.blue;
            }
            else
            {
                _renderer.material.color = Color.white;
            }
        }
    }

    public static bool IsAdjacent(GemController first, GemController second)
    {
        bool isAdjacent = false;
        do
        {
            if (System.Math.Abs(first.Row - second.Row) > 1 || System.Math.Abs(first.Column - second.Column) > 1)
            {
                break;
            }
            if (System.Math.Abs(first.Row - second.Row) == 1 && System.Math.Abs(first.Column - second.Column) == 1)
            {
                break;
            }
            if (first.Row == second.Row && first.Column == second.Column)
            {
                break;
            }

            isAdjacent = true;

        } while (false);

        return isAdjacent;
    }

    public static bool IsMatched(GemController first, GemController second)
    {
        bool isMatched = false;

        do
        {
            if (first.Type == second.Type)
            {
                isMatched = true;
                break;
            }

            // TODO:
            // Super Gem
        } while (false);

        return isMatched;
    }

    public static GemController CreateGemObject(int row, int column, GemType type, Sprite sprite)
    {
        foreach(var controller in _gemControllers)
        {
            if (controller.gameObject.activeSelf == false)
            {
                controller.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
                controller._type = type;
                controller._row = row;
                controller._column = column;
                controller.gameObject.SetActive(true);
                return controller;
            }
        }

        Debug.Assert(false, "Not enough gem instances in cache.");

        return null;
    }

    public void DestroyGem()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        if (_gemControllers == null)
        {
            _gemControllers = new List<GemController>();
        }
        _gemControllers.Add(this);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _gemControllers.Remove(this);
        if (_gemControllers.Count == 0)
        {
            _gemControllers = null;
        }
    }

    // Use this for initialization
    void Start ()
    {
        _renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnMouseEnter()
    {
        if (GemOperationEvent != null)
        {
            GemOperationEvent(_row, _column, GemOperation.TouchEnter);
        }
    }
    private void OnMouseExit()
    {
        if (GemOperationEvent != null)
        {
            GemOperationEvent(_row, _column, GemOperation.TouchExit);
        }
    }
    private void OnMouseDown()
    {
        //_renderer.material.color = Color.red;

        if (GemOperationEvent != null)
        {
            GemOperationEvent(_row, _column, GemOperation.TouchDown);
        }
    }
    private void OnMouseUp()
    {
        //_renderer.material.color = Color.white;
        if (GemOperationEvent != null)
        {
            GemOperationEvent(_row, _column, GemOperation.TouchUp);
        }
    }
    public void SetIsSelected(bool isSelected)
    {
        if (isSelected)
        {
            _renderer.material.color = Color.yellow;
        }
        else
        {
            _renderer.material.color = Color.white;
        }
    }
}