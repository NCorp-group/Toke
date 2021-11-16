using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestingEvents : MonoBehaviour
{
   public event EventHandler<OnSpacePressedEventArgs> OnSpacePressed;

   public class OnSpacePressedEventArgs : EventArgs
   {
      public int space_count;
   }

   private int space_count = 0;


   public event TestEventDelegate OnFloatEvent;
   public delegate void TestEventDelegate(float f);

   public event Action<bool, int> OnActionEvent;

   public UnityEvent OnUnityEvent;

   private void Start()
   {
      // OnSpacePressed += (sender, args) => Debug.Log("Space!");
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Space))
      {
         space_count++;
         // only call if OnSpacePressed != null
         OnSpacePressed?.Invoke(this,new OnSpacePressedEventArgs {
            space_count = space_count
         });
         
         OnFloatEvent?.Invoke(5.5f);
         
         OnActionEvent?.Invoke(true, 42);
         
         OnUnityEvent?.Invoke();
      }
   }
}
