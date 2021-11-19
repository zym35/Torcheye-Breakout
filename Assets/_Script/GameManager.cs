using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private int autoSaveTime;
    [SerializeField] private Transform[] spawnAnchors;
    [SerializeField] private int amountInaRow, amountInaCol;
    
    public static GameManager Instance { get; private set; }
    public bool InLaunchPrep { get; set; }
    public Camera MainCam { get; private set; }

    private List<GameObject> _brickPool;
    private int _score;
    private Guid _gameGuid;
    private int _countdown;

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
        _gameGuid = Guid.NewGuid();
        _countdown = 60;
        Time.timeScale = 1;

        InstantiateBlocks();
        UIManager.Instance.DisplayHighScore();
        StartCoroutine(AutoSave(autoSaveTime));
        StartCoroutine(CountDown());
    }

    public void Restart()
    {
        _gameGuid = Guid.NewGuid();
        SceneManager.LoadScene(0);
    }

    public void Fail()
    {
        UIManager.Instance.Fail();
        Time.timeScale = 0;
        StopCoroutine(nameof(AutoSave));
        StopCoroutine(nameof(CountDown));
    }

    private void InstantiateBlocks()
    {
        _brickPool = new List<GameObject>();
        var x1 = spawnAnchors[0].position.x;
        var y1 = spawnAnchors[0].position.y;
        var x2 = spawnAnchors[1].position.x;
        var y2 = spawnAnchors[1].position.y;

        // ReSharper disable once TooWideLocalVariableScope
        GameObject temp;
        for (var i = 0; i < amountInaRow; i++)
        {
            for (var j = 0; j < amountInaCol; j++)
            {
                var pos = new Vector2(x1 + i * (x2 - x1) / (amountInaRow - 1), y1 + j * (y2 - y1) / (amountInaCol - 1));
                temp = Instantiate(brickPrefab, pos, Quaternion.identity);
                _brickPool.Add(temp);
            }
        }
    }

    public void AddScore(int num)
    {
        _score += num;
        UIManager.Instance.DisplayScore(_score);
    }

    private void Save(string n)
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

    private IEnumerator CountDown()
    {
        UIManager.Instance.DisplayTimer(_countdown);
        while (true)
        {
            yield return new WaitForSeconds(1);
            _countdown--;
            UIManager.Instance.DisplayTimer(_countdown);
            if (_countdown == 0)
            {
                Save("zym");
                Fail();
            }
        }
    }
}