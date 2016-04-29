## Changelog

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
