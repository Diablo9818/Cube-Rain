using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    [SerializeField] private Color _initialColor = Color.white;
    [SerializeField] private Color _touchedColor = Color.red;
    [SerializeField] private float _minTime = 2f;
    [SerializeField] private float _maxTime = 5f;

    private bool _hasTouchedPlatform = false;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private ObjectPool _pool;
    private GameObject _gameObject;

    private float _pushTimer = -1;

    public Cube Initialize(ObjectPool pool)
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
        _pool = pool;
        _gameObject = gameObject;
        _gameObject.SetActive(false);
        return this;
    }

    public void Push()
    {
        _pool.Push(this);
        SetActive(false);
    }

    public Cube Pull(Vector3 position)
    {
        _hasTouchedPlatform = false;
        GetComponent<Renderer>().material.color = _initialColor;
        _transform.position = position;
        SetActive(true);
        return this;
    }

    public void SetActive(bool value)
    {
        _gameObject.SetActive(value);
    }

    public void SetVelocity(Vector3 vector3)
    {
       _rigidbody.velocity = vector3;
    }

    private void Update()
    {
        if(_pushTimer >= 0)
        {
            _pushTimer-= Time.deltaTime;

            if(_pushTimer < 0)
            {
                _pushTimer = -1;
                Push();
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!_hasTouchedPlatform && collision.gameObject.TryGetComponent(out Platform platform))
        {
            _hasTouchedPlatform = true;
            GetComponent<Renderer>().material.color = _touchedColor;
            PushDelayed(Random.Range(_minTime, _maxTime));
        }
    }


    private void PushDelayed(float time)
    {
       _pushTimer = time;
    }
}
