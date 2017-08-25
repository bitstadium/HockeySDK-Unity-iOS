#import <Foundation/Foundation.h>
#import "HockeySDK.h"

@interface BITUtils : NSObject

+ (BITAuthenticatorIdentificationType)identificationTypeForString:(NSString *)typeString;
+ (BITCrashManagerStatus)statusForAutoSendEnabled:(BOOL)autoSendEnabled;

@end
