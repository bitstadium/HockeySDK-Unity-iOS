## Changelog

### 1.1.8

* [IMPROVEMENT] Upgrade to HockeySDK for iOS 4.1.6
* [IMPROVEMENT] Metrics info will be send to the backend every time the application goes from foreground to background

### 1.1.7
This release officially drops the support for iOS 6.

* [IMPROVEMENT] Upgrade to HockeySDK for iOS 4.1.5

### 1.1.6
* [BUGFIX] Fix plugin initialization bug

### 1.1.5
* [IMPROVEMENT] Upgrade to HockeySDK for iOS 4.1.4
* [UPDATE] Minor bugfixes

### 1.1.4
* [IMPROVEMENT] Upgrade to HockeySDK for iOS 4.1.3
* [NEW] Norwegian (Bokmal) localization
* [NEW] Persian (Farsi) localization

### 1.1.3
* [BREAKING CHANGE] Feedback not supported by version 1.1.3 or higher.
* [IMPROVEMENT] Upgrade to PLCrashReporter 1.3
* [IMPROVEMENT] Upgrade to HockeySDK for iOS 4.1.2
* [IMPROVEMENT] Updated Chinese translations

### 1.1.2
* [BUGFIX] Fix bug where report for managed exceptions didn't contain a CrashReporter Key. The key is needed to get proper user statistics on the portal
* [UPDATE] Plugin now uses HockeySDK iOS 4.0.1
* [UPDATE] Prefix Objective-C classes do avoid duplicate symbol errors

### 1.1.1

* [BUGFIX] Fixes an issue where the whole app's Application Support directory was accidentally excluded from backups.
This SDK release explicitly includes the Application Support directory into backups. If you want to opt-out of this fix and keep the Application Directory's backup flag untouched, add the following to the code of your exported Xcode project:

```objectivec
[[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"kBITExcludeApplicationSupportFromBackup"];
```

### 1.1.0
* [NEW] User Metrics (user and session tracking)
* [NEW] Trigger version update check explicitly
* [IMPROVEMENT] Bitcode support
* [BUGFIX] Issue #22: Avoid app crash when first launching app without internet connection 
* [BUGFIX] Fix crash in native SDK that occured after authentication
* [UPDATE] Plugin now uses HockeySDK iOS 4.0.0
* [UPDATE] Minor bugfixes

### 1.0.11

* Update plugin to HockeySDK iOS 3.8.5
* Fix a crash where appStoreReceiptURL was accidentally accessed on iOS 6

### 1.0.10

* Update plugin to HockeySDK iOS 3.8.4
* Replace old post build script 
* Minor bugfixes

### 1.0.9

* Update plugin to HockeySDK iOS 3.8.2
* Add iOS 9 support
* Minor bugfixes

### 1.0.8

* Update plugin to HockeySDK iOS 3.7.1
* Append SDK and App information to managed exception reports
		* SDK name & version
		* App code & version

### 1.0.7

* Update plugin to HockeySDK iOS 3.6.4
* Fix broken interop & warnings in HockeyAppUnityWrapper.m (thanks to Andy Mroczkowski)
* Fix exception handling
* Report Error types
* Update version numbers
* Extend demo project
 
### 1.0.6

* Fix 'Example Project'
* Add readme section: Add dependencies
* Add entry in troubleshooting section: Disable ARC

### 1.0.5

* Add Unity 5 support
* Remove warnings
* Allow custom plugin folder structure
* Ease plugin configuration
