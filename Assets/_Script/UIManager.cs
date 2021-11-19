using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private LineRenderer ballPreviewLineRenderer;
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    [SerializeField] private TextMeshProUGUI highScoreDisplay;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private GameObject fail;
    [SerializeField] [Range(0, 20)] private float ballPreviewLength;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
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
        fail.SetActive(false);
    }

    public void Fail()
    {
        fail.SetActive(true);
    }

    public void DisplayTimer(int time)
    {
        timer.text = time.ToString();
    }

    public void DisplayScore(int score)
    {
        scoreDisplay.text = score.ToString();
    }

    public void DisplayHighScore()
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
}