using UnityEngine;
using System.Collections.Generic;

public class ContentId
{
	public int Id;
	public GUIContent Content;

	public ContentId (int id, GUIContent content)
	{
		Id = id;
		Content = content;
	}
}

public class Popup
{
	public GameObject RelatedObj;
	public bool MenuClosed;
	public Rect ListRect;

	static int useControlID = -1;
	static float listRectHeight = 1.5f;
	int newSelectedItemIndex = -1;
	int selectedItemIndex = -1;

	ContentId SpecificContentId;
	List<ContentId> ContentIdList;

	string buttonStyle;
	string boxStyle;
	GUIStyle listStyle;
	Rect inputRect;

	public Popup(Rect rect, List<ContentId> listContent, GUIStyle listStyle, GameObject relatedObject)
	{
		this.inputRect = rect;
		this.ContentIdList = listContent;
		this.buttonStyle = "button";
		this.boxStyle = "box";
		this.listStyle = listStyle;
		this.RelatedObj = relatedObject;

		ListRect = new Rect(inputRect.x, inputRect.y, inputRect.width, listStyle.CalcHeight
		                    ((ContentIdList [0]).Content, 1.0f) * listRectHeight * ContentIdList.Count);

		MenuClosed = false;
	}
  
	public Popup(Rect rect, List<ContentId> listContent, string buttonStyle, string boxStyle, GUIStyle listStyle, GameObject relatedObject)
	{
		this.inputRect = rect;
		this.ContentIdList = listContent;
		this.buttonStyle = buttonStyle;
		this.boxStyle = boxStyle;
		this.listStyle = listStyle;
		this.RelatedObj = relatedObject;

		ListRect = new Rect(inputRect.x, inputRect.y, inputRect.width, listStyle.CalcHeight
		                    ((ContentIdList [0]).Content, 1.0f) * listRectHeight * ContentIdList.Count);

		MenuClosed = false;
	}
  
	public ContentId Show()
	{
		int controlID = GUIUtility.GetControlID(FocusType.Passive);       
		EventType evtType = Event.current.GetTypeForControl(controlID);

		List<GUIContent> guiContentList = new List<GUIContent>();

		foreach(ContentId cId in ContentIdList)
		{
			guiContentList.Add(cId.Content);
		}

		newSelectedItemIndex = GUI.SelectionGrid(ListRect, selectedItemIndex, guiContentList.ToArray(), 1, buttonStyle);

		if (GUI.changed)
		{
			switch (evtType)
			{
				case EventType.mouseUp:
				{
					if (newSelectedItemIndex != selectedItemIndex)
					{
						selectedItemIndex = newSelectedItemIndex;
						SpecificContentId = (ContentIdList [selectedItemIndex]);
						MenuClosed = true;

						//GameController.AddLog("Menu Selection", buttonContent.text, RelatedObj.name);
					}
					break;
				}
			}
		}

		return SpecificContentId;
	}
}
