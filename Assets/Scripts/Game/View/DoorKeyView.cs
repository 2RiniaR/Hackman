using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hackman.Game {
    public class DoorKeyView : MonoBehaviour {

        [SerializeField]
        private List<DoorKeyViewItem> items = null;

        public void SetCount(int count)
        {
            count = Mathf.Clamp(count, 0, items.Count);
            foreach (var (item, i) in items.Select((x, i) => (x, i)))
            {
                item.SetShow(i < count);
            }
        }

    }
}
