using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 用于管理城堡属性
/// </summary>
public class CastleManager : MonoBehaviour
{

   
    public static CastleManager instance;

    [Header("Base Info")]
    private int _castleID;
    private int _castleHP;
    private int _castleDef;
    private int _castleAtk;
    private string _castleName;
    private Image _castleImage;
    private Transform _initialPosition;
    private CastleType castleType = CastleType.None;
    private Image _castleBackground;

    private WorkerType workerType = WorkerType.None;
    private WorkerManager work;
    //
    private CastleAsset castleAsset;

    public Text CastleHp;

    //技能图标
    public Image CastleSkillImage;
    //城堡图标
    public Image CastleGraphicImage;
     
    #region GS variable
    public int CastleID
    {
        get
        {
            return _castleID;
        }

        set
        {
            _castleID = value;
        }
    }

    public int CastleHP
    {
        get
        {
            return _castleHP;
        }

        set
        {
            _castleHP = value;
        }
    }

    public int CastleDef
    {
        get
        {
            return _castleDef;
        }

        set
        {
            _castleDef = value;
        }
    }

    public int CastleAtk
    {
        get
        {
            return _castleAtk;
        }

        set
        {
            _castleAtk = value;
        }
    }

    public string CastleName
    {
        get
        {
            return _castleName;
        }

        set
        {
            _castleName = value;
        }
    }

    public Image CastleImage
    {
        get
        {
            return _castleImage;
        }

        set
        {
            _castleImage = value;
        }
    }

    public Transform InitialPosition
    {
        get
        {
            return _initialPosition;
        }

        set
        {
            _initialPosition = value;
        }
    }

    public CastleType CastleType
    {
        get
        {
            return castleType;
        }

        set
        {
            castleType = value;
        }
    }

    public Image CastleBackground
    {
        get
        {
            return _castleBackground;
        }

        set
        {
            _castleBackground = value;
        }
    }
    #endregion


    private void Awake()
    {
        instance = this;
        
    }


    
    /// <summary>
    /// Start this instance.
    /// </summary>
    private void Start()
    {
        if (castleAsset != null)
        {
LoadCastleAsset();
        }
        
    }

    private void LoadCastleAsset()
    {
        CastleHp.text = castleAsset.castleDef.ToString();
        CastleGraphicImage.sprite = castleAsset.castleImage.sprite;
        CastleSkillImage.sprite = castleAsset.skillImage.sprite;
    }


}