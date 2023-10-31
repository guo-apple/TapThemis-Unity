//
//  libXDCrash.m
//  libXDCrash
//
//  Created by tianguo on 2021/9/28.
//

#import "libXDTHEMIS.h"
#import <THEMIS/THEMIS.h>

@implementation libXDTHEMIS

//void _lib_init_THEMIS_agent(){
//    __InitTHEMISAgentWithID("");
//}

void _lib_init_THEMIS_agent_with_id(const char* appId){
    __InitTHEMISAgentWithID(appId);
}

//void _lib_set_game_player(const char *game_player){
//    __SetGamePlayer(game_player);
//}
//void _lib_set_game_current_scene(int sceneID){
//    __SetGameCurrentScene(sceneID);
//}
void _lib_enable_debug_mode(bool isTurnOn){
    __EnableDebugMode(isTurnOn);
}
void _lib_report_custom_exception(const char* name, const char* reason, const char* stackTrace, bool isQuitApp){
    
    __ReportCustomException(name,reason,stackTrace,isQuitApp);
}
void _lib_report_exception_with_type(const char* name, const char* reason,
                               const char* stackTrace, int exception_type, bool bIsQuit){
    __ReportExceptionWithType(name,reason,stackTrace,exception_type,bIsQuit);
}

void _lib_report_custom_exception_ex(const char* name, const char* reason,const char* message,bool bIsQuit,const char* extra_message,int extra_len){
    __ReportCustomExceptionEx(name,reason,message,bIsQuit,extra_message,extra_len);
}

void _lib_event_tracking(const char *logData){
    __EventTracking(logData);
}
void _lib_log_report(const char *logMessage){
    __LogReport(logMessage);
}

void _lib_debug_log(const char *lpLogData){
    __DebugLog(lpLogData);
}

void _add_custom_field(const char * str_key,const char * str_value){
    __add_custom_field(str_key,str_value);
}
    
void _lib_set_native_callback(long themis_state_cb,long extra_message_cb){
    __SetNativeCallback(themis_state_cb,extra_message_cb);
}


void _lib_set_exception_callback(long message_cb){
    [XDGameSDK SetExceptionCallback:message_cb];
}

void _lib_set_exception_oom_callback(long message_cb){
    [XDGameSDK SetOOMCallback:message_cb];
}

void _lib_set_strong_kill_callback(long message_cb){
    [XDGameSDK setStrongKillCallback:message_cb];
}


void _lib_set_extra_callback_ex(long message_cb){
    [XDGameSDK SetExtraCallbackEx:message_cb];
}

void _lib_set_use_extend_callback(bool b){
    [XDGameSDK SetUseExtendCallback:b];
}

const char * _lib_get_themis_heartbeat(int index, long random){
    return [XDGameSDK getHeartbeat:index random:random];
}


const char * _lib_get_oneid_data(){
    return [XDGameSDK GetOneidData];
}





//void _report_custom_message_with_tag(const char* name, const char* reason, const char* str_message,
//                              int nType, bool isSync){
//    __report_custom_message_with_tag(name, reason, str_message,nType,isSync);
//}
@end
