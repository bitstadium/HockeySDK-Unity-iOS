@interface HockeyAppUnity : NSObject

+ (void)startManagerWithIdentifier:(NSString *)appIdentifier;
+ (NSString *)bundleIdentifier;
+ (NSString *)appVersion;

@end
