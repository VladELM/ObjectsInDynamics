using static UnityEngine.Random;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private int _maxPoolSize = 20;
    [SerializeField] private int _minCoordinate;
    [SerializeField] private int _maxCoordinate;
    [SerializeField] private int _minHeight;
    [SerializeField] private int _maxHeight;
    [SerializeField] private int _delay;

    private Queue<Cube> _cubesPool;

    private void Awake()
    {
        _cubesPool = new Queue<Cube>();

        for (int i = 0; i < _maxPoolSize; i++)
        {
            Cube cube = Instantiate(_cubePrefab);
            cube.gameObject.SetActive(false);
            _cubesPool.Enqueue(cube);
        }

        StartCoroutine(Spawning());
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(Range(_minCoordinate, _maxCoordinate + 1),
                            Range(_minHeight, _maxHeight + 1),
                            Range(_minCoordinate, _maxCoordinate + 1));
    }

    private IEnumerator Spawning()
    {
        WaitForSeconds interval = new WaitForSeconds(_delay);

        while (enabled)
        {
            yield return interval;

            GetCube(GetRandomPosition());
        }
    }

    public void GetCube(Vector3 position)
    {
        if (_cubesPool.Count > 0)
        {
            Cube cube = _cubesPool.Dequeue();
            cube.gameObject.SetActive(true);
            cube.Initialize(position);

            if (cube.TryGetComponent(out Cube component))
                component.TimeCounted += GiveBackCube;
        }
    }

    private void GiveBackCube(Cube cube)
    {
        cube.gameObject.SetActive(false);
        cube.TimeCounted -= GiveBackCube;
        _cubesPool.Enqueue(cube);
    }
}