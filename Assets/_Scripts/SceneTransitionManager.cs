using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{

    public FadeScreen fadeScreen; // Reference to the FadeScreen component

    public void GoToScene(int sceneIndex)
    {
        StartCoroutine(GoToSceneRoutine(sceneIndex));
    }

    IEnumerator GoToSceneRoutine(int sceneIndex)
    {        
        // Start fading out
        fadeScreen.FadeOut();
        // Wait for the fade duration to complete
        yield return new WaitForSeconds(fadeScreen.fadeDuration);
        // Load the new scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }

    public void GoToSceneAsync(int sceneIndex)
    {
        StartCoroutine(GoToSceneRoutineAsync(sceneIndex));
    }

    IEnumerator GoToSceneRoutineAsync(int sceneIndex)
    {        
        // Start fading out
        fadeScreen.FadeOut();
        // Wait for the fade duration to complete
        // Load the new scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false; // Prevent the scene from activating immediately

        float timer = 0f;
        while (timer < fadeScreen.fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}
