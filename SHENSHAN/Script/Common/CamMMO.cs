using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMMO : MonoBehaviour
{
   public Transform target;
   public int mouseBtn =1;
   public float distance=20;
   public float maxDistance=20;
   public float minDistance=3;

   public float zoomSpeedMouse=1;
   public float zoomSpeedTouch=0.2f;
   public float rotationSpeed=2;
   public float xMinAngle = -40;
   public float xMaxAngle=80;
   public Vector3 offset =Vector3.zero;
   public LayerMask viewBlock;

   Vector3 rotation ;

   bool rotationInit;

     /// <summary>
   /// LateUpdate is called every frame, if the Behaviour is enabled.
   /// It is called after all Update functions have been called.
   /// </summary>
   void LateUpdate()
   {
      if(!target)return;
      Vector3 targetPos = target.position+offset;

      if(Input.mousePresent){
          if(Input.GetMouseButton(mouseBtn)){
              if(!rotationInit){
                  rotation = transform.eulerAngles;
                  rotationInit =true;
              }
              //
              rotation.y+= Input.GetAxis("Mouse X")+rotationSpeed;
              rotation.x -= Input.GetAxis("Mouse Y")+rotationSpeed;

              transform.rotation =Quaternion.Euler(rotation.x,rotation.y,0);
          }
      }else{
          transform.rotation = Quaternion.Euler(new Vector3(45,0,0));
      }   
      //
      float speed = Input.mousePresent  ? zoomSpeedMouse : zoomSpeedTouch;
      float step = GetZoomUniversal() *  speed;
      distance = Mathf.Clamp(distance-step,minDistance,maxDistance);
      //
      transform.position = targetPos - (transform.rotation * Vector3.forward *distance);
      //
      if(Physics.Linecast(targetPos,transform.position,out RaycastHit hit,viewBlock)){
          float d = Vector3.Distance(targetPos,hit.point) -0.1f;
          //
          transform.position= targetPos - ( transform.rotation*Vector3.forward*d);
      }

   }

   public static float GetZoomUniversal(){
       if(Input.mousePresent){
           return GetAxisRawScrollUniversal();
       }else if(Input.touchSupported){
           return GetPinch();
       }
       return 0;
   }

   public static float GetAxisRawScrollUniversal(){
       float scroll=Input.GetAxisRaw("Mouse ScrollWheel");
       if(scroll<0)return -1;
       if(scroll>0)return 1;
       return 0;
   }



public static float GetPinch(){
    if(Input.touchCount==2){
        Touch touZero = Input.GetTouch(0);
        Touch touOne=Input.GetTouch(1);

        //
        Vector2 touchZeroPrePos=touZero.position-touZero.deltaPosition;
        Vector2 touchOnerevPos=touOne.position - touOne.deltaPosition;

        //
        float prevTouchDeltaMag = (touchZeroPrePos - touchOnerevPos).magnitude;
        float touchDeltaMag=(touZero.position-touOne.position).magnitude;
        //
        return touchDeltaMag - prevTouchDeltaMag;
    }
    return 0;
}
}