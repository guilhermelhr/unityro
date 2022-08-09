using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Runtime.CompilerServices;

public static class AsyncOperationHandleExtensions {
    public struct AsyncOperationHandleAwaiter<T> : INotifyCompletion {
        private AsyncOperationHandle<T> _handle;

        public AsyncOperationHandleAwaiter(AsyncOperationHandle<T> handle) {
            _handle = handle;
        }

        public bool IsCompleted {
            get {
                return _handle.IsDone;
            }
        }

        public T GetResult() {
            if (_handle.Status == AsyncOperationStatus.Succeeded) {
                return _handle.Result;
            }
            throw _handle.OperationException;
        }

        public void OnCompleted(Action continuation) {
            _handle.Completed += _ => continuation();
        }
    }

    public struct AsyncOperationHandleAwaiter : INotifyCompletion {
        private AsyncOperationHandle _handle;

        public AsyncOperationHandleAwaiter(AsyncOperationHandle handle) {
            _handle = handle;
        }

        public bool IsCompleted {
            get {
                return _handle.IsDone;
            }
        }

        public object GetResult() {
            if (_handle.Status == AsyncOperationStatus.Succeeded) {
                return _handle.Result;
            }
            throw _handle.OperationException;
        }

        public void OnCompleted(Action continuation) {
            _handle.Completed += _ => continuation();
        }
    }

    /// <summary>
    /// Used to support the await keyword for AsyncOperationHandle.
    /// </summary>
    public static AsyncOperationHandleAwaiter<T> GetAwaiter<T>(this AsyncOperationHandle<T> handle) {
        return new AsyncOperationHandleAwaiter<T>(handle);
    }

    /// <summary>
    /// Used to support the await keyword for AsyncOperationHandle.
    /// </summary>
    public static AsyncOperationHandleAwaiter GetAwaiter(this AsyncOperationHandle handle) {
        return new AsyncOperationHandleAwaiter(handle);
    }
}