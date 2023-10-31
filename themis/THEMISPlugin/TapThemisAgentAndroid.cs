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
#if UNITY_ANDROID
    public class TapThemisAgentAndroid : TapThemisAgent
    {
        private static readonly string GAME_AGENT_CLASS = "com.tds.themis.Themis";
        //private static readonly int TYPE_U3D_CRASH = 4;
        //private static readonly int EXCEPTION_TYPE_UNITY = 2;
        //private static readonly int EXCEPTION_TYPE_CUSTOM = 4;
        //private static bool hasSetGameType = false;
        private static string m_abi = "";
        private static AndroidJavaClass _gameAgentClass = null;

        private delegate void addCustomFieldDel(string str_key, string str_value);//addCustomField(string str_key, string str_value)
        private delegate void reportExceptionDel(string name, string reason, string stackTrace, int type, bool isQuitApp);
        private delegate void eventTrackingDel(string strEvent); //private static void _EventTracking(string strEvent)
        private delegate void enableDebugModeDel(bool isEnable);

        private static addCustomFieldDel addCustomFieldFunc = null;
        private static reportExceptionDel reportExceptionFunc = null;
        private static eventTrackingDel eventTrackingFunc = null;
        private static enableDebugModeDel enableDebugModeFunc = null;

        private static AndroidJavaClass THEMIS
        {
            get
            {
                if (_gameAgentClass == null)
                {
                    _gameAgentClass = new AndroidJavaClass(GAME_AGENT_CLASS);
                }
                return _gameAgentClass;
            }
        }

        private static void setAbi()
        {
            using (var system = new AndroidJavaClass("android.os.Build"))
            {
                m_abi = system.GetStatic<string>("CPU_ABI");
                TapThemis.LocalDebugLog("get system abi = " + m_abi);
            }
        }

        private static void realInit(string appID)
        {
            try
            {
                IntPtr ptrFunc;
                long funcAddr = 0;
                if (string.IsNullOrEmpty(appID))
                {
                    THEMIS.CallStatic("initAll");
                }
                else
                {
                    THEMIS.CallStatic("initAll", appID);
                }
                funcAddr = THEMIS.CallStatic<long>("get_proc_addCustomField");
                ptrFunc = new IntPtr(funcAddr);
                addCustomFieldFunc = (addCustomFieldDel)System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(ptrFunc, typeof(addCustomFieldDel));
                Console.WriteLine("addCustomFieldFunc:" + addCustomFieldFunc);

                funcAddr = THEMIS.CallStatic<long>("get_proc_ReportExceptionWithType");
                ptrFunc = new IntPtr(funcAddr);
                reportExceptionFunc = (reportExceptionDel)System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(ptrFunc, typeof(reportExceptionDel));
                Console.WriteLine("reportExceptionFunc:" + reportExceptionFunc);

                funcAddr = THEMIS.CallStatic<long>("get_proc_EventTracking");
                ptrFunc = new IntPtr(funcAddr);
                eventTrackingFunc = (eventTrackingDel)System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(ptrFunc, typeof(eventTrackingDel));
                Console.WriteLine("eventTrackingFunc:" + eventTrackingFunc);

                funcAddr = THEMIS.CallStatic<long>("get_proc_EnableDebugMode");
                ptrFunc = new IntPtr(funcAddr);
                enableDebugModeFunc = (enableDebugModeDel)System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(ptrFunc, typeof(enableDebugModeDel));
                Console.WriteLine("enableDebugModeFunc:" + enableDebugModeFunc);

            }
            catch
            {

            }
        }

        public override void InitTHEMISAgent()
        {
            // Assembly.LoadFile()
            if (TapThemis.IsInitialized)
            {
                return;
            }

            realInit(null);
            setAbi();
        }

        public override void InitTHEMISAgentWithID(string appID)
        {
            // Assembly.LoadFile()
            if (TapThemis.IsInitialized)
            {
                return;
            }

            realInit(appID);
            setAbi();
        }

        public override void ReportException(string name, string reason, string stackTrace, bool isQuitApp)
        {
            try
            {
                THEMIS.CallStatic("ReportException", name, reason, stackTrace, isQuitApp);
            }
            catch (Exception e)
            {
                TapThemis.LocalDebugLog("ReportException Error " + e.Message);
            }
        }

        public override void ReportCustomException(string name, string reason, string message, bool isQuitApp)
        {
            try
            {
                THEMIS.CallStatic("ReportCustomException", name, reason, message, isQuitApp);
            }
            catch (Exception e)
            {
                TapThemis.LocalDebugLog("ReportException Error " + e.Message);
            }
        }

        public override void SetGamePlayer(string userInfo)
        {
            try
            {
                if (addCustomFieldFunc != null)
                {
                    addCustomFieldFunc("playerinfo", userInfo);
                }
            }
            catch (Exception e)
            {
                TapThemis.LocalDebugLog("_SetGamePlayer Error " + e.Message);
            }
        }

        public override void SetGameCurrentScene(string sceneId)
        {
            try
            {
                if (addCustomFieldFunc != null)
                {
                    addCustomFieldFunc("gamescene", sceneId);
                }
            }
            catch (Exception e)
            {
                TapThemis.LocalDebugLog("SetGameCurrentScene Error " + e.Message);
            }
        }

        public override void EnableDebugMode(bool enable)
        {
            try
            {
                if (enableDebugModeFunc != null)
                {
                    enableDebugModeFunc(enable);
                }
            }
            catch (Exception e)
            {
                TapThemis.LocalDebugLog("EnableDebugMode Error " + e.Message);
            }
        }

        public override void EventTracking(string strEvent)
        {
            try
            {
                if (eventTrackingFunc != null)
                {
                    eventTrackingFunc(strEvent);
                }
            }
            catch (Exception e)
            {
                TapThemis.LocalDebugLog("EventTracking Error " + e.Message);
            }
        }

        public override void AddCustomField(string str_key, string str_value)
        {
            try
            {
                if (addCustomFieldFunc != null)
                {
                    addCustomFieldFunc(str_key, str_value);
                }
            }
            catch (Exception e)
            {
                TapThemis.LocalDebugLog("addCustomField Error " + e.Message);
            }

        }

        public override void SetCallback(TapThemisCallBackImp cb)
        {
            if (!TapThemis.IsInitialized)
            {
                return;
            }
            THEMIS.CallStatic("setCallback", cb);
        }

        public override void SetEngine(bool isCustom, bool isOpenGl)
        {
            if (!TapThemis.IsInitialized)
            {
                return;
            }
            THEMIS.CallStatic("set_engine", isCustom, isOpenGl);
        }

        public override void ReportCustomExceptionEx(string name, string reason, string message, bool isQuitApp, string extra_message, int extra_len)
        {
            try
            {
               THEMIS.CallStatic("ReportCustomExceptionEx", name, reason, message, isQuitApp, extra_message, extra_len);
            }
            catch (Exception e)
            {
                TapThemis.LocalDebugLog("ReportException Error " + e.Message);
            }
        }

        public override string GetHeartbeat(int index, long random)
        {
            try
            {
                string hb = THEMIS.CallStatic<string>("get_themis_heartbeat", index, random);
                return string.Copy(hb);
            }
            catch (Exception e)
            {
                TapThemis.LocalDebugLog("ReportException Error " + e.Message);
            }
            return "";
        }

        public override string GetOneidData()
        {
            try
            {
                string hb = THEMIS.CallStatic<string>("GetOneidData");
                return string.Copy(hb);
            }
            catch (Exception e)
            {
                TapThemis.LocalDebugLog("ReportException Error " + e.Message);
            }
            return "";
        }

        public override void SetUseExtendCallback(bool b)
        {
            if (!TapThemis.IsInitialized)
            {
                return;
            }
            THEMIS.CallStatic("SetUseExtendCallback", b);
        }
    }
#endif
}