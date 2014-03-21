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
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.InteropServices;

public class HockeyAppIOS : MonoBehaviour {
	
	private const string APP_CRASHED_KEY = "CRASH";
	private const string HOCKEYAPP_BASEURL = "https://rink.hockeyapp.net/api/2/apps/";
	private const string HOCKEYAPP_CRASHESPATH = "/crashes/upload";
	private const int MAX_CHARS = 199800;
	public string appID = "your-hockey-app-id";
	public Boolean exceptionLogging = false;

	#if (UNITY_IPHONE && !UNITY_EDITOR)
	[DllImport("__Internal")]
	private static extern void HockeyApp_StartHockeyManager(string identifier);
	[DllImport("__Internal")]
	private static extern string HockeyApp_GetAppVersion();
	[DllImport("__Internal")]
	private static extern string HockeyApp_GetBundleIdentifier();
	#endif
	
	void Awake(){
		
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		DontDestroyOnLoad(gameObject);
		HockeyApp_StartHockeyManager(appID);
		CheckLogs();
		#endif
	}

	public void OnEnable(){
		
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		if(exceptionLogging){
			System.AppDomain.CurrentDomain.UnhandledException += new System.UnhandledExceptionEventHandler(OnHandleUnresolvedException);
			Application.RegisterLogCallback(OnHandleLogCallback);
		}
		#endif
	}
	
	public void OnDisable(){
		
		Application.RegisterLogCallback(null);
	}
	
	void OnDestroy(){
		
		Application.RegisterLogCallback(null);
	}

	/// <summary>
	/// Collect the header fields of the log file.
	/// </summary>
	/// <returns>A list which contains the header fields for a log file.</returns>
	private List<string> GetLogHeaders() {
		List<string> list = new List<string>();
		
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		string bundleID = HockeyApp_GetBundleIdentifier();
		list.Add("Package: " + bundleID);
		
		string appVersion = HockeyApp_GetAppVersion();
		list.Add("Version: " + appVersion);
		
		string osVersion = "OS: " + SystemInfo.operatingSystem.Replace("iPhone OS ", "");
		list.Add (osVersion);
		
		list.Add("Model: " + SystemInfo.deviceModel);

		list.Add("Date: " + DateTime.UtcNow.ToString("ddd MMM dd HH:mm:ss {}zzzz yyyy").Replace("{}", "GMT"));
		#endif
		
		return list;
	}
	
	/// <summary>
	/// Create the form data for a single exception report.
	/// </summary>
	/// <param name="log">A string that contains information about the exception.</param>
	/// <returns>The form data for the current exception report.</returns>
	private WWWForm CreateForm(string log){
		
		WWWForm form = new WWWForm();
		
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		FileStream fs = File.OpenRead(log);
		byte[] bytes = null;
		if (fs.Length > MAX_CHARS)
		{
			StreamReader reader = new StreamReader(fs);
			reader.BaseStream.Seek( fs.Length - MAX_CHARS, SeekOrigin.Begin );
			string resizedLog = reader.ReadToEnd();
			reader.Close();
			
			List<string> logHeaders = GetLogHeaders();
			string logHeader = "";
			
			foreach (string header in logHeaders)
			{
				logHeader += header + "\n";
			}
			
			resizedLog = logHeader + "\n" + "[...]" + resizedLog;
			bytes = System.Text.Encoding.Default.GetBytes(resizedLog);
		}else
		{
			bytes = File.ReadAllBytes(log);
		}
		
		fs.Close();
		form.AddBinaryData("log", bytes, log, "text/plain");
		#endif
		
		return form;
	}

	/// <summary>
	/// Handle existing exception reports.
	/// </summary>
	private void CheckLogs() {
		
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		string logsDirectoryPath = Application.persistentDataPath + "/logs/";

		if (Directory.Exists(logsDirectoryPath) == false)
		{
			Directory.CreateDirectory(logsDirectoryPath);
		}
		
		DirectoryInfo info = new DirectoryInfo(logsDirectoryPath);
		bool appDidCrashed = PlayerPrefs.HasKey(APP_CRASHED_KEY);
		PlayerPrefs.DeleteKey(APP_CRASHED_KEY);
		FileInfo[] files = info.GetFiles();
		List<string> logs = new List<string>();
		
		if (files.Length > 0)
		{
			foreach (FileInfo file in files)
			{
				if (exceptionLogging && appDidCrashed == true && file.Extension == ".log")
				{
					logs.Add(file.FullName);
				}else
				{
					File.Delete(file.FullName);
				}
			}
		}
		
		if ( logs.Count > 0 && exceptionLogging)
		{

			StartCoroutine(SendLogs(logs));
		}
		#endif
	}
	
	/// <summary>
	/// Upload existing reports to HockeyApp.
	/// </summary>
	private IEnumerator SendLogs(List<string> logs){
		
		foreach (string log in logs)
		{		
			string url = HOCKEYAPP_BASEURL + appID + HOCKEYAPP_CRASHESPATH;
			WWWForm postForm = CreateForm(log);
			string lContent = postForm.headers["Content-Type"].ToString();
			lContent = lContent.Replace("\"", "");
			Hashtable headers = new Hashtable();
			headers.Add("Content-Type", lContent);
			WWW www = new WWW(url, postForm.data, headers);
			yield return www;
		}
	}

	/// <summary>
	/// Create the form data for a single exception report.
	/// </summary>
	/// <param name="logString">A string that contains the reason for the exception.</param>
	/// <param name="stackTrace">The stacktrace for the exception.</param>
	private void WriteLogToDisk(string logString, string stackTrace){
		
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		string logSession = DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss_fff");
		string log = logString.Replace("\n", " ");
		string[]stacktraceLines = stackTrace.Split('\n');
		
		log = "\n" + log + "\n";
		foreach (string line in stacktraceLines)
		{
			if(line.Length > 0)
			{
				log +="  at " + line + "\n";
			}
		}
		
		List<string> logHeaders = GetLogHeaders();
		using (StreamWriter file = new StreamWriter(Application.persistentDataPath + "/logs/LogFile_" + logSession + ".log", true))
		{
			foreach (string header in logHeaders)
			{
				file.WriteLine(header);
			}
			
			file.WriteLine(log);
			
			PlayerPrefs.SetInt(APP_CRASHED_KEY, 1);
		}
		
		#endif
	}
	
	/// <summary>
	/// Handle log messages.
	/// </summary>
	/// <param name="logString">A string that contains the reason for the exception.</param>
	/// <param name="stackTrace">The stacktrace for the exception.</param>
	/// <param name="type">The type of the log message.</param>
	public void OnHandleLogCallback(string logString, string stackTrace, LogType type){
		
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		if(LogType.Assert != type && LogType.Exception != type)	
		{	
			return;	
		}		
		
		WriteLogToDisk(logString, stackTrace);
		#endif
	}

	public void OnHandleUnresolvedException(object sender, System.UnhandledExceptionEventArgs args){
		
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		if(args == null || args.ExceptionObject == null)
		{	
			return;	
		}

		if(args.ExceptionObject.GetType() != typeof(System.Exception))
		{	
			return;	
		}
		
		System.Exception e	= (System.Exception)args.ExceptionObject;
		WriteLogToDisk(e.Source, e.StackTrace);
		#endif
	}
}
