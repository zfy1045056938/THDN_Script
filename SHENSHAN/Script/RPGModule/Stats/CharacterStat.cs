using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using PixelCrushers.DialogueSystem.UnityGUI.Wrappers;


[Serializable]
    public class CharacterStat
    {
        public float BaseValue;


        protected bool isDirty = true;
        protected float lastBaseValue;

        protected float _value;

        public virtual float value
        {

            get
            {
                if (isDirty || lastBaseValue != BaseValue)
                {
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isDirty = false;
                }

                return _value;
            }

         


        }

        protected readonly List<StatModifier> stateModifier;
        public readonly ReadOnlyCollection<StatModifier> StatModifier;

        public CharacterStat()
        {
            stateModifier = new List<StatModifier>();
            StatModifier = stateModifier.AsReadOnly();
        }
        
        public CharacterStat(float baseValue) : this()
        {
            BaseValue = baseValue;
        }


        public virtual bool RemoveModifiter(StatModifier mod)
        {
            if (stateModifier.Remove(mod))
            {
                isDirty = true;
                return true;
            }

            return false;

        }

        public virtual void AddModifiter(StatModifier mod)
        {
            isDirty = true;
            stateModifier.Add(mod);
        }

        public virtual bool RemoveAllModifiterFromSource(object source)
        {

            int numToRemove = stateModifier.RemoveAll(mod => mod.Source == source);
            Debug.Log("number is"+numToRemove);

            if (numToRemove > 0)
            {
                isDirty = true;
                return true;
            }
            else
                return false;




        }

   

        protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
        {

            if (a.Order < b.Order)
            {
                return -1;
            }
            else if (a.Order > b.Order)
            {
                return 1;
            }

            return 0;
        }

        private float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0;

            stateModifier.Sort(CompareModifierOrder);

            for (int i = 0; i < stateModifier.Count; i++)
            {
                StatModifier mod = stateModifier[i];
                //
                if (mod.Type == StatModType.Flat)
                {
                    finalValue += mod.Value;
                }
            }

            return (float) Mathf.Round(finalValue);
        }
    
}

