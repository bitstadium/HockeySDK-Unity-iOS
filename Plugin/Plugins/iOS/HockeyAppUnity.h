@interface HockeyAppUnity : NSObject

+ (void)startManagerWithIdentifier:(NSString *)appIdentifier;
+ (void)startManagerWithIdentifier:(NSString *)appIdentifier authType:(NSString *)authType secret:(NSString *)secret updateManagerEnabled:(BOOL)updateManagerEnabled;
+ (void)startManagerWithIdentifier:(NSString *)appIdentifier authType:(NSString *)authType secret:(NSString *)secret updateManagerEnabled:(BOOL)updateManagerEnabled autoSendEnabled:(BOOL)autoSendEnabled;
+ (void)showFeedbackListView;
+ (NSString *)bundleIdentifier;
+ (NSString *)appVersion;
+ (BOOL)handleOpenURL:(NSURL *) url sourceApplication:(NSString *) sourceApplication annotation:(id) annotation;
+ (void)sendViewLoadedMessageToUnity;

@end