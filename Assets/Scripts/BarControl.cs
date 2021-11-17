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
        if (!GameManager.InLaunchPrep && Input.GetMouseButton(0))
        {
            var temp = _camera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(temp.x, -3.8f);
        }
    }
}