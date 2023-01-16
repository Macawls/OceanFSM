<h1 align="center"> Ocean Finite State Machine</h1>
<p align="center">
  <img src="https://i.imgur.com/foM5qZO.png" alt="apu" />
</p>
<p align="center">
  A code-only, simple and easy to use Finite State Machine for your Unity Projects!
</p>

## Documentation
There is no full documentation yet! 
## Adding to your project
Open the package manager, click the plus sign and choose "Add package from git URL".
Use the link below.
```
https://github.com/Macawls/OceanFsm.git
```
## Samples
After adding the package, you can use the import button to have a look around the examples.

## Getting Started 
There are three concepts to know:
1. State Machine
2. States
3. Transitions

## 1. Creating a State Machine
The StateMachineBuilder is used to create a state machine. 
It is a fluent API, meaning you can chain methods together to create a state machine.

### Door Example
```csharp
private StateMachine<IDoor> _mFsm;

private void Awake()
{
    _mFsm = new StateMachineBuilder<IDoor>(this)
        .SetStartingState(closed)
        .AddTransition(from: closed, to: open, 
            condition: () => {
            return PlayerIsNearby() && Input.GetKeyDown(KeyCode.Space) 
            && _mIsLocked == false)
        })
        .AddTransition(from: open, to: closed, 
            condition: () => {
            return PlayerIsNearby() && Input.GetKeyDown(KeyCode.Space)
        })
        .Build();
}
```

The constructor for the builder requires an object that implements the IStateMachineRunner interface.
The state machine, states and transitions are bound by T so that states are tied to a specific type.

```csharp
public interface IStateMachineRunner<out T> where T : class
{ 
    T Runner { get; }
}
```

So, in the example above, we create a monobehaviour like so,

```csharp
public class Door : MonoBehaviour, IDoor
{
    public IDoor Runner => this;
}
```
IDoor inherits from IStateMachineRunner, so we can pass it to the builder.
```csharp
public interface IDoor : IStateMachineRunner<IDoor>
{
    void Close();
    void Open();
}
```
This is ideal rather than passing the entire monobehaviour itself. 
For example, you can have an interface like like IPlayer, ITrafficLight, ICheckpoint or whatever you want :D.

All the states will have access to the instance the generic type T.

## 2. Creating a State
All the methods in the builder require concrete states.
What I personally like to do is serialize states by adding the serializable attribute to the class. They can then be held in scriptable objects or monobehaviours or whatever you like.
```csharp
[Serializable]
public class Closed : State<IDoor>
{
    public override void Enter()
    {
        Runner.Close();
    }
}
```

## 3. Creating a Transition
Transitions are created by calling the AddTransition method on the builder or creating a transition object.
They require a predicate or condition to be met before the transition can occur.
```csharp
var closedToOpen = new Transition<IDoor>(from: closed, to: open, 
    condition: () => {
    return PlayerIsNearby() && Input.GetKeyDown(KeyCode.Space) 
    && _mIsLocked == false)
});
```

## 4. Running the State Machine
It is completely up to you how you want to run the state machine.
The key methods are:

```csharp
void Start();
void Stop();
void Update();
void FixedUpdate();
void Evaluate();
```

Important to note that the state machine will not run until you call Start().

I would recommend calling Evaluate() in late update or after all your other updates have been called.

Evaluate() will continuously check all transitions of the current state.
If any condition is met, the state machine will transition to the new state.


