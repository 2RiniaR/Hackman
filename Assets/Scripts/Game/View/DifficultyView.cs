using TMPro;
using UnityEngine;

namespace Game.View
{
    public class DifficultyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lifeCountText;

        public void SetValue(int count)
        {
            lifeCountText.text = count.ToString();
        }
    }
}