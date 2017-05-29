﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GemController : MonoBehaviour
{
    [SerializeField]
    private int row = 0;
    [SerializeField]
    private int column = 0;
    [SerializeField]
    private GemType type = GemType.Gem_Count;

    private Renderer _renderer;
    private static List<GemController> _gemControllers;

    public GemType Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }

    public int Column
    {
        get
        {
            return column;
        }

        set
        {
            column = value;
        }
    }

    public int Row
    {
        get
        {
            return row;
        }

        set
        {
            row = value;
        }
    }

    public static GemController CreateGemObject(int row, int column, GemType type, Sprite sprite, GameObject board)
    {
        if (_gemControllers == null)
        {
            _gemControllers = new List<GemController>();
        }

        var startX = GameConfig.Instance.gemStartX;
        var startY = GameConfig.Instance.gemStartY;
        var gemWidth = GameConfig.Instance.gemWidth;
        var gemHeight = GameConfig.Instance.gemHeight;
        Vector3 pos = new Vector3(0, 0, board.transform.position.z);
        pos.x = startX + column * gemWidth;
        pos.y = startY - row * gemHeight;

        foreach(var controller in _gemControllers)
        {
            if (controller.gameObject.activeSelf == false)
            {
                controller.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
                controller.Type = type;
                controller.Row = row;
                controller.Column = column;
                return controller;
            }
        }

        GameObject gemObj = Instantiate(GameController.Instance.GemPrefab, pos, board.transform.rotation) as GameObject;
        gemObj.transform.SetParent(board.transform, false);

        gemObj.GetComponent<SpriteRenderer>().sprite = sprite;
        var gemController = gemObj.GetComponent<GemController>();
        gemController.Type = type;
        gemController.Row = row;
        gemController.Column = column;

        return gemObj.GetComponent<GemController>();
    }

    public void DestroyGem()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        _gemControllers.Add(this);
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