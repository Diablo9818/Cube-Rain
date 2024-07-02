using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private Cube _cube;
    [SerializeField] private float _Yposition;

    [SerializeField] private float _minXposition;
    [SerializeField] private float _maxXposition;

    [SerializeField] private float _minZposition;
    [SerializeField] private float _maxZposition;
    [SerializeField] private float _spawnDelay;

    private Queue<Cube> _cubeQueue = new Queue<Cube>();
    private Transform _transform;
    private bool _isGameRunnig;

    private void Awake()
    {
        _transform = transform;
        _isGameRunnig = true;
    }

    private IEnumerator Start()
    {
        yield return null;

        while (_isGameRunnig)
        {
            yield return new WaitForSeconds(_spawnDelay);

            var spawned = Pull(GetSpawnPoint());
            spawned.SetVelocity(Vector3.down);

        }
    }

    private Cube Pull(Vector3 spawnPoint)
    {
        if (_cubeQueue.Count == 0)
        {
            Instantiate(_cube, _transform.position, _transform.rotation, _transform).Initialize(this).Push();
        }
        
        var spawned = _cubeQueue.Dequeue();
        spawned.Pull(spawnPoint);

        return spawned;
    }

    public void Push(Cube cube)
    {
        cube.SetActive(false);
        _cubeQueue.Enqueue(cube);
    }

    private Vector3 GetSpawnPoint()
    {
        return new Vector3(UnityEngine.Random.Range(_minXposition,_maxXposition), 
            _Yposition, UnityEngine.Random.Range(_minZposition, _maxZposition));
    }
}
