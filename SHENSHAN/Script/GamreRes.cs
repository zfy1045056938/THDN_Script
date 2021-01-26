using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamreRes : MonoBehaviour
{

    [SerializeField] public Dropdown ResolutionDrop;

    public Resolution[] res;

    public List<Dropdown.OptionData> opd = new List<Dropdown.OptionData>();

  
    

    void Start()
    {
      
    ResolutionDrop.gameObject.GetComponent<Dropdown>();
    
    
       res = Screen.resolutions;

      
        ResolutionDrop.options.Clear();
        //
        for (int i = 0; i < res.Length; i++)
        {
            
            //
           opd.Add(new Dropdown.OptionData());
            //
            opd[i].text = ShowRes(res[i]);
            ResolutionDrop.value = i;
            //
            ResolutionDrop.options.Add(opd[i]);
            //
          
            //
            ResolutionDrop.onValueChanged.AddListener(index =>
            {
                ResolutionDrop.captionText.text = ShowRes(res[index]);
                //
                Screen.SetResolution(res[index].width,res[index].height,false);
            });
            
            //
            ResolutionDrop.captionText.text = "分辨率";
        }


    }

    public string ShowRes(Resolution res)
    {
        return res.width + "X" + res.height;
    }
}
