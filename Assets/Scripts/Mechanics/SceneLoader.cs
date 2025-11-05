using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int targetScene = 2;
    private bool loadingScene;
    

    private void OnTriggerEnter(Collider other)
    {
        if (!loadingScene)
        {
            loadingScene = true;
            StartCoroutine(LoadTargetScene());
        }
    }

    IEnumerator LoadTargetScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(targetScene);

        yield return null;
    }
}
