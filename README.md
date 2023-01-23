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

There is no full documentation yet!
More samples are on the way!

## Adding to your project
### OpenUPM
```bash
openupm add com.macawls.oceanfsm
```
### Git URL

1. Open the package manager window
2. Click the plus sign and choose "Add package from git URL"
3. Use the link below.

```
https://github.com/Macawls/OceanFsm.git
```

### Manual
Add the following to your manifest.json.
Remember to replace the version with the lastest one if you so wish.

```json5
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
    "com.macawls.oceanfsm": "1.0.0" // Replace with the latest version
  }
}
```

## Samples
After adding the package, you can use the import button to import the samples.
The samples were created from the default 3D Unity template.

## Getting Started

There are three concepts to know:

1. State Machine
2. States
3. Transitions

[State Pattern on Refactoring Guru](https://refactoring.guru/design-patterns/state)

## State Machine Types

There are two types of state machines at the moment:
- ```Autonomous``` - States themselves are responsible for transitioning to other states.
- ```Transitional``` - States are changed by transitions with conditionals. States are not responsible for transitioning.

There are two builders to aid in creating state machines, they are not required to construct them.
### Transitional State Machine
```csharp
private ITransitionalStateMachine<IDoor> _mFsm;

private void Awake()
{
    var closed = new Closed();
    var open = new Open();

    _mFsm = new TransitionStateMachineBuilder<IDoor>(this)
        .SetStartingState(closed)
        .AddTransition(from: closed, to: open,
            condition: () => {
            return PlayerIsNearby() && Input.GetKeyDown(KeyCode.Space)
            && _mIsLocked == false)
        })
        .AddTransition(from: open, to: closed,
            condition: () => {
            return PlayerIsNearby() && Input.GetKeyDown(KeyCode.Space)
        }, onTransition: () => {
            Debug.Log("Closing the door"); // action is optional
        })
        .Build();
}
```

### Autonomous State Machine
```csharp
private IAutonomousStateMachine<IPlayer> _mFsm;

private void Awake()
{
    _mFsm = new AutonomousStateMachineBuilder<IDoor>(this)
        .SetStartingState(new Idle())
        .AddState(new Walk())
        .AddState(new Jump())
        .Build();
}
```

### Which one should I use?
I've found that if it's a simple entity with a finite amount of states that won't be extended, the ```Transitional``` state machine is the way to go.
For example a door, checkpoint, traffic light, treasure chest etc.


If it's an entity that is fairly complex and will be extended, the ```Autonomous``` state machine is the way to go.
Something like a player, enemy, NPC etc. 

It all depends, there's nothing stopping you from using either one. It's very flexible.
What you could do is, if you're unsure as to how many states you require, initially use the ```Autonomous``` state machine and keep extending it. 
At some point you may find that you're not extending it anymore and you can extract your conditionals to use a ```Transitional``` state machine.
<hr> 

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
    private ITransitionalStateMachine<IDoor> _mFsm;
    
    // Instance of IDoor passed to constructor of the builder 
    _mFsm = new TransitionalStateMachineBuilder(this) 
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

States also have a ```Machine``` property. This will be null if a ```Transitional``` state machine is used.
The example below is from an ```Autonomous``` state machine.
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
virtual void OnFixedUpdate(float fixedDeltaTime) { }
```

### Configuration
What I personally like to do is serialize states by adding the ```serializable``` attribute to the class. They can then be held in scriptable objects or monobehaviours or whatever you like which is useful for configuration.

```csharp
[Serializable]
public class RunState : State<IPlayer>
{
    // bunch of serialized fields, 
    // move speed, acceleration, etc. 
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

## 4. Running the State Machine

It is completely up to you how you want to run the state machine.
The key methods are:

```csharp
void Start();
void Stop();
void Update(float deltaTime);
void FixedUpdate(float fixedDeltaTime);
void Evaluate(); // only for Transitional state machines
```

Important to note that the state machine will not run until you call Start().

I would recommend calling **Evaluate() in [Late Update](https://docs.unity3d.com/ScriptReference/MonoBehaviour.LateUpdate.html)** or after all your other updates have been called.

Evaluate() will continuously check all transitions of the current state. If a transition is met, it will change to the new state.

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

private void FixedUpdate()
{
    _mFsm.FixedUpdate(Time.fixedDeltaTime);
}

private void LateUpdate() // if using Transitional
{
    _mFsm.Evaluate();
}
```



