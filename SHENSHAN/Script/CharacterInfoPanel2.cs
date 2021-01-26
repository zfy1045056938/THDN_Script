using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CharacterInfoPanel2 : MonoBehaviour
{

    private PlayerData player;
    public List<EquipmentSlot> slots;
    
   
    [Header("UI")]
    public Text str;
    // Start is called before the first frame update
    void Start()
    {
        player=FindObjectOfType<PlayerData>();

    CloseOrOpen(false);
        if(NetworkClient.isConnected && player!=null){
            // LoadCharacterData();
        }
    }


    // void LoadCharacterData(){
    //   str.text=player.Strength.ToString();
    // }

    public string ExtraBouns(ItemManager item){
        string amount ="";
        slots=new List<EquipmentSlot>();
        for(int i=0;i<slots.Count;i++){
            if(slots[i].item!=null){
                // if(item.strength !=0){
                //     amount += item.strength.ToString();
                //     return player.Strength+amount;
                // }else{ 
                //     return player.Strength.ToString();
                // }
            }
        }
        return amount;
    }

    public void CloseOrOpen(bool b)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
           
            transform.GetChild(i).gameObject.SetActive(b);
        }
    }
}
