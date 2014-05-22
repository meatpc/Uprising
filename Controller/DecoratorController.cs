using UnityEngine;
using System.Collections;

public class DecoratorController : MonoBehaviour {

}


public abstract class ComponentAbstract
{
	public abstract void Operation();
}


class ConcreteComponent : ComponentAbstract
{
	public override void Operation()
	{
	}
}

abstract class Decorator : ComponentAbstract
{
	protected ConcreteComponent Component;

	public void SetComponent(ConcreteComponent component)
	{
		Component = component;
	}

	public override void Operation()
	{
		if(Component != null)
		{
			Component.Operation();
		}
	}
}

class ConcreteDecoratorA : Decorator
{
	public override void Operation()
	{
		base.Operation();
		AddedBehaviour();
	}

	void AddedBehaviour()
	{
	}
}