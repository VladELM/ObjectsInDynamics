using Random = System.Random;
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
    private Random _random;
    private bool _isTouched;

    public event Action<Cube> Counted;

    private void OnEnable()
    {
        _random = new Random();
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _random = new Random();
        _isTouched = false;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (_isTouched == false)
        {
            if (collision.gameObject.TryGetComponent(out Platform component))
            {
                SetCollidedParameters(out int lifeTime);
                _coroutine = StartCoroutine(Reducing(lifeTime));
            }
        }
    }

    private void SetCollidedParameters(out int lifeTime)
    {
        _renderer.material = _collisionMaterial;
        lifeTime = _random.Next(_minLifeTime, _maxLifeTime + 1);
    }

    private IEnumerator Reducing(int lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);

        Counted.Invoke(this);
    }
}
