#import "HockeyAppUnity_tvOS.h"
#import "BITUtils.h"

@interface HockeyAppUnity()

@end

@implementation HockeyAppUnity

#pragma mark - Setup SDK

+ (void)startManagerWithIdentifier:(NSString *)appIdentifier
                         serverURL:(NSString *)serverURL
                          authType:(NSString *)authType
                            secret:(NSString *)secret
              updateManagerEnabled:(BOOL)updateManagerEnabled
                userMetricsEnabled:(BOOL)userMetricsEnabled
                   autoSendEnabled:(BOOL)autoSendEnabled {
  [self configHockeyManagerWithAppIdentifier:appIdentifier serverURL:serverURL];
  [self configAuthentificatorWithIdentificationType:authType secret:secret];
  [self configUpdateManagerWithUpdateManagerEnabled:updateManagerEnabled];
  [self configMetricsManagerWithUserMetricsEnabled:userMetricsEnabled];
  [self configCrashManagerWithAutoSendEnabled:autoSendEnabled];
  [self startManager];
}

+ (void)configHockeyManagerWithAppIdentifier:(NSString *)appIdentifier serverURL:(NSString *)serverURL {
  [[BITHockeyManager sharedHockeyManager] configureWithIdentifier:appIdentifier];
  if(serverURL && serverURL.length > 0) {
    [[BITHockeyManager sharedHockeyManager] setServerURL:serverURL];
  }
}

+ (void)configCrashManagerWithAutoSendEnabled:(BOOL)autoSendEnabled {
  [[BITHockeyManager sharedHockeyManager].crashManager setCrashManagerStatus:[BITUtils statusForAutoSendEnabled:autoSendEnabled]];
}

+ (void)configMetricsManagerWithUserMetricsEnabled:(BOOL)userMetricsEnabled {
  [[BITHockeyManager sharedHockeyManager] setDisableMetricsManager:!userMetricsEnabled];
}

+ (void)configUpdateManagerWithUpdateManagerEnabled:(BOOL)updateManagerEnabled {
  [[BITHockeyManager sharedHockeyManager] setDisableUpdateManager:!updateManagerEnabled];
}

+ (void)configAuthentificatorWithIdentificationType:(NSString *)identificationType secret:(NSString *)secret {
  if(secret && secret.length > 0) {
    [[BITHockeyManager sharedHockeyManager].authenticator setIdentificationType:[BITUtils identificationTypeForString:identificationType]];
    [[BITHockeyManager sharedHockeyManager].authenticator setAuthenticationSecret:secret];
  }
}

+ (void)startManager {
  [[BITHockeyManager sharedHockeyManager] startManager];
  [[BITHockeyManager sharedHockeyManager].authenticator authenticateInstallation];
}

#pragma mark - Setup SDK

+ (void)checkForUpdate {
  [[BITHockeyManager sharedHockeyManager].updateManager checkForUpdate];
}

+ (NSString *)versionCode {
  return [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleVersion"];
}

+ (NSString *)versionName {
  return [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleShortVersionString"];
}

+ (NSString *)bundleIdentifier {
  return [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleIdentifier"];
}

+ (NSString *)sdkVersion {
  return [[BITHockeyManager sharedHockeyManager] version];
}

+ (NSString *)sdkName {
  return @"HockeySDK";
}

+ (NSString *)crashReporterKey {
  return [BITHockeyManager sharedHockeyManager].installString;
}

@end
