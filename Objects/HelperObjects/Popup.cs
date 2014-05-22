using UnityEngine;

public class Popup
{
	public GameObject RelatedObj;
	public bool MenuClosed;
	public Rect ListRect;

	static int useControlID = -1;
	static float listRectHeight = 1.5f;
	int newSelectedItemIndex = -1;
	int selectedItemIndex = -1;
	GUIContent buttonContent;
	GUIContent[] listContent;
	string buttonStyle;
	string boxStyle;
	GUIStyle listStyle;
	Rect inputRect;

	public Popup(Rect rect, GUIContent[] listContent, GUIStyle listStyle, GameObject relatedObject)
	{
		this.inputRect = rect;
		this.listContent = listContent;
		this.buttonStyle = "button";
		this.boxStyle = "box";
		this.listStyle = listStyle;
		this.RelatedObj = relatedObject;

		ListRect = new Rect(inputRect.x, inputRect.y, inputRect.width, listStyle.CalcHeight
		                    (listContent [0], 1.0f) * listRectHeight * listContent.Length);

		MenuClosed = false;
	}
  
	public Popup(Rect rect, GUIContent[] listContent, string buttonStyle, string boxStyle, GUIStyle listStyle, GameObject relatedObject)
	{
		this.inputRect = rect;
		this.listContent = listContent;
		this.buttonStyle = buttonStyle;
		this.boxStyle = boxStyle;
		this.listStyle = listStyle;
		this.RelatedObj = relatedObject;

		ListRect = new Rect(inputRect.x, inputRect.y, inputRect.width, listStyle.CalcHeight
		                    (listContent [0], 1.0f) * listRectHeight * listContent.Length);

		MenuClosed = false;
	}
  
	public string Show()
	{
		int controlID = GUIUtility.GetControlID(FocusType.Passive);       
		EventType evtType = Event.current.GetTypeForControl(controlID);

		newSelectedItemIndex = GUI.SelectionGrid(ListRect, selectedItemIndex, listContent, 1, buttonStyle);

		if (GUI.changed)
		{
			switch (evtType)
			{
				case EventType.mouseUp:
				{
					if (newSelectedItemIndex != selectedItemIndex)
					{
						selectedItemIndex = newSelectedItemIndex;
						buttonContent = listContent [selectedItemIndex];
						MenuClosed = true;
						GameController.AddLog("Menu Selection", buttonContent.text, RelatedObj.name);
					}
					break;
				}
			}
		}

		string selectedAction = null;

		if(buttonContent != null)
		{
			selectedAction = buttonContent.text;
		}

		return selectedAction;
	}
}
