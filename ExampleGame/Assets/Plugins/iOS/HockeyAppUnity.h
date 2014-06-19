@interface HockeyAppUnity : NSObject

+ (void)startManagerWithIdentifier:(NSString *)appIdentifier;

+ (void)startManagerWithIdentifier:(NSString *)appIdentifier
                          authType:(NSString *)authType
                            secret:(NSString *)secret
              updateManagerEnabled:(BOOL)updateManagerEnabled;

+ (void)startManagerWithIdentifier:(NSString *)appIdentifier
                          authType:(NSString *)authType
                            secret:(NSString *)secret
              updateManagerEnabled:(BOOL)updateManagerEnabled
                   autoSendEnabled:(BOOL)autoSendEnabled;

+ (void)startManagerWithIdentifier:(NSString *)appIdentifier
                         serverUrl:(NSString *)serverURL
                          authType:(NSString *)authType
                            secret:(NSString *)secret
              updateManagerEnabled:(BOOL)updateManagerEnabled
                   autoSendEnabled:(BOOL)autoSendEnabled;

+ (void)showFeedbackListView;
+ (NSString *)appVersion;
+ (NSString *)bundleIdentifier;
+ (BOOL)handleOpenURL:(NSURL *) url sourceApplication:(NSString *) sourceApplication annotation:(id) annotation;
+ (void) sendViewLoadedMessageToUnity;

@end