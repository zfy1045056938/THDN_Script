using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDataEditor;
using Invector.vItemManager;
using UnityEngine.UI;
using TMPro;

public class ShrineManager : MonoBehaviour
{
    public GameObject content;
    private Players p ;
    public List<GameObject> shrineList;
    public Transform sPos;
    public GameObject shrinePrefab;

    public Button selectBtn;
    public Button cancelBtn;


    public TextMeshProUGUI detailText;
public int deNumber =3;
    public int selectIndex=-1;

    public UnityEngine.Events.UnityEvent onSelect;


    void Start(){
        p = FindObjectOfType<Players>();

        //
        
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
        selectBtn.interactable=(selectIndex!=-1);
    }


    void ActiveEffect(){
        if(selectIndex!=-1){
            p.CmdActiveDE(shrineList[selectIndex].GetComponent<ShrineIcon>().de.DeID);
            //
            
        }
        content.SetActive(false);
    }

   public void LoadShrineList(){

       ///
       content.SetActive(true);
       //clear old obj
       foreach(var obj in shrineList){
           if(obj!=null){
               Destroy(obj);
           }
       }

//
  while(deNumber>0){

      var counter = 0;
          var de = Random.Range(0,TownManager.instance.itemDatabase.deList.Count);

          DungeonEvent cde = TownManager.instance.itemDatabase.GotDE(de);
          //Set Obj
          GameObject deObj = Instantiate(shrinePrefab,sPos.position,Quaternion.identity)as GameObject;
          deObj.transform.SetParent(sPos);
          deObj.GetComponent<ShrineIcon>().de = cde;
          deObj.GetComponent<ShrineIcon>().index = counter;
          deObj.GetComponent<ShrineIcon>().nameText.text = deObj.GetComponent<ShrineIcon>().de.DeName;
          
          
          counter++;

          deNumber--;
      }
    }

}
