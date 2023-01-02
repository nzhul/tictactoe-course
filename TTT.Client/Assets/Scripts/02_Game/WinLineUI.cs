using Assets.Scripts.Games;
using Assets.Scripts.PacketHandlers;
using NetworkShared.Models;
using NetworkShared.Packets.ServerClient;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TTT.Game
{
    public class WinLineUI : MonoBehaviour
    {
        [SerializeField] Color _xColor;
        [SerializeField] Color _oColor;

        private Image _image;
        private RectTransform _rt;
        private Dictionary<WinLineType, LineConfig> _lineConfigs;

        private void Start()
        {
            _image = GetComponent<Image>();
            _rt = GetComponent<RectTransform>();
            OnMarkCellHandler.OnMarkCell += HandleMarkCell;

            _lineConfigs = InitLineConfigs();
        }

        private void OnDestroy()
        {
            OnMarkCellHandler.OnMarkCell -= HandleMarkCell;
        }

        private void HandleMarkCell(Net_OnMarkCell msg)
        {
            if (msg.Outcome == MarkOutcome.Win)
            {
                _image.enabled = true;
                var config = _lineConfigs[msg.WinLineType];
                SetupLine(config, msg.Actor);

                StopCoroutine(AnimateLine());
                StartCoroutine(AnimateLine());
            }
        }

        System.Collections.IEnumerator AnimateLine()
        {
            yield return new WaitForSeconds(0.5f);

            while (_image.fillAmount < 1f)
            {
                _image.fillAmount += 2f * Time.deltaTime;
                yield return null;
            }
        }

        private void SetupLine(LineConfig config, string actorId)
        {
            Color color = _xColor;

            if (!string.IsNullOrEmpty(actorId))
            {
                var type = GameManager.Instance.ActiveGame.GetPlayerType(actorId);
                color = type == MarkType.X ? _xColor : _oColor;
            }

            _rt.localPosition = new Vector2(config.X, config.Y);
            _rt.localRotation = Quaternion.Euler(0, 0, config.ZRotation);
            _rt.sizeDelta = new Vector2(_rt.sizeDelta.x, config.Height);
            _image.color = color;
        }

        private Dictionary<WinLineType, LineConfig> InitLineConfigs()
        {
            return new Dictionary<WinLineType, LineConfig>
            {
                { WinLineType.None, new LineConfig() },
                { WinLineType.Diagonal, new LineConfig() { Height = 370f,  ZRotation = 45f, X = 0f, Y = 0f } },
                { WinLineType.AntiDiagonal, new LineConfig() { Height = 370f,  ZRotation = -45f, X = 0f, Y = 0f } },
                { WinLineType.ColLeft, new LineConfig() { Height = 290f,  ZRotation = 0f, X = -94f, Y = 0f } },
                { WinLineType.ColMid, new LineConfig() {  Height = 290f,  ZRotation = 0f, X = 0f, Y = 0f } },
                { WinLineType.ColRight, new LineConfig() {  Height = 290f,  ZRotation = 0f, X = 94f, Y = 0f } },
                { WinLineType.RowTop, new LineConfig() {  Height = 290f,  ZRotation = 90f, X = 0, Y = 94f } },
                { WinLineType.RowMiddle, new LineConfig() {  Height = 290f,  ZRotation = 90f, X = 0, Y = 0f } },
                { WinLineType.RowBottom, new LineConfig() {  Height = 290f,  ZRotation = 90f, X = 0, Y = -94f } },
            };
        }
    }

    public class LineConfig
    {
        public float X { get; set; }

        public float Y { get; set; }

        public float Height { get; set; }

        public float ZRotation { get; set; }
    }
}
