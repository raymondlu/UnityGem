//#define LOG_TRACE_INFO
//#define LOG_EXTRA_INFO

using UnityEngine;
using System.Collections;

//------------------------------------------------------------------------------
// class definition
//------------------------------------------------------------------------------
public class SpawnPointController : MonoBehaviour
{
	// singleton instance
	private static SpawnPointController spawnPointController;

	//--------------------------------------------------------------------------
	// public static methods
	//--------------------------------------------------------------------------
	//--------------------------------------------------------------------------
	// protected mono methods
	//--------------------------------------------------------------------------
	protected void Awake()
	{
		spawnPointController = this;
	}
	
	protected void OnDestroy()
	{
		if(spawnPointController != null)
		{
			spawnPointController = null;
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
	}
	
	protected void Update()
	{
		// rotate the spawn point
		transform.Rotate(Vector3.forward, Time.deltaTime * 60.0f);

		// create an agent?
		if(Input.GetKeyDown(KeyCode.Space) == true)
		{
			// we just request an agent whether one is available or not
			AgentController.Spawn(transform.position, transform.eulerAngles);
		}
	}

	//--------------------------------------------------------------------------
	// private methods
	//--------------------------------------------------------------------------
}
