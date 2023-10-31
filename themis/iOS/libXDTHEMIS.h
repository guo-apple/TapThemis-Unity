//
//  libXDCrash.h
//  libXDCrash
//
//  Created by tianguo on 2021/9/28.
//

#import <Foundation/Foundation.h>



@interface libXDTHEMIS : NSObject
#ifdef __cplusplus
extern "C" {
#endif
//extern void _lib_init_THEMIS_agent();
    
extern void _lib_init_THEMIS_agent_with_id(const char* appId);
extern void _lib_report_custom_exception(const char* name, const char* reason, const char* stackTrace, bool isQuitApp);
extern void _lib_report_exception_with_type(const char* name, const char* reason,const char* stackTrace, int exception_type, bool bIsQuit);
extern void _lib_report_custom_exception_ex(const char* name, const char* reason,const char* message,bool bIsQuit,const char* extra_message,int extra_len);
extern void _lib_set_native_callback(long themis_state_cb,long extra_message_cb);
extern void _add_custom_field(const char * str_key,const char * str_value);
    
extern const char * _lib_get_themis_heartbeat(int index, long random);
    
extern const char * _lib_get_oneid_data();
    
extern void _lib_event_tracking(const char *logData);
    
extern void _lib_log_report(const char *logMessage);
extern void _lib_debug_log(const char *lpLogData);
extern void _lib_enable_debug_mode(bool isTurnOn);
extern void _lib_set_exception_callback(long message_cb);
extern void _lib_set_exception_oom_callback(long message_cb);
extern void _lib_set_strong_kill_callback(long message_cb);
    
extern void _lib_set_extra_callback_ex(long message_cb);
extern void _lib_set_use_extend_callback(bool b);

        
    
#ifdef __cplusplus
}
#endif
@end
