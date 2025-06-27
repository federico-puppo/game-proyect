using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneVisitMarker : MonoBehaviour
{
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        GameManager.Instance.MarkLocationVisited(sceneName);
    }
}
