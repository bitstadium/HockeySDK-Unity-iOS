/*******************************************************************************
 *
 * Author: Christoph Wendt
 * 
 * Copyright (c) 2013-2014 HockeyApp, Bit Stadium GmbH.
 * All rights reserved.
 * 
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 * 
 ******************************************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class TestUI : MonoBehaviour{

	public GUISkin customUISkin;
	private int controlHeight = 64;
	private int horizontalMargin = 20;
	private int space = 20;

	#if (UNITY_IPHONE && !UNITY_EDITOR)
	[DllImport("__Internal")]
	private static extern void ExamplePlugin_ForceAppCrash();
	#endif

	void OnGUI(){	

		AutoResize (640, 1136);
		GUI.skin = customUISkin;

		GUI.Label(GetControlRect(1), "Choose an exception type");

		if(GUI.Button(GetControlRect(2), "Divide By Zero"))
		{

			int i = 0;
			i = 5 / i;
		}

		if(GUI.Button(GetControlRect(3), "Native Code Crash"))
		{	
			ForceAppCrash();	
		}

		if(GUI.Button(GetControlRect(4), "Index Out Of Range"))
		{
			string[] arr	= new string[3];
			arr[4]	= "Out of Range";
		}

		if(GUI.Button(GetControlRect(5), "Custom Exception"))
		{	
			throw new System.Exception("My Custom Exception");	
		}

		if(GUI.Button(GetControlRect(6), "Custom Coroutine Exception"))
		{	
			StartCoroutine(CorutineCrash());	
		}

		if(GUI.Button(GetControlRect(7), "Null Pointer Exception"))
		{
			string crash = null;
			crash	= crash.ToLower();
		}

		if(GUI.Button(GetControlRect(8), "Coroutine Null Exception"))
		{	
			StartCoroutine(CorutineNullCrash());	
		}
	}
	
	System.Collections.IEnumerator CorutineNullCrash(){

		string crash = null;
		crash	= crash.ToLower();
		yield break;
	}
	
	System.Collections.IEnumerator CorutineCrash(){	

		throw new System.Exception("Custom Coroutine Exception");
	}

	private Rect GetControlRect(int controlIndex){

		return new Rect (horizontalMargin,
		                controlIndex * (controlHeight + space),
		                640 - (2 * horizontalMargin),
		                controlHeight);
	}

	public void AutoResize(int screenWidth, int screenHeight){

		Vector2 resizeRatio = new Vector2((float)Screen.width / screenWidth, (float)Screen.height / screenHeight);
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(resizeRatio.x, resizeRatio.y, 1.0f));
	}

	public void ForceAppCrash(){
		
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		ExamplePlugin_ForceAppCrash();
		#endif
	}
}