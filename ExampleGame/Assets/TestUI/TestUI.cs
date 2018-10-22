using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class TestUI : MonoBehaviour{

	public GUISkin customUISkin;
	private int controlHeight = 60;
	private int horizontalMargin = 16;
	private int topMargin = 20;
	private int space = 16;

	#if (UNITY_IPHONE && !UNITY_EDITOR)
	[DllImport("__Internal")]
	private static extern void ExamplePlugin_ForceAppCrash();
	#endif

	void OnGUI(){	

		AutoResize (640, 1136);
		GUI.skin = customUISkin;

		GUI.Label(GetControlRect(0), "Choose an exception type");

		if(GUI.Button(GetControlRect(1), "Divide By Zero"))
		{
			// ObjectiveC does not trigger Division by zero exception for NSInteger type. 
			// We can see "DivideByZeroException: Division by zero" exception by using decimal type.
			// https://developer.apple.com/documentation/foundation/nsdecimalnumberdividebyzeroexception?language=objc
			decimal i = 0;
			i = 5 / i;
		}

		if(GUI.Button(GetControlRect(2), "Native Code Crash"))
		{	
			ForceAppCrash();	
		}

		if(GUI.Button(GetControlRect(3), "Index Out Of Range"))
		{
			string[] arr	= new string[3];
			arr[4]	= "Out of Range";
		}

		if(GUI.Button(GetControlRect(4), "Custom Exception"))
		{	
			throw new Exception("My Custom Exception");	
		}

		if(GUI.Button(GetControlRect(5), "Custom Coroutine Exception"))
		{	
			StartCoroutine(CorutineCrash());	
		}

		if(GUI.Button(GetControlRect(6), "Handled Null Pointer Exception"))
		{	
			try {
				NullReferenceException();
			} catch (Exception e) {
				throw new Exception("Null Pointer Exception");
			}	
		}

		if(GUI.Button(GetControlRect(7), "Null Pointer Exception"))
		{
			NullReferenceException();
		}

		if(GUI.Button(GetControlRect(8), "Coroutine Null Exception"))
		{	
			StartCoroutine(CorutineNullCrash());	
		}

		GUI.Label(GetControlRect(9), "Features");

		if(GUI.Button(GetControlRect(10), "Check For Update"))
		{	
			CheckForUpdate();
		}

		if (GUI.Button(GetControlRect(11), "Track Event"))
		{
			TrackEvent();
		}

		if (GUI.Button(GetControlRect(12), "Show Feedback"))
		{
			ShowFeedbackForm();
		}
	}
	public void AutoResize(int screenWidth, int screenHeight){
		
		Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));
	}
	
	System.Collections.IEnumerator CorutineNullCrash(){

		string crash = null;
		crash	= crash.ToLower();
		yield break;
	}
	
	System.Collections.IEnumerator CorutineCrash(){	

		throw new Exception("Custom Coroutine Exception");
	}

	private Rect GetControlRect(int controlIndex){

		return new Rect (horizontalMargin,
		                topMargin + controlIndex * (controlHeight + space),
		                640 - (2 * horizontalMargin),
		                controlHeight);
	}

	public void NullReferenceException(){
		object testObject = null;
		testObject.GetHashCode();
	}

	public void ForceAppCrash(){
		
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		ExamplePlugin_ForceAppCrash();
		#endif
	}

	public void CheckForUpdate(){
        Debug.Log("CheckForUpdate");
		HockeyAppIOS.CheckForUpdate ();
    }

    public void ShowFeedbackForm(){
        Debug.Log("ShowFeedbackForm");
        HockeyAppIOS.ShowFeedbackForm ();
    }
	public void TrackEvent(){
		HockeyAppIOS.TrackEvent("Test Unity");
		HockeyAppIOS.TrackEvent("Test Unity with properties and measurements",
		    new Dictionary<string, string> { { "Prop1", "Val1" }, { "Prop2", "Val2" } },
		    new Dictionary<string, double> { { "M1", 1.0 }, { "M2", 2.0 } });
	}
}
