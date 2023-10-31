using UnityEngine;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using AOT;

namespace TapTap.Themis
{
#if UNITY_STANDALONE && UNITY_STANDALONE_WIN
    public class TapThemisAgentWin : TapThemisAgent
    {
        static byte[] version = new byte[] { 74, 99, 166, 177, 235, 38, 118, 118, 209, 35, 211, 129, 119, 86, 51, 75, 211, 85, 75, 143, 204, 183, 222, 123, 100, 211, 248, 226, 69, 46, 74, 204, 100, 50, 56, 49, 0 };

        static IntPtr tapThemis = IntPtr.Zero;

        private static TapThemisCallBackImp _callbackImp = null;

        // callback interface
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void onThemisStateCB(int state,string message);
        [MonoPInvokeCallback(typeof(onThemisStateCB))]
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

        [DllImport("themis_x64")]
        private static extern IntPtr init_themis_by_appid(string appID, byte[] version);
        [DllImport("themis_x64")]
        private static extern void add_custom_field(IntPtr themis, string key, string value);
        [DllImport("themis_x64")]
        private static extern void tmcr(IntPtr themis, string scene_id, bool on_off);
        [DllImport("themis_x64")]
        private static extern void tminit(IntPtr themis, UInt64 ptr);
        [DllImport("themis_x64")]
        private static extern void tminit_windows(IntPtr themis, UInt64 mptr, UInt64 kptr);
        [DllImport("themis_x64")]
        private static extern void report_exception(IntPtr themis, string name, string reason, string stackTrace, bool isQuitApp);
        [DllImport("themis_x64")]
        private static extern void report_custom_exception(IntPtr themis, string name, string reason, string stackTrace, bool isQuitApp);
        [DllImport("themis_x64")]
        private static extern void event_tracking(IntPtr themis, string data);
        [DllImport("themis_x64")]
        private static extern void enable_debug_mode(IntPtr themis, bool debug);
        [DllImport("themis_x64")]
        private static extern Int32 get_tod(IntPtr themis, byte[] buffer, UInt32 max_size);
        [DllImport("themis_x64")]
        private static extern IntPtr get_themis_heartbeat(IntPtr themis, UInt32 index, UInt64 random);
        [DllImport("themis_x64")]
        private static extern void set_native_callback(IntPtr themis, IntPtr themis_state_cb, IntPtr extra_message_cb);
        [DllImport("themis_x64")]
        private static extern void report_custom_exception_ex(IntPtr themis, string name, string reason, string stackTrace, bool isQuitApp, string extra_message,int extra_len);
        [DllImport("themis_x64")]
        private static extern void set_exception_callback(IntPtr message_cb);
        [DllImport("themis_x64")]
        private static extern IntPtr get_oneid_data(IntPtr themis);
        [DllImport("themis_x64")]
        private static extern void set_use_extend_callback(bool b);
        [DllImport("themis_x64")]
        private static extern void set_extra_callback_ex(IntPtr cb);

        public override void InitTHEMISAgent()
        {
        }

        public override void InitTHEMISAgentWithID(string appID)
        {
            //_isInitialized = true;
            if (!string.IsNullOrEmpty(appID))
            {
                tapThemis = init_themis_by_appid(appID, version);
            }
        }

        public override void SetGamePlayer(string gamePlayer)
        {
            AddCustomField("playerinfo", gamePlayer);
        }

        public override void SetGameCurrentScene(string sceneId)
        {
            AddCustomField("gamescene", sceneId);
        }

        public override void EnableDebugMode(bool enable)
        {
            if (tapThemis != IntPtr.Zero)
            {
                enable_debug_mode(tapThemis, enable);
            }
        }

        public override void ReportException(string name, string reason, string stackTrace, bool isQuitApp)
        {
            if (tapThemis != IntPtr.Zero)
            {
                report_exception(tapThemis, name, reason, stackTrace, isQuitApp);
            }
        }

        public override void ReportCustomException(string name, string reason, string message, bool isQuitApp)
        {
            if (tapThemis != IntPtr.Zero)
            {
                report_custom_exception(tapThemis, name, reason, message, isQuitApp);
            }
        }

        public override void ReportCustomExceptionEx(string name, string reason, string message, bool isQuitApp, string extra_message, int extra_len)
        {
            if (tapThemis != IntPtr.Zero)
            {
                report_custom_exception_ex(tapThemis, name, reason, message, isQuitApp, extra_message, extra_len);
            }
        }

        public override void EventTracking(string strEvent)
        {
            if (tapThemis != IntPtr.Zero)
            {
                event_tracking(tapThemis, strEvent);
            }
        }

        public override void AddCustomField(string str_key, string str_value)
        {
            if (tapThemis != IntPtr.Zero)
            {
                add_custom_field(tapThemis, str_key, str_value);
            }
        }

        public override void SetCallback(TapThemisCallBackImp cb)
        {
            if (tapThemis != IntPtr.Zero)
            {
                _callbackImp = cb;

                onThemisStateCB state_handler = new onThemisStateCB(_onThemisStateCB);
                getExtraMessageCB message_handler = new getExtraMessageCB(_getExtraMessageCB);
                getExceptionMessageCB exception_msg_handler = new getExceptionMessageCB(_getExceptionMessageCB);
                getExtraMessageCBEx ex_message_handler = new getExtraMessageCBEx(_getExtraMessageCBEx);

                IntPtr state_cb = Marshal.GetFunctionPointerForDelegate(state_handler);
                IntPtr message_cb = Marshal.GetFunctionPointerForDelegate(message_handler);
                IntPtr exception_msg_cb = Marshal.GetFunctionPointerForDelegate(exception_msg_handler);
                IntPtr message_cb_ex = Marshal.GetFunctionPointerForDelegate(ex_message_handler);

                set_native_callback(tapThemis,state_cb, message_cb);
                set_exception_callback(exception_msg_cb);
                set_extra_callback_ex(message_cb_ex);
            }
        }

        public override void SetEngine(bool isCustom, bool isOpenGl)
        { }

        public override string GetHeartbeat(int index, long random)
        {
            if (tapThemis != IntPtr.Zero){
                IntPtr hb = get_themis_heartbeat(tapThemis,(uint)index,(ulong)random);

                string str_hb = Marshal.PtrToStringAnsi(hb);
                return str_hb;
            }
            return "";
        }

        public override string GetOneidData()
        {
            if (tapThemis != IntPtr.Zero){
                IntPtr hb = get_oneid_data(tapThemis);

                string str_hb = Marshal.PtrToStringAnsi(hb);
                return str_hb;
            }
            return "";
        }

        public override void SetUseExtendCallback(bool b)
        {
            if (tapThemis != IntPtr.Zero)
            {
                set_use_extend_callback(b);
            }
        }
    }
#endif
}