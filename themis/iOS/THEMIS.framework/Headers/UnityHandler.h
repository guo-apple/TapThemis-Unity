//
// Created by xiaowu on 2021/6/22.
//

#ifndef CRASHRP_UNITYHANDLER_H
#define CRASHRP_UNITYHANDLER_H

#include <stdio.h>
#import <Foundation/Foundation.h>



#ifdef __cplusplus  //如果是C++语言
#define PASSPORT_EXTERN         extern "C" __attribute__((visibility ("default")))
#else
#define PASSPORT_EXTERN         extern __attribute__((visibility ("default")))
#endif

#ifdef __cplusplus
extern "C" {
#endif

// 初始化引擎
extern void __InitTHEMISAgentWithID(const char *appID);
extern void __InitAll(const char *server,int prot,const char *appid);

// 添加自定数字段
extern void __add_custom_field(const char *str_key,const char *str_value);

// 设置游戏场景的数据
extern void __SetGamePlayer(const char *game_player);
extern void __SetGameCurrentScene(const char * sceneID);

extern int32_t __getTOD(char* buffer,uint32_t max_size);

extern void __ReportCustomException(const char* name, const char* reason, const char* stackTrace, bool isQuitApp);


extern void __ReportExceptionWithType(const char* name, const char* reason,
                                         const char* stackTrace, int exception_type, bool bIsQuit);

extern void __ReportCustomExceptionEx(const char* name, const char* reason,const char* message,bool bIsQuit,const char* extra_message,int extra_len);
/**
 * crash 以后回调通知
 */
//extern void __setNativeCrashCallback(long callback);


void init_unity(bool b);

void add_unity_switch(bool bl);

void getEventTracking(NSMutableString * str_event);

// 这里是新接口
extern void __SetNativeCallback(long themis_state_cb,long extra_message_cb);

// debug模式下开启一些打印日志
extern void __EnableDebugMode(bool isTurnOn);

// 主要的自定义50条埋点数据接口
extern void __EventTracking(const char *logData);

// 日志上报
extern void __LogReport(const char *logMessage);

extern void __DebugLog(const char *lpLogData);

extern const char* __get_version(void);


//***********************************************************

#ifdef __cplusplus
}
#endif


#endif //CRASHRP_UNITYHANDLER_H
