using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// 기존에는 어드레서블 사용했지만,
/// 현재는 메모리에 있는 프리팹을 PhotonNetwork.PrefabPool에 직접 넣고
/// string으로 불러서 쓰는것 이 가능하다. 
/// 
/// 참고 링크 
/// https://forum.unity.com/threads/solved-photon-instantiating-prefabs-without-putting-them-in-a-resources-folder.293853/
/// https://ansohxxn.github.io/unity%20lesson%202/ch3/
/// </summary>
public class PrefabPool : MonoBehaviour
{
    public List<GameObject> Prefabs;

    private void Start()
    {
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        //만약 pool이 널이 아니고  "이것"의 프리팹이 널이 아닐경우
        if(pool != null && this.Prefabs != null)
        {
            foreach(GameObject prefabs in this.Prefabs)
            {
                pool.ResourceCache.Add(prefabs.name, prefabs);
            }
        }
    }


}
