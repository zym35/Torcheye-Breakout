using System;
using UnityEngine;

public class BarControl : MonoBehaviour
{
    private Camera _camera;
    private Rigidbody2D _rigidBody;

    private void Start()
    {
        _camera = Camera.main;
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var temp = _camera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(temp.x, -3.8f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        return;
        if (other.gameObject.CompareTag("Ball"))
        {
            var vOther = other.relativeVelocity;
            var normal = other.GetContact(0).normal;
            other.rigidbody.velocity = Vector2.Reflect(vOther, normal);
        }
    }
}