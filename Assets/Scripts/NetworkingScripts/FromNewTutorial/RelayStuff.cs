using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayStuff : MonoBehaviour
{
    public TMP_Text _joinCodeText;
    public TMP_InputField _joinInput;    
    private UnityTransport _transport;
    private const int MaxPlayers = 5;

    private async void Awake() {
        _transport = FindObjectOfType<UnityTransport>();
        
        await Authenticate();
    }

    private static async Task Authenticate() {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateGame() {
        Allocation a = await RelayService.Instance.CreateAllocationAsync(MaxPlayers);
        _joinCodeText.text = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

        _transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);
        
        NetworkManager.Singleton.StartHost();
        GameObject.Find("BallSpawner").GetComponent<SpawnBall>().enabled = true;
    }
    
    public async void JoinGame() {        
        JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(_joinInput.text);
        
        _transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);
        
        NetworkManager.Singleton.StartClient();
    }
}