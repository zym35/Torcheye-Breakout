using UnityEngine;
using MoreMountains.NiceVibrations;

public class Ball : MonoBehaviour
{
    [SerializeField][Range(50, 500)] private float speed;

    private Rigidbody2D _rigidbody;
    
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameManager.InLaunchPrep)
        {
            LaunchPrep();
        }
    }

    private void LaunchPrep()
    {
        var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Gm.SetBallPreview(true);
        }

        if (Input.GetMouseButton(0))
        {
            GameManager.Gm.DrawBallPreview(transform.position, mousePos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            var dir = mousePos - transform.position;
            //dir.Normalize();
            _rigidbody.AddForce(dir * speed);
            GameManager.Gm.SetBallPreview(false);
            GameManager.InLaunchPrep = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // if (other.gameObject.CompareTag("Brick"))
        // {
        //     var vOther = other.relativeVelocity;
        //     var normal = other.GetContact(0).normal;
        //     other.rigidbody.velocity = Vector2.Reflect(vOther, normal);
        //     other.gameObject.SetActive(false);
        // }
        if (other.gameObject.CompareTag("Brick"))
        {
            GameManager.Gm.AddScore(1);
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
            other.gameObject.SetActive(false);
        }
    }
}