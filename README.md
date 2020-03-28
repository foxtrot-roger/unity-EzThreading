## Overview
__Ez.Threading__ allows you to start asynchronous tasks in Unity.
These tasks can be performed on the Update or FixedUpdate, and it is possible to define more.

## What should I use it?
Moving an object each frame at a give speed is as simple as :
```c#
public class MoveOnUpdate : MonoBehaviour
{
    public float Speed;

    void Start()
    {
        UnityContext.Update
            .CreateTask()
            .RelativeToLast()
            .ProcessWith(deltaTime =>
            {
                var direction = new Vector3(
                    Input.GetAxis("Horizontal"),
                    Input.GetAxis("Vertical"));

                transform.position += direction * Speed * deltaTime;
            });
    }
}
```

Starting a physics movement is as simple as :
```c#
public class DashBehavior : MonoBehaviour
{
    public float Duration;
    public float Speed;
	public RigidBody rigidBody;

    void Update()
    {
		if(Input.GetButtonDown("Fire1"))
			UnityContext.FixedUpdate
				.CreateTask()
				.EndingIn(Duration)
				.RelativeToLast()
				.ProcessWith(deltaTime =>
				{
					var direction = transform.up;

					rigidBody.MoveTo(transform.position + (direction * Speed * deltaTime));
				});
    }
}
```
## Getting started
* Copy the library to your project
* Add a _UnityContext to the scene, the prefab can be found in "_Libraries\Ez\Prefabs\_UnityContext"
* You are now ready to use the system

## What does it do?
The namespace __Ez.Threading.UnityContext__ exposes 2 task managers
* Update : will run tasks on the Update
* FixedUpdate : will run tasks on the FixedUpdate

From there you can easily start an action on each frame :
```c#
// this will execute on each frame
UnityContext.Update
	.CreateTask()
	.RelativeToLast()
	.ProcessWith(deltaTime =>
	{
		var direction = new Vector3(
			Input.GetAxis("Horizontal"),
			Input.GetAxis("Vertical"));

		transform.position += direction * Speed * deltaTime;
	});
```

Or you can start a task on each physics update :
```c#
var rigidBody = GetComponent<RigidBody>();

// this will execute on each physics update
UnityContext.FixedUpdate
	.CreateTask()
	.RelativeToLast()
	.ProcessWith(deltaTime =>
	{
		var direction = new Vector3(
			Input.GetAxis("Horizontal"),
			Input.GetAxis("Vertical"));

		rigidBody.MoveTo(transform.position + (direction * Speed * deltaTime));
	});
```

## How to
### Process messages
```c#
// this will execute on each frame
var task = UnityContext.FixedUpdate
	.CreateTask()
	.ProcessWith(now =>
	{ 
		// here comes the code that will be executed
		// optionally you can tell the task to finish early by returning false
	});

// this will execute on each physics update
var task = UnityContext.FixedUpdate
	.CreateTask();
	.ProcessWith(now =>
	{ 
		// here comes the code that will be executed
		// optionally you can tell the task to finish early by returning false
	});
```

### End a task early
```c#
	task
	.ProcessWith(message => 
	{
		# # // you can tell the task to finish early by returning false
		# # // you can tell the task to finish early by returning false
		# // you can tell the task to finish early by returning false
	})
```

### Execute something when a task completes
```c#
// this will execute on each frame
var task = UnityContext.FixedUpdate
	.CreateTask()
	.ContinueWith(completed =>
	{ 
		// optionally you can tell the task to finish early by returning false
	});

// this will execute on each physics update
var task = UnityContext.FixedUpdate
	.CreateTask();
	.ContinueWith(completed =>
	{ 
		// here comes the code that will be executed when the task is completed
	});
```

### Get more from my tasks
The __Ez.Threading.TaskExtension__ and __Ez.Threading.TimeExtension__ classes provide may useful methods :
#### Filter message
```c#
	task
	.Where(message => 
	{
		// return true if the message should be processed, 
		// return false otherwise
	})
	.ProcessWith(validMessage =>
	{
		// only the messages filtered will reach here
		// optionally you can tell the task to finish early by returning false
	});
```

#### Convert messages
```c#
	task
	.Select(message => 
	{
		// return the message converted
	})
	.ProcessWith(convertedMessage =>
	{
		// the messages here are the converted messages
		// optionally you can tell the task to finish early by returning false
	});
```

#### Delay a task start
```c#
	UnityContext.Update
	.StartingIn(timeToWaitInSeconds)
	.ProcessWith(now => 
	{
		// the first time this code is executed is timeToWaitInSeconds later
		// optionally you can tell the task to finish early by returning false
	});
```

#### End a task after a duration
```c#
	UnityContext.Update
	.EndingIn(timeToWaitInSeconds)
	.ProcessWith(now => 
	{
		// this will be executed until the timeToWaitInSeconds has passed
		# // optionally you can tell the task to finish early by returning false
		# // optionally you can tell the task to finish early by returning false
	});
```

#### Force a task to end
```c#
	task.PublishCompletion(false);
```

## How does it work?
It uses one MonoBehaviour to push messages through __Ez.Threading.UnityContext__.


## Notes
### On performances
* There is an overhead of creating objects when creating a task and a garbage collection when a task is completed
	* This can impact performances
	* This might get improved in future versions