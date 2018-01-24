﻿using ChromaSDK;
// Access to the Chroma API
using ChromaSDK.Api;
// Access to the Session data structures
using RazerSDK.ChromaPackage.Model;
// Access to the Session API
using RazerSDK.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

using Object = UnityEngine.Object;

// Unity 3.X doesn't like namespaces
#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class ChromaConnectionManager : MonoBehaviour, IUpdate
{
    #region Connection Setup

    private const string CONNECTED = "Connected";
    private const string CONNECTING = "Connecting";
    private const string NOT_CONNECTED = "Not Connected";
    private const string RECONNECT_CHROMA_API_NULL = "Reconnect, ChromaAPI is null!";
    private const string RECONNECT_CHROMA_API_HEARTBEAT_FAILURE = "Reconnnect, Heartbeat failed!";
    private const string RECONNECT_CHROMA_API_HEARTBEAT_TIMEOUT = "Reconnnect, Heartbeat timeout!";
    private const string RECONNECT_RAZER_API_TIMEOUT = "Reconnnect, Connect timeout!";
    private const string RECONNECT_VERSION_IS_NULL = "Reconnect, Version is null!";
    private const string RECONNECT_VERSION_IS_UNKNOWN = "Reconnect, Version is unknown!";
    private const string RECONNECT_SERVER_UNREACHABLE = "Reconnect, Server can't be reached! Is Synapse Installed?";
    private const string RECONNECT_SYNAPSE_OUTDATED = "Reconnect, Synapse is out of date!";
    private const string VERSION_CHECK = "Checking Version";

    /// <summary>
    /// The connection info
    /// </summary>
    public ChromaSdkInput _mInfo = null;

    #endregion

    #region Singleton Setup

    /// <summary>
    /// Singleton name
    /// </summary>
    private const string INSTANCE_NAME = "ChromaConnectionManager";

    /// <summary>
    /// Singleton instance
    /// </summary>
    private static ChromaConnectionManager _sInstance = null;

    /// <summary>
    /// Singleton interface
    /// </summary>
    public static ChromaConnectionManager Instance
    {
        get
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    break;
                default:
                    return null;
            }
            if (null == _sInstance)
            {
                GameObject go = GameObject.Find(INSTANCE_NAME);
                if (null == go)
                {
                    go = new GameObject(INSTANCE_NAME);
                }
                else
                {
                    _sInstance = go.GetComponent<ChromaConnectionManager>();
                }
                if (null == _sInstance)
                {
                    _sInstance = go.AddComponent<ChromaConnectionManager>();
                }
            }
            return _sInstance;
        }
    }

    #endregion

    #region API Setup

    /// <summary>
    /// Instance of the RazerAPI
    /// </summary>
    private static RazerApi _sApiRazerInstance = null;

    /// <summary>
    /// Instance of the API
    /// </summary>
    private static ChromaApi _sApiChromaInstance = null;

    /// <summary>
    /// Attempt only one connection at a time
    /// </summary>
    private static bool _sConnecting = false;

    /// <summary>
    /// Track the connected state
    /// </summary>
    private static bool _sConnected = false;

    /// <summary>
    /// The connection status
    /// </summary>
    private static string _sConnectionStatus = NOT_CONNECTED;

    /// <summary>
    /// Accessor for Chroma Instance
    /// </summary>
    public ChromaApi ApiChromaInstance
    {
        get
        {
            return _sApiChromaInstance;
        }
    }

    /// <summary>
    /// Is the API connecting
    /// </summary>
    public bool Connecting
    {
        get
        {
            return _sConnecting;
        }
    }

    /// <summary>
    /// Is the API connected?
    /// </summary>
    public bool Connected
    {
        get
        {
            return _sConnected;
        }
    }

    /// <summary>
    /// Get the detailed connection status
    /// </summary>
    public string ConnectionStatus
    {
        get
        {
            return _sConnectionStatus;
        }
        private set
        {
            //Debug.Log(value);
            _sConnectionStatus = value;
        }
    }

    #endregion

    #region Thread handling

    /// <summary>
    /// Shutdown the connection on recompile or app exit
    /// </summary>
    private static bool _sWaitForExit = true;

    /// <summary>
    /// Keep track if connection is active
    /// </summary>
    private static bool _sConnectionIsActive = false;

    /// <summary>
    /// Use for co-routine simulation
    /// </summary>
    private static List<IEnumerator> _sPendingRoutines = new List<IEnumerator>();

    /// <summary>
    /// Handle coroutines in edit-mode
    /// </summary>
    /// <param name="routineName"></param>
    /// <param name="routine"></param>
    protected void SafeStartCoroutine(string routineName, IEnumerator routine)
    {
        if (Application.isPlaying)
        {
            StartCoroutine(routine);
        }
        else
        {
            //LogOnMainThread(string.Format("SafeStartCoroutine: {0}", routineName));
            _sPendingRoutines.Add(routine);
        }
    }

    /// <summary>
    /// Actions to run on the main thread
    /// </summary>
    private static List<Action> _sMainActions = new List<Action>();

    /// <summary>
    /// Run on the main thread,
    /// used to initialize
    /// </summary>
    /// <param name="action"></param>
    private void RunOnMainThread(Action action)
    {
        _sMainActions.Add(action);
    }

    #endregion

    #region MonoBehaviour Events

    public void Awake()
    {
        if (!ChromaUtils.IsPlatformSupported())
        {
            ChromaUtils.SetActive(gameObject, false);
            return;
        }
        if (Application.isPlaying)
        {
            DontDestroyOnLoad(gameObject);
        }
        Connect();
    }

    public void OnApplicationQuit()
    {
        if (ChromaUtils.IsPlatformSupported())
        {
            Disconnect();
        }
    }

    /// <summary>
    /// If compile is detected, disconnect once
    /// </summary>
    bool _mDetectedCompile = false;

    public void Update()
    {
#if UNITY_EDITOR
        if (EditorApplication.isCompiling)
        {
            if (!_mDetectedCompile)
            {
                _mDetectedCompile = true;
                Disconnect();
            }
            return;
        }
        else if (_mDetectedCompile)
        {
            _mDetectedCompile = false;
            Connect();
        }
#endif
        //LogOnMainThread("ChromaConnectionManager: Update");
        if (_sMainActions.Count > 0)
        {
            Action action = _sMainActions[0];
            _sMainActions.RemoveAt(0);
            action.Invoke();
        }

        int i = 0;
        while (i < _sPendingRoutines.Count)
        {
            //LogOnMainThread(string.Format("EditorUpdate: {0}", _sPendingRoutines.Count));
            IEnumerator routine = _sPendingRoutines[i];
            try
            {
                if (null != routine &&
                    routine.MoveNext())
                {
                    //more work
                    ++i;
                    continue;
                }
            }
            catch (Exception)
            {
                // something bad happened,
                // stop coroutine
            }
            //done
            _sPendingRoutines.RemoveAt(i);
            continue;
        }
    }

#endregion

#region Initialization

    /// <summary>
    /// Initialize after a delay
    /// </summary>
    /// <returns></returns>
    IEnumerator Initialize()
    {
        // wait to initialize in case recompile just shutdown
        //LogOnMainThread("Waiting to initialize ChromaSDK...");

        // delay, WaitForSeconds doesn't work in edit mode
        DateTime wait = DateTime.Now + TimeSpan.FromSeconds(2);
        while (_sWaitForExit &&
            DateTime.Now < wait)
        {
            yield return null;
        }

        if (_sWaitForExit)
        {
            ChromaUtils.RunOnThread(() =>
            {
                // start initialization
                PostChromaSdk();
            });
        }
    }

    void LogOnMainThread(string text)
    {
        RunOnMainThread(() =>
        {
            Debug.Log(text);
        });
    }

    void LogErrorOnMainThread(string text)
    {
        RunOnMainThread(() =>
        {
            Debug.LogError(text);
        });
    }

    public void SetupDefaultInfo()
    {
        _mInfo = new ChromaSdkInput();

        _mInfo.Title = "UnityPlugin";
        _mInfo.Description = "REST client for Unity";
        _mInfo.Author = new ChromaSdkInputAuthor();
        _mInfo.Author.Name = "Chroma Developer";
        _mInfo.Author.Contact = "www.razerzone.com";
        _mInfo.DeviceSupported = new List<string>
            {
                "keyboard",
                "mouse",
                "headset",
                "mousepad",
                "keypad",
                "chromalink",
            };
        _mInfo.Category = "application";
    }

    /// <summary>
    /// Initialize Chroma by hitting the REST server and set the API port
    /// </summary>
    /// <returns></returns>
    void PostChromaSdk()
    {
        _sConnectionIsActive = true;

        //LogOnMainThread("PostChromaSdk:");
        bool reconnect = false;
        try
        {
            if (null != _sApiRazerInstance)
            {
                return;
            }

            // use the Razer API to get the session
            _sApiRazerInstance = new RazerApi();

            if (null == _mInfo)
            {
                SetupDefaultInfo();
            }

            PostChromaSdkResponse result = null;
            DateTime timeout = DateTime.Now + TimeSpan.FromSeconds(5);
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    ConnectionStatus = VERSION_CHECK;
                    GetChromaSdkResponse getResult = _sApiRazerInstance.GetChromaSdk();

                    if (null == getResult ||
                        string.IsNullOrEmpty(getResult.Version))
                    {
                        ConnectionStatus = RECONNECT_VERSION_IS_NULL;
                        reconnect = true;
                        return;
                    }

                    string[] parts = getResult.Version.Split(".".ToCharArray());
                    if (parts.Length != 3)
                    {
                        ConnectionStatus = RECONNECT_VERSION_IS_UNKNOWN;
                        reconnect = true;
                        return;
                    }
                    int[] version = new int[3];
                    if (int.TryParse(parts[0], out version[0]) &&
                        int.TryParse(parts[1], out version[1]) &&
                        int.TryParse(parts[2], out version[2]))
                    {
                        if (version[0] >= 2 ||
                            (version[0] == 2 &&
                            version[1] >= 3) ||
                            (version[0] == 2 &&
                            version[1] == 3 &&
                            version[2] >= 6))
                        {
                            //good!
                        }
                        else
                        {
                            ConnectionStatus = RECONNECT_SYNAPSE_OUTDATED;
                            reconnect = true;
                        }
                    }
                    else
                    {
                        ConnectionStatus = RECONNECT_VERSION_IS_UNKNOWN;
                        reconnect = true;
                    }
                }
                catch (Exception)
                {
                    ConnectionStatus = RECONNECT_SERVER_UNREACHABLE;
                    reconnect = true;
                }

                if (!reconnect)
                {
                    try
                    {
                        //LogOnMainThread("Initializing...");
                        ConnectionStatus = CONNECTING;
                        result = _sApiRazerInstance.PostChromaSdk(_mInfo);
                    }
                    catch (Exception)
                    {
                        ConnectionStatus = RECONNECT_SERVER_UNREACHABLE;
                        reconnect = true;
                    }
                }
            }));
            thread.Start();
            while (_sWaitForExit &&
                DateTime.Now < timeout &&
                thread.IsAlive)
            {
                Thread.Sleep(0);
            }
            if (_sWaitForExit &&
                timeout < DateTime.Now &&
                thread.IsAlive)
            {
                //Debug.LogError("Connect: Timeout detected!");
                thread.Abort();
                reconnect = true;
                ConnectionStatus = RECONNECT_RAZER_API_TIMEOUT;
                ThreadWaitForSecond();
            }

            if (_sWaitForExit)
            {
                if (null != result)
                {
                    //LogOnMainThread(result);

                    // setup the api instance with the session uri
                    _sApiChromaInstance = new ChromaApi(result.Uri);

                    //LogOnMainThread("Init complete.");

                    // use heartbeat to keep the REST API alive
                    DoHeartbeat();
                }
                else
                {
                    //Debug.LogError("Connect: Result is null!");
                    reconnect = true;
                }
            }
        }
        catch (Exception)
        {
            reconnect = true;
        }

        if (reconnect)
        {
            //LogErrorOnMainThread(string.Format("Exception when calling RazerApi.PostChromaSdk: {0}", e));
            _sApiRazerInstance = null;

            //attempt reconnect
            // Coroutines can only start from the main thread
            RunOnMainThread(() =>
            {
                if (_sWaitForExit)
                {
                    // retry
                    SafeStartCoroutine("Initialize", Initialize());
                }
            });
        }

        _sConnectionIsActive = false;
    }

#endregion

#region Heartbeat

    /// <summary>
    /// Wait for a second, from a thread
    /// </summary>
    void ThreadWaitForSecond()
    {
        // Wait for a sec
        DateTime wait = DateTime.Now + TimeSpan.FromSeconds(1);
        // avoid blocking exit
        while (_sWaitForExit &&
            DateTime.Now < wait)
        {
            Thread.Sleep(0);
        }
    }

    /// <summary>
    /// Use heartbeat to keep the REST API listening after initialization,
    /// be sure to call from a thread and not the main thread
    /// </summary>
    void DoHeartbeat()
    {
        bool reconnect = false;

        if (null == _sApiChromaInstance)
        {
            //LogErrorOnMainThread("DoHeartbeat: ApiChromaInstance is null!");
            reconnect = true;
            ConnectionStatus = RECONNECT_CHROMA_API_NULL;
            ThreadWaitForSecond();
        }
        else
        {
            //Uri uri = new Uri(_sApiChromaInstance.ApiClient.BasePath);
            //LogOnMainThread(string.Format("Monitoring Heartbeat {0}...", uri.Port));
            while (_sWaitForExit &&
                null != _sApiChromaInstance)
            {
                DateTime timeout = DateTime.Now + TimeSpan.FromSeconds(5);
                try
                {
                    Thread thread = new Thread(new ThreadStart(() =>
                    {
                        try
                        {
                            // The Chroma API uses a heartbeat every 1 second
                            _sApiChromaInstance.Heartbeat();
                        }
                        catch (Exception /* ex */)
                        {
                            //Debug.LogError(string.Format("Failed to invoke action! {0}", ex));
                        }
                    }));
                    thread.Start();
                    while (_sWaitForExit &&
                        DateTime.Now < timeout &&
                        thread.IsAlive)
                    {
                        Thread.Sleep(0);
                    }
                    if (_sWaitForExit &&
                        timeout < DateTime.Now &&
                        thread.IsAlive)
                    {
                        //Debug.LogError("Heartbeat: Timeout detected!");
                        thread.Abort();
                        reconnect = true;
                    }
                }
                catch (Exception)
                {
                    //LogErrorOnMainThread("Failed to check heartbeat!");
                    reconnect = true;
                    ConnectionStatus = RECONNECT_CHROMA_API_HEARTBEAT_FAILURE;
                    ThreadWaitForSecond();
                }
                if (timeout < DateTime.Now)
                {
                    //LogErrorOnMainThread("Timeout detected!");
                    reconnect = true;
                    ConnectionStatus = RECONNECT_CHROMA_API_HEARTBEAT_TIMEOUT;
                    ThreadWaitForSecond();
                }
                if (reconnect)
                {
                    break;
                }

                //LogOnMainThread(string.Format("Monitoring Heartbeat {0}...", uri.Port));
                _sConnected = true;
                _sConnecting = false;
                ConnectionStatus = CONNECTED;
                ThreadWaitForSecond();
            }

            //LogOnMainThread(string.Format("Heartbeat {0} exited", uri.Port));
            _sConnected = false;
            _sConnecting = false;
            ConnectionStatus = NOT_CONNECTED;
            ThreadWaitForSecond();
        }

        if (reconnect)
        {
            ThreadWaitForSecond();
            if (_sWaitForExit)
            {
                RunOnMainThread(() =>
                {
                    if (_sWaitForExit)
                    {
                        Connect();
                    }
                });
            }
        }
    }

#endregion

    /// <summary>
    /// Reset the connections
    /// </summary>
    private void ResetConnections()
    {
        // clear the references
        _sApiRazerInstance = null;
        _sApiChromaInstance = null;
    }

#region Close the connection

    /// <summary>
    /// Uninitialize Chroma
    /// </summary>
    /// <returns></returns>
    void DeleteChromaSdk()
    {
        try
        {
            if (null == _sApiChromaInstance)
            {
                return;
            }

            // destroy the Chroma session
            _sApiChromaInstance.DeleteChromaSdk();
        }
        catch (Exception)
        {
            // the instance will auto-close in 15 seconds
            //LogErrorOnMainThread(string.Format("Exception when calling RazerApi.DeleteChromaSdk: {0}", e));
        }
        finally
        {
            ResetConnections();
        }
    }

#endregion

#region API

    /// <summary>
    /// Unload animations in the scene
    /// </summary>
    private void UnloadSceneAnimations()
    {
        //Debug.Log(string.Format("UnloadAnimations: Connected={0}", Connected));

        // unload 1D animations
        ChromaSDKAnimation1D[] animations1D =
            (ChromaSDKAnimation1D[])Object.FindObjectsOfType(typeof(ChromaSDKAnimation1D));
        //Debug.Log(string.Format("UnloadAnimations: 1D animations={0}", animations1D.Length));
        foreach (ChromaSDKAnimation1D animation in animations1D)
        {
            //Debug.Log("Unload animation.");
            animation.Reset();
        }

        ChromaSDKAnimation2D[] animations2D =
            (ChromaSDKAnimation2D[])Object.FindObjectsOfType(typeof(ChromaSDKAnimation2D));
        //Debug.Log(string.Format("UnloadAnimations: 2D animations={0}", animations2D.Length));
        foreach (ChromaSDKAnimation2D animation in animations2D)
        {
            //Debug.Log("Unload animation.");
            animation.Reset();
        }

        //Debug.Log("Animation unloaded.");
    }


    /// <summary>
    /// Connect and start the heartbeat
    /// </summary>
    public void Connect()
    {
        if (!_sConnecting &&
            !Connected)
        {
            //Debug.Log(string.Format("Connect: Connected={0}", Connected));

            ResetConnections();
            UnloadSceneAnimations();
            _sWaitForExit = true;
            _sConnecting = true;
            ConnectionStatus = CONNECTING;
            SafeStartCoroutine("Initialize", Initialize());
        }
    }

    /// <summary>
    /// Unload animations and disconnect
    /// </summary>
    public void Disconnect()
    {
        //Debug.Log(string.Format("Disconnect: Connected={0}", Connected));

        UnloadSceneAnimations();

        // stop heartbeat
        _sWaitForExit = false;

        if (!_sConnectionIsActive)
        {
            _sConnected = false;
            _sConnecting = false;
            ConnectionStatus = NOT_CONNECTED;
        }
    }

#endregion
}
