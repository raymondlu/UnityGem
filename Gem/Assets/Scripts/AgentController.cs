//#define LOG_TRACE_INFO
//#define LOG_EXTRA_INFO

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//------------------------------------------------------------------------------
// class definition
//------------------------------------------------------------------------------
public class AgentController : MonoBehaviour
{
	// singleton list to hold all our projectiles
	static private List<AgentController> agentControllers;

	//--------------------------------------------------------------------------
	// static public methods
	//--------------------------------------------------------------------------
	static public AgentController Spawn( Vector3 location, Vector3 heading )
	{
		// search for the first free agentController
		foreach( AgentController agentController in agentControllers )
		{
			// if disabled, then it's available
			if( agentController.gameObject.activeSelf == false )
			{
				// set it up
				agentController.transform.position = location;
				agentController.transform.eulerAngles = heading;
				
				// switch it back on
				agentController.gameObject.SetActive(true);
				
				// return a reference to the caller
				return agentController;

			}
		}

		// if we get here, we haven't pooled enough agents.
		// it might be good to write a log here to make this known.
		return null;
	}

	//--------------------------------------------------------------------------
	// protected mono methods
	//--------------------------------------------------------------------------
	protected void Awake()
	{
		// does the pool exist yet?
		if( agentControllers == null )
		{
			// lazy initialize it
			agentControllers = new List<AgentController>();
		}
		// add myself
		agentControllers.Add(this);
	}
	
	protected void OnDestroy()
	{
		// remove myself from the pool
		agentControllers.Remove(this);
		// was I the last one?
		if(agentControllers.Count == 0)
		{
			// remove the pool itself
			agentControllers = null;
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
		gameObject.SetActive(false);
	}
	
	protected void Update()
	{
		// travel in a straight line at 4 units per second
		transform.position += transform.up * (Time.deltaTime * 4);
	}

	protected void OnBecameInvisible()
	{
		// I've left the screen. Disable myself so I'm available again
		gameObject.SetActive(false);
	}


	//--------------------------------------------------------------------------
	// private methods
	//--------------------------------------------------------------------------
}
