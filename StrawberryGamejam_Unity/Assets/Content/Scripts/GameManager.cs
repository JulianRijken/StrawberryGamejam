using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Obstacle m_ObstaclePrefab;

    private void Start()
    {
        StartCoroutine(SpawnEnumerator());
        Application.targetFrameRate = 300;

        Controls controls = new Controls();
        controls.Enable();
        controls.Game.Restart.performed += RestartGame;

    }

    private void RestartGame(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }



    private IEnumerator SpawnEnumerator()
    {
        while (true)
        {
            // Wait till the end of the frame before spawn
            yield return new WaitForEndOfFrame();

            Obstacle spawnedObstacle = Instantiate(m_ObstaclePrefab);

            if (spawnedObstacle)
            {
                HalfCircleSettings settings;
                settings.EdgeSize = 5f;
                settings.FillAlpha = 0.5f;
                settings.RotationAlpha = Random.value;

                spawnedObstacle.InitializeObstacle(100f, 60f, settings);
            }



            yield return new WaitForSeconds(0.1f - Time.deltaTime);
        }
    }




//      while (true)
//        {
//            // Wait till the end of the frame before spawn
//            yield return new WaitForEndOfFrame();

//    Obstacle spawnedObstacle = Instantiate(m_ObstaclePrefab);

//            if (spawnedObstacle)
//            {
//                HalfCircleSettings settings;
//    settings.EdgeSize = 5f;
//                settings.FillAlpha = 0.5f;
//                settings.RotationAlpha = Random.value;

//                spawnedObstacle.InitializeObstacle(100f, 60f, settings);
//            }



//yield return new WaitForSeconds(0.1f - Time.deltaTime);
//        }

}
