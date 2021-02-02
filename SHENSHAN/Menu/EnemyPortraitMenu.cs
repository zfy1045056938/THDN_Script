using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyPortraitMenu : MonoBehaviour
{
    public EnemyAsset asset;
    private EnemyPortraitVisual portrait;
    private float InitialScale;
    private float TargetScale = 1.3f;
    private bool selected = false;

    public static EnemyPortraitMenu instance;
    void Awake()
    {
        if(instance==null)instance=this;
        portrait = GetComponent<EnemyPortraitVisual>();
        portrait.enemyAsset = asset;
        portrait.ReadFromEnemyAsset();
        InitialScale = transform.localScale.x;
    }

    void OnMouseDown()
    {
        // show the animation
        if (!selected)
        {
            selected = true;
            transform.DOScale(TargetScale, 0.5f);
            // CharacterSelectionScreen.Instance.HeroPanel.SelectCharacter(this);
            //Choose object 
            // EnemySelection.instance.SelectEnemy(this);
            // deselect all the other Portrait Menu buttons 
            //  [] allPortraitButtons = GameObject.FindObjectsOfType<EnemyPortraitMenu>();
            // foreach (EnemyPortraitMenu m in allPortraitButtons)
            //     if (m != this)
            //         m.Deselect();
        }
        else
        {
            Deselect();
//            CharacterSelectionScreen.instance..SelectCharacter(null);
        }
    }

    public void Deselect()
    {
        transform.DOScale(InitialScale, 0.5f);
        selected = false;
    }
}
