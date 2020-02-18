using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    Board gameBoard;
    public Spawner spawnerP1;
    public Spawner spawnerP2;

    SoundManager soundManager;
    ScoreManager scoreManager;
    public HighScore highScore;

    // currently active shape
    Shape activeShapeP1;
    Shape activeShapeP2;
    // ghost shape for active shape
    public Ghost ghostP1;
    public Ghost ghostP2;

    // falling speed of blocks
    public float dropInterval = 0.25f;
    float dropIntervalModded;

    float timeToDropP1;
    float timeToDropP2;

    //// timing base
    //float m_timeToNextKey;
    //[Range(0.02f, 1f)]
    //public float m_keyRepeatRate = 0.25f;

    // left right timing
    float timeToNextKeyLeftRightP1;
    float timeToNextKeyLeftRightP2;
    [Range(0.02f, 1f)]
    public float keyRepeatRateLeftRight = 0.25f;

    // down timing
    float timeToNextKeyDownP1;
    float timeToNextKeyDownP2;
    [Range(0.02f, 1f)]
    public float keyRepeatRateDown = 0.1f;

    // rotation timing
    float timeToNextKeyRotateP1;
    float timeToNextKeyRotateP2;
    [Range(0.02f, 1f)]
    public float keyRepeatRateRotate = 0.25f;

    bool gameOver = false;
    public GameObject gameOverPanel;

    public IconToggle rotationIconToggle;
    bool clockwise = true;

    public GameObject PauseGamePanel;
    public bool isPaused = false;

    public InputField inputField;

    private string[] p1Input = { "MoveRightP1", "MoveLeftP1", "RotateP1", "MoveDownP1", "InstantDropP1" };
    private string[] p2Input = { "MoveRightP2", "MoveLeftP2", "RotateP2", "MoveDownP2", "InstantDropP2" };

    void Start()
    {
        // setting starting values
        timeToNextKeyLeftRightP1 = Time.time + keyRepeatRateLeftRight;
        timeToNextKeyDownP1 = Time.time + keyRepeatRateDown;
        timeToNextKeyRotateP1 = Time.time + keyRepeatRateRotate;
        timeToNextKeyLeftRightP2 = Time.time + keyRepeatRateLeftRight;
        timeToNextKeyDownP2 = Time.time + keyRepeatRateDown;
        timeToNextKeyRotateP2 = Time.time + keyRepeatRateRotate;

        
        // connecting w Board and Spawner
        gameBoard = GameObject.FindObjectOfType<Board>();

        soundManager = GameObject.FindObjectOfType<SoundManager>();
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();


        if (!scoreManager)
        {
            Debug.Log("WARNING! There is no score manager defined!");
        }
        if (!gameBoard)
        {
            Debug.Log("WARNING! There is no game board defined!");
        }
        if (!soundManager)
        {
            Debug.Log("WARNING! There is no sound manager defined!");
        }
        if (!spawnerP1 || !spawnerP2)
        {
            Debug.Log("WARNING! There is no game spawner defined!");
        }
        else
        {
            // if no shapes active, it will spawn new shape
            if (activeShapeP1 == null)
            {
                activeShapeP1 = spawnerP1.SpawnShape();
            }
            if (activeShapeP2 == null)
            {
                activeShapeP2 = spawnerP2.SpawnShape();
            }

            // in case spawner position is not a whole number (integer), it will round it to closest int
            spawnerP1.transform.position = Vector3Int.RoundToInt(spawnerP1.transform.position);
            spawnerP2.transform.position = Vector3Int.RoundToInt(spawnerP2.transform.position);
        }

        if (gameOverPanel)
        {
            gameOverPanel.SetActive(false);
        }
        if (PauseGamePanel)
        {
            PauseGamePanel.SetActive(false);
        }

        dropIntervalModded = dropInterval;



    }

    private void LateUpdate()
    {
        if (ghostP1)
        {
            ghostP1.DrawGhost(activeShapeP1, gameBoard);
        }
        if (ghostP2)
        {
            ghostP2.DrawGhost(activeShapeP2, gameBoard);
        }
    }

    void Update()
    {
        if (!gameBoard || !spawnerP1 || !spawnerP2 || !activeShapeP1 || gameOver || !soundManager || !scoreManager)
        {
            return;
        }
        //// Plater L
        PlayerInput(ref p1Input[0], ref p1Input[1], ref p1Input[2], ref p1Input[3], ref p1Input[4],ref activeShapeP1,ref spawnerP1,ref ghostP1,ref timeToNextKeyLeftRightP1,ref timeToNextKeyDownP1,ref timeToNextKeyRotateP1,ref timeToDropP1);
        //// Player R
        PlayerInput(ref p2Input[0], ref p2Input[1], ref p2Input[2], ref p2Input[3], ref p2Input[4], ref activeShapeP2, ref spawnerP2, ref ghostP2, ref timeToNextKeyLeftRightP2, ref timeToNextKeyDownP2, ref timeToNextKeyRotateP2, ref timeToDropP2);


    }

    private void PlayerInput(ref string moveRightCommand,ref string moveLeftCommand,ref string rotateCommand,ref string moveDownCommand, ref string instantDropCommand,ref Shape activeShape,ref Spawner playerSpawner,ref Ghost ghost,
       ref float timeToNextKeyLeftRight,ref float timeToNextKeyDown,ref float timeToNextKeyRotate,ref float timeToDrop)
    {
        if ((Input.GetButton(moveRightCommand) && (Time.time > timeToNextKeyLeftRight)) || (Input.GetButtonDown(moveRightCommand)))
        {
            activeShape.MoveRight();
            timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;
            if (!gameBoard.IsValidPosition(activeShape))
            {
                activeShape.MoveLeft();
                PlaySound(soundManager.errorSound, 0.5f);
            }
            else
            {
                PlaySound(soundManager.moveSound, 0.3f);
            }

        }
        else if ((Input.GetButton(moveLeftCommand) && (Time.time > timeToNextKeyLeftRight)) || (Input.GetButtonDown(moveLeftCommand)))
        {
            activeShape.MoveLeft();

            timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;

            // fixes invalid pos
            if (!gameBoard.IsValidPosition(activeShape))
            {

                activeShape.MoveRight();
            }

        }
        else if (Input.GetButtonDown(rotateCommand) && (Time.time > timeToNextKeyRotate))
        {
            // m_activeShape.RotateRight();
            activeShape.RotateClockwise(clockwise);
            timeToNextKeyRotate = Time.time + keyRepeatRateRotate;
            if (!gameBoard.IsValidPosition(activeShape))
            {
                activeShape.RotateClockwise(!clockwise);
                // m_activeShape.RotateLeft();
            }
            else
            {
                PlaySound(soundManager.moveSound, 0.3f);
            }

        }
        else if ((Input.GetButton(moveDownCommand) && (Time.time > timeToNextKeyDown)) || (Time.time > timeToDrop))
        {
            timeToDrop = Time.time + dropIntervalModded;
            timeToNextKeyDown = Time.time + keyRepeatRateDown;
            activeShape.MoveDown();

            if (!gameBoard.IsValidPosition(activeShape))
            {
                if (gameBoard.IsOverLimit(activeShape))
                {
                    GameOver(ref activeShape);
                }
                else
                {
                    ShapeLanded(ref activeShape,ref ghost,ref playerSpawner,ref timeToNextKeyLeftRight,ref timeToNextKeyDown,ref timeToNextKeyRotate);
                }
            }
        }
        else if (Input.GetButtonDown("ToggleRotation"))
        {
            ToggleRotationDirection();
        }
        else if (Input.GetButtonDown("PauseGame"))
        {
            TogglePauseGame();
        }
        else if (Input.GetButtonDown(instantDropCommand))
        {
            activeShape.transform.position = ghost.GhostShapePosition();
        }


    }

    private void PlaySound(AudioClip clip, float volumeMultiplier = 1f)
    {
        if (soundManager.fxEnabled)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(soundManager.fxVolume * volumeMultiplier, 0.05f, 1f));
        }
    }

    private void GameOver(ref Shape activeShape)
    {
        activeShape.MoveUp();
        gameOver = true;
        Debug.LogWarning(activeShapeP1.name + " is over the limit");
        if (gameOverPanel)
        {
            gameOverPanel.SetActive(true);
        }

        PlaySound(soundManager.gameOverSound, 5f);
        PlaySound(soundManager.gameOverVocal, 5f);
    }

    private void ShapeLanded(ref Shape activeShape, ref Ghost ghost,ref Spawner spawner,ref float timeToNextKeyLeftRight,ref float timeToNextKeyDown, ref float timeToNextKeyRotate)
    {
        activeShape.MoveUp();
        gameBoard.StoreShapeInGrid(activeShape);
        if (ghost)
        {
            ghost.Reset();
        }
        activeShape = spawner.SpawnShape();
        // resetting timings for input
        timeToNextKeyLeftRight = Time.time;
        timeToNextKeyDown = Time.time;
        timeToNextKeyRotate = Time.time;

        gameBoard.StartCoroutine("ClearAllRows");

        PlaySound(soundManager.dropSound, 0.75f);

        if (gameBoard.m_completedRows > 0)
        {
            scoreManager.ScoreLines(gameBoard.m_completedRows);
            if (gameBoard.m_completedRows > 1)
            {
                AudioClip randomVocal = soundManager.GetRandomClip(soundManager.vocalClips);
                PlaySound(randomVocal);
            }

            if (scoreManager.didLevelUp)
            {
                PlaySound(soundManager.levelUpClip);
                dropIntervalModded = Mathf.Clamp(dropInterval - (((float)scoreManager.level - 1f) * 0.1f), 0.05f, 1f);
            }

            PlaySound(soundManager.clearRowSound, 1f);
        }

    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToggleRotationDirection()
    {
        clockwise = !clockwise;
        if (rotationIconToggle)
        {
            rotationIconToggle.ToggleIcon(clockwise);
        }
    }

    public void TogglePauseGame()
    {
        if (gameOver)
        {
            return;
        }

        isPaused = !isPaused;
        if (PauseGamePanel)
        {
            PauseGamePanel.SetActive(isPaused);
            if (soundManager)
            {
                soundManager.musicSource.volume = (isPaused) ? soundManager.musicVolume * 0.25f : soundManager.musicVolume;
            }

            Time.timeScale = (isPaused) ? 0 : 1;
        }
    }

    public void SaveScore()
    {      
        highScore.AddPlayerScore(inputField.text, scoreManager.GetScore());
        inputField.gameObject.SetActive(false);   
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
