using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public static class PostBuildTrigger
{
	enum Position { Begin, End };

	private static string rn = "\n";

	private static string PATH_AUTH = "/Classes/UnityAppController.mm";
	private static string SIGNATURE_AUTH = 
		"- (BOOL)application:(UIApplication*)application openURL:(NSURL*)url sourceApplication:(NSString*)sourceApplication annotation:(id)annotation";
	private static string CODE_AUTH = rn + 
		"if([HockeyAppUnity handleOpenURL:url sourceApplication:sourceApplication annotation:annotation]){" + rn +
        "return YES;" + rn +
    	"}" + rn;
	private static string CODE_LIB_IMPORT = 
		"#import \"HockeyAppUnity.h\"" + rn;

	[PostProcessBuild(100)] 
	public static void OnPostProcessBuild(BuildTarget target, string path)
	{
		Debug.Log( "HockeyApp Unity: Post build script starts");
		if (target == BuildTarget.iOS)
		{
			// Get target for Xcode project
			string projPath = PBXProject.GetPBXProjectPath(path);
			Debug.Log( "HockeyApp Unity: Project path is " + projPath);

			PBXProject proj = new PBXProject();
			proj.ReadFromString(File.ReadAllText(projPath));

			string targetName = PBXProject.GetUnityTargetName();
			string projectTarget = proj.TargetGuidByName(targetName);

			// Add dependencies
			Debug.Log( "HockeyApp Unity: Adding frameworks");

			proj.AddFrameworkToProject(projectTarget, "AssetsLibrary.framework", false);
			proj.AddFrameworkToProject(projectTarget, "CoreText.framework", false);
			proj.AddFrameworkToProject(projectTarget, "MobileCoreServices.framework", false);
			proj.AddFrameworkToProject(projectTarget, "QuickLook.framework", false);
			proj.AddFrameworkToProject(projectTarget, "Security.framework", false);
			proj.AddFrameworkToProject(projectTarget, "Photos.framework", false);
			proj.AddFrameworkToProject(projectTarget, "libz.dylib", false);

			File.WriteAllText(projPath, proj.WriteToString());

			// Insert callback code
			Debug.Log( "HockeyApp Unity: Insert code");

			InsertCodeIntoControllerClass(path);
		}
	}

	private static void InsertCodeIntoControllerClass(string projectPath) {
		string filepath = projectPath + PATH_AUTH;
		string[] methodSignatures = {SIGNATURE_AUTH};
		string[] valuesToAppend = {CODE_AUTH};
		Position[] positionsInMethod = new Position[]{Position.Begin};
				
		InsertCodeIntoClass (filepath, methodSignatures, valuesToAppend, positionsInMethod);
	}

	private static void InsertCodeIntoClass(string filepath, string[] methodSignatures, string[] valuesToAppend, Position[]positionsInMethod) {
		if (!File.Exists (filepath)) {
			return;
		}

		string fileContent = File.ReadAllText (filepath);
		List<int> ignoredIndices = new List<int> ();

		for (int i = 0; i < valuesToAppend.Length; i++) {
			string val = valuesToAppend [i];

			if (fileContent.Contains (val)) {
				ignoredIndices.Add (i);
			}
		}

		string[] fileLines = File.ReadAllLines(filepath);
		List<string> newContents = new List<string>();
		bool found = false;   
		int foundIndex = -1;

		newContents.Add (CODE_LIB_IMPORT);
		foreach(string line in fileLines) {
			if (line.Trim().Contains(CODE_LIB_IMPORT.Trim())){
				continue;
			}

			newContents.Add(line + rn);
			for(int j = 0;j<methodSignatures.Length; j++) {
				if ((line.Trim().Equals(methodSignatures[j])) && !ignoredIndices.Contains(j)){
					foundIndex = j;
					found = true;
				}
			}

			if(found) {
				if((positionsInMethod[foundIndex] == Position.Begin) && line.Trim().Equals("{")){
					newContents.Add(valuesToAppend[foundIndex] + rn);
					found = false;
				} else if((positionsInMethod[foundIndex] == Position.End) && line.Trim().Equals("}")) {
					newContents = newContents.GetRange(0, newContents.Count - 1);
					newContents.Add(valuesToAppend[foundIndex] + rn + "}" + rn);
					found = false;
				}
			}
		}
		string output = string.Join("", newContents.ToArray());
		File.WriteAllText(filepath, output);
	}
}