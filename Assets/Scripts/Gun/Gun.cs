using System;
using UnityEngine;
using UnityEngine.Pool;
using Cinemachine;
using UnityEditor.Search;
using System.Collections;

public class Gun : MonoBehaviour
{

    public static Action OnShoot;


    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _gunFireCD = .5f;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private float _muzzleFlashTime = .05f;

    private Coroutine _muzzleFlashRoutine;
    private ObjectPool<Bullet> _bulletPool;
    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private Vector2 _mousePos;
    private float _lastFireTime = 0f;

    private CinemachineImpulseSource _impulseSource;
    private Animator _animator;

    private void Awake() {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _animator = GetComponent<Animator>();
    }

    private void Start() {
        CreateBulletPool();
    }
    
    private void Update()
    {
        Shoot();
        RotateGun();
    }

    private void OnEnable() {
        OnShoot += FireAnimation;
        OnShoot += GunScreenShake;
        OnShoot += ShootProjectile;
        OnShoot += ResetLastFireTime;        
        OnShoot += MuzzleFlash;        
    }

    private void OnDisable() {
        OnShoot -= FireAnimation;        
        OnShoot -= GunScreenShake;        
        OnShoot -= ShootProjectile;
        OnShoot -= ResetLastFireTime;        
        OnShoot -= MuzzleFlash;        
    }

    public void ReleaseBulletFromPool(Bullet bullet) {
        _bulletPool.Release(bullet);
    }

    private void CreateBulletPool() {
        _bulletPool = new ObjectPool<Bullet>(() => {
            return Instantiate(_bulletPrefab);
        }, bullet => {
            bullet.gameObject.SetActive(true);
        }, bullet => {
            bullet.gameObject.SetActive(false);
        }, bullet => {
            Destroy(bullet);
        }, false, 20, 40);
    }


    private void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time >= _lastFireTime) {
            OnShoot?.Invoke();
        }
    }

    private void ShootProjectile()
    {
        Bullet newBullet = _bulletPool.Get();
        newBullet.Init(this, _bulletSpawnPoint.position, _mousePos);
    }

    private void FireAnimation() {
        _animator.Play(FIRE_HASH, 0, 0f); // 0 is base layer, 0f for start at the start of animation (0-1 relative)
    }

    private void ResetLastFireTime() {
        _lastFireTime = Time.time + _gunFireCD;
    }

    private void GunScreenShake() {
        _impulseSource.GenerateImpulse();
    }

    private void RotateGun() {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // return value between 180 and -180 degrees
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void MuzzleFlash() {
        if (_muzzleFlashRoutine != null) {
            StopCoroutine(_muzzleFlashRoutine);
        }

        _muzzleFlashRoutine = StartCoroutine(MuzzleFlashRoutine());
    }

    private IEnumerator MuzzleFlashRoutine() {
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(_muzzleFlashTime);
        _muzzleFlash.SetActive(false);
    }
}
