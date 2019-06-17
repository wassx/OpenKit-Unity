using System;

namespace Dynatrace.OpenKit.Unity.Logger {
    public class UnityLogger : Dynatrace.OpenKit.API.ILogger {
        bool Dynatrace.OpenKit.API.ILogger.IsErrorEnabled {
            get {
                return true;
            }
        }

        bool Dynatrace.OpenKit.API.ILogger.IsWarnEnabled {
            get {
                return true;
            }
        }

        bool Dynatrace.OpenKit.API.ILogger.IsInfoEnabled {
            get {
                return true;
            }
        }

        bool Dynatrace.OpenKit.API.ILogger.IsDebugEnabled {
            get {
                return true;
            }
        }

        public void Debug(string message) {
            UnityEngine.Debug.Log(message);
        }

        public void Error(string message) {
            UnityEngine.Debug.LogError(message);
        }

        public void Error(string message, Exception exception) {
            UnityEngine.Debug.LogError(message);
            UnityEngine.Debug.LogException(exception);
        }

        public void Info(string message) {
            UnityEngine.Debug.Log(message);
        }

        public void Warn(string message) {
            UnityEngine.Debug.LogWarning(message);
        }
    }
}
