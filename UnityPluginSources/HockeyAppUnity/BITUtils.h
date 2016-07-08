#import <Foundation/Foundation.h>

@interface BITUtils : NSObject

+ (BITAuthenticatorIdentificationType)identificationTypeForString:(NSString *)typeString;
+ (BITCrashManagerStatus)statusForAutoSendEnabled:(BOOL)autoSendEnabled;

@end
