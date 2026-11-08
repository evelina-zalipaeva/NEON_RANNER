using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Панели интерфейса")]
    public GameObject mainMenuPanel;
    public GameObject hudPanel;
    public GameObject gameOverPanel;

    [Header("Элементы экрана HUD")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI dailyProgressText;
    public TextMeshProUGUI coinText;

    [Header("Элементы экрана Game Over")]
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI gameOverCoinsText;

    [Header("Объекты игры")]
    public GameObject player;

    private float startZ;
    private float distanceTraveled;
    private int coinsCollected = 0;

    void Start()
    {
        ShowMainMenu();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && hudPanel != null && hudPanel.activeSelf)
        {
            ShowGameOver();
        }

        if (player != null && player.activeSelf && Time.timeScale > 0)
        {
            distanceTraveled = Mathf.Max(0, player.transform.position.z - startZ);

            if (scoreText != null)
            {
                scoreText.text = "Score: " + Mathf.FloorToInt(distanceTraveled).ToString();
            }
        }
    }

    // ВОТ ТА САМАЯ ФУНКЦИЯ, КОТОРОЙ НЕ ХВАТАЛО
    public void AddCoin(int amount)
    {
        coinsCollected += amount;
        if (coinText != null)
        {
            coinText.text = "Coin: " + coinsCollected.ToString();
        }
    }

    public void StartGame()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (hudPanel != null) hudPanel.SetActive(true);

        coinsCollected = 0;
        if (coinText != null) coinText.text = "Coin: 0";

        if (player != null)
        {
            player.SetActive(true);
            startZ = player.transform.position.z;
        }

        Time.timeScale = 1f;
    }

    public void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (hudPanel != null) hudPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (player != null) player.SetActive(false);
        Time.timeScale = 0f;
    }

    public void ShowGameOver()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (hudPanel != null) hudPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (player != null) player.SetActive(false);

        if (distanceText != null)
            distanceText.text = "Distance: " + Mathf.FloorToInt(distanceTraveled).ToString() + " м";

        if (gameOverCoinsText != null)
            gameOverCoinsText.text = "Coin: " + coinsCollected.ToString();

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // снимаем паузу, иначе новая сцена приедет замороженной
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}