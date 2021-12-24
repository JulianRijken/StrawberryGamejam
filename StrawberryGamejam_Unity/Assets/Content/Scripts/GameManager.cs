using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private int m_targetFPS = 300;

    [ShowInInspector]
    public int TargetFPS
    {
        get { return m_targetFPS; }
        set
        {
            Application.targetFrameRate = value;
            m_targetFPS = value;
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




