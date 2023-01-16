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
It uses the [builder pattern](https://refactoring.guru/design-patterns/builder)/has a fluent API, meaning you can chain methods together to create a state machine.

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

The constructor for the builder requires the type T runner.
So, in the example above, we create a monobehaviour like so which implements IDoor.
```csharp
public class Door : MonoBehaviour, IDoor
{
    void Open(){
        // Open the door
    }
    
    void Close(){
       // Close the door
    }
}
```
Here, all the states will have access to instance which implements IDoor using the Runner property.
```csharp
public interface IDoor 
{
    void Close();
    void Open();
}
```
In my opinion this is ideal rather than passing the entire monobehaviour itself. 
For example, you can have an interface like like IPlayer, ITrafficLight, ICheckpoint or whatever you want :D.

You can however pass the monobehaviour itself if you wish.
If that is the case, we change the generic type and it would look like this.
```csharp
private StateMachine<Door> _mFsm;

private void Awake()
{
    _mFsm = new StateMachineBuilder<Door>(this).Build();
}
```



## 2. Creating a State
All the methods in the builder require **concrete** states which can be done by inheriting from the State<T> class.

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

To add functionality to a state, you can override the following methods.
```csharp
public virtual void OnInitialize(T runner) { }
public virtual void OnEnter() { }
public virtual void OnExit() { }
public virtual void OnUpdate(float deltaTime) { }
public virtual void OnFixedUpdate(float fixedDeltaTime) { }
```

OnInitialize(T runner) is called when the state machine is built using the builder.
It's useful for any initialization/setup you only need to once.

### Inheritance

If you're using inheritance with your states, you can call the base methods like so.
```csharp
public override void OnEnter()
{
    base.OnEnter();
    Debug.Log("Entered a State");
}
```

For example, suppose we have a base class called PlayerStateBase where when we enter a new state, we'd like to play a specific animation.
It would look something like this.
```csharp
[Serializable]
public class PlayerStateBase : State<IPlayer>
{
    [SerializeField] private AnimationClip stateAnimation;
    
    public override void Enter()
    {
        Runner.PlayAnimation(stateAnimation);
    }
}
```

```csharp
[Serializable]
public class Jump : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();
    }
    
    public override void OnUpdate(float deltaTime)
    {
        // jump logic
    }
}
```



## 3. Creating a Transition
Transitions are created by calling the AddTransition method on the builder or creating a transition object.
They require a predicate or condition to be met before the transition can occur.
```csharp
var closedToOpen = new StateTransition<IDoor>(from: closed, to: open, 
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
void Update(float deltaTime);
void FixedUpdate(float fixedDeltaTime);
void Evaluate();
```

Important to note that the state machine will not run until you call Start().

I would recommend calling Evaluate() in late update or after all your other updates have been called.

Evaluate() will continuously check all transitions of the current state.
If any condition is met, the state machine will transition to the new state set in the transition.


