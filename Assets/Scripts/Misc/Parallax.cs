using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _parallaxOffset = .9f; // opposite direction of camera (<1)
    private Vector2 _startPos;
    private Camera _mainCamera;

    private Vector2 _travel => (Vector2)_mainCamera.transform.position - _startPos;

    private void Awake() {
        _mainCamera = Camera.main;
    }

    private void Start() {
        _startPos = transform.position;
    }

    // camera follows player that moves in fixed update
    private void FixedUpdate() {
        Vector2 newPostition = _startPos + new Vector2(_travel.x * _parallaxOffset, 0f);
        transform.position = new Vector2(newPostition.x, transform.position.y);
    }
}
