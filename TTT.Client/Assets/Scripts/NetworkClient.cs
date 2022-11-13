using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetworkClient : MonoBehaviour, INetEventListener
{
    private NetManager _netManager;
    private NetPeer _server;
    private NetDataWriter _writer;

    public event Action OnServerConnected;

    private static NetworkClient _instance;

    public static NetworkClient Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        _netManager.PollEvents();
    }

    public void Init()
    {
        _writer = new NetDataWriter();
        _netManager = new NetManager(this)
        {
            DisconnectTimeout = 100000
        };
        _netManager.Start();
    }

    public void Connect()
    {
        _netManager.Connect("localhost", 9050, "");
    }

    public void SendServer<T>(T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : INetSerializable
    {
        if (_server == null)
            return;

        _writer.Reset();
        packet.Serialize(_writer);
        _server.Send(_writer, deliveryMethod);
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
    {
        var data = Encoding.UTF8.GetString(reader.RawData).Replace("\0", "");
        Debug.Log($"Data received from server: '{data}'");
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
    }

    public void OnPeerConnected(NetPeer peer)
    {
        Debug.Log("We connected to server at " + peer.EndPoint);
        _server = peer;
        OnServerConnected?.Invoke();
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        Debug.Log("Lost connection to server!");
    }
}
