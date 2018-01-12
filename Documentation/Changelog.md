## Changelog

### 5.2.0

This version wraps HockeySDK-iOS 5.1.2. It contains the following changes

- [IMPROVEMENT] This release can be compiled with Xcode 9.2 without warnings. [#502](https://github.com/bitstadium/HockeySDK-iOS/pull/503)
- [BUGFIX] Fix warnings when integrating the SDK as source in Xcode 9. [#501](https://github.com/bitstadium/HockeySDK-iOS/pull/501)
- [BUGFIX] Fix a potential memory leak in `BITChannel`. [#500](https://github.com/bitstadium/HockeySDK-iOS/pull/500)
- [BUGFIX] Version 5.1.X broke support for app extension. We're sorry about this and we've updated our test matrix to make sure this does not happen again. [#499](https://github.com/bitstadium/HockeySDK-iOS/pull/499)
- [BUGFIX] Fix a bug in the Feedback UI when Feedback was shown in landscape. [#498](https://github.com/bitstadium/HockeySDK-iOS/pull/498)

### 5.1.0

This version wraps HockeySDK-iOS 5.1.1. It contains the following changes:

#### HockeySDK-iOS 5.1.1

- [BUGFIX] Fixes a critical bug that would cause apps to freeze when calling `trackEvent` in UIApplicationDelegate callbacks. [#492](https://github.com/bitstadium/HockeySDK-iOS/pull/493)
- [BUGFIX] Fix a critical bug in the crashonly variant of the SDK. [#49](https://github.com/bitstadium/HockeySDK-iOS/pull/494)

#### HockeySDK-iOS 5.1.0

- [FEATURE] Add Turkish localization thanks to [Ozgur](https://github.com/ozgur).[#478](https://github.com/bitstadium/HockeySDK-iOS/pull/478) 
- [FEATURE] Add support to detect low memory and OS kill heuristics for extensions. Thx to [Dave Weston](https://github.com/dtweston) for this! [#470](https://github.com/bitstadium/HockeySDK-iOS/pull/470) 
- [IMPROVEMENT] Support tracking events in the background. [#475](https://github.com/bitstadium/HockeySDK-iOS/pull/475)
- [FIX] Improvements around thread-safety and concurrency for Metrics. [#471](https://github.com/bitstadium/HockeySDK-iOS/pull/471) [#479](https://github.com/bitstadium/HockeySDK-iOS/pull/479)
- [FIX] Fix runtime warnings of Xcode 9's main thread checker tool. [#484](https://github.com/bitstadium/HockeySDK-iOS/pull/484)
- [FIX] Fix caching of previews for attachments to Feedback. [#487](https://github.com/bitstadium/HockeySDK-iOS/pull/487)


### 5.0.0

This version wraps the latest iOS SDK and makes sure the SDK is fully compatible with iOS 11. 

The most notable changes are:

* The SDK is now supporting iOS 8 and later.
* This version brings back the Feedback feature, which requires that you add the `NSPhotoLibraryUsageDescription` key to your `Info.plist` to avoid an AppStore rejection during upload of your app. 
* We now use the plugin**S** folder instead of `Plugin`.

Please also check out the full changelog below:

- [FEATURE] Added support for Metrics in app extensions. [#449](https://github.com/bitstadium/HockeySDK-iOS/pull/449)
- [FEATURE] User Metrics can now be enabled after it was disabled. [#451](https://github.com/bitstadium/HockeySDK-iOS/pull/451)
- [IMPROVEMENT] The code has been cleaned up as we have decided to drop support for iOS 7.
- [IMPROVEMENT] All properties of type `NSString` now use the `copy` attribute.
- [IMPROVEMENT] Use `UIAlertController` in Feedback instead of `UIAlertView`. [#460](https://github.com/bitstadium/HockeySDK-iOS/pull/460)
- [IMPROVEMENT] Don't use `UIAlertView` but `UIAlertController`.[#446](https://github.com/bitstadium/HockeySDK-iOS/pull/446)
- [IMPROVEMENT] `BITAttributedLabel` is now based on `TTTAttributedLabel` 2.0. [#450](https://github.com/bitstadium/HockeySDK-iOS/pull/450)
- [BUGFIX] Fix a bug in `BITAuthenticator`. [#447](https://github.com/bitstadium/HockeySDK-iOS/pull/447)
- [BUGFIX] Fix a bug in `BITImageAnnotation`. [#453](https://github.com/bitstadium/HockeySDK-iOS/pull/453)
- [BUGFIX] The logic that makes sure that the directory for HockeySDK-iOS is excluded from backups was changed, to make sure it doesn't block app launch [#443](https://github.com/bitstadium/HockeySDK-iOS/pull/443).  
- [BUGFIX] Fix bugs in the Feedback UI on iOS 11. [#459](https://github.com/bitstadium/HockeySDK-iOS/pull/459)

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
