using static UnityEngine.Random;
using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private CubePool _cubePool;
    [SerializeField] private int _minCoordinate;
    [SerializeField] private int _maxCoordinate;
    [SerializeField] private int _minHeight;
    [SerializeField] private int _maxHeight;
    [SerializeField] private int _delay;

    private Coroutine _coroutine;

    private void Start()
    {
        _coroutine = StartCoroutine(Spawning());
    }

    private Vector3 GetPosition()
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

            _cubePool.GetCube(GetPosition());
        }
    }
}
