using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBall : MonoBehaviour, IHitable
{
    private Flash _flash;
    private DiscoBallManager _discoBallManager;

    private void Awake() {
        _flash = GetComponent<Flash>();
    }

    private void Start() {
        _discoBallManager = FindFirstObjectByType<DiscoBallManager>();
    }

    public void TakeHit()
    {
        _discoBallManager.DiscoBallParty();
        _flash.StartFlash();
    }
}
