using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimerUp : ITransition, IResettableTransition
{
    public IReadOnlyCollection<AIAction> PossibleNextStates { get; }

    private float timeToTransition = 0f;
    public float startTime = 0f;

    public TimerUp(float _timeToTransition, IEnumerable<AIAction> _actions)
    {
        timeToTransition = _timeToTransition;
        PossibleNextStates = _actions.ToList<AIAction>();
    }

    public bool ShouldTransition()
    {
        return Time.time - startTime >= timeToTransition; ;
    }

    public void Reset()
    {
        startTime = Time.time;
    }
}
