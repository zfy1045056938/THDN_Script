using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class CharacterMovement : MonoBehaviour
{
    private PlayerData player;
    public NavMeshAgent navmesh;
    public Rigidbody rigidbody;
    public Animator anim;
    public Vector3 targetPos;
    public LayerMask layerMask;
    public CharacterController controller;
    public float speed;
    public float rotationSpeed;
    private Quaternion _lookRotation;
    private Vector3 _direction;

    // Start is called before the first frame update
    void Start()
    {
        player=PlayerData.localPlayer;
        controller = GetComponent<CharacterController>();
        targetPos = transform.position;

       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)&&player!=null&&! EventSystem.current.IsPointerOverGameObject()){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit; 
            if(Physics.Raycast(ray,out hit,Mathf.Infinity,layerMask)){
                targetPos=hit.point;
            }
        }
            //
            anim.SetBool("IsRun",true);
        //
        controller.Move((targetPos-transform.position).normalized *speed *Time.deltaTime);
        if((targetPos-transform.position).magnitude>0.2f){
            
            //
            _direction = (targetPos-transform.position).normalized;

            //
            _lookRotation = Quaternion.LookRotation(_direction);

            //
            transform.rotation = Quaternion.Slerp(transform.rotation,_lookRotation,Time.deltaTime*rotationSpeed);
            transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);

        }else{
           anim.SetBool("IsRun",false);
        }
    }
}
