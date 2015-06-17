using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AssetLoader : MonoBehaviour {

    public static AssetLoader Instance;

    public string url;
    public int version = 1;
    public string AssetName;

    public Text infoText;

    AssetBundle bundle;


    void Awake()
    {
        Instance = this;
    }

    void OnDisable()
    {
        //AssetBundleManager.Unload(url, version);
    }


    public void DownloadAsset()
    {
        // bundle = AssetBundleManager.getAssetBundle (url, version);
        //   Debug.Log(bundle);

        if (!bundle)
            StartCoroutine(DownloadAssetBundle());
    }

    IEnumerator DownloadAssetBundle()
    {
        yield return StartCoroutine(AssetBundleManager.downloadAssetBundle(url, version));
        
        bundle = AssetBundleManager.getAssetBundle(url, version);

        if (bundle != null)
            AssetLoader.Instance.SetInfoText("Download Success.... ");
        else
            AssetLoader.Instance.SetInfoText("Download error please retry");

        GameObject obj = Instantiate(bundle.LoadAsset("ExampleObject"),Vector3.zero,Quaternion.identity) as GameObject;
        // Unload the AssetBundles compressed contents to conserve memory
        //Debug.Log(obj);
        bundle.Unload(false);

    }


    public void SetInfoText(string text)
    {
        infoText.text = text;
    }
}
