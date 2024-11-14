using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayPanel : Panel
{
    [SerializeField] private TextMeshProUGUI levelMaxText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreBestText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private Image countTimeImage;
    [SerializeField] private float fillTime;

    
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        if(countTimeImage.fillAmount > 0)
        {
            Debug.Log(countTimeImage.fillAmount);
            countTimeImage.fillAmount -= Time.deltaTime * fillTime;
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene(0);
    }

    public void BackHome()
    {

    }

    public void TurnOnAchievement()
    {

    }

    public void Pause()
    {

    }

    public void ShowTutorial()
    {

    }
}
