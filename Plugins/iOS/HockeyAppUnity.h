@interface HockeyAppUnity : NSObject

+ (void)startManagerWithIdentifier:(NSString *)appIdentifier;
+ (void)showFeedbackListView;
+ (NSString *)bundleIdentifier;
+ (NSString *)appVersion;

@end