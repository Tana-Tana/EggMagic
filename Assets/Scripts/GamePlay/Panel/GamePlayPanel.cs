using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayPanel : MonoBehaviour
{
    public void Replay()
    {
        SceneManager.LoadScene(0);
    }
}
