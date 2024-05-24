using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public TMP_Text killCount;
    public TMP_Text rocketCount;
    private int score;
    private int realRocketCount;
    private PlayerController playerController;

    public bool gameOver = false;
    public TextMeshProUGUI gameOverText;
    public TMP_Text endScore;
    public Button restartButton;
    public Button menuButton;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        killCount.text = "Robots Reaped: " + score;
        
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        realRocketCount = playerController.rocketCount;
        rocketCount.text = "Rockets: " + realRocketCount;
    }

    public void GameOver()
    {
        endScore.text = killCount.text;
        endScore.gameObject.SetActive(true);
        killCount.gameObject.SetActive(false);
        rocketCount.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
        gameOver = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Title Screen");
    }

    public void AddScore()
    {
        score += 1;
        killCount.text = "Robots Reaped: " + score;
    }

}
