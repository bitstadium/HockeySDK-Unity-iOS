## Introduction 

The HockeyAppUnity-iOS plugin implements support for using HockeyApp in your Unity3D-iOS builds. It easily lets you keep track of crashes that have been caused by your scripts or Objective-C code.

## Requirements

* Unity 4.2 or newer (older versions might work, but we haven't tested them).
* iOS 5 or newer.

## Installation & Setup

The following steps illustrate how to integrate the HockeyAppUnity-iOS plugin:

1. Copy the **Plugins** folder into the **Assets** directory of your Unity3D project.

2. Create an empty game object and add the **HockeyAppIOS.cs** as one of its components.

3. Select the game object in the **Hierarchy** pane and fill in the App ID provided by HockeyApp (Inspector window). If you want to get more precise information about exceptions in your Unity3D scripts, you can also check the **Exception Logging** property.

4. You are now ready to build the Xcode project: Select **File -> Build Settings...** and switch to **iOS** in the platform section. Check **Development Build** and **Script Debugging**.

5. Open the player settings and make sure that **Bundle identifier** (**Other settings -> Identification**) equals the package name of your HockeyApp app.

6. If you want to enable exception logging, please also select **Other settings -> Optimization -> Slow and safe** as well. Otherwise all exceptions will result in an app crash.

7. Press the **Build** button.

8. Open the Xcode project Unity3D has created for you. Select the **Build Phases** tab of your target and add the **Security.framework** and **CoreText.framework** (**Link binary with libraries**).

9. Before you can build and run your app you need to add the **HockeySDKResources.bundle** (located at **Plugins/iOS**) to the **Libraries** folder. Please repeat this step after each rebuild of the Xcode project.

10. Thats's it. You can now build and run your app.
