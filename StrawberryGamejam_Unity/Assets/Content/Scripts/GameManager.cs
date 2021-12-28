using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [ShowInInspector]
    public int TargetFPS
    {
        get 
        { 
            return Application.targetFrameRate;
        }
        set
        {
            Application.targetFrameRate = value;
        }
    }

    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Start()
    {
        // Setup Controls
        Controls controls = new Controls();
        controls.Enable();
        controls.Game.Restart.performed += RestartGame;
        controls.Game.Quit.performed += QuitGame;
    }




    private void QuitGame(InputAction.CallbackContext obj)
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
        #else
         Application.Quit();
        #endif
    }


    private void RestartGame(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

}




