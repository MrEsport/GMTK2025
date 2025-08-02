using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void OnPlayButtonInput()
    {
        SceneManager.Instance.LoadGame();
    }
}
