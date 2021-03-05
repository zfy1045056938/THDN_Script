using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Security.Cryptography;
using System.Reflection;
using UnityEngine.AI;
using UnityEngine.Events;
using GameDataEditor;

public static class Utils 
{
    static Dictionary<KeyValuePair<Type,string>, MethodInfo[]> lookup = new Dictionary<KeyValuePair<Type,string>, MethodInfo[]>();
    public static MethodInfo[] GetMethodsByPrefix(Type type, string methodPrefix)
    {
        KeyValuePair<Type,string> key = new KeyValuePair<Type,string>(type, methodPrefix);
        if (!lookup.ContainsKey(key))
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                .Where(m => m.Name.StartsWith(methodPrefix))
                .ToArray();
            lookup[key] = methods;
        }
        return lookup[key];
    }

    public static void InvokeMany(Type type, object onObject, string methodPrefix, params object[] args)
    {
        foreach (MethodInfo method in GetMethodsByPrefix(type, methodPrefix))
            method.Invoke(onObject, args.ToArray());
    }

    public static void SetListener( UnityEvent uEvent, UnityAction call)
    {
        uEvent.RemoveAllListeners();
        uEvent.AddListener(call);
    }

    public static void SetListener<T>( UnityEvent<T> uEvent, UnityAction<T> call)
    {
        uEvent.RemoveAllListeners();
        uEvent.AddListener(call
        );
    }

    public static bool IsClickUI(){
        if(EventSystem.current!=null){
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position =  new Vector2(Input.mousePosition.x,Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData,results);
            return results.Count>0;
        }
        return false;
    }

    public static string PBKDF2Hash(string loginPassword, string v)
    {
        byte[] saltBytes = Encoding.UTF8.GetBytes(v);
        Rfc2898DeriveBytes pbkbf2 = new Rfc2898DeriveBytes(loginPassword,saltBytes,10000);
        byte[] hash = pbkbf2.GetBytes(20);
        return BitConverter.ToString(hash).Replace("-", string.Empty);
    }

    public static Sprite CreateSprite(Texture2D t)
    {
        return Sprite.Create(t,new Rect(0,0,t.width,t.height),Vector2.zero );
    }

    public static PlayerJob GetPlayerJob(string job)
    {
        if (job == "生存者")
        {
            return PlayerJob.Survicer;
        }
        else if (job == "祈求者")
        {
            return PlayerJob.Magic;
        }else if (job == "猎人")
        {
            return PlayerJob.Hunter;
        }

        return PlayerJob.None;
    }
    public static string GetCardEffect(string cn)
{
    List<GDECardEffectGuideData> cfgd = GDEDataManager.GetAllItems<GDECardEffectGuideData>();

    for(int i=0;i<cfgd.Count;i++){
        if(cn==cfgd[i].GName){
            return cfgd[i].EGName;
        }
    }
    return null;
}   
       public static CardRatityOption GetCardRarity(string data)
    {
        	
		 if(data == "Rare"){
			return CardRatityOption.RARE;
		}else if(data == "Epic"){
			return CardRatityOption.Epic;
		}else if(data == "Ancient"){
			return CardRatityOption.Ancient;
		}else if(data=="Legend"){
			return CardRatityOption.LEGEND;
		}
		
		return CardRatityOption.NORMAL;
    }
    

    public static ItemRatity ConvertRarity(string names){
        if(names=="Rare"){
            return ItemRatity.Rare;
        }else if(names=="Epic"){
            return ItemRatity.Epic;
        }
        return ItemRatity.Normal;
    }

    public static CardType GetCreatureType(string creatureType)
    {
        if(creatureType=="Animals"){
			return CardType.Animals;
		}else if(creatureType=="Human"){
			return CardType.Human;
		}else if(creatureType=="Qika"){
			return CardType.Qika;
		}

       return CardType.None;
    }

    public static CharacterAsset GetCardCharacterAsset(CardAsset nItem, string characterAsset)
    {
         List<GDECharacterAssetData> gda = GDEDataManager.GetAllItems<GDECharacterAssetData>();
		for(int i=0;i<gda.Count;i++){
	     if(gda[i].ClassName==characterAsset){
			 GDECharacterAssetData cs =new GDECharacterAssetData(gda[i].Key);
			return new  CharacterAsset(GetPlayerJob(cs.PlayersJob),cs.ClassName,cs.MaxHealth,cs.PowerName,CreateSprite(cs.AvatarImage),cs.Detail,CreateSprite(cs.BGSprite),cs.AttackCard,cs.ArmorCard);
		 }
		}
		return null;
    }

    public static CharacterAsset GetCardCharacterAsset(string characterAsset)
    {
        List<GDECharacterAssetData> gda = GDEDataManager.GetAllItems<GDECharacterAssetData>();
		for(int i=0;i<gda.Count;i++){
	     if(gda[i].ClassName==characterAsset){
			 GDECharacterAssetData cs =new GDECharacterAssetData(gda[i].Key);
			return new  CharacterAsset(GetPlayerJobs(cs.PlayersJob),cs.ClassName,cs.MaxHealth,
			cs.PowerName,GetCsAvaSprite(cs.AvatarImage),cs.Detail,GetCsBGSpritecs(cs.BGSprite),
			cs.AttackCard,cs.ArmorCard);
		 }
		}
		return null;
		
    }

    
    public static Sprite GetCsAvaSprite(Texture2D avatarImage)
    {
		if(avatarImage!=null){
        return Sprite.Create(avatarImage,new Rect(0,0,avatarImage.width,avatarImage.height),Vector2.zero);
		}else{
			Debug.Log("Can't Get Sprite");
		}
		return null;
	}

    public static TypeOfCards GetTypeOfCard(string cardType)
    {
         if(cardType=="Creature"){
		  return TypeOfCards.Creature;
	  }else if(cardType=="Spell"){
		  return TypeOfCards.Spell;
	  }else if(cardType=="Token"){
		  return TypeOfCards.Token;
	  }
	  return TypeOfCards.Creature;
    }

    
    public static Sprite GetCsBGSpritecs(Texture2D bGSprite)
    {
        return Sprite.Create(bGSprite,new Rect(0,0,bGSprite.width,bGSprite.height),Vector2.zero);
    }
     public static PlayerJob GetPlayerJobs(string playersJob)
    {
        if(playersJob=="猎人"){
			return PlayerJob.Hunter;
		}else if(playersJob=="祈求者"){
			return PlayerJob.Magic;
		}else if(playersJob=="生存者"){
			return PlayerJob.Survicer;
		}
		return PlayerJob.None;
    }


    public static NpcType ConvertNpcType(string names){
        if(names=="Merchant"){
            return NpcType.Merchant;
        }else if(names=="Enemy"){
            return NpcType.Enemy;
        }else if(names=="Others"){
            return NpcType.Others;
        }
        
        return NpcType.None;
    }
    
    public static List<CardAsset> ConvertCard(List<GDECardAssetData> c){
              List<CardAsset> cs = new List<CardAsset>();
              for( int i=0;i<c.Count; i++){
                  CardAsset ca = CardCollection.instance.GetCardAssetByName(c[i].CardName);
                    Debug.Log("Pack to card"+ca.name.ToString());
                  cs.Add(ca);
                
              }

        return cs;
    }

    public static TargetOptions GetSpellTarget(string spellTarget)
    {
        if(spellTarget=="AllCharacter"){
			return TargetOptions.AllCharacter;
		}else if(spellTarget=="Creature"){
			return TargetOptions.Creature;
		}else if(spellTarget=="EnemyCharacter"){
			return TargetOptions.EmenyCharacter;
		}else if(spellTarget=="EnemyCreature"){
			return TargetOptions.EmenyCreature;

		}else if(spellTarget=="YoursCharacters"){
			return TargetOptions.YoursCharacters;

		}else if(spellTarget=="AllCreature"){
			return TargetOptions.AllCharacter;
		}
		return TargetOptions.None;
    }

    public static List<Items> ConvertListItems(List<string> ds){
        List<Items> its = new List<Items>();

        if(ds.Count>0){
        for(int i=0;i<ds.Count;i++){
            Debug.Log("try got itemname"+ds[i]);
            Items item = ItemDatabase.instance.GetItemByName(ds[i].ToString());
            if(item!=null){
                its.Add(item);
            }else{
                Debug.Log("Item null");
            }
        }
        }

        return its;

    }

    public static List<int> ConvertItems(List<GDEItemsData> it){
            List<int> itemList = new List<int>();
            for(int i=0;i<it.Count;i++){
                Items its = ItemDatabase.instance.FindItem(int.Parse(it[i].ItemID));
                itemList.Add(int.Parse(its.itemID));
                return itemList;
            }

            return null;
    }

    internal static SpellBuffType GetSpellType(string spellType)
    {
        if(spellType=="Health"){
		   return SpellBuffType.Health;
	   }else if(spellType =="Armor"){
		   return SpellBuffType.Armor;
	   }else if(spellType=="Atk"){
		   return SpellBuffType.Atk;
	   }else if(spellType=="STR"){
		   return SpellBuffType.STR;
	   }else if(spellType=="DEX"){
		   return SpellBuffType.DEX;
	   }else if(spellType=="INT"){
		   return SpellBuffType.INT;
        }else if(spellType=="Taunt"){
           return SpellBuffType.Taunt;
       }

	   return SpellBuffType.None;
    }

    internal static DamageElementalType GetElementalDamageType(string damageEffectType)
    {
      	if(damageEffectType=="Fire"){
			return DamageElementalType.Fire;
		}else if(damageEffectType=="Ice"){
			return DamageElementalType.Ice;
		}else if(damageEffectType=="Posion"){
			return DamageElementalType.Posion;
		}else if(damageEffectType=="Electronic"){
			return DamageElementalType.Electronic;
		}else if (damageEffectType == "Bloody")
        {
            return DamageElementalType.Bloody;
        }else if (damageEffectType == "Freeze")
        {
            return DamageElementalType.Freeze;
        }
       return DamageElementalType.None;
    }

    public static Transform GetNearestTransform(List<Transform> startPositions, Vector3 @from)
    {
        throw new NotImplementedException();
    }


    public static bool RaycastWithout(Ray ray, out RaycastHit hit, GameObject ignore)
    {
        Dictionary<Transform,int>backups = new Dictionary<Transform, int>();
        //
        foreach (Transform ts in ignore.GetComponentsInChildren<Transform>(true))
        {
            backups[ts] = ts.gameObject.layer;
            ts.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        //
        bool result = Physics.Raycast(ray, out hit);
        //
        foreach (KeyValuePair<Transform,int> kvp in backups)
        {
            kvp.Key.gameObject.layer = kvp.Value;
        }

        return result;
    }

    public static Vector3 NearestValidDestination(this NavMeshAgent agent, Vector3 destination)
    {
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(destination, path))
        {
            return path.corners[path.corners.Length - 1];
        }
        
        //
        if (NavMesh.SamplePosition(destination, out NavMeshHit hit, agent.speed * 2, NavMesh.AllAreas))
        {
            if (agent.CalculatePath(hit.position
                , path))
            {
                return path.corners[path.corners.Length - 1];
            }
        }

        return agent.transform.position;
    }


    public static float CloseDistance(BoxCollider2D a, BoxCollider2D b)
    {
        if (a.bounds.Intersects(b.bounds)) return 0;

        return Vector3.Distance(
       a.transform.position,
        b.transform.position);
    }
    public static void ResetMovement(this NavMeshAgent agent)
    {
        agent.ResetPath();
        agent.velocity= Vector3.zero;
        
    }

    public static CraftingTabType ConvertCraftType(string n){
        if(n=="Equipment"){
            return CraftingTabType.Equipment;
        }else if(n=="Reagent"){
            return CraftingTabType.Reagent;
        }
        return CraftingTabType.All;
    }
    
}
