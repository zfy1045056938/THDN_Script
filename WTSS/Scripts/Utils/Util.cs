using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Reflection;
using System.Text;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GameDataEditor;
using Invector.vItemManager;


public enum SkillCastType
{
    None,
    Init,
    Passive,
}
/// <summary>
/// TODO save config data from gde
/// </summary>
public static class Util
{


    public static void InvokeMany(Type type, object Obj, string methodPrefix, params object[] args)
    {
        foreach (MethodInfo method in GetMethodsByprefix(type, methodPrefix))
        {
            method.Invoke(Obj, args);
        }
    }

    static Dictionary<KeyValuePair<Type, string>, MethodInfo[]> lookup = new Dictionary<KeyValuePair<Type, string>, MethodInfo[]>();


    private static IEnumerable<MethodInfo> GetMethodsByprefix(Type type, string methodPrefix)
    {
        KeyValuePair<Type, string> keys = new KeyValuePair<Type, string>();
        //
        if (!lookup.ContainsKey(keys))
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
            .Where(m => m.Name.StartsWith(methodPrefix)).ToArray();
            lookup[keys] = methods;
        }
        return lookup[keys];
    }

    public static string PBKDF2Hash(string text, string salt)
    {
        byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
        Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(text, saltBytes, 10000);
        byte[] hash = pbkdf2.GetBytes(20);
        return BitConverter.ToString(hash).Replace("-", string.Empty);
    }

    public static Transform GetNearestTransform(List<Transform> ts, Vector3 from)
    {
        Transform nearest = null;
        foreach (Transform f in ts)
        {
            if (nearest == null || Vector3.Distance(f.position, from) < Vector3.Distance(nearest.position, from))
                nearest = f;
        }
        return nearest;
    }

    public static float GetZoomUniversal()
    {
        if (Input.mousePresent)
        {
            return GetAxisRawScrollUniversal();
        }
        else if (Input.touchSupported)
        {
            return GetPinch();
        }

        return 0;
    }

    public static void ResetMovement(this NavMeshAgent agent)
    {
        agent.ResetPath();
        agent.velocity = Vector3.zero;

    }

    public static float GetAxisRawScrollUniversal()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll < 0) return -1;
        if (scroll > 0) return 1;
        return 0;
    }

    public static float GetPinch()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            //
            Vector2 touchZeroPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePos = touchOne.position - touchOne.deltaPosition;

            //
            float prevTouchDeltaMag = (touchZeroPos - touchOnePos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
            //
            return touchDeltaMag - prevTouchDeltaMag;
        }

        return 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static bool IsCursorOverUserInterface()
    {
        if (EventSystem.current.IsPointerOverGameObject())

            return true;

        //
        for (int i = 0; i < Input.touchCount; i++)

            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
            {
                return true;
            }
        //
        return GUIUtility.hotControl != 0;




    }

    public static bool AnyInyActival()
    {
        foreach (Selectable sel in Selectable.allSelectables)

            if (sel is InputField inf && inf.isFocused) return true;

        return false;
    }

    public static bool RaycastWithout(Ray hit, out RaycastHit p, GameObject ignore)
    {
        Dictionary<Transform, int> backups = new Dictionary<Transform, int>();
        //
        foreach (Transform t in ignore.GetComponentsInChildren<Transform>(true))
        {
            backups[t] = t.gameObject.layer;
            t.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        //
        bool result = Physics.Raycast(hit, out p);
        //
        foreach (KeyValuePair<Transform, int> kvp in backups)
        {
            kvp.Key.gameObject.layer = kvp.Value;
        }

        return result;
    }

    public static float CloserDistance(Collider a, Collider b)
    {
        if (a.bounds.Intersects(b.bounds)) return 0;

        return Vector3.Distance(a.ClosestPoint(b.transform.position), b.ClosestPoint(a.transform.position));
    }

    public static void BalancePrefab(GameObject gameobject, int amount, Transform content)
    {
        for (int i = content.childCount; i < amount; ++i)
        {
            GameObject go = GameObject.Instantiate(gameobject);
            go.transform.SetParent(content, false);
        }

        //
        for (int i = content.childCount - 1; i >= amount; --i)
        {
            GameObject.Destroy(content.GetChild(i).gameObject);
        }
    }

    public static bool InputActive()
    {
        foreach (Selectable sel in Selectable.allSelectables)
        {
            if (sel is InputField intf && intf.isFocused)
            {
                return true;
            }
        }

        return false;
    }



    [System.Serializable]
    public struct ExpontialLong
    {
        public long mult;
        public float baseValue;
        public long Get(int level) => Convert.ToInt64(mult * Mathf.Pow(baseValue, (level - 1)));
    }





}