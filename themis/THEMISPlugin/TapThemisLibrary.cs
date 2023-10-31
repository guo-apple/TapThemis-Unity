using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace TapTap.Themis
{
    public interface TapThemisCallBack
    {
        void onHandleThemisState(int state, string message);
        string getExtraMessage();
        string getExceptionMessage();
        string getExtraMessageEx(string message);
        string getOOMMessage(float v);
        string getStrongKillMessage();
    }
#if UNITY_EDITOR
    public class TapThemisCallBackImp
    {
        public TapThemisCallBackImp(TapThemisCallBack cb)
        {
        }
        public void onHandleThemisState(int state, string message)
        {

        }

        public string getExtraMessage()
        {
            return null;
        }

        public string getExceptionMessage()
        {
            return null;
        }

        public string getExtraMessageEx(string message)
        {
            return null;
        }

        public string getOOMMessage(float v)
        {
            return null;
        }
        public string getStrongKillMessage()
        {
            return null;
        }
    }
#elif UNITY_ANDROID
    [SuppressMessage("ReSharper","InconsistentNaming")]
    public class TapThemisCallBackImp : AndroidJavaProxy
    {
        private static readonly string GAME_AGENT_CALLBACK_CLASS = "com.tds.themis.ThemisCallBack";
        private static TapThemisCallBack _callback;
        public TapThemisCallBackImp(TapThemisCallBack cb):base(GAME_AGENT_CALLBACK_CLASS)
        {
            _callback = cb;
        }
        public void onHandleThemisState(int state,string message)
        {
            if(_callback != null){
                _callback.onHandleThemisState(state,message);
            }
        }
        public string getExtraMessage()
        {
            if (_callback != null)
            {
                return _callback.getExtraMessage();
            }
            return null;
        }
        public string getExceptionMessage()
        {
            if (_callback != null)
            {
                return _callback.getExceptionMessage();
            }
            return null;
        }
        public string getExtraMessageEx(string message)
        {
            if (_callback != null)
            {
                return _callback.getExtraMessageEx(message);
            }
            return null;
        }
        public string getOOMMessage(float v)
        {
            return null;
        }
        public string getStrongKillMessage()
        {
            return null;
        }
    }
#elif UNITY_IPHONE || UNITY_IOS
    public class TapThemisCallBackImp
    {
        private static TapThemisCallBack _callback;
        public TapThemisCallBackImp(TapThemisCallBack cb)
        {
            _callback = cb;
        }

        public void onHandleThemisState(int state, string message)
        {
            if (_callback != null)
            {
                _callback.onHandleThemisState(state, message);
            }
        }

        public string getExtraMessage()
        {
            if (_callback != null)
            {
                return _callback.getExtraMessage();
            }
            return null;
        }

        public string getExceptionMessage()
        {
            if (_callback != null)
            {
                return _callback.getExceptionMessage();
            }
            return null;
        }

        public string getExtraMessageEx(string message)
        {
            if (_callback != null)
            {
                return _callback.getExtraMessageEx(message);
            }
            return null;
        }

        public string getOOMMessage(float v)
        {
            if (_callback != null)
            {
                return _callback.getOOMMessage(v);
            }
            return null;
        }

        public string getStrongKillMessage()
        {
            if (_callback != null)
            {
                return _callback.getStrongKillMessage();
            }
            return null;
        }
    }
#elif UNITY_STANDALONE_OSX
    public class TapThemisCallBackImp
    {
        private static TapThemisCallBack _callback;
        public TapThemisCallBackImp(TapThemisCallBack cb)
        {
            _callback = cb;
        }

        public void onHandleThemisState(int state, string message)
        {
            if (_callback != null)
            {
                _callback.onHandleThemisState(state, message);
            }
        }

        public string getExtraMessage()
        {
            if (_callback != null)
            {
                return _callback.getExtraMessage();
            }
            return null;
        }

        public string getExceptionMessage()
        {
            if (_callback != null)
            {
                return _callback.getExceptionMessage();
            }
            return null;
        }

        public string getExtraMessageEx(string message)
        {
            if (_callback != null)
            {
                return _callback.getExtraMessageEx(message);
            }
            return null;
        }

        public string getOOMMessage(float v)
        {
            if (_callback != null)
            {
                return _callback.getOOMMessage(v);
            }
            return null;
        }

        public string getStrongKillMessage()
        {
            if (_callback != null)
            {
                return _callback.getStrongKillMessage();
            }
            return null;
        }
    }
#elif UNITY_STANDALONE && UNITY_STANDALONE_WIN
    public class TapThemisCallBackImp
    {
        private static TapThemisCallBack _callback;
        public TapThemisCallBackImp(TapThemisCallBack cb)
        {
            _callback = cb;
        }

        public void onHandleThemisState(int state, string message)
        {
            if (_callback != null)
            {
                _callback.onHandleThemisState(state, message);
            }
        }

        public string getExtraMessage()
        {
            if (_callback != null)
            {
                return _callback.getExtraMessage();
            }
            return null;
        }

        public string getExceptionMessage()
        {
            if (_callback != null)
            {
                return _callback.getExceptionMessage();
            }
            return null;
        }

        public string getExtraMessageEx(string message)
        {
            if (_callback != null)
            {
                return _callback.getExtraMessageEx(message);
            }
            return null;
        }

        public string getOOMMessage(float v)
        {
            return null;
        }

        public string getStrongKillMessage()
        {
            return null;
        }
    }
#else
    public class TapThemisCallBackImp
    {
        public TapThemisCallBackImp(TapThemisCallBack cb)
        {
        }

        public void onHandleThemisState(int state, string message)
        {

        }

        public string getExtraMessage()
        {
            return null;
        }

        public string getExceptionMessage()
        {
            return null;
        }

        public string getExtraMessageEx(string message){         
            return null;
        }

        public string getOOMMessage(float v)
        {
            return null;
        }

        public string getStrongKillMessage()
        {
            return null;
        }
    }
#endif
}