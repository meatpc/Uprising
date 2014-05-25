using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	void Start()
	{
		GUIText DebugT = (GUIText)GameObject.FindWithTag("EditorOnly").GetComponent("GUIText");
		DebugT.text = "\n";

		GuiController.OnClickedMouse += ProcessMouseInput;
	}

	void Update()
	{
		
		if (Input.GetKeyDown (KeyCode.Space))
		{
			CameraController.MoveCameraToTarget();
			AddLog("Space Key Pressed","","");
		}
	}

	static void ProcessMouseInput()
	{
		RaycastHit hit;
		Ray ray;

		GameObject hitObject = null;

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider != null)
			{
				hitObject = hit.collider.gameObject;
			}
		}

		switch (GuiController.MouseClickInputCurrent)
		{
			case GuiController.MouseClickInputs.LeftClick:
				if(hitObject != null)
				{
					AddLog("Left Click On Object", GuiController.MousePositionCurrent, hitObject.name);
				}

				CameraController.WorldPointDragStartPosition = GuiController.MousePositionCurrent;

				if(GuiController.ShouldCloseCurrentMenu())
					GuiController.OnClickedMenu -= OnMenuClick;
					
				break;

			case GuiController.MouseClickInputs.RightClick:
				if(hitObject != null)
				{
					AddLog("Right Click On Object", GuiController.MousePositionCurrent, hitObject.name);
					OpenActionMenu(hitObject);
				}

				break;

			case GuiController.MouseClickInputs.LeftDrag:
				CameraController.ScreenPointDragEndPosition = GuiController.MousePositionCurrent;
				break;

			case GuiController.MouseClickInputs.RightDrag:
				break;
		}
	}
	
	static void OpenActionMenu(GameObject hitObject)
	{
		int i = ObjectController.AllSystemObjects.FindIndex(m => m.InternalGameObject.GetInstanceID().Equals(hitObject.GetInstanceID()));

		if(i >= 0)
		{
			GameSystemAbstract currentSystem = ObjectController.AllSystemObjects[i];
			System.Type typeOfObject = currentSystem.GetType();

			if(typeOfObject.Equals(typeof(Server)))
			{
				List<int> commandIds = new List<int>();
				commandIds.Add(CommandController.Instance.CreateCommand (ActionListEnum.Hack, hitObject));
				commandIds.Add(CommandController.Instance.CreateCommand (ActionListEnum.Destroy, hitObject));
				
				GuiController.OnClickedMenu += OnMenuClick;
				GuiController.CreateMenu(hitObject, commandIds);
			}
		}
	}
	
	static void OnMenuClick(GameObject relatedObject, int commandIndex)
	{
		CommandController.Instance.ExcecuteCommandFromIndex(commandIndex);
		GuiController.OnClickedMenu -= OnMenuClick;
	}

	public static void AddLog(string text, object value, string obj)
	{
		GUIText DebugT = (GUIText)GameObject.FindWithTag("EditorOnly").GetComponent("GUIText");

		string currentText = DebugT.text;
		string tempText = currentText;
		string newText = "";
		int posOfLineBreak = -1;
		int currentLine = 1;

		int maxNoOfLines = 10;

		do
		{
			if(tempText.Length <= 0)
				break;

			currentLine++;
			posOfLineBreak = tempText.IndexOf("\n", System.StringComparison.Ordinal);

			if(posOfLineBreak > 0)
			{
				newText += tempText.Substring(0, posOfLineBreak + 1);
				tempText = tempText.Substring(posOfLineBreak + 1);
			}

		}while(currentLine < maxNoOfLines);

		string newLine = text + ": Value = [" + value + "] Object = [" + obj + "]";
		DebugT.text = newLine + "\n" + newText;
	}
}
