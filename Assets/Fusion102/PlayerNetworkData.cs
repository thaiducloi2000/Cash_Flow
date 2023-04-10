using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkData : NetworkBehaviour
{
	private RunnerManager gameManager = null;

	[Networked(OnChanged = nameof(OnPlayerNameChanged))] public string PlayerName { get; set; }
	[Networked(OnChanged = nameof(OnIsReadyChanged))] public NetworkBool IsReady { get; set; }
	[Networked(OnChanged = nameof(OnIsLeftRoomChanged))] public NetworkBool IsLeftRoom { get; set; }

	public override void Spawned()
	{
		gameManager = RunnerManager.Instance;

		transform.SetParent(RunnerManager.Instance.transform);

		gameManager.PlayerList.Add(Object.InputAuthority, this);
		gameManager.UpdatePlayerList();

		if (Object.HasInputAuthority)
		{
			SetPlayerName_RPC(gameManager.PlayerName);
		
		}
	}

	[Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
	public void SetPlayerName_RPC(string name)
	{
		PlayerName = name;
	}

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void SetReady_RPC(bool isReady)
    {
        IsReady = isReady;
    }

	[Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
	public void LeftRoom_RPC(bool isLeftRoom)
	{
		IsLeftRoom = isLeftRoom;
	}

	private static void OnPlayerNameChanged(Changed<PlayerNetworkData> changed)
	{
		RunnerManager.Instance.UpdatePlayerList();
	}

	private static void OnIsReadyChanged(Changed<PlayerNetworkData> changed)
	{
		RunnerManager.Instance.UpdatePlayerList();
	}

	private static void OnIsLeftRoomChanged(Changed<PlayerNetworkData> changed)
	{
		RunnerManager.Instance.UpdatePlayerList();
	}
}
