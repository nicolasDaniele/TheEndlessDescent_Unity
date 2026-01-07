# Enemy AI Architecture

This project uses a Finite State Machine combined with
action-based transitions.

- States execute behavior
- Transitions expose possible AIAction options
- AIControllers decide which action to take
- StateMachine performs the actual state change

This design allows easy extension and future migration
to Utility AI if needed.