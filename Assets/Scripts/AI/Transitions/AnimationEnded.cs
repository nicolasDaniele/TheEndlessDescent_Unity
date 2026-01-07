using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationEnded : ITransition
{
    public IReadOnlyCollection<AIAction> PossibleNextStates { get; }

    private Animator animator;

    public AnimationEnded(Animator _animator, IEnumerable<AIAction> _actions)
    {
        animator = _animator;
        PossibleNextStates = _actions.ToList<AIAction>();
    }

    public bool ShouldTransition()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
    }
}