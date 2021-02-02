using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class StatDis : MonoBehaviour
{
    // Start is called before the first frame update
  
   		public Text ValueText;
        public Text NameText;
   		[NonSerialized]
   		public CharacterStat Stat;
   
   		private void OnValidate()
   		{
   			Text[] texts = GetComponentsInChildren<Text>();
   			NameText = texts[1];
   			ValueText = texts[0];
   		}

}
