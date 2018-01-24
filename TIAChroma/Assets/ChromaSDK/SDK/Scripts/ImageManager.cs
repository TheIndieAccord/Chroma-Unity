﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace ChromaSDK
{
    public class ImageManager
    {
#if UNITY_3 || UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
        const string DLL_NAME = "UnityImageNativePlugin3";

#else
    const string DLL_NAME = "UnityImageNativePlugin";
#endif
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        [DllImport(DLL_NAME)]
        private static extern void PluginLoadImage(IntPtr path);

        [DllImport(DLL_NAME)]
        public static extern int PluginGetFrameCount();

        [DllImport(DLL_NAME)]
        public static extern int PluginGetHeight();

        [DllImport(DLL_NAME)]
        public static extern int PluginGetWidth();

        [DllImport(DLL_NAME)]
        public static extern int PluginGetPixel(int frame, int x, int y);

        #region Handle Debug.Log from unmanged code

        [DllImport(DLL_NAME)]
        private static extern void PluginSetLogDelegate(IntPtr logDelegate);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DebugLogDelegate(string text);

        private static IntPtr _sLogDelegate = IntPtr.Zero;

        private static void LogCallBack(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            Debug.Log(string.Format(":C++: {0}", text));
        }

        private static void SetupLogMechanism()
        {
            DebugLogDelegate logCallback = new DebugLogDelegate(LogCallBack);
            _sLogDelegate = Marshal.GetFunctionPointerForDelegate(logCallback);

            // Call the API passing along the function pointer.
            PluginSetLogDelegate(_sLogDelegate);
        }

        #endregion

        public static void LoadImage(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                byte[] array = ASCIIEncoding.ASCII.GetBytes(fi.FullName + "\0");
                IntPtr lpData = Marshal.AllocHGlobal(array.Length);
                Marshal.Copy(array, 0, lpData, array.Length);
                PluginLoadImage(lpData);
                Marshal.FreeHGlobal(lpData);
            }
        }

        static ImageManager()
        {
            SetupLogMechanism();
        }
#endif
    }
}
