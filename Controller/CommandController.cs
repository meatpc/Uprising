using UnityEngine;
using System.Collections.Generic;


#region Client and Invoker
public class CommandController : MonoBehaviour {
	ActionReceiver receiver;
	CommandAbstract command;
	List<CommandInvoker> Invokers;

	HackCommand hackCmd;
	DestroyCommand destroyCmd;

	static readonly CommandController instance = new CommandController();
	public static CommandController Instance
	{
		get
		{
			return instance;
		}
	}

	public CommandController()
	{
		Invokers = new List<CommandInvoker>();
	}

	public int CreateCommand(ActionListEnum actionType, GameObject targetServer)
	{
		receiver = new GameObjectCommand(targetServer);
		
		if(actionType== ActionListEnum.Hack)
			command = new HackCommand(receiver);
		
		else if(actionType== ActionListEnum.Destroy)
			command = new DestroyCommand(receiver);
		
		CommandInvoker invoker = new CommandInvoker(command);
		Invokers.Add(invoker);

		int invokerIndex = Invokers.IndexOf(invoker);

		return invokerIndex;
	}

	public string GetLabelFromIndex(int index)
	{
		CommandInvoker invoker = Invokers[index];
		string label = invoker.Command.Label;
		return label;
	}

	public void ExcecuteCommandFromIndex(int index)
	{
		Invokers[index].ExecuteCommand();
	}
}

class CommandInvoker
{
	public CommandAbstract Command;

	public CommandInvoker()
	{ }

	public CommandInvoker(CommandAbstract command)
	{
		SetCommand(command);
	}

	void SetCommand(CommandAbstract command)
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
	public string Label;

	public CommandAbstract(ActionReceiver receiver)
	{
		Receiver = receiver;
		Label = "No Label";
	}
	
	public abstract void Execute();
}
#endregion

#region Commands
class HackCommand : CommandAbstract
{
	public HackCommand(ActionReceiver receiver) : base(receiver)
	{ 
		Label = "Hack!";
	}

	public override void Execute()
	{
		Receiver.SetAction(ActionListEnum.Hack);
		Receiver.Execute();
	}
}

class DestroyCommand : CommandAbstract
{
	public DestroyCommand(ActionReceiver receiver) : base(receiver)
	{
		Label = "Destroy!";
	}
	
	public override void Execute()
	{
		Receiver.SetAction(ActionListEnum.Destroy);
		Receiver.Execute();
	}
}
#endregion
