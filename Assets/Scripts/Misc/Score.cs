using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private int _currentScore = 0;
    private TMP_Text _scoreText;

    private void Awake() {
        _scoreText = GetComponent<TMP_Text>();
    }

    private void Start() {
        _scoreText.text = _currentScore.ToString("D3");
    }

    private void OnEnable() {
        Health.OnDeath += EnemyDestroyed;
    }

    private void OnDisable() {
        Health.OnDeath += EnemyDestroyed;        
    }

    private void EnemyDestroyed(Health sender) {
        if (sender.GetComponent<Enemy>()) {
            _currentScore++;
            _scoreText.text = _currentScore.ToString("D3");
        }
    }
}
