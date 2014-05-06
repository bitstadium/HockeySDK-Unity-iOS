@interface HockeyAppUnity : NSObject

+ (void)startManagerWithIdentifier:(NSString *)appIdentifier updateManagerEnabled:(BOOL)updateManagerEnabled;
+ (void)showFeedbackListView;
+ (NSString *)bundleIdentifier;
+ (NSString *)appVersion;

@end