using UnityEngine;

public class IdleState : State
{
    public IdleState(StateMachine _stateMachine, AIController _controller,
        Animator _animator, string _animationName)
        : base(_stateMachine, _controller, _animator, _animationName)
    {
    }
}
