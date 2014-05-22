using UnityEngine;

#region Client and Invoker
public static class CommandController {
	static ActionReceiver receiver = null;
	static CommandAbstract command = null;
	static CommandInvoker invoker = null;
	
	static HackCommand hackCmd = null;
	static DestroyCommand destroyCmd = null;

	public static void NewCommand(ActionListEnum actionType, GameObject targetServer)
	{
		receiver = new GameObjectCommand(targetServer);
		invoker = new CommandInvoker();
		
		if(actionType== ActionListEnum.Hack)
			command = new HackCommand(receiver);
		
		else if(actionType== ActionListEnum.Destroy)
			command = new DestroyCommand(receiver);
		
		invoker.SetCommand(command);
		invoker.ExecuteCommand();
	}
}

class CommandInvoker
{
	CommandAbstract Command;
	
	public void SetCommand(CommandAbstract command)
	{
		Command = command;
	}
	
	public void ExecuteCommand()
	{
		Command.Execute();
	}
}
#endregion

#region Setup
public enum ActionListEnum
{
	Hack,
	Destroy
}

interface ActionReceiver
{
	void SetAction(ActionListEnum action);
	void Execute();
}

class GameObjectCommand : ActionReceiver
{
	GameObject Target;
	ActionListEnum CurrentAction;

	public GameObjectCommand(GameObject target) 
	{
		Target = target;
	}

	public void SetAction(ActionListEnum action)
	{
		CurrentAction = action;
	}

	public void Execute()
	{
		switch (CurrentAction)
		{
			case(ActionListEnum.Hack):
				ServerObjectLogic targetScript = (ServerObjectLogic)Target.GetComponent("ServerObjectLogic");
				targetScript.StartServerHack();
				break;

			case(ActionListEnum.Destroy):
				break;
		}
	}
}

abstract class CommandAbstract
{
	public ActionReceiver Receiver = null;
	
	public CommandAbstract(ActionReceiver receiver)
	{
		Receiver = receiver;
	}
	
	public abstract void Execute();
}
#endregion

#region Commands
class HackCommand : CommandAbstract
{
	public HackCommand(ActionReceiver receiver) : base(receiver)
	{ }

	public override void Execute()
	{
		Receiver.SetAction(ActionListEnum.Hack);
		Receiver.Execute();
	}
}

class DestroyCommand : CommandAbstract
{
	public DestroyCommand(ActionReceiver receiver) : base(receiver)
	{ }
	
	public override void Execute()
	{
		Receiver.SetAction(ActionListEnum.Destroy);
		Receiver.Execute();
	}
}
#endregion
