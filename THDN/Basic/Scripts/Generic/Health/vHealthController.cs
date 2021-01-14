using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
    [vClassHeader("HealthController", iconName = "HealthControllerIcon")]
    public class vHealthController : vMonoBehaviour, vIHealthController
    {
        #region Variables

        [vEditorToolbar("Stats", order = 0)]
        [SerializeField] [vReadOnly] protected bool _isDead;
        [vBarDisplay("maxHealth")] [SerializeField] public float _currentHealth;
        
        [vBarDisplay("maxMana")] [SerializeField] public float _currentMana;
        public bool isImmortal = false;
        [vHelpBox("If you want to start with different value, uncheck this and make sure that the current health has a value greater zero")]
        public bool fillHealthOnStart = true;
        public int maxHealth = 100;
        public int MaxHealth
        {
            get
            {
                return maxHealth;
            }
             set
            {
                maxHealth = value;
            }
        }

         public float maxMana = 100f;
        public float MaxMana
        {
            get
            {
                return maxMana;
            }
             set
            {
                maxMana = value;
            }
        }
        public float currentHealth
        {
            get
            {
                return _currentHealth;
            }
             set
            {
                
                    _currentHealth = value;
                    // onChangeHealth.Invoke(_currentHealth);
                    // Debug.Log(value.ToString()+"Now health0");
                

                if (!_isDead && _currentHealth <= 0)
                {
                    _isDead = true;
                    // onDead.Invoke(gameObject);
                }
                else if (isDead && _currentHealth > 0)
                {
                    _isDead = false;
                }
            }
        }
         public float currentMana
        {
            get
            {
                return _currentMana;
            }
             set
            {
                
                    _currentMana = value;
           
            }
        }
        public bool isDead
        {
            get
            {
                if (!_isDead && currentHealth <= 0)
                {
                    _isDead = true;
                    onDead.Invoke(gameObject);
                }
                return _isDead;
            }
            set
            {
                _isDead = value;
            }
        }
        public float healthRecovery = 0f;
        
        public float manaRecovery = 0f;
        public float healthRecoveryDelay = 0f;
        
        public float manaRecoveryDelay = 0f;
        [HideInInspector]
        public float currentHealthRecoveryDelay;
        public float currentManaRecoveryDelay;
        [vEditorToolbar("Events", order = 100)]
        public List<CheckHealthEvent> checkHealthEvents = new List<CheckHealthEvent>();
        [SerializeField] protected OnReceiveDamage _onReceiveDamage = new OnReceiveDamage();
        [SerializeField] protected OnDead _onDead = new OnDead();
        public ValueChangedEvent onChangeHealth;
        public OnReceiveDamage onReceiveDamage { get { return _onReceiveDamage; } protected set { _onReceiveDamage = value; } }
        public OnDead onDead { get { return _onDead; } protected set { _onDead = value; } }
        public UnityEvent onResetHealth;
        internal bool inHealthRecovery;

        #endregion

        protected virtual void Start()
        {
            if (fillHealthOnStart)
                currentHealth = maxHealth;
                currentMana = MaxMana;
            currentHealthRecoveryDelay = healthRecoveryDelay;
            currentManaRecoveryDelay =manaRecoveryDelay;
        }

        protected virtual bool canRecoverHealth
        {
            get
            {
                return (currentHealth >= 0 && healthRecovery > 0 && currentHealth < maxHealth);
            }
        }

        protected virtual IEnumerator RecoverHealth()
        {
            inHealthRecovery = true;
            while (canRecoverHealth)
            {
                HealthRecovery();
                yield return null;
            }
            inHealthRecovery = false;
        }

        protected virtual void HealthRecovery()
        {
            if (!canRecoverHealth) return;
            if (currentHealthRecoveryDelay > 0)
                currentHealthRecoveryDelay -= Time.deltaTime;
            else
            {
                if (currentHealth > maxHealth)
                    currentHealth = maxHealth;
                if (currentHealth < maxHealth)
                currentHealth += healthRecovery * Time.deltaTime;
                //todo
                    Players.localPlayer.CmdHealthRecovery(currentHealth);
                   
            }
        }

        /// <summary>
        /// Add the currentHealth of Character
        /// </summary>
        /// <param name="value"></param>
        public virtual void AddHealth(int value)
        {   
           
            currentHealth += value;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            if (!isDead && currentHealth <= 0)
            {
                isDead = true;
                onDead.Invoke(gameObject);
            }
             Players.localPlayer.CmdAddHealth(currentHealth);
            HandleCheckHealthEvents();
        }

        /// <summary>
        /// Change the currentHealth of Character
        /// </summary>
        /// <param name="value"></param>
        public virtual void ChangeHealth(int value)
        {
            currentHealth = value;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            if (!isDead && currentHealth <= 0)
            {
                isDead = true;
                onDead.Invoke(gameObject);
            }
            HandleCheckHealthEvents();
        }

        /// <summary>
        /// Reset's current health to max health
        /// </summary>
        public virtual void ResetHealth()
        {
            currentHealth = maxHealth;
            onResetHealth.Invoke();
            if (isDead) isDead = false;
        }

        /// <summary>
        /// Change the MaxHealth of Character
        /// </summary>
        /// <param name="value"></param>
        public virtual void ChangeMaxHealth(int value)
        {
            maxHealth += value;
            if (maxHealth < 0)
                maxHealth = 0;
        }

        /// <summary>
        /// Apply Damage to Current Health
        /// </summary>
        /// <param name="damage">damage</param>
        public virtual void TakeDamage(vDamage damage)
        {
            if (damage != null)
            {
                currentHealthRecoveryDelay = currentHealth <= 0 ? 0 : healthRecoveryDelay;

                if (currentHealth > 0 && !isImmortal)
                {
                    currentHealth -= damage.damageValue;
                    //TODO
                    Players.localPlayer.CmdDealDamage(damage.damageValue);
                }

                if (damage.damageValue > 0)
                    onReceiveDamage.Invoke(damage);
                HandleCheckHealthEvents();
            }
        }

        protected virtual void HandleCheckHealthEvents()
        {
            var events = checkHealthEvents.FindAll(e => (e.healthCompare == CheckHealthEvent.HealthCompare.Equals && currentHealth.Equals(e.healthToCheck)) ||
                                                        (e.healthCompare == CheckHealthEvent.HealthCompare.HigherThan && currentHealth > (e.healthToCheck)) ||
                                                        (e.healthCompare == CheckHealthEvent.HealthCompare.LessThan && currentHealth < (e.healthToCheck)));

            for (int i = 0; i < events.Count; i++)
            {
                events[i].OnCheckHealth.Invoke();
            }
            if (currentHealth < maxHealth && this.gameObject.activeInHierarchy && !inHealthRecovery)
                StartCoroutine(RecoverHealth());
        }

        [System.Serializable]
        public class CheckHealthEvent
        {
            public int healthToCheck;
            public bool disableEventOnCheck;

            public enum HealthCompare
            {
                Equals,
                HigherThan,
                LessThan
            }

            public HealthCompare healthCompare = HealthCompare.Equals;

            public UnityEngine.Events.UnityEvent OnCheckHealth;
        }

        [System.Serializable]
        public class ValueChangedEvent : UnityEvent<float>
        {
            
        }
    }
}

