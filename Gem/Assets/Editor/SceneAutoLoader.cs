using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

/// <summary>
/// Scene auto loader.
/// </summary>
/// <description>

/// This class adds a File > Scene Autoload menu containing options to select
/// a "master scene" enable it to be auto-loaded when the user presses play
/// in the editor. When enabled, the selected scene will be loaded on play,
/// then the original scene will be reloaded on stop.
///
/// Based on an idea on this thread:
/// http://forum.unity3d.com/threads/157502-Executing-first-scene-in-build-settings-when-pressing-play-button-in-editor
/// </description>
[InitializeOnLoad]
public static class SceneAutoLoader
{
    // Static constructor binds a playmode-changed callback.
    // [InitializeOnLoad] above makes sure this gets execusted.
    static SceneAutoLoader()
    {
		// Raymond:
		// Failed to show menu item to set master scene.
		// So set the flag manually here.
		//LoadMasterOnPlay = true;
        EditorApplication.playmodeStateChanged += OnPlayModeChanged;
    }
    // Menu items to select the "master" scene and control whether or not to load it.
    [MenuItem("File/Scene Autoload/Select Master Scene...")]
    private static void SelectMasterScene()
    {
        string masterScene = EditorUtility.OpenFilePanel("Select Master Scene", Application.dataPath, "unity");
        if (!string.IsNullOrEmpty(masterScene))
        {
            MasterScene = masterScene;
            LoadMasterOnPlay = true;
        }
    }

    [MenuItem("File/Scene Autoload/Load Master On Play", true)]
    private static bool ShowLoadMasterOnPlay()
    {
        return !LoadMasterOnPlay;
    }
    [MenuItem("File/Scene Autoload/Load Master On Play")]
    private static void EnableLoadMasterOnPlay()
    {
        LoadMasterOnPlay = true;
    }

    [MenuItem("File/Scene Autoload/Don't Load Master On Play", true)]
    private static bool ShowDontLoadMasterOnPlay()
    {
        return LoadMasterOnPlay;
    }
    [MenuItem("File/Scene Autoload/Don't Load Master On Play")]
    private static void DisableLoadMasterOnPlay()
    {
        LoadMasterOnPlay = false;
    }

    // Play mode change callback handles the scene load/reload.
    private static void OnPlayModeChanged()
    {

        if (!LoadMasterOnPlay) return;

        if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
        {
            // User pressed play -- persist the currently loaded scenes.
            string[] _scenes = new string[EditorSceneManager.loadedSceneCount];
            for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
            {
                _scenes[i] = EditorSceneManager.GetSceneAt(i).path;
            }
            LoadedScenes = _scenes;
            ActiveScene = EditorSceneManager.GetActiveScene().name;
            

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(MasterScene, OpenSceneMode.Single);
            }
            else
            {
                // User cancelled the save operation -- cancel play as well.
                EditorApplication.isPlaying = false;
            }
        }
        if (EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            // User pressed stop -- reload previous scene.
            EditorApplication.update += ReloadLastScene;
        }
    }

    public static void ReloadLastScene()
    {
        if (EditorApplication.isPlaying)
            return;

        Debug.Log("Reloading editor scene setup...");

        bool _dropRunScene = true;

        UnityEngine.SceneManagement.Scene _runScene = EditorSceneManager.GetActiveScene();

        string[] _scenesToLoad = LoadedScenes;

        //Loop through the stored scene list and add them back in
        for (int i = 0; i < _scenesToLoad.Length; i++)
        {
            if (_scenesToLoad[i] == string.Empty)
                break;

            //If the scene to load is the run mode scene then leave it alone and clear the drop flag
            if (_scenesToLoad[i] == _runScene.path)
            {
                _dropRunScene = false;
            }
            else
            {
                EditorSceneManager.OpenScene(_scenesToLoad[i],OpenSceneMode.Additive);
            }
        }

        //Now set the active scene
        for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
        {
            if (EditorSceneManager.GetSceneAt(i).name == ActiveScene)
            {
                EditorSceneManager.SetActiveScene(EditorSceneManager.GetSceneAt(i));
                break;
            }
        }

        //Finally close out the run scene if it is not required
        if (_dropRunScene)
        {
            EditorSceneManager.CloseScene(_runScene, true);
        }

        EditorApplication.update -= ReloadLastScene; ;
    }

    // Properties are remembered as editor preferences.
    private static string cEditorPrefLoadMasterOnPlay { get { return "SceneAutoLoader." + PlayerSettings.productName + ".LoadMasterOnPlay"; } }
    private static string cEditorPrefMasterScene { get { return "SceneAutoLoader." + PlayerSettings.productName + ".MasterScene"; } }
    private static string cEditorPrefPreviousScene { get { return "SceneAutoLoader." + PlayerSettings.productName + ".ActiveScene"; } }
    private static string cEditorPrefLoadedScenes { get { return "SceneAutoLoader." + PlayerSettings.productName + ".LoadedScenes"; } }

    public static bool LoadMasterOnPlay
    {
        get { return EditorPrefs.GetBool(cEditorPrefLoadMasterOnPlay, false); }
        set { EditorPrefs.SetBool(cEditorPrefLoadMasterOnPlay, value); }
    }

    private static string MasterScene
    {
        get { return EditorPrefs.GetString(cEditorPrefMasterScene, "Assets/Scenes/MainScene.unity"); }
        set { EditorPrefs.SetString(cEditorPrefMasterScene, value); }
    }

    public static string ActiveScene
    {
        get
        {
            return EditorPrefs.GetString(cEditorPrefPreviousScene, "");
        }
        set
        {
            
            EditorPrefs.SetString(cEditorPrefPreviousScene, value);
        }
    }

    public static string[] LoadedScenes
    {
        get
        {
            string _prefValue = EditorPrefs.GetString(cEditorPrefLoadedScenes, "");

            if (_prefValue == string.Empty)
                return new string[0];

            string[] _retValue = _prefValue.Split('|');

            return _retValue;
        }
        set
        {
            string _storeValue = "";

            for ( int i = 0; i < value.Length; i++ )
            {
                _storeValue += value[i];
                _storeValue += "|";
            }

            EditorPrefs.SetString(cEditorPrefLoadedScenes, _storeValue);
        }
    }
}
