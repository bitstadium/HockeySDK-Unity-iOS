/*
 * Author: Christoph Wendt
 *
 * Version: 1.0.7
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
 */

#import "HockeyAppUnity.h"
#import "Utils.h"

@interface HockeyAppUnity()

@end

@implementation HockeyAppUnity

+ (void)startManagerWithIdentifier:(NSString *)appIdentifier {
  
  [self startManagerWithIdentifier:appIdentifier
                          authType:@"BITAuthenticatorIdentificationTypeAnonymous"
                            secret:nil updateManagerEnabled:YES];
}

+ (void)startManagerWithIdentifier:(NSString *)appIdentifier
                          authType:(NSString *)authType
                            secret:(NSString *)secret
              updateManagerEnabled:(BOOL)updateManagerEnabled{
  
  [self configHockeyManagerWithAppIdentifier:appIdentifier serverURL:nil];
  [self configAuthentificatorWithIdentificationType:authType secret:secret];
  [self configUpdateManagerWithUpdateManagerEnabled:updateManagerEnabled];
  [self startManager];
}

+ (void)startManagerWithIdentifier:(NSString *)appIdentifier
                          authType:(NSString *)authType
                            secret:(NSString *)secret
              updateManagerEnabled:(BOOL)updateManagerEnabled
                   autoSendEnabled:(BOOL)autoSendEnabled{
  
  [self configHockeyManagerWithAppIdentifier:appIdentifier serverURL:nil];
  [self configAuthentificatorWithIdentificationType:authType secret:secret];
  [self configUpdateManagerWithUpdateManagerEnabled:updateManagerEnabled];
  [self configCrashManagerWithAutoSendEnabled:autoSendEnabled];
  [self startManager];
}

+ (void)startManagerWithIdentifier:(NSString *)appIdentifier
                         serverURL:(NSString *)serverURL
                          authType:(NSString *)authType
                            secret:(NSString *)secret
              updateManagerEnabled:(BOOL)updateManagerEnabled
                   autoSendEnabled:(BOOL)autoSendEnabled{
  
  [self configHockeyManagerWithAppIdentifier:appIdentifier serverURL:serverURL];
  [self configAuthentificatorWithIdentificationType:authType secret:secret];
  [self configUpdateManagerWithUpdateManagerEnabled:updateManagerEnabled];
  [self configCrashManagerWithAutoSendEnabled:autoSendEnabled];
  [self startManager];
}

+ (void)configHockeyManagerWithAppIdentifier:(NSString *)appIdentifier serverURL:(NSString *)serverURL{
  
  [[BITHockeyManager sharedHockeyManager] configureWithIdentifier:appIdentifier];
  
  if(serverURL && serverURL.length > 0) {
    [[BITHockeyManager sharedHockeyManager] setServerURL:serverURL];
  }
}

+ (void)configCrashManagerWithAutoSendEnabled:(BOOL)autoSendEnabled{
  
    [[BITHockeyManager sharedHockeyManager].crashManager setCrashManagerStatus:[Utils statusForAutoSendEnabled:autoSendEnabled]];
}

+ (void)configUpdateManagerWithUpdateManagerEnabled:(BOOL)updateManagerEnabled{
  
  [[BITHockeyManager sharedHockeyManager] setDisableUpdateManager:!updateManagerEnabled];
}

+ (void)configAuthentificatorWithIdentificationType:(NSString *)identificationType secret:(NSString *)secret{
  
  if(secret && secret.length > 0){
    [[BITHockeyManager sharedHockeyManager].authenticator setIdentificationType:[Utils identificationTypeForString:identificationType]];
    [[BITHockeyManager sharedHockeyManager].authenticator setAuthenticationSecret:secret];
  }
}

+ (void)startManager{
  
  [[BITHockeyManager sharedHockeyManager] startManager];
  [[BITHockeyManager sharedHockeyManager].authenticator authenticateInstallation];
}

+ (void)showFeedbackListView{
  
  [[[BITHockeyManager sharedHockeyManager] feedbackManager] showFeedbackListView];
}

+ (NSString *)versionCode{
  
  return [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleVersion"];
}

+ (NSString *)versionName{
	
	return [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleShortVersionString"];
}

+ (NSString *)bundleIdentifier;{
  
  return [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleIdentifier"];
}

+ (NSString *)sdkVersion{
	
	return @"3.7.1";
}

+ (NSString *)sdkName{
	
	return @"HockeySDK";
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
