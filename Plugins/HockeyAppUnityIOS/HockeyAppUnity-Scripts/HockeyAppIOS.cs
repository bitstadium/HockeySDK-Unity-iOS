/*
 * Version: 5.2.0
 */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

public class HockeyAppIOS : MonoBehaviour
{

	protected const string HOCKEYAPP_BASEURL = "https://rink.hockeyapp.net/";
	protected const string HOCKEYAPP_CRASHESPATH = "api/2/apps/[APPID]/crashes/upload";
	protected const string LOG_FILE_DIR = "/logs/";
	private const string SERVER_URL_PLACEHOLDER = "your-custom-server-url"; 
	protected const int MAX_CHARS = 199800;
	private static HockeyAppIOS instance;

	public enum AuthenticatorType
	{
		Anonymous,
		Device,
		HockeyAppUser,
		HockeyAppEmail,
		WebAuth
	}

	[Header ("HockeyApp Setup")]
	public string appID = "your-hockey-app-id";
	public string serverURL = SERVER_URL_PLACEHOLDER;

	[Header ("Authentication")]
	public AuthenticatorType authenticatorType;
	public string secret = "your-hockey-app-secret";

	[Header ("Crashes & Exceptions")]
	public bool autoUploadCrashes = false;
	public bool exceptionLogging = true;

	[Header ("Metrics")]
	public bool userMetrics = true;

	[Header ("Version Updates")]
	public bool updateAlert = true;

	#if (UNITY_IPHONE && !UNITY_EDITOR)
	[DllImport("__Internal")]
	private static extern void HockeyApp_StartHockeyManager(string appID, string serverURL, string authType, string secret, bool updateManagerEnabled, bool userMetricsEnabled, bool autoSendEnabled);
	[DllImport("__Internal")]
	private static extern string HockeyApp_GetVersionCode();
	[DllImport("__Internal")]
	private static extern string HockeyApp_GetVersionName();
	[DllImport("__Internal")]
	private static extern string HockeyApp_GetBundleIdentifier();
	[DllImport("__Internal")]
	private static extern string HockeyApp_GetCrashReporterKey();
	[DllImport("__Internal")]
	private static extern string HockeyApp_GetSdkVersion();
	[DllImport("__Internal")]
	private static extern string HockeyApp_GetSdkName();
	[DllImport("__Internal")]
	private static extern void HockeyApp_ShowFeedbackListView();
	[DllImport("__Internal")]
	private static extern void HockeyApp_CheckForUpdate();
	[DllImport("__Internal")]
	private static extern void HockeyApp_TrackEvent(string eventName);
	[DllImport("__Internal")]
	private static extern void HockeyApp_TrackEventWithPropertiesAndMeasurements(string eventName,
			string[] propertiesKeys, string[] propertiesValues, int propertiesCount,
			string[] measurementsKeys, double[] measurementsValues, int measurementsCount);
	#endif

	void Awake ()
	{

		#if (UNITY_IPHONE && !UNITY_EDITOR)
		if (instance != null) {
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
		CreateLogDirectory();

		if (exceptionLogging == true && IsConnected() == true) {
			List<string> logFileDirs = GetLogFiles();
			if (logFileDirs.Count > 0) {
				Debug.Log("Found files: " + logFileDirs.Count);
				StartCoroutine(SendLogs(logFileDirs));
			}
		}
		#endif
	}

	void OnEnable ()
	{
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		if (exceptionLogging == true) {
			System.AppDomain.CurrentDomain.UnhandledException += OnHandleUnresolvedException;
			Application.logMessageReceived += OnHandleLogCallback;
		}
		StartPlugin();
		#endif
	}

	void OnDisable ()
	{
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		if (exceptionLogging == true) {
			System.AppDomain.CurrentDomain.UnhandledException -= OnHandleUnresolvedException;
			Application.logMessageReceived -= OnHandleLogCallback;
		}
		#endif
	}

	void StartPlugin ()
	{
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		string urlString = GetBaseURL();
		string authTypeString = GetAuthenticatorTypeString();
		HockeyApp_StartHockeyManager(appID, urlString, authTypeString, secret, updateAlert, userMetrics, autoUploadCrashes);
		instance = this;
		#endif
	}

	/// <summary>
	/// This method allows to track an event that happened in your app.
	/// Remember to choose meaningful event names to have the best experience when diagnosing your app
	/// in the web portal.
	/// </summary>
	/// <param name="eventName">The name of the event, which should be tracked.</param>
	public static void TrackEvent(string eventName)
	{
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		if (instance != null)
		{
			HockeyApp_TrackEvent(eventName);
		}
		else
		{
			Debug.Log("Failed to track event. SDK has not been initialized, yet.");
		}
		#endif
	}

	/// <summary>
	/// This method allows to track an event that happened in your app.
	/// Remember to choose meaningful event names to have the best experience when diagnosing your app
	/// in the web portal.
	/// </summary>
	/// <param name="eventName">The name of the event, which should be tracked.</param>
	/// <param name="properties">Key value pairs with additional info about the event.</param>
	/// <param name="measurements">Key value pairs, which contain custom metrics.</param>
	public static void TrackEvent(string eventName, IDictionary<string, string> properties, IDictionary<string, double> measurements)
	{
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		if (instance != null)
		{
			HockeyApp_TrackEventWithPropertiesAndMeasurements(eventName,
				properties != null ? properties.Keys.ToArray() : null,
				properties != null ? properties.Values.ToArray() : null,
				properties != null ? properties.Count : 0,
				measurements != null ? measurements.Keys.ToArray() : null,
				measurements != null ? measurements.Values.ToArray() : null,
				measurements != null ? measurements.Count : 0);
		}
		else
		{
			Debug.Log("Failed to track event. SDK has not been initialized, yet.");
		}
		#endif
	}

	/// <summary>
	/// Present the modal feedback list user interface.
	/// </summary>
	public static void ShowFeedbackForm() {
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		HockeyApp_ShowFeedbackListView();
		#endif
	}

	/// <summary>
	/// Call this to trigger a check if there is a new update available on the HockeyApp servers. If there's a new update, an alert will be shown. When running the app from the App Store, this method call is ignored.
	/// </summary>
	public static void CheckForUpdate() {
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		HockeyApp_CheckForUpdate();
		#endif
	}

	/// <summary>
	/// Collect all header fields for the custom exception report.
	/// </summary>
	/// <returns>A list which contains the header fields for a log file.</returns>
	protected virtual List<string> GetLogHeaders ()
	{
		List<string> list = new List<string> ();

		#if (UNITY_IPHONE && !UNITY_EDITOR)
		string bundleID = HockeyApp_GetBundleIdentifier();
		list.Add("Package: " + bundleID);

		string versionCode = HockeyApp_GetVersionCode();
		list.Add("Version Code: " + versionCode);

		string versionName = HockeyApp_GetVersionName();
		list.Add("Version Name: " + versionName);

		string osVersion = "OS: " + SystemInfo.operatingSystem.Replace("iPhone OS ", "");
		list.Add (osVersion);

		list.Add("Model: " + SystemInfo.deviceModel);

		string crashReporterKey = HockeyApp_GetCrashReporterKey();
		list.Add("CrashReporter Key: " + crashReporterKey);

		list.Add("Date: " + DateTime.UtcNow.ToString("ddd MMM dd HH:mm:ss {}zzzz yyyy").Replace("{}", "GMT"));
		#endif

		return list;
	}

	/// <summary>
	/// Create the form data for a single exception report.
	/// </summary>
	/// <param name="log">A string that contains information about the exception.</param>
	/// <returns>The form data for the current exception report.</returns>
	protected virtual WWWForm CreateForm (string log)
	{

		WWWForm form = new WWWForm ();

		#if (UNITY_IPHONE && !UNITY_EDITOR)

		if(!File.Exists(log)) {

			return form;
		}

		byte[] bytes = null;
		using(FileStream fs = File.OpenRead(log)) {

			if (fs.Length > MAX_CHARS) {
				string resizedLog = null;

				using(StreamReader reader = new StreamReader(fs)) {
					reader.BaseStream.Seek( fs.Length - MAX_CHARS, SeekOrigin.Begin );
					resizedLog = reader.ReadToEnd();
				}

				List<string> logHeaders = GetLogHeaders();
				string logHeader = "";

				foreach (string header in logHeaders) {
					logHeader += header + "\n";
				}

				resizedLog = logHeader + "\n" + "[...]" + resizedLog;

				try {
					bytes = System.Text.Encoding.Default.GetBytes(resizedLog);
				} catch (ArgumentException ae) {
					if (Debug.isDebugBuild) Debug.Log("Failed to read bytes of log file: " + ae);
				}
			} else {
				try {
					bytes = File.ReadAllBytes(log);
				} catch (SystemException se) {
					if (Debug.isDebugBuild) {
						Debug.Log("Failed to read bytes of log file: " + se);
					}
				}

			}
		}

		if (bytes != null) {
			form.AddBinaryData("log", bytes, log, "text/plain");
		}
		#endif

		return form;
	}

	/// <summary>
	/// Create the log directory if needed.
	/// </summary>
	protected virtual void CreateLogDirectory ()
	{
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		string logsDirectoryPath = Application.persistentDataPath + LOG_FILE_DIR;

		try {
			Directory.CreateDirectory (logsDirectoryPath);
		} catch (Exception e) {
			if (Debug.isDebugBuild) Debug.Log ("Failed to create log directory at " + logsDirectoryPath + ": " + e);
		}
		#endif
	}

	/// <summary>
	/// Get a list of all existing exception reports.
	/// </summary>
	/// <returns>A list which contains the filenames of the log files.</returns>
	protected virtual List<string> GetLogFiles ()
	{
		List<string> logs = new List<string> ();

		#if (UNITY_IPHONE && !UNITY_EDITOR)
		string logsDirectoryPath = Application.persistentDataPath + LOG_FILE_DIR;

		try {
			DirectoryInfo info = new DirectoryInfo(logsDirectoryPath);
			FileInfo[] files = info.GetFiles();

			if (files.Length > 0) {
				foreach (FileInfo file in files) {
					if (file.Extension == ".log") {
						logs.Add(file.FullName);
					} else {
						File.Delete(file.FullName);
					}
				}
			}
		} catch (Exception e) {
			if (Debug.isDebugBuild) Debug.Log("Failed to write exception log to file: " + e);
		}
		#endif

		return logs;
	}

	/// <summary>
	/// Upload existing reports to HockeyApp and delete them locally.
	/// </summary>
	protected virtual IEnumerator SendLogs (List<string> logs)
	{
		string crashPath = HOCKEYAPP_CRASHESPATH;
		string url = GetBaseURL () + crashPath.Replace ("[APPID]", appID);

		#if (UNITY_IPHONE && !UNITY_EDITOR)
		string sdkVersion = HockeyApp_GetSdkVersion ();
		string sdkName = HockeyApp_GetSdkName ();
		if (sdkName != null && sdkVersion != null) {
			url += "?sdk=" + WWW.EscapeURL(sdkName) + "&sdk_version=" + WWW.EscapeURL(sdkVersion);
		}
		#endif

		foreach (string log in logs) {

			WWWForm postForm = CreateForm (log);
			string lContent = postForm.headers ["Content-Type"].ToString ();
			lContent = lContent.Replace ("\"", "");
			Dictionary<string, string> headers = new Dictionary<string, string> ();
			headers.Add ("Content-Type", lContent);
			WWW www = new WWW (url, postForm.data, headers);
			yield return www;

			if (String.IsNullOrEmpty (www.error)) {
				try {
					File.Delete (log);
				} catch (Exception e) {
					if (Debug.isDebugBuild)
						Debug.Log ("Failed to delete exception log: " + e);
				}
			} else {
				if (Debug.isDebugBuild)
					Debug.Log ("Crash sending error: " + www.error);
			}
		}
	}

	/// <summary>
	/// Write a single exception report to disk.
	/// </summary>
	/// <param name="logString">A string that contains the reason for the exception.</param>
	/// <param name="stackTrace">The stacktrace for the exception.</param>
	protected virtual void WriteLogToDisk (string logString, string stackTrace)
	{

		#if (UNITY_IPHONE && !UNITY_EDITOR)
		string logSession = DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss_fff");
		string log = logString.Replace("\n", " ");
		string[]stacktraceLines = stackTrace.Split('\n');

		log = "\n" + log + "\n";
		foreach (string line in stacktraceLines) {
			if (line.Length > 0) {
				log += "  at " + line + "\n";
			}
		}

		List<string> logHeaders = GetLogHeaders();
		using (StreamWriter file = new StreamWriter(Application.persistentDataPath + LOG_FILE_DIR + "LogFile_" + logSession + ".log", true)) {
			foreach (string header in logHeaders) {
				file.WriteLine(header);
			}
			file.WriteLine(log);
		}
		#endif
	}

	/// <summary>
	/// Get the base url used for custom exception reports.
	/// </summary>
	/// <returns>A formatted base url.</returns>
	protected virtual string GetBaseURL ()
	{

		string baseURL = "";

		#if (UNITY_IPHONE && !UNITY_EDITOR)

		string urlString = serverURL.Trim();
		if (urlString.Length > 0 && urlString != SERVER_URL_PLACEHOLDER) {
			baseURL = urlString;
			if (baseURL[baseURL.Length - 1].Equals("/") != true) {
				baseURL += "/";
			}
		} else {
			baseURL = HOCKEYAPP_BASEURL;
		}
		#endif

		return baseURL;
	}

	/// <summary>
	/// Convert selected authentication type to string.
	/// </summary>
	/// <returns>A formatted base url.</returns>
	protected virtual string GetAuthenticatorTypeString ()
	{
		string authType = "";

		#if (UNITY_IPHONE && !UNITY_EDITOR)
		switch (authenticatorType) {
		case AuthenticatorType.Device:
			authType = "BITAuthenticatorIdentificationTypeDevice";
			break;
		case AuthenticatorType.HockeyAppUser:
			authType = "BITAuthenticatorIdentificationTypeHockeyAppUser";
			break;
		case AuthenticatorType.HockeyAppEmail:
			authType = "BITAuthenticatorIdentificationTypeHockeyAppEmail";
			break;
		case AuthenticatorType.WebAuth:
			authType = "BITAuthenticatorIdentificationTypeWebAuth";
			break;
		default:
			authType = "BITAuthenticatorIdentificationTypeAnonymous";
			break;
		}
		#endif

		return authType;
	}

	/// <summary>
	/// Checks whether internet is reachable
	/// </summary>
	protected virtual bool IsConnected ()
	{
		bool connected = false;
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		if  (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork ||
		(Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)) {
			connected = true;
		}
		#endif

		return connected;
	}

	/// <summary>
	/// Handle a single exception. By default the exception and its stacktrace gets written to disk.
	/// </summary>
	/// <param name="logString">A string that contains the reason for the exception.</param>
	/// <param name="stackTrace">The stacktrace for the exception.</param>
	protected virtual void HandleException (string logString, string stackTrace)
	{
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		WriteLogToDisk(logString, stackTrace);
		#endif
	}

	/// <summary>
	/// Callback for handling log messages.
	/// </summary>
	/// <param name="logString">A string that contains the reason for the exception.</param>
	/// <param name="stackTrace">The stacktrace for the exception.</param>
	/// <param name="type">The type of the log message.</param>
	public void OnHandleLogCallback (string logString, string stackTrace, LogType type)
	{

		#if (UNITY_IPHONE && !UNITY_EDITOR)
		if (LogType.Assert == type || LogType.Exception == type || LogType.Error == type) {
			HandleException(logString, stackTrace);
		}
		#endif
	}

	/// <summary>
	/// Callback for handling unresolved exceptions.
	/// </summary>
	public void OnHandleUnresolvedException (object sender, System.UnhandledExceptionEventArgs args)
	{
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		if (args == null || args.ExceptionObject == null) {
			return;
		}

		if (args.ExceptionObject.GetType() == typeof(System.Exception)) {
			System.Exception e	= (System.Exception)args.ExceptionObject;
			HandleException(e.Source, e.StackTrace);
		}
		#endif
	}
}