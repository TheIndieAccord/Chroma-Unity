﻿using ChromaSDK;
using ChromaSDK.ChromaPackage.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

// Unity 3.X doesn't like namespaces
public class ChromaSDKBaseAnimation : MonoBehaviour, IUpdate
{
    /// <summary>
    /// Only used to serialize to disk
    /// </summary>
    [Serializable]
    public class ColorArray
    {
        [SerializeField]
        public int[] Colors;
    }

    /// <summary>
    /// Get the list of effect ids
    /// </summary>
    /// <returns></returns>
    public virtual List<EffectResponseId> GetEffects()
    {
        return null;
    }

    /// <summary>
    /// Get the number of frames
    /// </summary>
    /// <returns></returns>
    public virtual int GetFrameCount()
    {
        return 0;
    }

    /// <summary>
    /// Used by the editor
    /// </summary>
    public virtual void RefreshCurve()
    {
    }

    /// <summary>
    /// Used by the editor
    /// </summary>
    public virtual void Unload()
    {
    }

    /// <summary>
    /// Used by the editor
    /// </summary>
    public virtual void Play()
    {
    }

    /// <summary>
    /// Used by the editor
    /// </summary>
    public virtual bool IsPlaying()
    {
        return false;
    }

    /// <summary>
    /// Update event to invoke in edit-mode
    /// </summary>
    public virtual void Update()
    {
        if (ChromaConnectionManager.Instance.Connected)
        {
        }
    }
}
