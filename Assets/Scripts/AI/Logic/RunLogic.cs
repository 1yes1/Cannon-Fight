using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CannonFightBase
{
    public class RunLogic : AILogic,ITickable
    {
        private AgentDamageHandler _agentDamageHandler;

        private int _health;

        private int _startHealth;

        private int _losedHealth = 0;

        private bool _losedHealthResetCountdown = false;

        private float _losedHealthCountdown = 5;

        public RunLogic(AIStateController aiStateController,AgentDamageHandler agentDamageHandler) : base(aiStateController)
        {
            _agentDamageHandler = agentDamageHandler;
        }

        public override void Initialize()
        {
            _agentDamageHandler.OnTakeDamageEvent += OnTakeDamage;
        }

        private void OnTakeDamage(Character attacker,int damage,int newHealth)
        {
            _health = newHealth;
            _losedHealth += damage;

            if (!_losedHealthResetCountdown)
                StartLosedHealthCountdown();

            //Bu her türlü baþlasýn. Yani her hasarda Countdown 5 den geriye sarsýn. Böylelikle %40 hasar sayýmý için çok uzun süre geçmeyecek
            //_losedHealthResetCountdown = true;
        }

        //Örneðin agent saðlýðýnýn % 40 ýný kaybetti. Bu durumda 5 saniye boyunca FireState uygun olmayacak yani agent kaçacak.
        private void StartLosedHealthCountdown()
        {
            if (_startHealth == 0)
                _startHealth = _health;

            if (_startHealth * 0.4f < _losedHealth)
            {
                _losedHealthResetCountdown = true;
                _losedHealthCountdown = 5;
                _controller.SetAppropriateState<FireState>(false);
                _controller.ChangeState<IdleMoveState>();
                _isExecuting = true;
                //Debug.Log("Run FROM ENEMY");

            }
        }

        public void Tick()
        {
            if (_losedHealthResetCountdown)
            {
                _losedHealthCountdown -= Time.deltaTime;
                //Debug.Log("goooo: " + _losedHealth);

                if (_losedHealthCountdown <= 0)
                {
                    _losedHealth = 0;
                    _startHealth = 0;
                    _losedHealthResetCountdown = false;
                    _controller.SetAppropriateState<FireState>(true);
                    _isExecuting = false;

                }
            }
        }

        public override void Dispose()
        {
            _agentDamageHandler.OnTakeDamageEvent -= OnTakeDamage;
        }
    }
}
