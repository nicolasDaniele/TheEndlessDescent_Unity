using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a single behavior state in the enemy finite state machine.
/// A State executes behavior but does not decide transitions.
/// </summary>
public abstract class State : IState
{
    protected StateMachine stateMachine;
    protected AIController controller;
    protected Animator animator;
    protected string animationName;
    protected List<ITransition> highPriorityTransitions = new List<ITransition>();
    protected List<ITransition> lowPriorityTransitions = new List<ITransition>();

    // Variables for the transition polling interval
    private float checkInterval = 0.1f;
    private float timer = 0f;

    protected State(StateMachine _stateMachine, AIController _controller,
        Animator _animator, string _animationName)
    {
        stateMachine = _stateMachine;
        controller = _controller;
        animator = _animator;
        animationName = _animationName;
    }

    // Checks if any transition must be reset when entering.
    public virtual void EnterState()
    {
        int animHash = Animator.StringToHash(animationName);
        animator?.CrossFade(animHash, controller.AnimTransitionTime, 0, 0f);

        foreach (var t in highPriorityTransitions)
        {
            if (t is IResettableTransition r)
                r.Reset();
        }

        foreach (var t in lowPriorityTransitions)
        {
            if (t is IResettableTransition r)
                r.Reset();
        }

        Debug.Log($"{this.ToString()} EnterState");
    }

    public virtual void ExitState() 
    {
        Debug.Log($"{this.ToString()} ExitState");
    }

    // States do not change state directly.
    // They only notify the StateMachine when a transition condition is met.
    public virtual void UpdateState()
    {
        timer += Time.deltaTime;

        if (timer < checkInterval)
            return;

        timer = 0f;

        foreach (var transition in highPriorityTransitions)
        {
            if (transition.ShouldTransition())
            {
                OnTransitionTriggered(transition);
                return;
            }
        }

        foreach (var transition in lowPriorityTransitions)
        {
            if (transition.ShouldTransition())
            {
                OnTransitionTriggered(transition);
                return;
            }
        }
    }

    public void AddHighPriorityTransition(ITransition transition)
    {
        highPriorityTransitions.Add(transition);
    }

    public void AddLowPriorityTransition(ITransition transition)
    {
        lowPriorityTransitions.Add(transition);
    }

    protected virtual void OnTransitionTriggered(ITransition transition)
    {
        stateMachine.HandleTransition(transition);
    }
}