using Assets.Scripts.Games;
using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Packets.ServerClient;
using System;
using UnityEngine.SceneManagement;

namespace TTT.PacketHandlers
{
    [HandlerRegister(PacketType.OnQuitGame)]
    public class OnQuitGameHandler : IPacketHandler
    {
        public static event Action<Net_OnQuitGame> OnQuitGame;

        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_OnQuitGame)packet;

            if (GameManager.Instance.MyUsername == msg.Quitter)
            {
                SceneManager.LoadScene("01_Lobby");
                return;
            }

            OnQuitGame?.Invoke(msg);
        }
    }
}
