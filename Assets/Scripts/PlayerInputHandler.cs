using UnityEngine;
using Zenject;
public class PlayerInputHandler : ITickable
{
    private readonly PlayerInputState _inputState;
    private readonly IActionMenu _actionMenu;
    private readonly GameState _gameState;

    public PlayerInputHandler(IActionMenu aM, PlayerInputState inputState, GameState g)
    {
        _actionMenu = aM;
        _gameState = g;
        _inputState = inputState;

        _inputState.IsEscapeAction = _gameState.NextPlayer;
    }

    public void Tick()
    {

        //_inputState.IsMovingLeft = false;
        //_inputState.IsMovingRight = false;
        //_inputState.IsMovingUp = false;
        //_inputState.IsMovingDown = false;
        //_inputState.IsMovingLeft = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
        //_inputState.IsMovingRight = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        //_inputState.IsMovingUp = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        //_inputState.IsMovingDown = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);

        //_inputState.IsFiring = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);

        if (_inputState.IsMovedAction != null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                _inputState.IsMovedAction(0, 1);
            if (Input.GetKeyDown(KeyCode.DownArrow))
                _inputState.IsMovedAction(0, -1);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                _inputState.IsMovedAction(-1, 0);
            if (Input.GetKeyDown(KeyCode.RightArrow))
                _inputState.IsMovedAction(1, 0);
        }
        if (_inputState.IsEscapeAction != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _inputState.IsEscapeAction();
            }
        }
        if(_inputState.IsConfirmedAction!=null)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                _inputState.IsConfirmedAction();
            }
        }
        if (_inputState.IsCancelAction!=null)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                _inputState.IsCancelAction();
            }
        }
        //if (Input.GetKeyDown(KeyCode.UpArrow) && _inputState.PressedLeft != null)
        //    _inputState.PressedLeft();
        //if (Input.GetKeyDown(KeyCode.UpArrow) && _inputState.PressedUp != null)
        //    _inputState.PressedRight();
        //if (Input.GetKeyDown(KeyCode.UpArrow) && _inputState.PressedDown != null)
        //    _inputState.PressedDown();
        //if (Input.GetKeyDown(KeyCode.UpArrow) && _inputState.PressedRight != null)
        //    _inputState.PressedUp();


    }
}