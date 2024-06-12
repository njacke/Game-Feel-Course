using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DiscoBallManager : MonoBehaviour
{
    [SerializeField] private float _discoBallPartyTime = 2f;
    [SerializeField] private float _discoGlobalLightIntensity = .2f;
    [SerializeField] private Light2D _globalLight; 

    private float _defaultGlobalLightIntensity;
    private Coroutine _discoRoutine;
    private static Action OnDiscoBallHitEvent;

    private ColorSpotlight[] _allSpotlights;

    private void Awake() {
        _defaultGlobalLightIntensity = _globalLight.intensity;
    }

    private void Start() {
        _allSpotlights = FindObjectsByType<ColorSpotlight>(FindObjectsSortMode.None);
    }


    private void OnEnable() {
        OnDiscoBallHitEvent += DimTheLights;
    }

    private void OnDisable() {
        OnDiscoBallHitEvent -= DimTheLights;
        
    }

    public void DiscoBallParty() {
        if (_discoRoutine != null) { return; }

        OnDiscoBallHitEvent?.Invoke();
    }

    private void DimTheLights() {
        foreach (ColorSpotlight spotlight in _allSpotlights) {
            StartCoroutine(spotlight.SpotlightDiscoParty(_discoBallPartyTime));
        }

        _discoRoutine = StartCoroutine(GlobalLightResetRoutine());   
    }

    private IEnumerator GlobalLightResetRoutine() {
        _globalLight.intensity = _discoGlobalLightIntensity;
        yield return new WaitForSeconds(_discoBallPartyTime);
        _globalLight.intensity = _defaultGlobalLightIntensity;
        _discoRoutine = null;
    }
}
