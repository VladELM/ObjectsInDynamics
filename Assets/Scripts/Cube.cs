using static UnityEngine.Random;
using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    [SerializeField] private Vector3 _defaultRotation;
    [SerializeField] private Vector3 _defaultVelocity;
    [SerializeField] private int _minLifeTime;
    [SerializeField] private int _maxLifeTime;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _collisionMaterial;

    private Coroutine _coroutine;
    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private bool _isTouched;

    public event Action<Cube> Timed;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _isTouched = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isTouched == false)
        {
            if (collision.gameObject.TryGetComponent(out Platform component))
            {
                _renderer.material = _collisionMaterial;
                int lifeTime = Range(_minLifeTime, _maxLifeTime + 1);
                _coroutine = StartCoroutine(Reducing(lifeTime));
            }
        }
    }

    public void Initialize(Vector3 position)
    {
        _rigidbody.velocity = _defaultVelocity;
        _rigidbody.angularVelocity = _defaultVelocity;

        transform.position = position;
        transform.rotation = Quaternion.Euler(_defaultRotation);

        _renderer.material = _defaultMaterial;
        _isTouched = false;
    }

    private IEnumerator Reducing(int lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);

        Timed.Invoke(this);
    }
}
