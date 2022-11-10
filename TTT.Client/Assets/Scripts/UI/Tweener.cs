using UnityEngine;

namespace Assets.Scripts.UI
{
    public class Tweener : MonoBehaviour
    {
        [SerializeField] float delay;

        private void OnEnable()
        {
            LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0.001f);
            LeanTween.scale(gameObject, new Vector3(0.3f, 0.3f, 0.3f), 0.5f)
                .setDelay(delay)
                .setEase(LeanTweenType.easeInOutCirc)
                .setLoopPingPong();
        }
    }
}
