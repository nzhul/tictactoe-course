using Assets.Scripts.Games;
using NetworkShared;
using NetworkShared.Attributes;
using NetworkShared.Models;
using NetworkShared.Packets.ServerClient;
using System;

namespace Assets.Scripts.PacketHandlers
{
    [HandlerRegister(PacketType.OnMarkCell)]
    public class OnMarkCellHandler : IPacketHandler
    {
        public static event Action<Net_OnMarkCell> OnMarkCell;

        public void Handle(INetPacket packet, int connectionId)
        {
            var msg = (Net_OnMarkCell)packet;

            GameManager.Instance.ActiveGame.SwitchCurrentPlayer();
            if (GameManager.Instance.IsMyTurn && msg.Outcome == MarkOutcome.None)
            {
                GameManager.Instance.InputsEnabled = true;
            }

            if (msg.Outcome > MarkOutcome.None)
            {
                GameManager.Instance.InputsEnabled = false;
            }


            OnMarkCell?.Invoke(msg);
        }
    }
}
