using UnityEngine;
using UnityEngine.UI;

namespace Game.View
{
    public class VitalIndexView : MonoBehaviour
    {
        [SerializeField] private Image vitalIndexGauge;

        public void SetValue(float amount)
        {
            vitalIndexGauge.fillAmount = Mathf.Clamp(amount, 0, 1);
        }
    }
}