using UnityEngine;

public class CameraController : MonoBehaviour {

	#region Property Declarations
	static Vector3 _worldPointDragStartPosition;
	public static Vector3 WorldPointDragStartPosition 
	{
		get { return _worldPointDragStartPosition; }
		set 
		{ 
			isActiveScreenPointDragEndPosition = false;
			isActiveWorldPointDragStartPosition = true;
			_worldPointDragStartPosition = PosToWorldPoint(value); 
			//GameController.AddLog("_worldPointDragSource", _worldPointDragSource.ToString(), "");
		}
	}

	static Vector3 _screenPointDragEndPosition;
	public static Vector3 ScreenPointDragEndPosition
	{
		get { return _screenPointDragEndPosition; }
		set 
		{
			isActiveScreenPointDragEndPosition = true;
			_screenPointDragEndPosition = value; 
			//GameController.AddLog("_screenPointDragTarget", _screenPointDragTarget.ToString(), "");
		}
	}

	static Vector3 WorldPointCameraTarget {get; set;}

	static float ZoomCameraTarget {get; set;}
	#endregion

	#region Variable Declarations
	static float dragSpeedx;
	static float dragSpeedy;
	static float minDragDistance;

	static float zoomSpeed;
	static float zoomSpeedFixed;
	static float zoomCameraCurrent;
	static float zoomMaxDistance;
	static float zoomMinDistance;
	static float dragSpeedZoomx;
	static float dragSpeedZoomy;

	static bool isActiveScreenPointDragEndPosition;
	static bool isActiveWorldPointDragStartPosition;
	static bool isProcessingSelection;
	static bool isCameraMoving;
	static bool isCameraZooming;
	#endregion

	// Use this for initialization
	void Start () {
		dragSpeedx = 10f;
		dragSpeedy = 10f;
		minDragDistance = 0.2f;

		zoomSpeed = 3f;
		zoomSpeedFixed = 0.25f;
		zoomCameraCurrent = Camera.main.orthographicSize;
		zoomMinDistance = 3f;
		zoomMaxDistance = 15f;
		dragSpeedZoomx = dragSpeedx / 10;
		dragSpeedZoomy = dragSpeedy / 10;

		isProcessingSelection = false;
		isActiveScreenPointDragEndPosition = false;
		isActiveWorldPointDragStartPosition = false;
		isCameraMoving = false;
		isCameraZooming = false;
	}

	void Update () {

		isCameraZooming = SetNewCameraZoom();
		isCameraMoving = SetNewCameraTarget();

		if(isCameraMoving || isCameraZooming)
		{
			MoveCameraToTarget();
		}
	}

	static Vector3 PosToWorldPoint(Vector3 position)
	{
		Vector3 positionWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2 + position.x, Screen.height/2 - position.y, 0f));
		Vector3 worldPointWithoutZ = new Vector3(positionWorldPoint.x, positionWorldPoint.y, 0f);
		return worldPointWithoutZ;
	}

	static bool SetNewCameraZoom ()
	{
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // zoomIn
		{
			Vector3 mousePositionWZoom = new Vector3(GuiController.MousePositionCurrentCentered.x + zoomSpeed, GuiController.MousePositionCurrentCentered.y + zoomSpeed, 0f);
			Vector3 mousePosition = PosToWorldPoint(mousePositionWZoom);

			ZoomCameraTarget = Mathf.Max(Camera.main.orthographicSize - zoomSpeed, zoomMinDistance);

			WorldPointDragStartPosition = new Vector3(Camera.main.transform.position.x,
			                                          -Camera.main.transform.position.y, 0f);

			WorldPointCameraTarget = new Vector3((mousePosition.x - WorldPointDragStartPosition.x) * dragSpeedZoomx,
			                                     (mousePosition.y - WorldPointDragStartPosition.y) * dragSpeedZoomy, 0f);

			//GameController.AddLog("WorldPointDragSource", WorldPointDragStartPosition.ToString(), "");
			//GameController.AddLog("WorldPointCameraTarget", WorldPointCameraTarget.ToString(), "");

			return true;
			
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0) // ZoomOut
		{
			Vector3 mousePositionWZoom = new Vector3(GuiController.MousePositionCurrentCentered.x + zoomSpeed, GuiController.MousePositionCurrentCentered.y + zoomSpeed, 0f);
			Vector3 mousePosition = PosToWorldPoint(mousePositionWZoom);

			ZoomCameraTarget = Mathf.Min(Camera.main.orthographicSize + zoomSpeed, zoomMaxDistance);

			WorldPointDragStartPosition = new Vector3(Camera.main.transform.position.x,
			                                          -Camera.main.transform.position.y, 0f);

			WorldPointCameraTarget = new Vector3((WorldPointDragStartPosition.x - mousePosition.x) * dragSpeedZoomx,
			                                     (WorldPointDragStartPosition.y - mousePosition.y) * dragSpeedZoomy, 0f);

			return true;
		}
		
		return isCameraZooming;
	}
	
	static bool SetNewCameraTarget()
	{
		if (!isActiveWorldPointDragStartPosition || !isActiveScreenPointDragEndPosition)
		{
			return false;
		}

		Vector3 WorldPointDragTarget = PosToWorldPoint(ScreenPointDragEndPosition);
		WorldPointCameraTarget = new Vector3(((WorldPointDragStartPosition.x - WorldPointDragTarget.x) * dragSpeedx), ((WorldPointDragStartPosition.y - WorldPointDragTarget.y) * dragSpeedy), 0);

		return true;
	}

	public static void MoveCameraToTarget()
	{
		Vector3 WorldPointDeltaTarget = WorldPointCameraTarget * Time.deltaTime;
		Camera.main.transform.Translate (WorldPointDeltaTarget, Space.World);

		if(System.Math.Abs(WorldPointCameraTarget.x - WorldPointDeltaTarget.x) < minDragDistance
		   && System.Math.Abs(WorldPointCameraTarget.y - WorldPointDeltaTarget.y) < minDragDistance)
		{
			//GameController.AddLog("Camera inactive at Pos", GuiController.MousePositionCurrent, "");
			
			isCameraMoving = false;
			isActiveScreenPointDragEndPosition = false;
		}

		if(isCameraZooming)
		{
			zoomCameraCurrent = Camera.main.orthographicSize;
			WorldPointDragStartPosition = GuiController.MousePositionCurrent;
			
			if (System.Math.Abs(zoomCameraCurrent - ZoomCameraTarget) < 0.5f)
			{
				//GameController.AddLog("Camera inactive at Pos", GuiController.MousePositionCurrent, "");

				isCameraZooming = false;
				isCameraMoving = false;
	
				zoomSpeedFixed = 0.25f;
			}
			else
			{
				zoomSpeedFixed = System.Math.Abs(zoomCameraCurrent - ZoomCameraTarget) / 10f;
				float newZoomTarget = (zoomCameraCurrent > ZoomCameraTarget) ? zoomCameraCurrent - zoomSpeedFixed : zoomCameraCurrent + zoomSpeedFixed ;
				Camera.main.orthographicSize = newZoomTarget;
				
			}
		}
	}

}
