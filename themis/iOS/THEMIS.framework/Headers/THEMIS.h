//
//  THEMIS.h
//  THEMIS
//
//  Created by tianguo on 2021/11/15.
//

#import <Foundation/Foundation.h>

//! Project version number for THEMIS.
FOUNDATION_EXPORT double THEMISVersionNumber;

//! Project version string for THEMIS.
FOUNDATION_EXPORT const unsigned char THEMISVersionString[];

// In this header, you should import all the public headers of your framework using statements like #import <THEMIS/PublicHeader.h>
#ifdef __APPLE__
#include <TargetConditionals.h>


#if TARGET_OS_IPHONE
#import <THEMIS/UnityHandler.h>
#import <THEMIS/XDGameSDK.h>

#elif TARGET_OS_MAC
#import <THEMIS_MAC/UnityHandler.h>
#import <THEMIS_MAC/XDGameSDK.h>
#endif
#endif

