using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField]
    private float timer = 60f;
    [SerializeField]
    private MazeRenderer _mazeRenderer;
    [SerializeField]
    private TextMeshProUGUI TimerText;
    [SerializeField]
    private TextMeshProUGUI EndGameText;

    private GameObject startTimertrigger;
    private GameObject endGametrigger;
    private GameObject player;

    private int roundCounter = 0;
    private float timerOriginalValue;
    private Coroutine timerCoroutine;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;

        timerOriginalValue = timer;
        TimerText.text = "Time Left: " + timer.ToString("F0");
        EndGameText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            _mazeRenderer = FindObjectOfType<MazeRenderer>();
            player = FindObjectOfType<CapsuleCollider>().gameObject;
            if (_mazeRenderer != null)
                StartMaze();
        }
    }

    private void StartMaze()
    {
        AssignTriggers();
        AttachInstanceToPlayer();
        roundCounter++;
        InitializeMaze();
        timer = timerOriginalValue;
        TimerText.text = "Time Left: " + timer.ToString("F0");
    }

    private void AssignTriggers()
    {
        startTimertrigger = _mazeRenderer.startTimerTrigger;
        endGametrigger = _mazeRenderer.endGameTrigger;
    }

    private void AttachInstanceToPlayer()
    {
        gameObject.transform.parent = player.transform;
        gameObject.transform.localPosition = Vector3.zero;
    }

    private void InitializeMaze()
    {
        if (roundCounter > 3)
            RunEndSequence(false);
        else
            _mazeRenderer.buildNewMaze();
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("startTimerTrigger"))
        {
            if (timerCoroutine != null)
                StopCoroutine(timerCoroutine);

            timerCoroutine = StartCoroutine(TimerCoroutine());
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("endGameTrigger"))
        {
            StopCoroutine(timerCoroutine);
            RunEndSequence(true);
        }
    }
    private IEnumerator TimerCoroutine()
    {
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            TimerText.text = "Time Left: " + timer.ToString("F0");
            yield return null;
        }
        ResetMap();
    }

    private void ResetMap()
    {
        roundCounter++;
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        if (roundCounter > 3)
        {
            RunEndSequence(false);
            return;
        }

        float decrementVal = Random.Range(0.66f, 0.9f);
        timerOriginalValue *= decrementVal;
        timer = timerOriginalValue;
        TimerText.text = "Time Left: " + timer.ToString("F0");

        InitializeMaze();
        timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    private void RunEndSequence(bool IsWon)
    {
        Time.timeScale = 0f;
        if (IsWon)
        {
            EndGameText.color = Color.green;
            EndGameText.text = "You Escaped!";
        }
        else
        {
            EndGameText.color = Color.red;
            EndGameText.text = "You Are Doomed!";
        }
        EndGameText.gameObject.SetActive(true);
    }
}
