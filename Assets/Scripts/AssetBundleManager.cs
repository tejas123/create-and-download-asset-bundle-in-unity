using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

static public class AssetBundleManager {

    // A dictionary to hold the AssetBundle references
    static private Dictionary<string, AssetBundleRef> dictAssetBundleRefs;
    static AssetBundleManager()
    {
        dictAssetBundleRefs = new Dictionary<string, AssetBundleRef>();
    }
    // Class with the AssetBundle reference, url and version
    private class AssetBundleRef
    {
        public AssetBundle assetBundle = null;
        public int version;
        public string url;
        public AssetBundleRef(string strUrlIn, int intVersionIn)
        {
            url = strUrlIn;
            version = intVersionIn;
        }
    };
    // Get an AssetBundle
    public static AssetBundle getAssetBundle(string url, int version)
    {
        string keyName = url + version.ToString();
        AssetBundleRef abRef;
        if (dictAssetBundleRefs.TryGetValue(keyName, out abRef))
            return abRef.assetBundle;
        else
            return null;
    }
    // Download an AssetBundle
    public static IEnumerator downloadAssetBundle(string url, int version)
    {
        AssetLoader.Instance.SetInfoText("Downloading .... ");
        string keyName = url + version.ToString();
        using (WWW www = WWW.LoadFromCacheOrDownload(url, version))
        {
            yield return www;
            if (www.error != null)
            {
                ///will remove the old file from cashe which cause the problem 
                AssetBundleManager.Unload(url,version,false);
                AssetLoader.Instance.SetInfoText("Download error please retry");
                Debug.Log("Error : "+www.error);
                throw new Exception("WWW download:" + www.error);
            }
            AssetBundleRef abRef = new AssetBundleRef(url, version);
            abRef.assetBundle = www.assetBundle;
            
            if (!dictAssetBundleRefs.ContainsKey(keyName))
            {
                dictAssetBundleRefs.Add(keyName, abRef);
            }
            else
            {
                AssetLoader.Instance.SetInfoText("Asset is unloading plz wait");
                Debug.Log("This is Just Test that how we can unload asset which is in cache");
                AssetBundleManager.Unload(url, version, false);
            }
        }
    }
    // Unload an AssetBundle
    public static void Unload(string url, int version, bool allObjects)
    {
        string keyName = url + version.ToString();
        AssetBundleRef abRef;
        if (dictAssetBundleRefs.TryGetValue(keyName, out abRef))
        {
            abRef.assetBundle.Unload(allObjects);
            abRef.assetBundle = null;
            dictAssetBundleRefs.Remove(keyName);
        }
    }
}
