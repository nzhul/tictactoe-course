using LiteNetLib;
using LiteNetLib.Utils;
using NetworkShared;
using NetworkShared.Registries;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class NetworkClient : MonoBehaviour, INetEventListener
{
    private NetManager _netManager;
    private NetPeer _server;
    private NetDataWriter _writer;
    private PacketRegistry _packetRegistry;
    private HandlerRegistry _handlerRegistry;

    public event Action OnServerConnected;

    private static NetworkClient _instance;

    public static NetworkClient Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
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

    private void OnDestroy()
    {
        if (_server != null)
        {
            _netManager.Stop();
        }
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public void Disconnect()
    {
        _netManager.DisconnectAll();
    }

    public void Init()
    {
        _packetRegistry = new PacketRegistry();
        _handlerRegistry = new HandlerRegistry();
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
        var packetType = (PacketType)reader.GetByte();
        var packet = ResolvePacket(packetType, reader);
        var handler = ResolveHandler(packetType);
        handler.Handle(packet, peer.Id);
        reader.Recycle();
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

    private IPacketHandler ResolveHandler(PacketType packetType)
    {
        var handlerType = _handlerRegistry.Handlers[packetType];
        return (IPacketHandler)Activator.CreateInstance(handlerType);
    }

    private INetPacket ResolvePacket(PacketType packetType, NetPacketReader reader)
    {
        var type = _packetRegistry.PacketTypes[packetType];
        var packet = (INetPacket)Activator.CreateInstance(type);
        packet.Deserialize(reader);
        return packet;
    }
}
