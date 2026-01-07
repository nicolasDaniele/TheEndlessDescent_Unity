using UnityEngine;

public class AttackState : State
{
    private bool repeatAttack;

    public AttackState(StateMachine _stateMachine, AIController _controller,
        Animator _animator, string _animationName, bool _repeatAttack = false)
        : base(_stateMachine, _controller, _animator, _animationName)
    {
        repeatAttack = _repeatAttack;
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}