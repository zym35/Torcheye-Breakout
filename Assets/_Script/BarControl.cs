using System;
using UnityEngine;

public class BarControl : MonoBehaviour
{
    private AreaEffector2D forceField;
    //private 

    private void Start()
    {
        forceField = GetComponentInChildren<AreaEffector2D>();
    }

    private void Update()
    {
        if (!GameManager.Instance.InLaunchPrep && Input.GetMouseButton(0))
        {
            var temp = InputManager.GetMouseWorldPosition();
            transform.position = new Vector2(temp.x, -3.8f);
        }
    }
}