#import "HockeyAppUnity.h"
#import "HockeySDK.h"

@interface HockeyAppUnity()

@end

@implementation HockeyAppUnity

+ (void)startManagerWithIdentifier:(NSString *)appIdentifier {
  
  [self startManagerWithIdentifier:appIdentifier authType:@"BITAuthenticatorIdentificationTypeAnonymous" secret:nil updateManagerEnabled:YES];
}

+ (void)startManagerWithIdentifier:(NSString *)appIdentifier authType:(NSString *)authType secret:(NSString *)secret updateManagerEnabled:(BOOL)updateManagerEnabled{
  
  [[BITHockeyManager sharedHockeyManager] setDisableUpdateManager:!updateManagerEnabled];
  [[BITHockeyManager sharedHockeyManager] configureWithIdentifier:appIdentifier];
  [[BITHockeyManager sharedHockeyManager].authenticator setIdentificationType:[self identificationTypeForString:authType]];
  [[BITHockeyManager sharedHockeyManager].authenticator setAuthenticationSecret:secret];
  [[BITHockeyManager sharedHockeyManager] startManager];
  [[BITHockeyManager sharedHockeyManager].authenticator authenticateInstallation];
}

+ (void)startManagerWithIdentifier:(NSString *)appIdentifier authType:(NSString *)authType secret:(NSString *)secret updateManagerEnabled:(BOOL)updateManagerEnabled autoSendEnabled:(BOOL)autoSendEnabled{
  
  [[BITHockeyManager sharedHockeyManager] setDisableUpdateManager:!updateManagerEnabled];
  [[BITHockeyManager sharedHockeyManager].crashManager setCrashManagerStatus:[self statusForAutoSendEnabled:autoSendEnabled]];
  [[BITHockeyManager sharedHockeyManager] configureWithIdentifier:appIdentifier];
  [[BITHockeyManager sharedHockeyManager].authenticator setIdentificationType:[self identificationTypeForString:authType]];
  [[BITHockeyManager sharedHockeyManager].authenticator setAuthenticationSecret:secret];
  [[BITHockeyManager sharedHockeyManager] startManager];
  [[BITHockeyManager sharedHockeyManager].authenticator authenticateInstallation];
}

+ (BITAuthenticatorIdentificationType)identificationTypeForString:(NSString *)typeString{
  
  if ([typeString isEqualToString:@"BITAuthenticatorIdentificationTypeDevice"]){
    
    return BITAuthenticatorIdentificationTypeDevice;
  }else if ([typeString isEqualToString:@"BITAuthenticatorIdentificationTypeHockeyAppUser"]){
    
    return BITAuthenticatorIdentificationTypeHockeyAppUser;
  }else if ([typeString isEqualToString:@"BITAuthenticatorIdentificationTypeHockeyAppEmail"]){

    return BITAuthenticatorIdentificationTypeHockeyAppEmail;
  }else if ([typeString isEqualToString:@"BITAuthenticatorIdentificationTypeWebAuth"]){
    
    return BITAuthenticatorIdentificationTypeWebAuth;
  }else{
    
    return BITAuthenticatorIdentificationTypeAnonymous;
  }
}

+ (BITCrashManagerStatus)statusForAutoSendEnabled:(BOOL)autoSendEnabled{
  
  if (autoSendEnabled){
    return BITCrashManagerStatusAutoSend;
  }
  
  return BITCrashManagerStatusAlwaysAsk;
}

+ (void)showFeedbackListView{
  
  [[[BITHockeyManager sharedHockeyManager] feedbackManager] showFeedbackListView];
}

+ (NSString *)appVersion{
  
  return [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleVersion"];
}

+ (NSString *)bundleIdentifier;{
  
  return [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleIdentifier"];
}

+ (BOOL)handleOpenURL:(NSURL *) url sourceApplication:(NSString *) sourceApplication annotation:(id) annotation{
  
  if ([[BITHockeyManager sharedHockeyManager].authenticator handleOpenURL:url
                                                        sourceApplication:sourceApplication
                                                               annotation:annotation]) {
    return YES;
  }
  return NO;
}

+ (void) sendViewLoadedMessageToUnity{
  
  NSString *gameObj = @"HockeyAppUnityIOS";
  NSString *msg = @"";
  NSString *method = @"GameViewLoaded";
  UnitySendMessage([gameObj UTF8String], [method UTF8String], [msg UTF8String]);
}

@end
