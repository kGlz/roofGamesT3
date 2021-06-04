using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Plane : MonoBehaviour
{
     void Start()
    {
        GameEvents.current.onGetAssests += GetAssets;
    }

     void GetAssets()
    {
        FirebaseDatabase.DefaultInstance.GetReference(Constants.DbRootName).GetValueAsync()
                                                   .ContinueWith(task =>
                                                   {
                                                       var assetList = new List<AssetModel>();

                                                       if (task.IsFaulted)
                                                       {
                                                           Debug.Log("Fault");
                                                       }
                                                       else if (task.IsCompleted)
                                                       {
                                                           DataSnapshot dataSnapshot = task.Result;
                                                           foreach (var host in dataSnapshot.Children)
                                                           {
                                                               IDictionary m = (IDictionary)host.Value;
                                                               DateTime currentDate = Convert.ToDateTime(m[Constants.ProcessDate].ToString()).Date;
                                                               bool loaded = Convert.ToBoolean(m[Constants.Loaded]);
                                                               if (currentDate == DateTime.Now.Date && !loaded)
                                                               {
                                                                       assetList.Add(new AssetModel()
                                                                       {
                                                                           Host = host.Key,
                                                                           Path = m[Constants.Path].ToString(),
                                                                           ProcessDate = currentDate,
                                                                           Loaded = loaded,
                                                                           AssetName = m[Constants.AssetName].ToString()
                                                                       });
                                                               }
                                                           }

                                                           //
                                                           foreach (var asset in assetList)
                                                           {
                                                               using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(asset.Path))
                                                               {
                                                                   if (uwr.result == UnityWebRequest.Result.ProtocolError)
                                                                   {
                                                                       Debug.Log(uwr.error);
                                                                   }
                                                                   else
                                                                   {
                                                                       // Get downloaded asset bundle
                                                                       AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
                                                                       var prefab = bundle.LoadAsset(asset.AssetName);
                                                                       Instantiate(prefab);
                                                                       //
                                                                       FirebaseDatabase.DefaultInstance.GetReference(Constants.DbRootName).Child(asset.Host).Child(Constants.Loaded).SetValueAsync("true");
                                                                   }
                                                               }
                                                           }
                                                       }
                                                   });
    }
}
