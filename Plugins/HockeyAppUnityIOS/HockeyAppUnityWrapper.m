#import "HockeyAppUnity.h"

static NSDictionary* ConvertPropertiesDictionary(char **keys, char **values, int count) {
  if (!keys || !values || !count) {
    return nil;
  }
  id result = [[NSMutableDictionary alloc] init];
  for (int i = 0; i < count; i++) {
    NSString *key = [NSString stringWithUTF8String:keys[i]];
    NSString *value = [NSString stringWithUTF8String:values[i]];
    [result setValue:value forKey:key];
  }
  return result;
}

static NSDictionary* ConvertMeasurementsDictionary(char **keys, double *values, int count) {
  if (!keys || !values || !count) {
    return nil;
  }
  id result = [[NSMutableDictionary alloc] init];
  for (int i = 0; i < count; i++) {
    NSString *key = [NSString stringWithUTF8String:keys[i]];
    NSNumber *value = [NSNumber numberWithDouble:values[i]];
    [result setValue:value forKey:key];
  }
  return result;
}

void HockeyApp_StartHockeyManager(char *appID, char *serverURL, char *authType, char *secret,
                                  bool updateManagerEnabled, bool userMetricsEnabled, bool autoSendEnabled) {
  [HockeyAppUnity startManagerWithIdentifier:[NSString stringWithUTF8String:appID]
                                   serverURL:[NSString stringWithUTF8String:serverURL]
                                    authType:[NSString stringWithUTF8String:authType]
                                      secret:[NSString stringWithUTF8String:secret]
                        updateManagerEnabled:updateManagerEnabled
                          userMetricsEnabled:userMetricsEnabled
                             autoSendEnabled:autoSendEnabled];
}

void HockeyApp_ShowFeedbackListView() {
  
  [HockeyAppUnity showFeedbackListView];
}

void HockeyApp_CheckForUpdate() {
  
  [HockeyAppUnity checkForUpdate];
}

void HockeyApp_TrackEvent(char *eventName) {
  [HockeyAppUnity trackEventWithName:[NSString stringWithUTF8String:eventName]];
}

void HockeyApp_TrackEventWithPropertiesAndMeasurements(char *eventName,
                                                       char **propertiesKeys, char **propertiesValues, int propertiesCount,
                                                       char **measurementsKeys, double *measurementsValues, int measurementsCount) {
  [HockeyAppUnity trackEventWithName:[NSString stringWithUTF8String:eventName]
                          properties:ConvertPropertiesDictionary(propertiesKeys, propertiesValues, propertiesCount)
                        measurements:ConvertMeasurementsDictionary(measurementsKeys, measurementsValues, measurementsCount)];
}

char* HockeyApp_GetVersionCode() {
  
  const char* versionCode = [[HockeyAppUnity versionCode] UTF8String];
  char* res = (char*)malloc(strlen(versionCode) + 1);
  strcpy(res, versionCode);
  
  return res;
}

char* HockeyApp_GetVersionName() {
  
  const char* versionName = [[HockeyAppUnity versionName] UTF8String];
  char* res = (char*)malloc(strlen(versionName) + 1);
  strcpy(res, versionName);
  
  return res;
}

char* HockeyApp_GetSdkVersion() {
  
  const char* sdkVersion = [[HockeyAppUnity sdkVersion] UTF8String];
  char* res = (char*)malloc(strlen(sdkVersion) + 1);
  strcpy(res, sdkVersion);
  
  return res;
}

char* HockeyApp_GetSdkName() {
  
  const char* sdkName = [[HockeyAppUnity sdkName] UTF8String];
  char* res = (char*)malloc(strlen(sdkName) + 1);
  strcpy(res, sdkName);
  
  return res;
}

char* HockeyApp_GetBundleIdentifier() {
  
  const char* bundleIdentifier = [[HockeyAppUnity bundleIdentifier] UTF8String];
  char* res = (char*)malloc(strlen(bundleIdentifier) + 1);
  strcpy(res, bundleIdentifier);
  
  return res;
}

char* HockeyApp_GetCrashReporterKey() {
  
  const char* crashReporterKey = [[HockeyAppUnity crashReporterKey] UTF8String];
  char* res = (char*)malloc(strlen(crashReporterKey) + 1);
  strcpy(res, crashReporterKey);
  
  return res;
}
