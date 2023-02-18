using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TR_SpawnManager : MonoBehaviour
{
    // Script component variable
    public PLR_Controller playerController;
    
    // Object variable
    public GameObject player;
    public GameObject[] carPrefabs;
    public GameObject[] pointPrefab;
    public GameObject[] powerupPrefab;

    // 3D UI variable
    public TextMeshPro scoreText;
    public TextMeshPro liveText;

    // 2D UI variable
    public TextMeshProUGUI scoreText02;
    public TextMeshProUGUI highScoreText;
    // Screens
    public GameObject startScreen;
    public GameObject gameOverScreen;
    public GameObject instructionScreen;
    public GameObject gamingScreen;
    public GameObject pausedScreen;
    public GameObject volumeAdjuster;
    // Buttons
    public Button startButton;
    public Button restartButton;
    public Button restartButtonPause;
    public Button pauseButton;
    public Button resumeButton;
    public Button volumeButton;
    public Button instructionButton;
    public Button backButton;
    public Button exitButton;
    public Button exitButtonGameOver;

    // Point variables
    private int shapeOne;
    private int shapeTwo;
    private int shapeThree;
    private GameObject[] shapeOneObj;
    private GameObject[] shapeTwoObj;
    private GameObject[] shapeThreeObj;

    // Powerup variable
    private int speedPowerup;
    private int lifePowerup;
    private int invinciblePowerup;
    private GameObject speedPowerupObj;
    private GameObject heartPowerupObj;
    private GameObject shieldPowerupObj;

    // Basic data type variable
    private float xRange = 14f;
    private float zPos1 = 4f;
    private float zPos2 = 0f;
    private float zPos3 = -4f;
    private float startDelay = 0f;
    private float xRangePoints = 4f;
    private int score = 0;
    public int life = 3;
    private int highScore;
    public bool hasPowerup = false;
    public bool isGameActive = false;
    private Quaternion objectRotation = Quaternion.Euler(-90, 0, 0); // initial rotation of objects
    public Vector3 playerPos;

    // SFX variables
    public GameObject bgMusic;
    public AudioSource soundEffects;
    public AudioClip clickSFX;
    public AudioClip pointSFX;
    public AudioClip powerupUpSFX;
    public AudioClip crashSFX;
    public AudioClip gameOverSFX;
    public AudioClip highScoreSFX;
    public AudioClip dropSFX;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PLR_Controller>();
        volumeButton = GameObject.Find("VolumeButton").GetComponent<Button>();
        soundEffects = GetComponent<AudioSource>();
        
        highScore = PlayerPrefs.GetInt("highScore", 0);
        highScoreText.text = "High score " + highScore.ToString();

        playerPos = player.transform.position;

        startButton.onClick.AddListener(StartGame);
        volumeButton.onClick.AddListener(OpenVolumeAdjuster);
        instructionButton.onClick.AddListener(OpenInfoScreen);
        exitButton.onClick.AddListener(ExitGame);
    }

    // Update is called once per frame
    void Update()
    {
        shapeOne = GameObject.FindGameObjectsWithTag("Shape 1").Length;
        shapeTwo = GameObject.FindGameObjectsWithTag("Shape 2").Length;
        shapeThree = GameObject.FindGameObjectsWithTag("Shape 3").Length;

        shapeOneObj = GameObject.FindGameObjectsWithTag("Shape 1");
        shapeTwoObj = GameObject.FindGameObjectsWithTag("Shape 2");
        shapeThreeObj = GameObject.FindGameObjectsWithTag("Shape 3");

        speedPowerup = GameObject.FindGameObjectsWithTag("Speed Powerup").Length;
        lifePowerup = GameObject.FindGameObjectsWithTag("Lives Powerup").Length;
        invinciblePowerup = GameObject.FindGameObjectsWithTag("Invincible Powerup").Length;

        speedPowerupObj = GameObject.FindGameObjectWithTag("Speed Powerup");
        heartPowerupObj = GameObject.FindGameObjectWithTag("Lives Powerup");
        shieldPowerupObj = GameObject.FindGameObjectWithTag("Invincible Powerup");

        if (isGameActive == true)
        {
            SpawnRandomPowerup();

            if ((shapeOne + shapeTwo + shapeThree) < 3)
            {
                SpawnPoints();
            }
        }

        if (life <= 0 && isGameActive == true)
        {
            isGameActive = false;
            life = 3;
            GameOver();
        }
    }

    /* - After pressing the play button this function will be called.
       - In-game screen or playing screen. */
    public void StartGame()
    {
        soundEffects.clip = clickSFX;
        soundEffects.Play();

        player.transform.position = playerPos;

        isGameActive = true;

        gamingScreen.SetActive(true);
        gameOverScreen.SetActive(false);
        startScreen.SetActive(false);
        volumeButton = GameObject.Find("VolumeButton").GetComponent<Button>();

        UpdateLives(0);
        SpawnRandomPowerup();

        Invoke("SpawnRandomCar", startDelay);

        pauseButton.onClick.AddListener(PauseGame);
        volumeButton.onClick.AddListener(OpenVolumeAdjuster);
        instructionButton.onClick.AddListener(OpenInfoScreen);
        exitButton.onClick.AddListener(ExitGame);
    }

    /* - After losing all lives this function will be called.
       - End screen. */
    public void GameOver()
    {
        bgMusic.SetActive(false);

        playerController.DeathEffect();

        playerController.movementSpeed = 0;

        isGameActive = false;
        gameOverScreen.SetActive(true);
        gamingScreen.SetActive(false);
        volumeButton = GameObject.Find("VolumeButton").GetComponent<Button>();

        scoreText02.text = "Score: " + score;
        restartButton.onClick.AddListener(RestartGame);
        volumeButton.onClick.AddListener(OpenVolumeAdjuster);
        exitButtonGameOver.onClick.AddListener(ExitGame);

        Destroy(speedPowerupObj);
        Destroy(heartPowerupObj);
        Destroy(shieldPowerupObj);

        foreach (GameObject shape1 in shapeOneObj)
        {
            Destroy(shape1);
        }
        foreach (GameObject shape2 in shapeTwoObj)
        {
            Destroy(shape2);
        }
        foreach (GameObject shape3 in shapeThreeObj)
        {
            Destroy(shape3);
        }
        
        if (score > highScore)
        {
            soundEffects.clip = highScoreSFX;
            soundEffects.Play();
            
            highScoreText.text = "High score: " + score.ToString();
            PlayerPrefs.SetInt("highScore", score);
        }
        else
        {
            soundEffects.clip = gameOverSFX;
            soundEffects.Play();
        }
    }

    /* - After pressing the restart button this function will be called.
       - Direct in Start menu. */
    public void RestartGame()
    {
        Time.timeScale = 1f;

        bgMusic.SetActive(true);
        soundEffects.clip = clickSFX;
        soundEffects.Play();

        score = 0;
        scoreText.text = "SCORE  " + score;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        volumeButton = GameObject.Find("VolumeButton").GetComponent<Button>();
        volumeButton.onClick.AddListener(OpenVolumeAdjuster);
    }

    /* - After pressing the pause button this function will be called.
       - Pause menu.
       - Stops the timer of the scene. */
    public void PauseGame()
    {
        Time.timeScale = 0f;

        soundEffects.clip = clickSFX;
        soundEffects.Play();

        pausedScreen.SetActive(true);
        gamingScreen.SetActive(false);
        volumeButton = GameObject.Find("VolumeButton").GetComponent<Button>();

        volumeButton.onClick.AddListener(OpenVolumeAdjuster);
        restartButtonPause.onClick.AddListener(RestartGame);
        resumeButton.onClick.AddListener(ResumeGame);
    }

    /* - After pressing the resume button this function will be called.
       - In-game screen or playing screen. 
       - Resumes the timer of the scene. */
    public void ResumeGame()
    {
        Time.timeScale = 1f;

        soundEffects.clip = clickSFX;
        soundEffects.Play();

        gamingScreen.SetActive(true);
        pausedScreen.SetActive(false);
        volumeButton.onClick.AddListener(OpenVolumeAdjuster);
    }

    /* - After pressing the instruction button this function will be called.
       - Open instruction menu. */
    public void OpenInfoScreen()
    {
        Debug.Log("Open Info Screen");

        soundEffects.clip = clickSFX;
        soundEffects.Play();

        instructionScreen.SetActive(true);
        startScreen.SetActive(false);
        backButton.onClick.AddListener(CloseInfoScreen);
    }

    /* - After pressing the instruction button this function will be called.
       - Close instruction menu. */
    public void CloseInfoScreen()
    {
        Debug.Log("Close Info Screen");

        soundEffects.clip = clickSFX;
        soundEffects.Play();

        startScreen.SetActive(true);
        instructionScreen.SetActive(false);
    }

    /* - After pressing the volume button this function will be called.
       - Open volume adjuster. */
    public void OpenVolumeAdjuster()
    {
        Debug.Log("GUMANA NA VOLUME");

        soundEffects.clip = clickSFX;
        soundEffects.Play();

        volumeAdjuster.SetActive(true);
        volumeButton.onClick.AddListener(CloseVolumeAdjuster);
    }

    /* - After pressing the volume button this function will be called.
       - Close volume adjuster. */
    public void CloseVolumeAdjuster()
    {
        Debug.Log("NASIRA NA VOLUME");

        soundEffects.clip = clickSFX;
        soundEffects.Play();

        volumeAdjuster.SetActive(false);
        volumeButton.onClick.AddListener(OpenVolumeAdjuster);
    }

    /* - After pressing the exit button this function will be called.
       - Close program. */
    public void ExitGame()
    {
        Debug.Log("Exit game");
        Application.Quit();
    }

    /* - This function is invoked/called repeatedly while the game is active.
       - Generate random cars at random fixed position. */
    void SpawnRandomCar()
    {
        if (isGameActive)
        {
            Debug.Log("SPAWN CAR");

            int randomIndex1 = Random.Range(0, carPrefabs.Length);
            int randomIndex2 = Random.Range(0, carPrefabs.Length);
            int randomIndex3 = Random.Range(0, carPrefabs.Length);

            float spawnInterval = Random.Range(0f, 1f);
            Vector3 spawnRotate = new Vector3(-90, 0, 180);

            Vector3 spawnPosLeft1 = new Vector3(-xRange, 0.1f, zPos1);
            Vector3 spawnPosRight = new Vector3(xRange, 0.1f, zPos2);
            Vector3 spawnPosLeft2 = new Vector3(-xRange, 0.1f, zPos3);

            if (spawnInterval < 0.33f)
            {
                Instantiate(carPrefabs[randomIndex1], spawnPosLeft1, carPrefabs[randomIndex1].transform.rotation);
            }
            else if (spawnInterval < 0.66f && spawnInterval >= 0.33f)
            {
                Instantiate(carPrefabs[randomIndex2], spawnPosRight, Quaternion.Euler(spawnRotate));
            }
            else
            {
                Instantiate(carPrefabs[randomIndex3], spawnPosLeft2, carPrefabs[randomIndex3].transform.rotation);
            }

            Invoke("SpawnRandomCar", spawnInterval);
        }
    }

    /* - This function will be called whenever there is only less than 3 points in the scene.
       - Generate random points at random fixed position. */
    void SpawnPoints()
    {
        Vector3 pointSpawnPos = new Vector3((Random.Range(-xRangePoints, xRangePoints)), 8, -14);

        int randomPointPrefab = Random.Range(0, pointPrefab.Length);

        Instantiate(pointPrefab[randomPointPrefab], pointSpawnPos, objectRotation);
    }

    /* - This function will be called whenever the point is dropped to their respective shape basket.
       - Updates the score UI at the scene. */
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "SCORE " + score;
        soundEffects.clip = pointSFX;
        soundEffects.Play();
    }

    /* - This function will be called whenever the player touches/collides with the obstacles/cars or life powerup.
       - Updates the life UI at the scene. */
    public void UpdateLives(int lifeToAdd)
    {
        life += lifeToAdd;
        liveText.text = "LIVES   " + life;
    }

    /* - This function will be called whenever there is no powerup present at the scene.
       - Generate random powerup at random position. */
    public void SpawnRandomPowerup()
    {
        int randomPowerupPrefab = Random.Range(0, powerupPrefab.Length);

        Vector3 powerupSpawnPos = new Vector3((Random.Range(-8, 8)), 0, (Random.Range(-4, 4)));

        if (!hasPowerup && (speedPowerup + lifePowerup + invinciblePowerup) < 1)
        {
            Instantiate(powerupPrefab[randomPowerupPrefab], powerupSpawnPos, powerupPrefab[randomPowerupPrefab].transform.rotation);
        }
    }
}
