using UnityEngine;

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
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Gm.SetBallPreview(true);
        }

        if (Input.GetMouseButton(0))
        {
            var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            GameManager.Gm.DrawBallPreview(transform.position, mousePos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _rigidbody.AddForce((Input.mousePosition - transform.position).normalized * speed);
            GameManager.Gm.SetBallPreview(false);
            GameManager.InLaunchPrep = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Brick"))
        {
            other.gameObject.SetActive(false);
        }
    }
}