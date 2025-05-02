using UnityEngine;
using System.Collections.Generic;

public class CubePool : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private int _maxPoolSize = 20;

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
    }

    public void GetCube(Vector3 position)
    {
        if (_cubesPool.Count > 0)
        {
            Cube cube = _cubesPool.Dequeue();
            cube.gameObject.SetActive(true);
            cube.Initialize(position);

            if (cube.TryGetComponent(out Cube component))
                component.Timed += GiveBackCube;
        }
    }

    private void GiveBackCube(Cube cube)
    {
        cube.gameObject.SetActive(false);
        cube.Timed -= GiveBackCube;
        _cubesPool.Enqueue(cube);
    }
}
