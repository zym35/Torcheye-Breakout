using UnityEngine;
using MoreMountains.NiceVibrations;

public class Ball : MonoBehaviour
{
    [SerializeField][Range(0, 10)] private float speed;

    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameManager.Instance.InLaunchPrep)
        {
            LaunchPrep();
        }
    }

    private void LaunchPrep()
    {
        var mousePos = InputManager.GetMouseWorldPosition();
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.SetBallPreview(true);
        }

        if (Input.GetMouseButton(0))
        {
            GameManager.Instance.DrawBallPreview(transform.position, mousePos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            var dir = (mousePos - (Vector2) transform.position).normalized;
            _rigidbody.velocity = dir * speed;
            GameManager.Instance.SetBallPreview(false);
            GameManager.Instance.InLaunchPrep = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Brick"))
        {
            GameManager.Instance.AddScore(1);
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Bar"))
        {
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
            AudioManager.Instance.PlaySFX("barBounce");
        }
    }
}