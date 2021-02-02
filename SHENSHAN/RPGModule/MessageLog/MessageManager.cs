using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Message
{
    private string message;
    public Text text;
    public float fadeTimer;
    public Color color;
}


public class MessageManager : MonoBehaviour
{
    public static MessageManager instance;
    private List<Message> message;
    public float fadeTime;
    public float upSpeed;
    public Text prefab;

    private void Start()
    {
       message= new List<Message>();
    }


    private void Update()
    {
        if (message.Count>0)
        {
            for (int i = 0; i < message.Count; i++)
            {
                message[i].fadeTimer += Time.deltaTime;
                //
                if (message[i].fadeTimer> fadeTime)
                {
                    Destroy(message[i].text.gameObject);
                    message.Remove(message[i]);
                    return;
                }
                //
                message[i].text.rectTransform.localPosition += new Vector3(0, Time.deltaTime*upSpeed,0);
                message[i].text.color = Color.Lerp(message[i].color, Color.clear, message[i].fadeTimer / fadeTime);
            }
        }
    }

    public void AddMessage(Color col, string v)
    {
       Message mes = new Message();
       mes.text = Instantiate(prefab, transform.position, Quaternion.identity);
       mes.text.transform.SetParent(transform);
       mes.text.rectTransform.localScale= Vector3.one;
       mes.text.text = v;
       mes.color = mes.text.color = col;
       message.Add(mes);
    }
}
