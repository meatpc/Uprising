using UnityEngine;

public class ServerObjectLogic : MonoBehaviour {

	TextMesh hackProgressText;
	public float HackProgress;
	public bool IsBeingHacked;

	public delegate void HackingFinishedAction(float hackProgress);
	public event HackingFinishedAction OnHackingFinished;

	public ServerObjectLogic()
	{
	}

	// Use this for initialization
	void Start () {
		hackProgressText = (TextMesh) GetComponentInChildren(typeof(TextMesh));
		HackProgress = 0;
		IsBeingHacked = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (IsBeingHacked) {
			HackProgress = HackProgress + 10f * Time.deltaTime;
			GuiController.DisplayNewValue (hackProgressText, CheckHackingProgress ());
		}
	}


	public void StartServerHack()
	{
		IsBeingHacked = !IsBeingHacked;
	}


	float CheckHackingProgress(){
		if (HackProgress > 100.0f)
		{
			IsBeingHacked = false;
			HackProgress = 100.0f;

			OnHackingFinished(HackProgress);
		}
		else if (HackProgress < 0.0f)
		{
			IsBeingHacked = false;
			HackProgress = 0.0f;

			OnHackingFinished(HackProgress);
		}

		return HackProgress;
	}
}