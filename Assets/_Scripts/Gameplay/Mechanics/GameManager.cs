using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameManager _instance;
    private MazeRenderer _mazeRenderer;
    private GameObject startTimertrigger;
    private GameObject endGametrigger;

    [SerializeField]
    private float timer = 0f;

    private float timerOriginalValue;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;

        _mazeRenderer = FindObjectOfType<MazeRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {   
        timerOriginalValue = timer;
        _mazeRenderer.buildNewMaze();
        startTimertrigger = _mazeRenderer.startTimerTrigger;
        endGametrigger = _mazeRenderer.endGameTrigger;
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag(nameof(startTimertrigger)))
            StartCoroutine(TimerCoroutine());
        if (other.gameObject.CompareTag(nameof(endGametrigger)))
            ResetMap();
    }   

    private IEnumerator TimerCoroutine()
    {
        while (timer >= 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
    }

    private void ResetMap(){
        _mazeRenderer.buildNewMaze();
        timer = timerOriginalValue * (3 / 4);
        StartCoroutine(TimerCoroutine());
    }
}
