//
//  XDGameSDK.h
//  XDGameCrash
//
//  Created by tianguo on 2021/9/14.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

#ifdef __cplusplus
extern "C" {
#endif



//typedef void (*ptrNonStaticFun_p)(float*,float*);
typedef void (*crashBlock)(int signo,long si_addr);
typedef void (*onExitOrKillBlock)(void);
typedef void (*infoReceiverFunc)(uint64_t,uint64_t);





// "killall -9 ProcessName"
@interface XDGameSDK : NSObject

/**
 *  初始化THEMIS
 *  @param appId 注册THEMIS分配的 App Key
 */
+(void)startWithAppId:(NSString*)appId;

/**
 *  初始化THEMIS
 *  描述：采用服务器地址、端口、项目id的方式初始化 themis
 *  @param server 服务器地址
 *  @param prot 服务器端口
 *  @param appid 项目id
 */
+(void)initAll:(NSString*)server prot:(int)prot appid:(NSString*)appid;

/**
 *  描述：设置属性
 *  @param gamePlayerName 游戏玩家name
 */
+(void)setGamePlayerName:(NSString*)gamePlayerName;

/**
 *  描述：设置属性
 *  @param gameScene 游戏场景
 */
+(void)setGameScene:(NSString*)gameScene;

/**
 *  自定义数据,以json字符串做为参数传递,可添加多个
 *    长度不要超过250个字符
 *  @param key 对应的json中的key
 *  @param value 对应的json中的value
 */
+(void)customData:(NSString*)key value:(NSString*)value;


// 包接口
+(const char*)getHeartbeat:(uint32_t)idx random:(uint64_t)random;

/**
 * 信回调接口
 */
+(void)onInfoReceiver:(infoReceiverFunc)func_p;

+(void)TMInit:(uint64_t)p;
+(void)TMCR:(NSString *)SceneID on_off:(bool)on_off;

+(NSString*)vsrsion;

// 提供一个自定义的异常上报接口
+(void)ReportCustomException:(NSString*)name reason:(NSString*)reason message:(NSString*)message isQuitApp:(BOOL)isQuitApp;

+(void)ReportCustomExceptionEx:(NSString*)name reason:(NSString*)reason message:(NSString*)message isQuitApp:(BOOL)isQuitApp extraMessage:(const char*)extraMessage extraLen:(uint32_t)extraLen;

+(void)ReportExceptionWithType:(NSString*)name reason:(NSString*)reason message:(NSString*)message exception_type:(int)exception_type isQuitApp:(BOOL)isQuitApp;

+(int32_t)getTOD:(char*)buffer max_size:(uint32_t)max_size;


+(const char*)GetOneidData;


// 所有回调函数设置函数
+(void)SetNativeCallback:(long)themis_state_cb extra_message_cb:(long)extra_message_cb;


+(void)SetExtraCallbackEx:(long)cb;

+(void)SetUseExtendCallback:(bool)b;

+(void)SetExceptionCallback:(long)message_cb;

+(void)SetOOMCallback:(long)message_cb;

+(void)setStrongKillCallback:(long)message_cb;




@end


#ifdef __cplusplus
}
#endif
NS_ASSUME_NONNULL_END
