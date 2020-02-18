using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GC_Backup : MonoBehaviour
{
    Board gameBoard;
    public Spawner spawnerP1;
    public Spawner spawnerP2;

    SoundManager soundManager;
    ScoreManager scoreManager;

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
    //float timeToNextKey;
    //[Range(0.02f, 1f)]
    //public float keyRepeatRate = 0.25f;

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
        //   m_spawnerP1 = GameObject.FindObjectOfType<Spawner>();
        // m_spawnerP2 = GameObject.FindObjectOfType<Spawner>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        // m_ghostP1 = GameObject.FindObjectOfType<Ghost>();
        // m_ghostP2 = GameObject.FindObjectOfType<Ghost>();

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

        PlayerInputP1();
        PlayerInputP2();


    }

    //private void PlayerInput(string moveRightCommand, string moveLeftCommand, string rotateCommand, string moveDownCommand, string instantDropCommand, Shape activeShape, Spawner playerSpawner, Ghost ghost)
    //{
    //    if ((Input.GetButton(moveRightCommand) && (Time.time > m_timeToNextKeyLeftRight)) || (Input.GetButtonDown(moveRightCommand)))
    //    {
    //        activeShape.MoveRight();
    //        m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
    //        if (!m_gameBoard.IsValidPosition(activeShape))
    //        {
    //            activeShape.MoveLeft();
    //            PlaySound(m_soundManager.m_errorSound, 0.5f);
    //        }
    //        else
    //        {
    //            PlaySound(m_soundManager.m_moveSound, 0.3f);
    //        }

    //    }
    //    else if ((Input.GetButton(moveLeftCommand) && (Time.time > m_timeToNextKeyLeftRight)) || (Input.GetButtonDown(moveLeftCommand)))
    //    {
    //        activeShape.MoveLeft();

    //        m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

    //        // fixes invalid pos
    //        if (!m_gameBoard.IsValidPosition(activeShape))
    //        {

    //            activeShape.MoveRight();
    //        }

    //    }
    //    else if (Input.GetButtonDown(rotateCommand) && (Time.time > m_timeToNextKeyRotate))
    //    {
    //        // m_activeShape.RotateRight();
    //        activeShape.RotateClockwise(m_clockwise);
    //        m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;
    //        if (!m_gameBoard.IsValidPosition(activeShape))
    //        {
    //            activeShape.RotateClockwise(!m_clockwise);
    //            // m_activeShape.RotateLeft();
    //        }
    //        else
    //        {
    //            PlaySound(m_soundManager.m_moveSound, 0.3f);
    //        }

    //    }
    //    else if ((Input.GetButton(moveDownCommand) && (Time.time > m_timeToNextKeyDown)) || (Time.time > m_timeToDrop))
    //    {
    //        m_timeToDrop = Time.time + m_dropIntervalModded;
    //        m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;
    //        activeShape.MoveDown();

    //        if (!m_gameBoard.IsValidPosition(activeShape))
    //        {
    //            if (m_gameBoard.IsOverLimit(activeShape))
    //            {
    //                GameOver(activeShape);
    //            }
    //            else
    //            {
    //                ShapeLanded(playerSpawner, activeShape, ghost);
    //            }
    //        }
    //    }
    //    else if (Input.GetButtonDown("ToggleRotation"))
    //    {
    //        ToggleRotationDirection();
    //    }
    //    else if (Input.GetButtonDown("PauseGame"))
    //    {
    //        TogglePauseGame();
    //    }
    //    else if (Input.GetButtonDown(instantDropCommand))
    //    {
    //        activeShape.transform.position = ghost.GhostShapePosition();
    //    }


    //}

    private void PlayerInputP1()
    {
        if ((Input.GetButton("MoveRightP1") && (Time.time > timeToNextKeyLeftRightP1)) || (Input.GetButtonDown("MoveRightP1")))
        {
            activeShapeP1.MoveRight();
            timeToNextKeyLeftRightP1 = Time.time + keyRepeatRateLeftRight;
            if (!gameBoard.IsValidPosition(activeShapeP1))
            {
                activeShapeP1.MoveLeft();
                PlaySound(soundManager.errorSound, 0.5f);
            }
            else
            {
                PlaySound(soundManager.moveSound, 0.3f);
            }

        }
        else if ((Input.GetButton("MoveLeftP1") && (Time.time > timeToNextKeyLeftRightP1)) || (Input.GetButtonDown("MoveLeftP1")))
        {
            activeShapeP1.MoveLeft();

            timeToNextKeyLeftRightP1 = Time.time + keyRepeatRateLeftRight;

            // fixes invalid pos
            if (!gameBoard.IsValidPosition(activeShapeP1))
            {

                activeShapeP1.MoveRight();
            }

        }
        else if (Input.GetButtonDown("RotateP1") && (Time.time > timeToNextKeyRotateP1))
        {
            // m_activeShape.RotateRight();
            activeShapeP1.RotateClockwise(clockwise);
            timeToNextKeyRotateP1 = Time.time + keyRepeatRateRotate;
            if (!gameBoard.IsValidPosition(activeShapeP1))
            {
                activeShapeP1.RotateClockwise(!clockwise);
                // m_activeShape.RotateLeft();
            }
            else
            {
                PlaySound(soundManager.moveSound, 0.3f);
            }

        }
        else if ((Input.GetButton("MoveDownP1") && (Time.time > timeToNextKeyDownP1)) || (Time.time > timeToDropP1))
        {
            timeToDropP1 = Time.time + dropIntervalModded;
            timeToNextKeyDownP1 = Time.time + keyRepeatRateDown;
            activeShapeP1.MoveDown();

            if (!gameBoard.IsValidPosition(activeShapeP1))
            {
                if (gameBoard.IsOverLimit(activeShapeP1))
                {
                    GameOverP1();
                }
                else
                {
                    ShapeLandedP1();
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
        else if (Input.GetButtonDown("InstantDropP1"))
        {
            activeShapeP1.transform.position = ghostP1.GhostShapePosition();
        }


    }

    private void PlayerInputP2()
    {
        if ((Input.GetButton("MoveRightP2") && (Time.time > timeToNextKeyLeftRightP2)) || (Input.GetButtonDown("MoveRightP2")))
        {
            activeShapeP2.MoveRight();
            timeToNextKeyLeftRightP2 = Time.time + keyRepeatRateLeftRight;
            if (!gameBoard.IsValidPosition(activeShapeP2))
            {
                activeShapeP2.MoveLeft();
                PlaySound(soundManager.errorSound, 0.5f);
            }
            else
            {
                PlaySound(soundManager.moveSound, 0.3f);
            }

        }
        else if ((Input.GetButton("MoveLeftP2") && (Time.time > timeToNextKeyLeftRightP2)) || (Input.GetButtonDown("MoveLeftP2")))
        {
            activeShapeP2.MoveLeft();

            timeToNextKeyLeftRightP2 = Time.time + keyRepeatRateLeftRight;

            // fixes invalid pos
            if (!gameBoard.IsValidPosition(activeShapeP2))
            {

                activeShapeP2.MoveRight();
            }

        }
        else if (Input.GetButtonDown("RotateP2") && (Time.time > timeToNextKeyRotateP1))
        {
            // m_activeShape.RotateRight();
            activeShapeP2.RotateClockwise(clockwise);
            timeToNextKeyRotateP2 = Time.time + keyRepeatRateRotate;
            if (!gameBoard.IsValidPosition(activeShapeP1))
            {
                activeShapeP2.RotateClockwise(!clockwise);
                // m_activeShape.RotateLeft();
            }
            else
            {
                PlaySound(soundManager.moveSound, 0.3f);
            }

        }
        else if ((Input.GetButton("MoveDownP2") && (Time.time > timeToNextKeyDownP2)) || (Time.time > timeToDropP2))
        {
            timeToDropP2 = Time.time + dropIntervalModded;
            timeToNextKeyDownP2 = Time.time + keyRepeatRateDown;
            activeShapeP2.MoveDown();

            if (!gameBoard.IsValidPosition(activeShapeP2))
            {
                if (gameBoard.IsOverLimit(activeShapeP2))
                {
                    GameOverP2();
                }
                else
                {
                    ShapeLandedP2();
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
        else if (Input.GetButtonDown("InstantDropP2"))
        {
            activeShapeP2.transform.position = ghostP2.GhostShapePosition();
        }


    }

    private void PlaySound(AudioClip clip, float volumeMultiplier = 1f)
    {
        if (soundManager.fxEnabled)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(soundManager.fxVolume * volumeMultiplier, 0.05f, 1f));
        }
    }



    private void GameOverP1()
    {
        activeShapeP1.MoveUp();
        gameOver = true;
        Debug.LogWarning(activeShapeP1.name + " is over the limit");
        if (gameOverPanel)
        {
            gameOverPanel.SetActive(true);
        }

        PlaySound(soundManager.gameOverSound, 5f);
        PlaySound(soundManager.gameOverVocal, 5f);
    }

    private void GameOverP2()
    {
        activeShapeP2.MoveUp();
        gameOver = true;
        Debug.LogWarning(activeShapeP2.name + " is over the limit");
        if (gameOverPanel)
        {
            gameOverPanel.SetActive(true);
        }

        PlaySound(soundManager.gameOverSound, 5f);
        PlaySound(soundManager.gameOverVocal, 5f);
    }

    private void ShapeLandedP1()
    {
        activeShapeP1.MoveUp();
        gameBoard.StoreShapeInGrid(activeShapeP1);
        if (ghostP1)
        {
            ghostP1.Reset();
        }
        activeShapeP1 = spawnerP1.SpawnShape();
        // resetting timings for input
        timeToNextKeyLeftRightP1 = Time.time;
        timeToNextKeyDownP2 = Time.time;
        timeToNextKeyRotateP1 = Time.time;

        gameBoard.ClearAllRows();

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

    private void ShapeLandedP2()
    {
        Debug.Log("Landed2");
        activeShapeP2.MoveUp();
        gameBoard.StoreShapeInGrid(activeShapeP2);
        if (ghostP2)
        {
            ghostP2.Reset();
        }
        activeShapeP2 = spawnerP2.SpawnShape();
        // resetting timings for input
        timeToNextKeyLeftRightP2 = Time.time;
        timeToNextKeyDownP2 = Time.time;
        timeToNextKeyRotateP2 = Time.time;

        gameBoard.ClearAllRows();

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
}
