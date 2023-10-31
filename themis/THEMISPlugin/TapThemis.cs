// ----------------------------------------
//
//  THEMISInit.cs
//
//  Author:
//       braindead, <jimengwu@xd.com>
//
//  Copyright (c) 2021 TapThemis.  All rights reserved.
//
// ----------------------------------------
//

using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using AOT;
//using UnityEngine;
//using UnityEngine.Diagnostics;
//using UnityEngine;

namespace TapTap.Themis
{
    // We dont use the LogType enum in Unity as the numerical order doesnt suit our purposes
    /// <summary>
    /// Log severity. 
    /// { Log, LogDebug, LogInfo, LogWarning, LogAssert, LogError, LogException }
    /// </summary>
    public enum LogSeverity
    {
        Log,
        LogDebug,
        LogInfo,
        LogWarning,
        LogAssert,
        LogError,
        LogException,
        LogIgnore
    }    

    public sealed class TapThemis
    {
        // Define delegate support multicasting to replace the 'Application.LogCallback'
        public delegate void LogCallbackDelegate(string condition, string stackTrace, LogType type);

        public static void InitTHEMIS()
        {
            if (IsInitialized)
            {
                LocalDebugLog("TapThemis has already been initialized.");
                return;
            }

            TapThemisAgent.Get().InitTHEMISAgent();
            LocalDebugLog("Initialized TapThemis");

            // Register the LogCallbackHandler by Application.RegisterLogCallback(Application.LogCallback)
            _RegisterExceptionHandler();
        }

        public static void InitTHEMISWithID(string appid)
        {
            if (IsInitialized)
            {
                LocalDebugLog("TapThemis has already been initialized.");
                return;
            }
            TapThemisAgent.Get().InitTHEMISAgentWithID(appid);
            LocalDebugLog("Initialized TapThemis");
            _RegisterExceptionHandler();
          
        }


        //public static void isableUnregisterExceptionHandler
        public static void ConfigDebugMode(bool enable)
        {
            _debugMode = enable;
            TapThemisAgent.Get().EnableDebugMode(enable);
            LocalDebugLog("{0} the log message print to console", enable ? "Enable" : "Disable");
        }

        /// <summary>
        /// Configs the auto quit application.
        /// </summary>
        /// <param name="autoQuit">If set to <c>true</c> auto quit.</param>
        public static void U3d_ConfigAutoQuitApplication(bool autoQuit)
        {
            _autoQuitApplicationAfterReport = autoQuit;
        }

        /// <summary>
        /// Configs the auto report log level. Default is LogSeverity.LogError.
        /// <example>
        /// LogSeverity { Log, LogDebug, LogInfo, LogWarning, LogAssert, LogError, LogException }
        /// </example>
        /// </summary>
        /// 
        /// <param name="level">Level.</param> 
        public static void U3d_ConfigAutoReportLogLevel(LogSeverity level)
        {
            _autoReportLogLevel = level;
        }


        public static void U3d_RegisterLogCallback(LogCallbackDelegate handler)
        {
            if (handler != null)
            {
                LocalDebugLog("Add log callback handler: {0}", handler);

                _LogCallbackEventHandler += handler;
            }
        }

        public static void U3d_UnregisterLogCallback(LogCallbackDelegate handler)
        {
            if (handler != null)
            {
                LocalDebugLog("Remove log callback handler");

                _LogCallbackEventHandler -= handler;
            }
        }

        //public static void U3d_SetLogCallbackExtrasHandler(Func<Dictionary<string, string>> handler)
        //{
        //    if (!IsInitialized)
        //    {
        //        return;
        //    }
        //    if (handler != null)
        //    {
        //        _LogCallbackExtrasHandler = handler;

        //        LocalDebugLog("Add log callback extra data handler : {0}", handler);
        //    }
        //}

        public static void SetGamePlayer(string playerId)
        {
            if (!IsInitialized)
            {
                return;
            }
            LocalDebugLog("Set player id: {0}", playerId);

            TapThemisAgent.Get().SetGamePlayer(playerId);
        }

        public static void SetGameScene(string sceneId)
        {
            if (!IsInitialized)
            {
                return;
            }
            LocalDebugLog("Set scene: {0}", sceneId);

            TapThemisAgent.Get().SetGameCurrentScene(sceneId);
        }

        public static void ReportException(System.Exception e, string message)
        {
            if (!IsInitialized)
            {
                return;
            }

            LocalDebugLog("Report exception: {0}\n------------\n{1}\n------------", message, e);

            _HandleException(e, message, false);
        }

        public static void ReportCustomException(string name, string reason, string message, bool isQuitApp)
        {
            if (!IsInitialized)
            {
                return;
            }

            LocalDebugLog("ReportCustomException: {0}\n------------\n{1}\n------------\n{2}\n", name, reason, message);

            TapThemisAgent.Get().ReportCustomException(name, reason, message, isQuitApp);
        }


        public static void EventTracking(string format, params object[] args)
        {
            if (!IsInitialized)
            {
                return;
            }
            if (string.IsNullOrEmpty(format))
            {
                return;
            }
            TapThemisAgent.Get().EventTracking(string.Format(format, args));
        }


        public static void AddCustomField(string str_key, string str_value)
        {
            if (!IsInitialized)
            {
                return;
            }
            if (string.IsNullOrEmpty(str_key))
            {
                return;
            }

            TapThemisAgent.Get().AddCustomField(str_key, str_value);
        }

        public static void SetCallback(TapThemisCallBackImp cb)
        {
            TapThemisAgent.Get().SetCallback(cb);
        }

        public static void SetEngine(bool isCustom, bool isOpenGl)
        {
            TapThemisAgent.Get().SetEngine(isCustom, isOpenGl);
        }

        public static void ReportCustomExceptionEx(string name, string reason, string message, bool isQuitApp, string extra_message, int extra_len)
        {
            if (!IsInitialized)
            {
                return;
            }            
            //UnityEngine.Debug.Log("THEMIS: ReportCustomExceptionEx: {0}\n------------\n{1}\n------------\n{2}\n"+ name+ reason+ message);            

            TapThemisAgent.Get().ReportCustomExceptionEx(name, reason, message, isQuitApp, extra_message, extra_len);
        }

        public static string GetHeartbeat(int index, long random)
        {
            if (!IsInitialized)
            {
                return "";
            }
            return TapThemisAgent.Get().GetHeartbeat(index,random);
        }

        public static string GetOneidData()
        {
            if (!IsInitialized)
            {
                return "";
            }
            return TapThemisAgent.Get().GetOneidData();
        }

        public static void SetUseExtendCallback(bool b)
        {
            TapThemisAgent.Get().SetUseExtendCallback(b);
        }

        #region Privated Fields and Methods 
        private static event LogCallbackDelegate _LogCallbackEventHandler;

        private static bool _isInitialized = false;
        private static LogSeverity _autoReportLogLevel = LogSeverity.LogError;

#pragma warning disable 414
        private static bool _debugMode = false;
        private static bool _autoQuitApplicationAfterReport = false;

        private static readonly string _pluginVersion = "1.0";

        private static Func<Dictionary<string, string>> _LogCallbackExtrasHandler;

        private static bool _uncaughtAutoReportOnce = false;

        public static string PluginVersion
        {
            get { return _pluginVersion; }
        }

        public static bool IsInitialized
        {
            get { return _isInitialized; }
        }

        public static bool AutoQuitApplicationAfterReport
        {
            get { return _autoQuitApplicationAfterReport; }
        }

        private static void _RegisterExceptionHandler()
        {
            try
            {
                // hold only one instance 

#if UNITY_5
                Application.logMessageReceived += _OnLogCallbackHandler;
#else
                // Application.RegisterLogCallback(_OnLogCallbackHandler);
                Application.logMessageReceived += _OnLogCallbackHandler;
#endif
                AppDomain.CurrentDomain.UnhandledException += _OnUncaughtExceptionHandler;

                _isInitialized = true;

                LocalDebugLog("Register the log callback in Unity {0}", Application.unityVersion);
            }
            catch
            {

            }
        }

        private static void _UnregisterExceptionHandler()
        {
            try
            {
#if UNITY_5
                Application.logMessageReceived -= _OnLogCallbackHandler;
#else
                Application.logMessageReceived -= _OnLogCallbackHandler;
                //Application.RegisterLogCallback(null);
#endif
                System.AppDomain.CurrentDomain.UnhandledException -= _OnUncaughtExceptionHandler;
                LocalDebugLog("Unregister the log callback in unity {0}", Application.unityVersion);
            }
            catch
            {

            }
        }

        public static void LocalDebugLog(string format, params object[] args)
        {
            if (!_debugMode)
            {
                return;
            }

            if (string.IsNullOrEmpty(format))
            {
                return;
            }
            System.Console.WriteLine("THEMIS: TapThemis: {0}", string.Format(format, args));
            //DebugLog(string.Format("TapThemis : {0}", string.Format(format, args)));

            UnityEngine.Debug.Log(string.Format("TapThemis : {0}", string.Format(format, args)));
        }

        private static void _OnLogCallbackHandler(string condition, string stackTrace, LogType type)
        {

            if (_LogCallbackEventHandler != null)
            {
                _LogCallbackEventHandler(condition, stackTrace, type);
            }

            if (!IsInitialized)
            {
                return;
            }
            if (!string.IsNullOrEmpty(condition) && condition.Contains("THEMIS:"))
            {
                return;
            }
            if (_uncaughtAutoReportOnce)
            {
                return;
            }
            // convert the log level
            LogSeverity logLevel = LogSeverity.Log;
            switch (type)
            {
                case LogType.Exception:
                    logLevel = LogSeverity.LogException;
                    break;
                case LogType.Error:
                    logLevel = LogSeverity.LogError;
                    break;
                case LogType.Assert:
                    logLevel = LogSeverity.LogAssert;
                    break;
                case LogType.Warning:
                    logLevel = LogSeverity.LogWarning;
                    break;
                case LogType.Log:
                    logLevel = LogSeverity.LogDebug;
                    break;
                default:
                    break;
            }

            if (LogSeverity.Log == logLevel)
            {
                return;
            }
            _HandleException(logLevel, null, condition, stackTrace, true);
        }

        private static void _OnUncaughtExceptionHandler(object sender, System.UnhandledExceptionEventArgs args)
        {

            if (args == null || args.ExceptionObject == null)
            {
                return;
            }
            try
            {
                if (args.ExceptionObject.GetType() != typeof(System.Exception))
                {
                    return;
                }
            }
            catch
            {
                if (UnityEngine.Debug.isDebugBuild == true)
                {
                    UnityEngine.Debug.Log("THEMIS: Failed to report Unity 3D uncaught exception");
                }

                return;
            }

            if (!IsInitialized)
            {
                return;
            }

            if (_uncaughtAutoReportOnce)
            {
                return;
            }

            _HandleException((System.Exception)args.ExceptionObject, null, true);
        }

        private static void _HandleException(System.Exception e, string message, bool uncaught)
        {
            if (e == null)
            {
                return;
            }


            if (!IsInitialized)
            {
                return;
            }

            string name = e.GetType().Name;
            string reason = e.Message;

            if (!string.IsNullOrEmpty(message))
            {
                reason = string.Format("{0}***{1}", reason, message);
            }

            StringBuilder stackTraceBuilder = new StringBuilder("");


            StackTrace stackTrace = new StackTrace(e, true);
            int count = stackTrace.FrameCount;
            for (int i = 0; i < count; i++)
            {

                StackFrame frame = stackTrace.GetFrame(i);

                stackTraceBuilder.AppendFormat("{0}.{1}", frame.GetMethod().DeclaringType.Name, frame.GetMethod().Name);

                ParameterInfo[] parameters = frame.GetMethod().GetParameters();
                if (parameters == null || parameters.Length == 0)
                {
                    stackTraceBuilder.Append(" () ");
                }
                else
                {
                    stackTraceBuilder.Append(" (");

                    int pcount = parameters.Length;

                    ParameterInfo param = null;
                    for (int p = 0; p < pcount; p++)
                    {
                        param = parameters[p];
                        stackTraceBuilder.AppendFormat("{0} {1}", param.ParameterType.Name, param.Name);

                        if (p != pcount - 1)
                        {
                            stackTraceBuilder.Append(", ");
                        }
                    }
                    param = null;

                    stackTraceBuilder.Append(") ");
                }


                string fileName = frame.GetFileName();
                if (!string.IsNullOrEmpty(fileName) && !fileName.ToLower().Equals("unknown"))
                {
                    fileName = fileName.Replace("\\", "/");

                    int loc = fileName.ToLower().IndexOf("/assets/");
                    if (loc < 0)
                    {
                        loc = fileName.ToLower().IndexOf("assets/");
                    }

                    if (loc > 0)
                    {
                        fileName = fileName.Substring(loc);
                    }

                    stackTraceBuilder.AppendFormat("(at {0}:{1})", fileName, frame.GetFileLineNumber());
                }
                stackTraceBuilder.AppendLine();
            }

            // report

            _reportException(name, reason, stackTraceBuilder.ToString(), uncaught);
        }

        private static void _HandleException(LogSeverity logLevel, string name, string message, string stackTrace, bool uncaught)
        {

            if (!IsInitialized)
            {
                LocalDebugLog("It has not been initialized.");
                return;
            }
            if (logLevel == LogSeverity.Log)
            {
                return;
            }
            if ((uncaught && logLevel < _autoReportLogLevel))
            {
                LocalDebugLog("Not report exception for level {0}", logLevel.ToString());
                return;
            }
            string type = null;
            string reason = null;

            if (!string.IsNullOrEmpty(message))
            {
                if(message.Contains("[themis ignore]")){
                    return;
                }
                try
                {
                    if ((LogSeverity.LogException == logLevel) && message.Contains("Exception"))
                    {

                        Match match = new Regex(@"^(?<errorType>\S+):\s*(?<errorMessage>.*)", RegexOptions.Singleline).Match(message);

                        if (match.Success)
                        {
                            type = match.Groups["errorType"].Value.Trim();
                            reason = match.Groups["errorMessage"].Value.Trim();
                        }
                    }
                    else if ((LogSeverity.LogError == logLevel))
                    {
                        if (message.StartsWith("Unhandled Exception:", System.StringComparison.Ordinal))
                        {
                            Match match = new Regex(@"^Unhandled\s+Exception:\s*(?<exceptionName>\S+):\s*(?<exceptionDetail>.*)", RegexOptions.Singleline).Match(message);

                            if (match.Success)
                            {
                                string exceptionName = match.Groups["exceptionName"].Value.Trim();
                                string exceptionDetail = match.Groups["exceptionDetail"].Value.Trim();

                                // 
                                int dotLocation = exceptionName.LastIndexOf(".");
                                if (dotLocation > 0 && dotLocation != exceptionName.Length)
                                {
                                    type = exceptionName.Substring(dotLocation + 1);
                                }
                                else
                                {
                                    type = exceptionName;
                                }

                                int stackLocation = exceptionDetail.IndexOf(" at ");
                                if (stackLocation > 0)
                                {
                                    // 
                                    reason = exceptionDetail.Substring(0, stackLocation);
                                    // substring after " at "
                                    string callStacks = exceptionDetail.Substring(stackLocation + 3).Replace(" at ", "\n").Replace("in <filename unknown>:0", "").Replace("[0x00000]", "");
                                    //
                                    stackTrace = string.Format("{0}\n{1}", stackTrace, callStacks.Trim());

                                }
                                else
                                {
                                    reason = exceptionDetail;
                                }


                                // for LuaScriptException
                                if (type.Equals("LuaScriptException") && exceptionDetail.Contains(".lua") && exceptionDetail.Contains("stack traceback:"))
                                {
                                    //exceptionDetail.Replace("stack traceback:", "lua_stack_traceback:");
                                    stackLocation = exceptionDetail.IndexOf("stack traceback:");

                                    if (stackLocation > 0)
                                    {
                                        reason = exceptionDetail.Substring(0, stackLocation);
                                        // substring after "stack traceback:"
                                        string callStacks = exceptionDetail.Substring(stackLocation).Replace("stack traceback:", "lua_stack_traceback:");

                                        //
                                        stackTrace = string.Format("{0}\n{1}", stackTrace, callStacks.Trim());
                                    }
                                }
                            }
                        }
                        else if (message.Contains("stack traceback:"))
                        {
                            int stackLocation = message.IndexOf("stack traceback:");

                            if (stackLocation > 0)
                            {
                                reason = message.Substring(0, stackLocation).Trim();
                                // substring after "stack traceback:"
                                string callStacks = message.Substring(stackLocation);
                                callStacks = callStacks.Replace("stack traceback:", "lua_stack_traceback:");

                                stackTrace = string.Format("{0}\n{1}", stackTrace, callStacks.Trim());
                            }
                        }


                    }
                }
                catch
                {

                }

                if (string.IsNullOrEmpty(reason))
                {
                    reason = message;
                }
            }

            if (string.IsNullOrEmpty(name))
            {
                if (string.IsNullOrEmpty(type))
                {
                    type = string.Format("Unity{0}", logLevel.ToString());
                }
            }
            else
            {
                type = name;
            }

            _reportException(type, reason, stackTrace, uncaught);
        }

        private static void _reportException(string name, string reason, string stackTrace, bool uncaught)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            if (string.IsNullOrEmpty(stackTrace))
            {
                stackTrace = StackTraceUtility.ExtractStackTrace();
            }

            if (string.IsNullOrEmpty(stackTrace))
            {
                stackTrace = "Empty";
            }
            else
            {

                try
                {
                    string[] frames = stackTrace.Split('\n');

                    if (frames != null && frames.Length > 0)
                    {

                        StringBuilder trimFrameBuilder = new StringBuilder();

                        string frame = null;
                        int count = frames.Length;
                        bool isLua = false;
                        for (int i = 0; i < count; i++)
                        {
                            frame = frames[i];

                            if (frame.StartsWith("lua_stack_traceback:"))
                            {
                                isLua = true;
                                trimFrameBuilder.AppendLine();
                            }

                            if (isLua)
                            {
                                trimFrameBuilder.Append(frame);
                                trimFrameBuilder.AppendLine();
                                continue;
                            }

                            if (string.IsNullOrEmpty(frame) || string.IsNullOrEmpty(frame.Trim()))
                            {
                                continue;
                            }

                            frame = frame.Trim();

                            // System.Collections.Generic
                            if (frame.StartsWith("System.Collections.Generic.") || frame.StartsWith("ShimEnumerator"))
                            {
                                continue;
                            }
                            if (frame.StartsWith("THEMIS"))
                            {
                                continue;
                            }
                            if (frame.Contains("..ctor"))
                            {
                                continue;
                            }

                            int start = frame.ToLower().IndexOf("(at");
                            int end = frame.ToLower().IndexOf("/assets/");

                            if (start > 0 && end > 0)
                            {
                                trimFrameBuilder.AppendFormat("{0}(at {1}", frame.Substring(0, start).Replace(":", "."), frame.Substring(end));
                            }
                            else
                            {
                                trimFrameBuilder.Append(frame.Replace(":", "."));
                            }

                            trimFrameBuilder.AppendLine();
                        }
                        //trimFrameBuilder.AppendLine();
                        stackTrace = trimFrameBuilder.ToString();
                    }
                }
                catch
                {
                    LocalDebugLog("{0}", "Error to parse the stack trace");
                }

            }

            LocalDebugLog("_reportException: name = {0}\n reason = {1}\nstackTrace = {2}", name, reason, stackTrace);

            _uncaughtAutoReportOnce = uncaught && _autoQuitApplicationAfterReport;
            TapThemisAgent.Get().ReportException(name, reason, stackTrace, _uncaughtAutoReportOnce);
        }
#endregion
    }
}