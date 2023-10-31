using UnityEngine;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using AOT;
//using System.Diagnostics.Debug;


namespace TapTap.Themis
{
#if UNITY_IPHONE || UNITY_IOS
    public class TapThemisAgentIOS : TapThemisAgent
    {
        private static TapThemisCallBackImp _callbackImp = null;

        //callback interface
        // [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        // public delegate void CrashBlock(int signo, long si_addr);
        // [MonoPInvokeCallback(typeof(CrashBlock))]
        // static void _crashBlock(int signo, long si_addr)
        // {
        //     if (_callbackImp != null)
        //     {
        //         _callbackImp.crashBlock(signo, si_addr);
        //     }
        // }

        // [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        // public delegate void OnExitOrKillBlock();
        // [MonoPInvokeCallback(typeof(OnExitOrKillBlock))]
        // static void _onExitOrKillBlock()
        // {
        //     if (_callbackImp != null)
        //     {
        //         _callbackImp.onExitOrKillBlock();
        //     }
        // }

        // 定义了一个协议
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void onThemisStateCB(int state,string message);
        [MonoPInvokeCallback(typeof(onThemisStateCB))]

        // 实现了该协议？
        static void _onThemisStateCB(int state, string message)
        {
            if (_callbackImp != null)
            {
                _callbackImp.onHandleThemisState(state, message);
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate string getExtraMessageCB();
        [MonoPInvokeCallback(typeof(getExtraMessageCB))]
        static string _getExtraMessageCB()
        {
            if (_callbackImp != null)
            {
                return _callbackImp.getExtraMessage();
            }
            return null;
        }
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate string getExceptionMessageCB();
        [MonoPInvokeCallback(typeof(getExceptionMessageCB))]
        static string _getExceptionMessageCB()
        {
            if (_callbackImp != null)
            {
                return _callbackImp.getExceptionMessage();
            }
            return null;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate string getOOMMessageCB(float v);
        [MonoPInvokeCallback(typeof(getOOMMessageCB))]
        static string _getOOMMessageCB(float v)
        {
            if (_callbackImp != null)
            {
                return _callbackImp.getOOMMessage(v);
            }
            return null;
        }


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate string getStrongKillMessageCB();
        [MonoPInvokeCallback(typeof(getStrongKillMessageCB))]
        static string _getStrongKillMessageCB()
        {
            if (_callbackImp != null)
            {
                return _callbackImp.getStrongKillMessage();
            }
            return null;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate string getExtraMessageCBEx(string message);
        [MonoPInvokeCallback(typeof(getExtraMessageCBEx))]
        static string _getExtraMessageCBEx(string message)
        {
            if (_callbackImp != null)
            {
                return _callbackImp.getExtraMessageEx(message);
            }
            return null;
        }


        // [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        // public delegate void SetNativeCallback(long themis_state_cb,long extra_message_cb);
        // [MonoPInvokeCallback(typeof(SetNativeCallback))]
        // static void _setNativeCallback(long themis_state_cb,long extra_message_cb)
        // {
        //     if (_callbackImp != null)
        //     {
        //         _callbackImp.setNativeCallback(themis_state_cb,extra_message_cb);
        //     }
        // }

        // --- dllimport start ---

        [DllImport("__Internal")]
        private static extern void _lib_init_THEMIS_agent_with_id(string appID);// InitTHEMISAgentWithID

        //[DllImport("__Internal")]
        //private static extern void _lib_set_game_player(string game_player);//SetGamePlayer

        [DllImport("__Internal")]
        private static extern void _add_custom_field(string str_key, string str_value);//AddCustomField

        //[DllImport("__Internal")]
        //private static extern void _lib_set_game_current_scene(int sceneID);//SetGameCurrentScene

        [DllImport("__Internal")]
        private static extern void _lib_enable_debug_mode(bool isTurnOn);

        [DllImport("__Internal")]
        private static extern void _lib_report_exception_with_type(string name, string reason, string stackTrace, int exception_type, bool isQuitApp);

        [DllImport("__Internal")]
        private static extern void _lib_report_custom_exception_ex(string name, string reason, string message, bool isQuitApp,string extra_message,int extra_len);

        //[DllImport("__Internal")]
        //private static extern void _lib_report_custom_exception(string name, string reason, string stackTrace, bool isQuitApp);

        [DllImport("__Internal")]
        private static extern void _lib_event_tracking(string logData);//EventTracking

        [DllImport("__Internal")]
        private static extern void _lib_log_report(string logMessage);

        [DllImport("__Internal")]
        private static extern void _lib_debug_log(string lpLogData);

        [DllImport("__Internal")]
        private static extern void _lib_set_native_callback(IntPtr themis_state_cb, IntPtr extra_message_cb);

        [DllImport("__Internal")]
        private static extern IntPtr _lib_get_themis_heartbeat(UInt32 index, UInt64 random);

        [DllImport("__Internal")]
        private static extern void _lib_set_exception_callback(IntPtr message_cb);

        [DllImport("__Internal")]
        private static extern void _lib_set_exception_oom_callback(IntPtr message_cb);

        [DllImport("__Internal")]
        private static extern void _lib_set_strong_kill_callback(IntPtr message_cb);

        [DllImport("__Internal")]
        private static extern IntPtr _lib_get_oneid_data();

        [DllImport("__Internal")]
        private static extern void _lib_set_extra_callback_ex(IntPtr message_cb);

        [DllImport("__Internal")]
        private static extern void _lib_set_use_extend_callback(bool b);


        

        

        // dllimport end
        public override void InitTHEMISAgent()
        {
            //_isInitialized = true;
            //_lib_init_THEMIS_agent();
        }

        public override void InitTHEMISAgentWithID(string appID)
        {
            //base.InitTHEMISAgentWithID(appID);

            if (!string.IsNullOrEmpty(appID))
            {
                UnityEngine.Debug.Log("-----Start InitTHEMISAgentWithID -----------");
                _lib_init_THEMIS_agent_with_id(appID);
                UnityEngine.Debug.Log("\n  Initialized InitTHEMISAgentWithID iOS _lib_init_THEMIS_agent_with_id " + appID + " \n\n");
            }
        }



        public override void SetGamePlayer(string gamePlayer)
        {
            if (!string.IsNullOrEmpty(gamePlayer))
            {
                AddCustomField("playerinfo", gamePlayer);
            }
        }


        public override void AddCustomField(string str_key, string str_value)
        {
            _add_custom_field(str_key, str_value);
        }

        public override void SetGameCurrentScene(string sceneId)
        {
            //_lib_set_game_current_scene(sceneId);
            AddCustomField("gamescene", sceneId.ToString());
        }

        public override void EnableDebugMode(bool enable)
        {
            _lib_enable_debug_mode(enable);
        }

        public override void ReportException(string name, string message, string stackTrace, bool isQuitApp)
        {
            if ((!string.IsNullOrEmpty(name)) && (!string.IsNullOrEmpty(message)) && (!string.IsNullOrEmpty(stackTrace)))
            {
                // UnityEngine.Debug.Log("\n ------------------------ : name = "+name +"\n\n");
                // UnityEngine.Debug.Log("\n ------------------------ : message = "+message+"\n\n");
                // UnityEngine.Debug.Log("\n ------------------------ : stackTrace = "+stackTrace+"\n\n");

                // name = name.Replace( " ", "&&" );
                // message = message.Replace( " ", "&&" );
                // stackTrace = stackTrace.Replace( " ", "&&" );

                // name = name.Replace( "\r\n", "@@" );
                // message = message.Replace( "\r\n", "@@" );
                // stackTrace = stackTrace.Replace( "\r\n", "@@" );

                // name = name.Replace( "\n", "@@" );
                // message = message.Replace( "\n", "@@" );
                // stackTrace = stackTrace.Replace( "\n", "@@" );

                //type = 1;
                //UnityEngine.Debug.Log("-----ReportException Unity 异常 接口已经调用 -----------");
                //Debug.Log("-----创建一个异常 ReportException 接口已经调用-----------");
                _lib_report_exception_with_type(name, message, stackTrace, 0, isQuitApp && TapThemis.AutoQuitApplicationAfterReport);
            }
        }

        public override void ReportCustomException(string name, string reason, string message, bool isQuitApp)
        {

            if ((!string.IsNullOrEmpty(name)) && (!string.IsNullOrEmpty(reason)) && (!string.IsNullOrEmpty(message)))
            {

                _lib_report_exception_with_type(name, reason, message, 1, isQuitApp && TapThemis.AutoQuitApplicationAfterReport);
            }
        }

        public override void ReportCustomExceptionEx(string name, string reason, string message, bool isQuitApp,string extra_message,int extra_len)
        {
            if ((!string.IsNullOrEmpty(name)) && (!string.IsNullOrEmpty(reason)) && (!string.IsNullOrEmpty(message)))
            {
                _lib_report_custom_exception_ex(name, reason, message, isQuitApp && TapThemis.AutoQuitApplicationAfterReport,extra_message,extra_len);
            }
        }

        public override void EventTracking(string strEvent)
        {
            if (!string.IsNullOrEmpty(strEvent))
            {

                _lib_event_tracking(strEvent);
            }
        }


        public override void SetCallback(TapThemisCallBackImp cb)
        {
            _callbackImp = cb;

            // CrashBlock crash_handler = new CrashBlock(_crashBlock);
            // OnExitOrKillBlock exit_handler = new OnExitOrKillBlock(_onExitOrKillBlock);

            // //SetNativeCallback native_handler = new SetNativeCallback(_setNativeCallback);

            // IntPtr func_crash = Marshal.GetFunctionPointerForDelegate(crash_handler);
            // IntPtr func_exit = Marshal.GetFunctionPointerForDelegate(exit_handler);
            // //IntPtr func_native = Marshal.GetFunctionPointerForDelegate(native_handler);

            // _setNativeCrashCallback(func_crash);
            // _setExitOrKillCallback(func_exit);
            // //_lib_set_native_callback(func_native);


            onThemisStateCB state_handler = new onThemisStateCB(_onThemisStateCB);
            getExtraMessageCB message_handler = new getExtraMessageCB(_getExtraMessageCB);
            getExceptionMessageCB exception_msg_handler = new getExceptionMessageCB(_getExceptionMessageCB);
            getExtraMessageCBEx ex_message_handler = new getExtraMessageCBEx(_getExtraMessageCBEx);
            getOOMMessageCB oom_handler = new getOOMMessageCB(_getOOMMessageCB);
            getStrongKillMessageCB strong_kill_handler = new getStrongKillMessageCB(_getStrongKillMessageCB);

            IntPtr state_cb = Marshal.GetFunctionPointerForDelegate(state_handler);
            IntPtr message_cb = Marshal.GetFunctionPointerForDelegate(message_handler);
            IntPtr exception_msg_cb = Marshal.GetFunctionPointerForDelegate(exception_msg_handler);
            IntPtr message_cb_ex = Marshal.GetFunctionPointerForDelegate(ex_message_handler);
            IntPtr oom_msg_cb = Marshal.GetFunctionPointerForDelegate(oom_handler);
            IntPtr strong_kill_cb = Marshal.GetFunctionPointerForDelegate(strong_kill_handler);

            _lib_set_native_callback(state_cb, message_cb);
            _lib_set_exception_callback(exception_msg_cb);
            _lib_set_exception_oom_callback(oom_msg_cb);
            _lib_set_strong_kill_callback(strong_kill_cb);

            _lib_set_extra_callback_ex(message_cb_ex);
        }

        public override void SetEngine(bool isCustom, bool isOpenGl)
        {
        }

        public override string GetHeartbeat(int index, long random)
        {

            IntPtr hb = _lib_get_themis_heartbeat((uint)index,(ulong)random);

            string str_hb = Marshal.PtrToStringAnsi(hb);
            return str_hb;
        }

        public override string GetOneidData()
        {

            IntPtr hb = _lib_get_oneid_data();

            string str_hb = Marshal.PtrToStringAnsi(hb);
            return str_hb;
        }

        public override void SetUseExtendCallback(bool b)
        {
            _lib_set_use_extend_callback(b);
        }
    }

#endif
}
