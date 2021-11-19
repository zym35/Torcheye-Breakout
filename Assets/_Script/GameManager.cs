using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private LineRenderer ballPreviewLineRenderer;
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    [SerializeField] [Range(0, 20)] private float ballPreviewLength;

    public static GameManager Instance { get; private set; }
    public bool InLaunchPrep { get; set; }
    public Camera MainCam { get; private set; }


    private List<GameObject> _brickPool;
    private int _score;

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
        for (var i = -10; i <= 10; i++)
        {
            for (var j = -10; j <= 10; j++)
            {
                temp = Instantiate(brickPrefab, new Vector2((float) i / 5, (float) j / 4), Quaternion.identity);
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
    }
}