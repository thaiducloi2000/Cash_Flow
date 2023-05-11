using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAvatarHandler : NetworkBehaviour
{
    public User_Data user;
    public GameObject playerAvatar;
    public List<GameObject> avatars = new List<GameObject>();

    public Transform Avt;

    struct NetworkAvatar : INetworkStruct
    {
        public byte avatarPrefabID;
    }

    [Networked(OnChanged = nameof(OnAvatarChanged))]
    NetworkAvatar networkAvatar { get; set; }

    private void Awake()
    {
        //avatars = Resources.LoadAll<GameObject>("Prefabs/").ToList();
        //avatars = avatars.OrderBy(n => n.name).ToList();

        foreach (var avatar in user.Items)
        {
            avatars.Add(avatar.Prefabs);
        }

    }
    private void Start()
    {
        NetworkAvatar newAvt = networkAvatar;
        newAvt.avatarPrefabID = (byte) lastIndexAvt();

        if(Object.HasInputAuthority)
            RPC_RequestAvatarChange(newAvt);
    }

    GameObject ReplaceAvatar(GameObject currentAvatar, GameObject prefabNewAvatar)
    {
        GameObject newPart = Instantiate(prefabNewAvatar, Avt); 
        //newPart.transform.parent = currentAvatar.transform.parent;
        Destroy(currentAvatar);

        return newPart;
    }

    void ReplaceAvatar()
    {
        playerAvatar = ReplaceAvatar(playerAvatar, avatars[networkAvatar.avatarPrefabID]);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_RequestAvatarChange(NetworkAvatar newNetworkAvatar, RpcInfo info = default)
    {
        Debug.Log($"Receive RPC_RequestAvatarChange for player {transform.name} . AvatarID {newNetworkAvatar.avatarPrefabID}");
        networkAvatar = newNetworkAvatar;
    }
    static void OnAvatarChanged(Changed<PlayerAvatarHandler> changed)
    {
        changed.Behaviour.OnAvatarChanged();
    }
    private void OnAvatarChanged()
    {
        ReplaceAvatar();
    }

    private int lastIndexAvt()
    {
        int lastIndex = 0;
        foreach(GameObject item in avatars)
        {
            if(item.gameObject.name.Equals(user.Last_Character_Selected.Prefabs.gameObject.name))
            {
                return lastIndex;
            }
            
            lastIndex++;
        }
        return 0;
    }
    private void Update()
    {
        Debug.Log("LastIndex: " + lastIndexAvt());
    }
}
