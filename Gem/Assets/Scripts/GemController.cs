using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GemController : MonoBehaviour
{
    private Renderer rend;

	// Use this for initialization
	void Start ()
    {
        rend = GetComponent<Renderer>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnMouseDown()
    {
        
    }
    private void OnMouseUp()
    {
        
    }
    private void OnMouseEnter()
    {
        //rend.material.color = Color.red;
    }
    private void OnMouseOver()
    {
        //rend.material.color -= new Color(0.1F, 0, 0) * Time.deltaTime;
        rend.material.color = Color.red;
    }
    private void OnMouseExit()
    {
        rend.material.color = Color.white;
    }
}