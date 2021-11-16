using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingEventSubscriber : MonoBehaviour
{
    // Start is called before the first frame update
    private TestingEvents test_event;
    void Start()
    {
        test_event = GetComponent<TestingEvents>();
        test_event.OnSpacePressed += OnSpacePressedHandler;

        test_event.OnFloatEvent += f => Debug.Log(f);

        test_event.OnActionEvent += (b, i) =>
        {
            Debug.Log(b);
            Debug.Log(i);
        };
    }

    private void OnSpacePressedHandler(object sender, TestingEvents.OnSpacePressedEventArgs e)
    {
        Debug.Log("Space! " + e.space_count);
        // test_event.OnSpacePressed -= OnSpacePressedHandler;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestingUnityEvent()
    {
        Debug.Log("this is a unity event");
    }
}
