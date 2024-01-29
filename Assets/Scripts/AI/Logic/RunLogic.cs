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

        // �rne�in bir kere vuruldu. Daha sonra yak�n zamanda hi� vurulmasa bile ba�ka biri vurunca ka�maya �al���yor.
        // Bunu �nlemek i�in s�rekli damage yemesi durumunda ka�mas�n� sa�lamak i�in koyduk bunu
        private float _resetCountdown; 

        private bool _canResetCountdown;

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

            //Debug.Log("StartLosedHealthCountdown");
            //Bu her t�rl� ba�las�n. Yani her hasarda Countdown 5 den geriye sars�n. B�ylelikle %40 hasar say�m� i�in �ok uzun s�re ge�meyecek
            //_losedHealthResetCountdown = true;
        }

        //�rne�in agent sa�l���n�n % 40 �n� kaybetti. Bu durumda 5 saniye boyunca FireState uygun olmayacak yani agent ka�acak.
        private void StartLosedHealthCountdown()
        {
            if (_startHealth == 0)
                _startHealth = _health;

            //Debug.Log("_startHealth: " + _startHealth);
            //Debug.Log("_losedHealth: " + _losedHealth);
            //Debug.Log("_startHealth * 0.4: " + (_startHealth * 0.4f));
            _resetCountdown = 2;
            _canResetCountdown = true;

            if (_startHealth * 0.6f < _losedHealth)
            {
                _losedHealthResetCountdown = true;
                _losedHealthCountdown = 5;
                _controller.SetAppropriateState<FireState>(false);
                _controller.ChangeState<IdleMoveState>();
                _isExecuting = true;
                //Debug.Log("Run FROM ENEMY");
                _canResetCountdown = false;

            }
        }

        private void ResetLosedHealth()
        {
            _losedHealth = 0;
            _startHealth = 0;
            _losedHealthResetCountdown = false;
            _isExecuting = false;
        }

        public void Tick()
        {
            if (_losedHealthResetCountdown)
            {
                _losedHealthCountdown -= Time.deltaTime;
                //Debug.Log("goooo: " + _losedHealth);

                if (_losedHealthCountdown <= 0)
                {
                    ResetLosedHealth();
                    _controller.SetAppropriateState<FireState>(true);

                }
            }

            if(_canResetCountdown)
            {
                _resetCountdown -= Time.deltaTime;

                if (_resetCountdown <= 0)
                {
                    _canResetCountdown = false;
                    ResetLosedHealth();
                }
            }

        }

        public override void Dispose()
        {
            _agentDamageHandler.OnTakeDamageEvent -= OnTakeDamage;
        }
    }
}
