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
    public class TapThemisAgent
    {
        static TapThemisAgent themisAgent = null;

        public static TapThemisAgent Get()
        {

            if (null == themisAgent)
            {
#if UNITY_EDITOR||TAPTHEMIS_DISABLE
                themisAgent = new TapThemisAgent();
#elif UNITY_STANDALONE && UNITY_STANDALONE_WIN
                themisAgent = new TapThemisAgentWin();
#elif UNITY_ANDROID
                themisAgent = new TapThemisAgentAndroid();
#elif UNITY_IPHONE || UNITY_IOS
                themisAgent = new TapThemisAgentIOS();
#elif UNITY_STANDALONE_OSX
                themisAgent = new TapThemisAgentMac();
#endif
            }
            return themisAgent;
        }


        public virtual void InitTHEMISAgent()
        {
        }

        public virtual void InitTHEMISAgentWithID(string appID)
        {
        }


        public virtual void SetGamePlayer(string gamePlayer)
        {
        }

        public virtual void SetGameCurrentScene(string sceneId)
        {
        }

        public virtual void EnableDebugMode(bool enable)
        {
        }

        public virtual void ReportException(string name, string reason, string stackTrace, bool isQuitApp)
        {
        }

        public virtual void ReportCustomException(string name, string reason, string message, bool isQuitApp)
        {

        }

        public virtual void EventTracking(string strEvent)
        {
        }

        public virtual void AddCustomField(string str_key, string str_value)
        {
        }

        public virtual void SetCallback(TapThemisCallBackImp cb)
        {
        }

        public virtual void SetEngine(bool isCustom, bool isOpenGl)
        {

        }

        public virtual void ReportCustomExceptionEx(string name, string reason, string message, bool isQuitApp,string extra_message,int extra_len)
        {

        }

        public virtual string GetHeartbeat(int index,long random)
        {
            return "";
        }

        public virtual string GetOneidData()
        {
            return "";
        }

        public virtual void SetUseExtendCallback(bool b)
        {

        }
    }
}