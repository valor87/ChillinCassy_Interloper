using UnityEngine;
using UnityEngine.SceneManagement;

public class changeScenesTrigger : MonoBehaviour
{
    public string sceneName = "Put the scene name here";

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(sceneName);
    }
}
