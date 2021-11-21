using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Aura : MonoBehaviour
{
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private AreaEffector2D field;
    [SerializeField] private float pointSpace;
    [SerializeField][Range(.1f, 1.3f)] private float waveRadius;

    private bool _pressed;
    private Vector2 _startPos, _endPos;
    private List<GameObject> _pointPool;
    private Bounds _bounds;

    private void Start()
    {
        _bounds = GetComponent<Collider2D>().bounds;
        _pointPool = new List<GameObject>();

        GameObject temp;
        for (var i = _bounds.min.x; i < _bounds.max.x; i += pointSpace)
        {
            for (var j = _bounds.min.y; j < _bounds.max.y; j += pointSpace)
            {
                var pos = new Vector2(i, j);
                temp = Instantiate(pointPrefab, pos, Quaternion.identity);
                temp.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .1f);
                _pointPool.Add(temp);
            }
        }
    }

    private void Update()
    {
        MouseDetect();
    }

    private void MouseDetect()
    {
        if (Input.GetMouseButtonDown(0) && !_pressed)
        {
            _pressed = true;
            _startPos = InputManager.GetMouseWorldPosition();
        }

        if (Input.GetMouseButtonUp(0) && _pressed)
        {
            _pressed = false;
            _endPos = InputManager.GetMouseWorldPosition();
            StartCoroutine(Fire());
        }
    }

    private IEnumerator Fire()
    {
        field.forceAngle = Vector2.Angle(_endPos - _startPos, Vector2.right);
        field.forceMagnitude = 50;
        StartCoroutine(ShowWave(_endPos - _startPos));

        yield return new WaitForSeconds(.5f);

        field.forceMagnitude = 0;
    }

    private IEnumerator ShowWave(Vector2 dir)
    {
        var outerRadius = _bounds.extents.magnitude + waveRadius;
        var beginPos = (Vector2) _bounds.center - dir.normalized * outerRadius;
        var endPos = (Vector2) _bounds.center + dir.normalized * outerRadius;

        for (var i = 0; i < 25; i++)
        {
            var timePos = Vector2.Lerp(beginPos, endPos, (float) i / 25);
            foreach (var p in _pointPool)
            {
                var pointVector = (Vector2) p.transform.position - timePos;
                var dist = Vector3.Project(pointVector, dir).magnitude;
                var a = 1.1f - dist / waveRadius;
                a = Mathf.Clamp(a, .1f, 1);
                p.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, a);
            }

            yield return new WaitForSeconds(.02f);
        }
    }
}