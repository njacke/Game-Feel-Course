using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public FrameInput FrameInput { get; private set; }
    private PlayerInputActions _playerInputAcitons;
    private InputAction _move, _jump, _jetpack, _grenade;

    private void Awake() {
        _playerInputAcitons = new PlayerInputActions();

        _move = _playerInputAcitons.Player.Move;
        _jump = _playerInputAcitons.Player.Jump;
        _jetpack = _playerInputAcitons.Player.Jetpack;
        _grenade = _playerInputAcitons.Player.Grenade;
    }

    private void OnEnable() {
        _playerInputAcitons.Enable();
    }

    private void OnDisable() {
        _playerInputAcitons.Disable();
    }

    private void Update() {
        FrameInput = GatherInput();
    }

    private FrameInput GatherInput() {
        return new FrameInput {
            Move = _move.ReadValue<Vector2>(),
            Jump = _jump.WasPressedThisFrame(),
            Jetpack = _jetpack.WasPressedThisFrame(),
            Grenade = _grenade.WasPressedThisFrame()
        };
    }
}

public struct FrameInput {
    public Vector2 Move;
    public bool Jump;
    public bool Jetpack;
    public bool Grenade;
}
