using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private LineRenderer ballPreviewLineRenderer;
    [SerializeField] private TextMeshProUGUI scoreDisplay;

    private List<GameObject> _brickPool;
    private int _score;
    
    public static bool InLaunchPrep { get; set; }
    public static GameManager Gm { get; private set; }

    private void Awake()
    {
        //Singleton pattern
        if (Gm != null && Gm != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Gm = this;
        }
    }

    private void Start()
    {
        InLaunchPrep = true;
        InstantiateBlocks();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        scoreDisplay.text = _score.ToString();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void InstantiateBlocks()
    {
        _brickPool = new List<GameObject>();

        GameObject temp;
        for (var i = -4; i <= 4; i++)
        {
            for (var j = -5; j <= 5; j++)
            {
                temp = Instantiate(brickPrefab, new Vector2((float) i / 2, (float) j / 2), Quaternion.identity);
                //temp.SetActive(false);
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
        ballPreviewLineRenderer.SetPosition(0, ballPos);
        ballPreviewLineRenderer.SetPosition(1, targetPos);
    }

    public void AddScore(int num)
    {
        _score += num;
    }
}