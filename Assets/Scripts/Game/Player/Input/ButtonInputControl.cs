using UnityEngine;
using UniRx;

namespace Hackman.Game.Player {
    public class ButtonInputControl {

        private readonly CompositeDisposable onDispose = new CompositeDisposable();
        private readonly ControlButtonSetting setting;

        public ButtonInputControl(ControlButtonSetting setting) {
            this.setting = setting;
            Observable.EveryUpdate().Subscribe(_ => Update()).AddTo(onDispose);
        }

        private void Update() {
            if (Input.GetButton(setting.MoveUpButtonName)) {
                // 移動方向を上に変更
            }
            if (Input.GetButton(setting.MoveDownButtonName)) {
                // 移動方向を下に変更
            }
            if (Input.GetButton(setting.MoveLeftButtonName)) {
                // 移動方向を左に変更
            }
            if (Input.GetButton(setting.MoveRightButtonName)) {
                // 移動方向を右に変更
            }
        }

    }
}
