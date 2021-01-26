using System.Collections;
using System.Collections.Generic;
using Mirror;
using PixelCrushers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayfulSystems.ProgressBar;
using PlayfulSystems;
using PixelCrushers.DialogueSystem;

public class ShenShanLoading : MonoBehaviour
{
    public static ShenShanLoading instance;
    public UIPanel panel;
    public ProgressBarPro loading;

    protected AsyncOperation _asyncOperation;
    public int _currProcess;
    public Image background;
    public bool isLoad = false;
    public bool firstLoad = true;
   
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        _currProcess = 0;
        _asyncOperation = null;
        loading = GetComponentInChildren<ProgressBarPro>();
      
    }

    public void StartLoading(string sceneName)
    {
        isLoad = true;
        panel.Open();
//        SceneManager.SetActiveScene(SceneManager.GetActiveScene());
        StartCoroutine(StartLoadingRoutine(sceneName));
    }

    IEnumerator StartLoadingRoutine(string sceneName)
    {
//        TownManager.instance.content.Close();
        if (firstLoad)
        {
            firstLoad = false;
        isLoad = true;
            int tmp;

            yield return new WaitForSeconds(0.25f);
//        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            _asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        
            //        _asyncOperation = NetworkManager.singleton.ClientChangeScene(sceneName, SceneOperation.LoadAdditive);
            //
            _asyncOperation.allowSceneActivation = false;
            //
            while (_asyncOperation.progress < 0.9f)
            {
                tmp = (int) _asyncOperation.progress * 100;
                while (_currProcess > tmp)
                {
                    ++_currProcess;
                    yield return new WaitForEndOfFrame();
                }

            }

            tmp = 100;
            while (_currProcess < tmp)
            {
                ++_currProcess;
                _asyncOperation.allowSceneActivation = true;

            }

            yield return new WaitForSeconds(4f);
            if (_currProcess == 100)
            {
                panel.Close();
                TownManager.instance.content.gameObject.SetActive(false);
                TownManager.instance.gameCamera.enabled = false;
                isLoad = false;
                _currProcess = 0;
                _asyncOperation.allowSceneActivation = false;
            }
        }
        else
        {
            Debug.Log("Start Jump");
            StartCoroutine(FastJump(sceneName));
          
        }

    }

    public void OpenPanel()
    {
        panel.Open();
    }

    public IEnumerator FastJump(string sn)
    {
        
        isLoad = true;
        int tmp = 0;
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(sn, LoadSceneMode.Additive);
        yield return new WaitForSeconds(0.5f);
        tmp = 100;
        while (_currProcess < tmp)
        {
            ++_currProcess;
        }

        if (_currProcess == 100)
        {
            TownManager.instance.content.gameObject.SetActive(false);
            TownManager.instance.gameCamera.enabled = false;
            panel.Close();
            isLoad = false;
            _currProcess = 0;
        }
    }

    public void LeaveTown()
    {
        
            panel.Open();
            StartCoroutine(StartLeaveing());
        

        
    }

    IEnumerator StartLeaveing()
    {


            // TownManager.instance.WorldMap();
        

        int tmp=0;
        
        yield return new WaitForSeconds(2.0f);
        tmp=90;
        
        while (_currProcess < tmp)
        {
            ++_currProcess;
            yield return new WaitForEndOfFrame();
        }

        tmp = 100;
        while (_currProcess < tmp)
        {
            ++_currProcess;
        }
        yield return new WaitForSeconds(2.0f);
        if (_currProcess == 100)
        {
            panel.Close();
            _currProcess = 0;
        }
        
    }

    public void Loading(){
        panel.gameObject.SetActive(true);

       StartCoroutine(LoadingForWaitRoutine()
           );
               }

    public   IEnumerator LoadingForWaitRoutine()
    {

        int tmp=0;
        panel.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        tmp=90;
         Debug.Log("Load Town Last Save Town Name");
 Debug.Log("Load Game");
            // shenshanloading.LoadingForWaitRoutine();
              TownManager.instance.ShowTown(true);
                  PlayerData.LOCTYPE = LocType.Town;
                  DialogueManager.StartConversation("回滚");
                //   TownManager.instance.ShowTown(true);
                //   PlayerData.LOCTYPE = LocType.Town;
                //   DialogueManager.StartConversation("回滚");

        while (_currProcess < tmp)
        {
            ++_currProcess;
            yield return new WaitForEndOfFrame();
        }

        tmp = 100;
        while (_currProcess < tmp)
        {
            ++_currProcess;
        }
        yield return new WaitForSeconds(2.0f);
        if (_currProcess == 100)
        {
            panel.Close();
            _currProcess = 0;
        }
        
    }
    

    // Update is called once per frame
    void Update()
    {
        if (isLoad==true)
        {
            loading.Value = _currProcess / 100.0f;
        }
    }
}
