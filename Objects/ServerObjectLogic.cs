using UnityEngine;

public class ServerObjectLogic : MonoBehaviour {

	TextMesh hackProgressText;
	public float HackProgress;
	public bool IsBeingHacked;

	// Use this for initialization
	void Start () {
		hackProgressText = (TextMesh) GetComponentInChildren(typeof(TextMesh));
		HackProgress = 0;
		IsBeingHacked = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (IsBeingHacked) {
			HackProgress = HackProgress + 2f * Time.deltaTime;
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
		}
		else if (HackProgress < 0.0f)
		{
			IsBeingHacked = false;
			HackProgress = 0.0f;
		}

		return HackProgress;
	}
}