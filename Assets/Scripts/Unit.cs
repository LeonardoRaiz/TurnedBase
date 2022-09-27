using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour
{
    private Vector3 targetPosition;
   
    void Update()
    {
        float stoppingDistance = .1f;
        if(Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            OnMove(MouseWorld.GetPosition());
        }
    }

    

    private void OnMove(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;            
    }
}
