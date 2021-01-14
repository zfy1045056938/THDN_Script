using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommmonMenu : Menu<CommmonMenu>
{
  [SerializeField]
  public float _playerDelay= 0.5f;

    public void OnPlayPressed(){
        StartCoroutine(OnGamePressRoutline());
    }

    public IEnumerator OnGamePressRoutline(){
        yield return new WaitForSeconds(_playerDelay);
    }


}
