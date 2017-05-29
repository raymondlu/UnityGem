using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GemController : MonoBehaviour
{
    [SerializeField]
    private int _row = 0;
    [SerializeField]
    private int _column = 0;
    [SerializeField]
    private GemType _type = GemType.Gem_Count;

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
    }

    public int Row
    {
        get
        {
            return _row;
        }
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

    private void OnMouseDown()
    {
        _renderer.material.color = Color.red;
    }
    private void OnMouseUp()
    {
        _renderer.material.color = Color.white;
    }
    private void OnMouseOver()
    {
        //rend.material.color = Color.gray;
    }
    private void OnMouseExit()
    {
        _renderer.material.color = Color.white;
    }
}