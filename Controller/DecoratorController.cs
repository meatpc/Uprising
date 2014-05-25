using UnityEngine;
using System.Collections;

public class DecoratorController : MonoBehaviour {

	public static void AddProgressBarToObject(GameSystemAbstract obj)
	{
		ConcreteComponent comp = new ConcreteComponent(obj);

		TextFieldDecorator newFldDecorator = new TextFieldDecorator(comp);
		newFldDecorator.Operation();
	}

	public static void MakeFriendly(GameSystemAbstract obj)
	{
		ConcreteComponent comp = new ConcreteComponent(obj);
		
		FriendlyObject newFldDecorator = new FriendlyObject(comp);
		newFldDecorator.Operation();
	}

	public static void MakeHostile(GameSystemAbstract obj)
	{
		ConcreteComponent comp = new ConcreteComponent(obj);
		
		TextFieldDecorator newFldDecorator = new TextFieldDecorator(comp);
		newFldDecorator.Operation();
	}
}

#region Setup
public abstract class ComponentAbstract
{
	public abstract void Operation();
}


class ConcreteComponent : ComponentAbstract
{
	public GameSystemAbstract RelatedObj;

	public ConcreteComponent(GameSystemAbstract obj)
	{
		SetRelatedObject(obj);
	}

	void SetRelatedObject(GameSystemAbstract obj)
	{
		RelatedObj = obj;
	}

	public override void Operation()
	{
	}
}

abstract class Decorator : ComponentAbstract
{
	protected ConcreteComponent Component;

	public Decorator()
	{}

	public Decorator(ConcreteComponent component)
	{
		SetComponent(component);
	}

	void SetComponent(ConcreteComponent component)
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
#endregion

#region Concrete Decorators
class TextFieldDecorator : Decorator
{
	public TextFieldDecorator(ConcreteComponent component) : base(component)
	{}

	public override void Operation()
	{
		base.Operation();
		AddedBehaviour();
	}

	void AddedBehaviour()
	{
		GameObject textProgressTemplate =  Resources.Load<GameObject>("TextProgress1");

		if(textProgressTemplate != null)
		{
			Server serverObject = (Server) Component.RelatedObj;

			GameObject progressBar = (GameObject) GameObject.Instantiate(textProgressTemplate);
			progressBar.transform.parent = serverObject.InternalGameObject.transform;
			progressBar.transform.localPosition = new Vector3(0.3f, -1, 0);
			
			serverObject.ProgressBar = progressBar;
		}
	}
}

class FriendlyObject : Decorator
{
	public FriendlyObject(ConcreteComponent component) : base(component)
	{}
	
	public override void Operation()
	{
		base.Operation();
		AddedBehaviour();
	}
	
	void AddedBehaviour()
	{
		Sprite spriteGreen = (Sprite) Resources.Load<Sprite>("server-icon_green");

		if(spriteGreen != null)
		{
			Server serverObject = (Server) Component.RelatedObj;
			SpriteRenderer sprite = (SpriteRenderer) serverObject.InternalGameObject.GetComponent(typeof(SpriteRenderer));

			sprite.sprite = spriteGreen;
		}
	}
}

class HostileObject : Decorator
{
	public HostileObject(ConcreteComponent component) : base(component)
	{}
	
	public override void Operation()
	{
		base.Operation();
		AddedBehaviour();
	}
	
	void AddedBehaviour()
	{
		Sprite spriteGreen = (Sprite) Resources.Load<Sprite>("server-icon_red");
		
		if(spriteGreen != null)
		{
			Server serverObject = (Server) Component.RelatedObj;
			SpriteRenderer sprite = (SpriteRenderer) serverObject.InternalGameObject.GetComponent(typeof(SpriteRenderer));
			
			sprite.sprite = spriteGreen;
		}
	}
}
#endregion