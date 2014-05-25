using UnityEngine;
using System.Collections.Generic;

public class GuiController : MonoBehaviour {

	#region Property Declarations
	public static Vector3 MousePositionCurrent;
	public static Vector3 MousePositionCurrentCentered
	{
		get
		{
			return new Vector3 (MousePositionCurrent.x - Screen.width/2, MousePositionCurrent.y - Screen.height/2, 0f);
		}
		set
		{Vector3 x = value;}
	}
	#endregion

	#region Variable Declarations
	public static bool IsMouseClicked;
	public static bool IsMouseHeld;

	static Vector3 MousePositionLast;
	static Vector3 ClickPositionLast;
	static Vector3 ClickPositionWorldLast;
	static Vector3 SelectionMenuPosition;

	static Popup SelectionMenu;
	#endregion

	#region Delegate Declarations
	public delegate void MenuClickAction(GameObject relatedObject, int commandIndex);
	public static event MenuClickAction OnClickedMenu;

	public delegate void MouseClickAction();
	public static event MouseClickAction OnClickedMouse;
	#endregion

	#region Enum Declaration
	public enum MouseClickInputs {None, LeftClick, RightClick, LeftDrag, RightDrag}
	public static MouseClickInputs MouseClickInputCurrent;
	#endregion

	// Use this for initialization
	void Start () {
		SelectionMenuPosition = new Vector3 (0, 0, 0);
		MousePositionCurrent = new Vector3 (0, 0, 0);
		MousePositionLast = new Vector3 (0, 0, 0);
		ClickPositionLast = new Vector3 (0, 0, 0);
	}

	void Update()
	{
		MousePositionLast = MousePositionCurrent;
		MousePositionCurrent = new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 0);

		ProcessMouseInputActions();
	}

	void OnGUI(){
		if (SelectionMenu != null && !SelectionMenu.MenuClosed) {
			ContentId command = SelectionMenu.Show();

			if(SelectionMenu != null && command != null)
			{
				GameObject relObj = SelectionMenu.RelatedObj;
				OnClickedMenu(relObj, command.Id);
			}
		}
	}

	static void ProcessMouseInputActions()
	{
		MouseClickInputCurrent = MouseClickInputs.None;

		if(Input.GetMouseButtonDown(0))
		{
			IsMouseClicked = true;
			ClickPositionLast = MousePositionCurrent;
			MouseClickInputCurrent = MouseClickInputs.LeftClick;
			OnClickedMouse();
		}

		if(Input.GetMouseButtonDown(1))
		{
			IsMouseClicked = true;
			ClickPositionLast = MousePositionCurrent;
			MouseClickInputCurrent = MouseClickInputs.RightClick;
			OnClickedMouse();
		}

		if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
		{
			IsMouseClicked = false;
			IsMouseHeld = false;
		}

		if(IsMouseClicked)
		{
			if(Input.GetMouseButton(0))
			{
				IsMouseHeld = true;

				if (MousePositionLast != MousePositionCurrent)
				{
					MouseClickInputCurrent = MouseClickInputs.LeftDrag;
					OnClickedMouse();
				}
			}
			else if(Input.GetMouseButton(1))
			{
				IsMouseHeld = true;

				if (MousePositionLast != MousePositionCurrent)
				{
					MouseClickInputCurrent = MouseClickInputs.RightDrag;
					OnClickedMouse();
				}
			}
		}
	}

	public static void DisplayNewValue(TextMesh textObject, float value)
	{
		string valueString = value.ToString("0.0");
		textObject.text = string.Concat(valueString,"%");
	}
	
	public static void CreateMenu(GameObject forObject, List<int> commandIds)
	{
		List<ContentId> guiContents = new List<ContentId>();

		foreach(int commandId in commandIds)
		{
			GUIContent guiContent = new GUIContent(CommandController.Instance.GetLabelFromIndex(commandId));
			ContentId contentId = new ContentId(commandId, guiContent);

			guiContents.Add(contentId);
		}

		float x = MousePositionCurrent.x;
		float y = MousePositionCurrent.y;
		Rect rect = new Rect(x, y, 150f, 50f);
		
		SelectionMenu = new Popup (rect, guiContents, GUIStyle.none, forObject);
	}

	public static bool ShouldCloseCurrentMenu()
	{
		bool overGUI = false;
		
		if(SelectionMenu != null)
		{
			overGUI = SelectionMenu.ListRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y));
		}
		
		if(!overGUI)
		{
			SelectionMenu = null;
			return true;
		}
		return false;
	}


	#region Public Methods



	#endregion

}
