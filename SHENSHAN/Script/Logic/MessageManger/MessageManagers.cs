using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;
using DG.Tweening;
//public enum MessageType{
//    None,
//    InBattle,
//    NewbeeDirection,
//    InCollection,
//    InShop,

//}
//消息管理器,用于进行内容提示,包含以下功能
//1.在对战回合时,用于提示回合方
//2.当条件不足时，提示以下信息
//3.用与新手指引条件，当玩家首次进行游戏时提示以下信息
public class MessageManagers:MonoBehaviour
{
    public List<Message> messages;
    public float fadeTime;
    public float upSpeed;
    
    public Text MessageText;
    public GameObject MessagePanel;

    public static MessageManagers instance;
    public float dur =2.0f;
    void Awake()
    {
        instance = this;
        MessagePanel.SetActive(false);

        messages = new List<Message>();
    }

    public  void AddMessage(string Message, float Duration)
    {
        StartCoroutine(ShowMessageCoroutine(Message, Duration));
    }

    public  void AddMessage(string Message)
    {
        StartCoroutine(ShowMessageCoroutine(Message, dur));
    }

   public IEnumerator ShowMessageCoroutine(string Message, float Duration)
    {
        //Debug.Log("Showing some message. Duration: " + Duration);
        MessageText.text = Message;
        MessagePanel.SetActive(true);

        yield return new WaitForSeconds(Duration);

        MessagePanel.SetActive(false);
    }



}