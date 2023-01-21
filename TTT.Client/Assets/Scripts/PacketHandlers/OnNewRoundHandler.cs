using Assets.Scripts.Games;
using NetworkShared;
using NetworkShared.Attributes;
using System;

namespace TTT.PacketHandlers
{
    [HandlerRegister(PacketType.OnNewRound)]
    public class OnNewRoundHandler : IPacketHandler
    {
        public static event Action OnNewRound;

        public void Handle(INetPacket packet, int connectionId)
        {
            OnNewRound?.Invoke();
            GameManager.Instance.ActiveGame.Reset();
            GameManager.Instance.InputsEnabled = true;
        }
    }
}
