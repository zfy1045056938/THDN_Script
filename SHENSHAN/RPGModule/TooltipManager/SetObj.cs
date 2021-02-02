using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum SetType{
    Str,
    Dex,
    Inte,
    SDI,
    HP,
    Call,
    Lowpowerful,
    GroupHealth,
    Destory,
    Damage,
    Armor,
    SD,
    DI,
    Token,
    FR,
    IR,
    PR,
    Burning,
    Freeze,
    Posion,
    NonDur,
    Powerful,


}

    public class SetObj:MonoBehaviour
    {
        public TextMeshProUGUI setName;
        public TextMeshProUGUI setDetail;
    }
