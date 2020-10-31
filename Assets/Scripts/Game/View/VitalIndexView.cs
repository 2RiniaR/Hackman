using UnityEngine;
using UnityEngine.UI;

namespace Hackman.Game {
    public class VitalIndexView : MonoBehaviour {

        [SerializeField]
        private Image vitalIndexGauge;

        public void SetValue(float amount)
        {
            vitalIndexGauge.fillAmount = Mathf.Clamp(amount, 0, 1);
        }

    }
}
