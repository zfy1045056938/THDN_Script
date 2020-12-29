using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParty : MonoBehaviour
{
 public GameObject content;
public GameObject characterPrefab;
public List<GameObject> partyList;
public int maxCharacter=3;
public int currentGuy=0;
public int selectIndex=-1;

public bool canSelect=false;
public bool fullTeam=false;


}
