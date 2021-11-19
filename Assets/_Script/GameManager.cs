using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private LineRenderer ballPreviewLineRenderer;
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    [SerializeField] [Range(0, 20)] private float ballPreviewLength;
    [SerializeField] private TextMeshProUGUI highScoreDisplay;
    [SerializeField] private int autoSaveTime;

    public static GameManager Instance { get; private set; }
    public bool InLaunchPrep { get; set; }
    public Camera MainCam { get; private set; }

    private List<GameObject> _brickPool;

    private int _score;
    //private bool _gameEnd;
    private Guid _gameGuid;

    private void Awake()
    {
        //Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        InLaunchPrep = true;
        MainCam = Camera.main;
        _score = 0;
        //_gameEnd = false;
        _gameGuid = Guid.NewGuid();

        InstantiateBlocks();
        DisplayHighScore();
        StartCoroutine(AutoSave(autoSaveTime));
    }

    public void Restart()
    {
        _gameGuid = Guid.NewGuid();
        SceneManager.LoadScene(0);
    }

    private void DisplayHighScore()
    {
        var data = SaveSystem.Instance.Load();
        if (data == null || data.Count == 0)
        {
            highScoreDisplay.text = "-----HighScore-----\n none";
            return;
        }

        var text = "-----HighScore-----\n";
        foreach (var pd in data)
        {
            text += $" [{pd.GetName()}]-{pd.GetScore()}  ";
        }

        highScoreDisplay.text = text;
    }

    private void InstantiateBlocks()
    {
        _brickPool = new List<GameObject>();

        GameObject temp;
        for (var i = -10; i <= 10; i++)
        {
            for (var j = -10; j <= 10; j++)
            {
                temp = Instantiate(brickPrefab, new Vector2((float) i / 5, (float) j / 4), Quaternion.identity);
                _brickPool.Add(temp);
            }
        }
    }

    public void SetBallPreview(bool active)
    {
        ballPreviewLineRenderer.enabled = active;
    }

    public void DrawBallPreview(Vector2 ballPos, Vector2 targetPos)
    {
        var dir = (targetPos - ballPos).normalized;

        var hit = Physics2D.Raycast(ballPos, dir, ballPreviewLength);
        Vector2 dest;
        if (hit.collider == null)
        {
            dest = dir * ballPreviewLength + ballPos;
        }
        else
        {
            dest = hit.point;
        }

        ballPreviewLineRenderer.SetPosition(0, ballPos);
        ballPreviewLineRenderer.SetPosition(1, dest);
    }

    public void AddScore(int num)
    {
        _score += num;
        scoreDisplay.text = _score.ToString();
    }

    public void Fail()
    {
        //_gameEnd = true;
    }

    public void Save(string n)
    {
        var data = new PlayerData(_score, n, _gameGuid);
        SaveSystem.Instance.Save(data);
    }

    private IEnumerator AutoSave(int time)
    {
        if (time < 1) time = 1;
        while (true)
        {
            yield return new WaitForSeconds(time);
            Save("zym");
        }
    }
}