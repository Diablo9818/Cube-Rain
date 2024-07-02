using System.Collections;
using UnityEngine;

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
        _gameObject.SetActive(false);
    }

    public void  Pull(Vector3 position)
    {
        _hasTouchedPlatform = false;
        GetComponent<Renderer>().material.color = _initialColor;
        _transform.position = position;
        _gameObject.SetActive(true);
    }

    public void SetVelocity(Vector3 vector3)
    {
       _rigidbody.velocity = vector3;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasTouchedPlatform == false && collision.gameObject.TryGetComponent(out Platform platform))
        {
            _hasTouchedPlatform = true;
            GetComponent<Renderer>().material.color = _touchedColor;
            StartCoroutine(PushDelayed(Random.Range(_minTime, _maxTime)));
        }
    }

    private IEnumerator PushDelayed(float time)
    {
        yield return new WaitForSeconds(time);
        Push();
    }
}
