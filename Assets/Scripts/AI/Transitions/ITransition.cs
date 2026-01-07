using System.Collections.Generic;

/// <summary>
/// Evaluates a condition to determine whether a state transition
/// should be triggered, and exposes the possible next actions.
/// </summary>
public interface ITransition
{
    bool ShouldTransition();
    public IReadOnlyCollection<AIAction> PossibleNextStates { get; }
}
