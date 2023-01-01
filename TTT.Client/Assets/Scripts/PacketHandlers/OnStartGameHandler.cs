using Assets.Scripts.Games;
using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ServerClient;
using UnityEngine.SceneManagement;

namespace TTT.PacketHandlers
{
    [HandlerRegister(PacketType.OnStartGame)]
    public class OnStartGameHandler : IPacketHandler
    {
        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_OnStartGame)packet;
            GameManager.Instance.RegisterGame(msg.GameId, msg.XUser, msg.OUser);
            SceneManager.LoadScene("02_Game");
        }
    }
}
