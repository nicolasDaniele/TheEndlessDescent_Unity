using UnityEngine;

public class MoveState : State
{
    public MoveState(StateMachine _stateMachine, AIController _controller,
        Animator _animator, string _animationName)
        : base(_stateMachine, _controller, _animator, _animationName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        controller.StartMoving();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        controller.UpdateMovement();
    }

    public override void ExitState()
    {
        base.ExitState();

        controller.StopMoving();
    }
}