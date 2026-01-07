using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Coordinates state execution and transitions.
/// This is the only class responsible for mapping AIActions to concrete states
/// and performing state changes.
/// </summary>
public class StateMachine
{
    public IState CurrentState { get; private set; }

    private AIController controller;
    private Dictionary<AIAction, IState> actionToStateMap;

    public StateMachine(AIController _controller)
    {
        controller = _controller;
    }

    public void SetActionStateMap(Dictionary<AIAction, IState> _map)
    {
        actionToStateMap = _map;
    }

    public void Update()
    {
        CurrentState?.UpdateState();
    }

    /// <summary>
    /// Called when a transition condition is met.
    /// If multiple actions are available, delegates the decision to the AIController.
    /// Resolves the chosen AIAction into a concrete state and performs the transition.
    /// </summary>
    /// <param name="transition">
    /// The transition whose condition has been met and which exposes
    /// the possible next actions.
    /// </param>
    public void HandleTransition(ITransition transition)
    {
        AIAction action;

        if (transition.PossibleNextStates.Count == 1)
            action = transition.PossibleNextStates.First();
        else
            action = controller.DecideNextAction(transition.PossibleNextStates);

        IState nextState = actionToStateMap[action];
        ChangeState(nextState);
    }

    public void SetDefaultState(IState defaultState)
    {
        ChangeState(defaultState);
    }

    /// <summary>
    /// Performs the actual state change, ensuring proper exit and enter calls.
    /// </summary>
    private void ChangeState(IState nextState)
    {
        CurrentState?.ExitState();
        CurrentState = nextState;
        CurrentState.EnterState();
    }
}