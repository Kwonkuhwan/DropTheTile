using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// �������� ��巹���� ���������,
/// ����� �޸𸮿� �ִ� �������� PhotonNetwork.PrefabPool�� ���� �ְ�
/// string���� �ҷ��� ���°� �� �����ϴ�. 
/// 
/// ���� ��ũ 
/// https://forum.unity.com/threads/solved-photon-instantiating-prefabs-without-putting-them-in-a-resources-folder.293853/
/// https://ansohxxn.github.io/unity%20lesson%202/ch3/
/// </summary>
public class PrefabPool : MonoBehaviour
{
    public List<GameObject> Prefabs;

    private void Start()
    {
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        //���� pool�� ���� �ƴϰ�  "�̰�"�� �������� ���� �ƴҰ��
        if(pool != null && this.Prefabs != null)
        {
            foreach(GameObject prefabs in this.Prefabs)
            {
                pool.ResourceCache.Add(prefabs.name, prefabs);
            }
        }
    }


}
