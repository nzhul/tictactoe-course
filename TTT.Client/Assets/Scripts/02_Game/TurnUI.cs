using Assets.Scripts.Games;
using NetworkShared.Models;
using TMPro;
using UnityEngine;

namespace TTT.Game
{
    public class TurnUI : MonoBehaviour
    {
        [SerializeField] Color _xColor;
        [SerializeField] Color _oColor;
        private TextMeshProUGUI _playerText;

        private void Awake()
        {
            _playerText = transform.Find("player").GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            var myColor = GameManager.Instance.MyType == MarkType.X ? _xColor : _oColor;
            var opponentColor = GameManager.Instance.OpponentType == MarkType.X ? _xColor : _oColor;

            if (GameManager.Instance.IsMyTurn)
            {
                _playerText.text = "your";
                _playerText.color = myColor;
                var rt = _playerText.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(82f, rt.sizeDelta.y);
            }
            else
            {
                _playerText.text = "enemy";
                _playerText.color = opponentColor;
                var rt = _playerText.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(120f, rt.sizeDelta.y);
            }

            LeanTween.scale(gameObject, new Vector3(1.1f, 1.1f, 1.1f), 0.3f)
                .setEase(LeanTweenType.easeOutBounce)
                .setOnComplete(() =>
                {
                    LeanTween.scale(gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.3f)
                    .setEase(LeanTweenType.easeOutBounce);
                });
        }
    }
}
