using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class URL
{
    #region AssetBundle
    public static string AssetBundlePath
    {
        get
        {
            string platform = "";
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                    platform = "file://";
                    break;
                case RuntimePlatform.WindowsEditor:
                    platform = "file://";
                    break;
                case RuntimePlatform.OSXEditor:
                    platform = "file://";
                    break;
                case RuntimePlatform.OSXPlayer:
                    platform = "file://";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    platform = "file://";
                    break;
                case RuntimePlatform.Android:
                    platform = "jar:file://";
                    break;
                default:
                    Debug.LogError("<Platform Error>:undeal plateform: load skillbundle on " + Application.platform);
                    break;
            }
            //return platform + Application.streamingAssetsPath + "/" + URL.GetPlatformFolderForAssetBundles(Application.platform) + "/";
#if UNITY_EDITOR
            return platform + Application.streamingAssetsPath + "/" + URL.GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget) + "/";
#else
                        return platform +  Application.streamingAssetsPath + "/" + URL.GetPlatformFolderForAssetBundles(Application.platform) + "/";
#endif
        }
    }

    public static string MainfestName
    {
        get
        {
#if UNITY_EDITOR
            return GetPlatformFolderForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
			return GetPlatformFolderForAssetBundles(Application.platform);
#endif
        }
    }
    public const string assetbundlePrefixName = "assetbundle_";
#if UNITY_EDITOR
    /// <summary>
    /// 在Editor 模式下获得路径信息
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static string GetPlatformFolderForAssetBundles(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "iOS";
            case BuildTarget.WebPlayer:
                return "WebPlayer";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
            case BuildTarget.StandaloneOSXUniversal:
                return "OSX";
            // Add more build targets for your own.
            // If you add more targets, don't forget to add the same platforms to GetPlatformFolderForAssetBundles(RuntimePlatform) function.
            default:
                return null;
        }
    }
#endif
    /// <summary>
    /// 获得不同平台的路径信息
    /// </summary>
    /// <param name="platform"></param>
    /// <returns></returns>
    public static string GetPlatformFolderForAssetBundles(RuntimePlatform platform)
    {
        switch (platform)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "iOS";
            case RuntimePlatform.WindowsWebPlayer:
            case RuntimePlatform.OSXWebPlayer:
                return "WebPlayer";
            case RuntimePlatform.WindowsPlayer:
                return "Windows";
            case RuntimePlatform.OSXPlayer:
                return "OSX";
            // Add more build platform for your own.
            // If you add more platforms, don't forget to add the same targets to GetPlatformFolderForAssetBundles(BuildTarget) function.
            default:
                return null;
        }
    }
    #endregion
    public static string DBPath
    {
        get
        {
            return Application.streamingAssetsPath + "/GameDB.sqlite";
        }
    }

    public static string EffectSkillPath
    {
        get
        {
            return "Prefab/Skill/";
        }
    }

    public static string StreamingAssetsPath
    {
        get
        {
            string platform_prefix = "";
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                    platform_prefix = "file://";
                    break;
                case RuntimePlatform.WindowsEditor:
                    platform_prefix = "file://";
                    break;
                case RuntimePlatform.OSXEditor:
                    platform_prefix = "file://";
                    break;
                case RuntimePlatform.OSXPlayer:
                    platform_prefix = "file://";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    platform_prefix = "file://";
                    break;
                case RuntimePlatform.Android:
                    platform_prefix = "jar:file://";
                    break;
                default:
                    Debug.LogError("<Platform Error>:undeal plateform: load skillbundle on " + Application.platform);
                    break;
            }
            return platform_prefix + Application.streamingAssetsPath + "/";
        }
    }
}
