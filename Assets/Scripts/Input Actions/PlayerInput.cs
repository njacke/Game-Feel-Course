using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public FrameInput FrameInput { get; private set; }
    private PlayerInputActions _playerInputAcitons;
    private InputAction _move;
    private InputAction _jump;

    private void Awake() {
        _playerInputAcitons = new PlayerInputActions();

        _move = _playerInputAcitons.Player.Move;
        _jump = _playerInputAcitons.Player.Jump;
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
        };
    }
}

public struct FrameInput {
    public Vector2 Move;
    public bool Jump;
}
