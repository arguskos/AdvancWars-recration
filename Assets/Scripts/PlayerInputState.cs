using System;

public class PlayerInputState
{
    public delegate void MoveAction(int dirX, int dirY);

    public delegate void EscapeAction();

    public delegate void ConfirmAction();
    public delegate void CancelAction();

    public delegate void SimpleAction();


    public EscapeAction IsEscapeAction;
    public CancelAction IsCancelAction;
    public ConfirmAction IsConfirmedAction;
    public MoveAction IsMovedAction;

    public SimpleAction PressedRight;
    public SimpleAction PressedLeft;
    public SimpleAction PressedDown;
    public SimpleAction PressedUp;

    public bool IsMovingLeft
    {
        get;
        set;
    }

    public bool IsMovingRight
    {
        get;
        set;
    }

    public bool IsMovingUp
    {
        get;
        set;
    }

    public bool IsMovingDown
    {
        get;
        set;
    }

    public bool IsFiring
    {
        get;
        set;
    }
}
