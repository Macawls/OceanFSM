<h1 align="center"> Ocean Finite State Machine</h1>
<p align="center">
  <img src="https://i.imgur.com/foM5qZO.png" alt="apu" />
</p>
<p align="center">
  A code-only, simple and easy to use Finite State Machine for your Unity Projects!
</p>

<p align="center">
	<img alt="GitHub package.json version" src ="https://img.shields.io/github/package-json/v/Macawls/OceanFSM" />
	<a href="https://github.com/Macawls/OceanFSM/blob/main/LICENSE.md">
		<img alt="GitHub license" src ="https://img.shields.io/github/license/Macawls/OceanFSM" />
	</a>
	<img alt="GitHub last commit" src ="https://img.shields.io/github/last-commit/Macawls/OceanFSM"/>
    <a href="https://openupm.com/packages/com.macawls.oceanfsm/">
    <img src="https://img.shields.io/npm/v/com.macawls.oceanfsm?label=openupm&amp;registry_uri=https://package.openupm.com"  alt="OpenUPM"/>
</a>
</p>

https://user-images.githubusercontent.com/80009513/212888509-3f0e4358-f7f9-4acc-9f89-594ea925bd11.mp4

## Documentation

There is no full documentation yet! However, there are samples and a getting started section below.

## Adding to your project
### OpenUPM (Recommended)
```bash
openupm add com.macawls.oceanfsm
```
### Git URL (Recommended)

1. Open the package manager window
2. Click the plus icon
3. Choose ``"Add package from git URL..."``
4. Use the link below.

```
https://github.com/Macawls/OceanFsm.git
```

### Manual (Not Recommended)
Add the following to your manifest.json.

```yml
{
  "scopedRegistries": [
    {
      "name": "OpenUPM",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.macawls.oceanfsm"
      ]
    }
  ],
  "dependencies": { 
    // Replace with the latest version of the package
    "com.macawls.oceanfsm": "{version}" 
  }
}
```

## Samples
After adding the package, you can use the **import** button from the package manager window to inspect the samples.

The samples were created from the Default 3D Unity template.

## Getting Started

There are three concepts to know:

1. State Machine
2. States
3. Transitions

[State Pattern on Refactoring Guru](https://refactoring.guru/design-patterns/state)

## State Machine Types

There are two types of state machines at the moment:

### Autonomous State Machine
- States are responsible for transitioning to other states.
- The machine can receive commands to transition to a specific state.
- Can freely transition to any state.

### Polling State Machine
- The machine holds an internal dictionary of states and their transitions.
- Transitions are triggered by a condition/predicate.
- States cannot transition by themselves.

## Creating the State Machine
There are two builders to aid in creating state machines.
### Transitional State Machine
```csharp
private IPollingMachine<IDoor> _mFsm;

private void Awake()
{
    var closed = new Closed();
    var open = new Open();

    _mFsm = new PollingBuilder<IDoor>(this)
        .SetStartingState(nameof(Closed))
        .AddTransition(closed, open, () => {
            return PlayerIsNearby && Input.GetKeyDown(KeyCode.Space) && !_mIsLocked)
        })
        .AddTransition(open, closed, () => {
            return PlayerIsNearby() && Input.GetKeyDown(KeyCode.Space)
        }, onTransition: () => {
            Debug.Log("Closing the door"); // optional
        })
        .Build();
}
```

### Autonomous State Machine
```csharp
private IAutonomousStateMachine<IPlayer> _mFsm;

private void Awake()
{
    _mFsm = new AutonomousBuilder<IDoor>(this)
        .SetStartingState(nameof(Idle))
        .AddState(new Idle())
        .AddState(new Walk())
        .AddState(new Jump())
        .Build();
}
```

### Command Usage (WIP)
Currently, only the ```AutonomousStateMachine``` supports commands.
Commands are useful for triggering actions or responding to events from outside the state machine. Conditions and the actions are optional.

```csharp
private void Awake()
{
    _mFsm = new AutonomousBuilder<IPlayer>(this)
        .SetInitialState(nameof(Idle)) 
        .AddStates(idle, run, jump)
        .Build();
    
    _mFsm.AddCommand("Jump")
        .SetTargetState<Jump>()
        .SetCondition(() => _mFsm.CurrentState is Run && IsGrounded)
        .OnSuccess(() => Debug.Log("Hi mom")) // visual fx for example
        .OnFailure(() => Debug.Log("depression")); // negative sound effect for example
}

private void OnJump(InputAction.CallbackContext ctx)
{
    if (ctx.performed)
    {
        _mFsm.ExecuteCommand("Jump");
    }
}
```

### Running the State Machine
It is completely up to you how you want to run the state machine.
The key methods are:

```csharp
void Start();
void Stop();
void Update(float deltaTime);
void Evaluate(); // only for Polling State machines
```

Important to note that the state machine will not run until you call ```Start()```

if you're using the ```Polling``` state machine, I would recommend calling ```Evaluate()``` in [Late Update](https://docs.unity3d.com/ScriptReference/MonoBehaviour.LateUpdate.html).

```Evaluate()``` will continuously check all transitions of the current state. If a transition is met, it will change to the new state.

### Using Monobehaviour Hooks
```csharp
private void OnEnable()
{
    _mFsm.Start();
}

private void OnDisable()
{
    _mFsm.Stop();
}

private void Update()
{
    _mFsm.Update(Time.deltaTime);
}

private void LateUpdate()
{
    _mFsm.Evaluate();
}
```

### Which one should I use?
I've found that if it's a simple entity with a few states and transitions, the ```Polling``` state machine is good.
For example a door, checkpoint, traffic light, treasure chest etc.

If it's an entity that is fairly complex and or reacts to external input , the ```Autonomous``` state machine is the way to go.
Something like a player, enemy, NPC, UI system etc.

The ```Autonomous``` one is easier to use and more flexible. Most of the time I recommend using it.

## 2. Creating a State
All states have to inherit from the ```State<T>``` class.

### Generic Usage
The generic reference type is used to associate your states and state machines.
You can use whatever you want, but I recommend using an interface to keep things tidy.
Lets define ```IDoor``` as our reference type.

```csharp
public interface IDoor
{
    void Close();
    void Open();
}
```
```csharp
public class Door : MonoBehaviour, IDoor 
{
    private IPollingMachine<IDoor> _mFsm;
    
    // Instance of IDoor passed to constructor of the builder 
    _mFsm = new PollingBuilder<IDoor>(this) 
    ...
 }
```

Here, all the states will have access to instance which implements ```IDoor``` using the ```Runner``` property. Like so.

```csharp
class Closed : State<IDoor>
{
    public override void OnEnter() => Runner.Close();
}
```

States also have a ```Machine``` property. If the machine is not castable to ```IAutonomousStateMachine<T>```, it will be null.
```csharp
class Closed : State<IDoor>
{
    public override void OnUpdate()
    {
        if (PlayerIsNearby() && Input.GetKeyDown(unlockKey))
        {
            Machine.ChangeState<Open>(() => {
                Debug.Log("Player has opened the door");
            });
        }
    }
}
```

### Functionality

To add functionality to a state, you can override the following methods.

```csharp
virtual void OnInitialize() { }
virtual void OnEnter() { }
virtual void OnExit() { }
virtual void OnUpdate(float deltaTime) { }
```

```OnInitialize()``` is called when the state machine is built and the state has been added.

### Configuration
What I personally like to do is serialize states by adding the ```Serializable``` attribute to the class. They can then be held in scriptable objects or monobehaviours or whatever you like which is useful for configuration.

```csharp
[Serializable]
public class RunState : State<IPlayer>
{
    // bunch of serialized fields, properties, etc.
}
```

### Inheritance Example
For a simple use case for inheritance, suppose we have a base class called PlayerStateBase where when we enter a new state, we'd like to play a specific animation.
It would look something like this.

```csharp
[Serializable]
public abstract class PlayerStateBase : State<IPlayer>
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
    // whatever fields you need
    
    public override void Enter()
    {
        base.Enter(); // play the animation
        // other logic
    }
}
```



### Utilities
Since all states have to inherit from ```State<T>``` class, you can use the ```State Generator``` tool to generate a state boilerplate class for you. 

You'll find it under the ```Tools``` dropdown menu.

<center>

![](https://i.imgur.com/SVG6GjB.jpg)


</center>


