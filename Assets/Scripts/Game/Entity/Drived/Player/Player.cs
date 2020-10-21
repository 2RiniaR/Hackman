using UnityEngine;
using System;
using UniRx;

namespace Hackman.Game.Entity.Player {
    public class Player : Entity {

        [SerializeField]
        private int initialLife = 3;

        [SerializeField]
        protected PlayerLifeDisplay lifeDisplay;

        private readonly Subject<Unit> onKilled = new Subject<Unit>();
        public IObservable<Unit> OnKilled => onKilled;

        private IInputControl inputControl;
        private DotEater dotEater;
        private LifeStatus lifeStatus;
        private LifeUpdater lifeUpdater;
        private MonsterCollisionChecker monsterCollisionChecker;
        private LifeDisplayUpdater lifeDisplayUpdater;

        protected override void Awake() {
            base.Awake();
            inputControl = new ButtonInputControl(moveControlStatus);
            dotEater = new DotEater(map, moveUpdater);
            lifeStatus = new LifeStatus();
            monsterCollisionChecker = new MonsterCollisionChecker(onKilled, transform);
            lifeUpdater = new LifeUpdater(onKilled, lifeStatus);
            lifeDisplayUpdater = new LifeDisplayUpdater(lifeDisplay, lifeStatus);
            lifeStatus.SetLife(initialLife);
        }

        protected override void OnDestroy() {
            lifeDisplayUpdater.Dispose();
            lifeUpdater.Dispose();
            dotEater.Dispose();
            inputControl.Dispose();
            base.OnDestroy();
        }

        protected override void Update() {
            base.Update();
            monsterCollisionChecker.CheckCollision();
        }

        public void Kill() {
            onKilled.OnNext(Unit.Default);
        }

    }
}
