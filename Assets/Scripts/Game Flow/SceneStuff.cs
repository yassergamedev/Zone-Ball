using System.Collections;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStuff : MonoBehaviour
{
    public GameObject loadingUI;
    public string sceneName;
    public CurrentGame game;
    public bool isLeaving = false;

    private void Start()
    {
        if (loadingUI == null)
        {
            loadingUI = GameObject.FindGameObjectWithTag("Opp");
        }
    }
    private void Update()
    {
     
        
    }
    public void LoadScene()
    {
        if (loadingUI != null)
        {
            loadingUI.SetActive(true);
        }
        // Activate the loading UI
        if (isLeaving)
        {
            if (GameTimer.Instance != null)
            {

                GameTimer.Instance.EndGame();
            }
        }
            // Start loading the scene asynchronously
            StartCoroutine(LoadSceneAsync());

      

     }
    public void setGameAndLoadScene()
    {
       
            FileDataHandler<CurrentGame> currHandler = new(Application.persistentDataPath, "Current Game");
            currHandler.Save(game);
        
        if (GameTimer.Instance != null)
        {
            GameTimer.Instance.StartGame();
        }
        LoadScene();
    }
    IEnumerator LoadSceneAsync()
    {
        // Create an AsyncOperation object to load the scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous operation is done
        while (!asyncOperation.isDone)
        {
            // Calculate the loading progress if needed
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // 0.9 is the progress when the scene is fully loaded

            // Update the loading progress UI if needed

            // Yielding null in a Coroutine waits for the next frame
            yield return null;
        }
        if (loadingUI != null)
            // Deactivate the loading UI once the scene is fully loaded
            loadingUI.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();

    }
 
}
