using NaughtyAttributes;
using UnityEngine;

using Manager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;
    public static SceneManager Instance { get => instance; }

    [SerializeField, Scene] string MenuScene;
    [SerializeField, Scene] string GameScene;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadMenu()
    {
        Manager.LoadScene(MenuScene);
    }

    public void LoadGame()
    {
        Manager.LoadScene(GameScene);
    }
}
