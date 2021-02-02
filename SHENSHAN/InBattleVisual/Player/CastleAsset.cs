using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;


/*
*	CASTLE ASSET MANAGERMENT INCLUDES
*	1.AUTO GENERATE CASTLE ASSET FROM INSPECTOR
*	2.CASTLE INFO
*	3.INTERACTIVE OBJECT(LEVELUP UND SET WORKER TO THE CASTLE 
*	4.WORKER TYPE TO GENERATE CRYSTALS
*/
public class CastleAsset : MonoBehaviour
{
	[Header("CASTLE INFO")]
	public string castleName;
	[Range(0,10)]
	public int castleLevel;
	[Range(0,5)]
	public int castleDef;
	public CastleType castleType;
	public WorkerType workType;

	[Header("Image")]
	public Image castleImage;
	public Image castleFrame;
	public Image workerImage;
	public Image workFrame;
    public Image skillImage;

	[Header("List")]
	private List<CastleAsset> castleList;
	private List<Worker> workerList;
	private Players player;
	private CastleManager castleManager;



	
}