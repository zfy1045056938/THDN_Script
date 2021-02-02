using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

/// <summary>
/// 工具类
/// </summary>
public   class Util :MonoBehaviour
{

 

    internal static void InitGamObject(GameObject root)
    {
        throw new NotImplementedException();
    }





    /// <summary>
    /// Copy Item ,If Equip or drag  then create instance
    /// </summary>
    /// <returns>The copy.</returns>
    /// <param name="item">Item.</param>
    //public  ItemClass DeepCopy(ItemClass item)
    //{
    //    GameObject obj = item.worldObject;
    //    Sprite tmpTex = item.itemIcon;
    //    item.itemIcon = null;
    //    AudioClip clip = item.itemSound;
    //    AudioClip useClip = item.useSound;
    //    if (obj == null)
    //    {
    //        return null;
    //    }
    //    //
    //    ItemClass i = (ItemClass)Process(obj);
    //    i.worldObject = obj;
    //    i.itemIcon = tmpTex;
    //    i.itemSound = clip;
    //    i.useSound = useClip;
    //    DestroyImmediate(GameObject.Find("New GameObject"));
    //    return i;

    //}


    #region SerializeField
    public  object Process(object obj)
    {
        if (obj == null)
        {
            return null;
        }
        Type type = obj.GetType();
        if (type.IsValueType || type == typeof(string))
        {
            return obj;
        }
        else if (type.IsArray)
        {
            Type elementsType = Type.GetType(
                type.FullName.Replace("[]", string.Empty));
            var array = obj as Array;
            Array copied = Array.CreateInstance(elementsType, array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                copied.SetValue(Process(array.GetValue(i)), i);
            }
            return Convert.ChangeType(copied, obj.GetType());
        }
        else if (type.IsClass)
        {
            object toret = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                object fieldValue = field.GetValue(obj);
                if (field == null)
                {
                    continue;
                }
                field.SetValue(toret, Process(fieldValue));

                return toret;
            }

        }
        else
        {
            return null;
        }

        return obj;
    }


    #endregion

 
    
}
