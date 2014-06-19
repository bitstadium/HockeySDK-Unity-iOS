#import <Foundation/Foundation.h>

@interface Utils : NSObject

+ (BITAuthenticatorIdentificationType)identificationTypeForString:(NSString *)typeString;
+ (BITCrashManagerStatus)statusForAutoSendEnabled:(BOOL)autoSendEnabled;

@end
