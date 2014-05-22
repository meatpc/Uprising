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

		GameObject hitObject = new GameObject("No Object");

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider != null)
			{
				hitObject = hit.collider.gameObject;
			}
		}

		AddLog("Click On Object", GuiController.MousePositionCurrent, hitObject.gameObject.name);

		switch (GuiController.MouseClickInputCurrent)
		{
			case GuiController.MouseClickInputs.LeftClick:
				CameraController.WorldPointDragStartPosition = GuiController.MousePositionCurrent;
				GuiController.CloseCurrentMenu();
				break;

			case GuiController.MouseClickInputs.RightClick:
				OpenActionMenu(hitObject);
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
		if(ObjectController.ServerObjects.Contains(hitObject))
		{
			GameObject server = hitObject;
			
			GUIContent hack = new GUIContent("Hack!");
			GUIContent destroy = new GUIContent("Destroy!");
			GUIContent[] gcArr = new GUIContent[2];
			gcArr [0] = hack;
			gcArr [1] = destroy;
			
			GuiController.OnClickedMenu += OnMenuClick;
			GuiController.CreateMenu(hitObject, gcArr);
		}
	}
	
	static void OnMenuClick(GameObject relatedObject, string command)
	{
		switch(command)
		{
			case "Hack!":
				CommandController.NewCommand (ActionListEnum.Hack, relatedObject);
				break;
			case "Destroy!":
				DestroyImmediate(relatedObject, true);
				break;
		}
		
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
