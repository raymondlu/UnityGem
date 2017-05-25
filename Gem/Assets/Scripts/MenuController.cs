//#define LOG_TRACE_INFO
//#define LOG_EXTRA_INFO

using UnityEngine;
using System.Collections;

//------------------------------------------------------------------------------
// class definition
//------------------------------------------------------------------------------
public class MenuController : MonoBehaviour
{
	private static MenuController menuController;

	public GameObject menuCanvas;
	public GameObject boardCanvas;

	//--------------------------------------------------------------------------
	// public static methods
	//--------------------------------------------------------------------------
	//--------------------------------------------------------------------------
	// protected mono methods
	//--------------------------------------------------------------------------
	protected void Awake()
	{
		menuController = this;
	}
	
	protected void OnDestroy()
	{
		if(menuController != null)
		{
			menuController = null;
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
		ShowMenu ();
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
		menuCanvas.SetActive (true);
		boardCanvas.SetActive (false);
	}

	public void ShowBoard()
	{
		menuCanvas.SetActive (false);
		boardCanvas.SetActive (true);
	}
}
