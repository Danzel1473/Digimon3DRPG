using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Toon.Universal.Samples;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float rotateSpeed = 3f;
    [SerializeField] Animator animator;

    float xRotation, yRotation;


    public void Update()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * rotateSpeed * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * rotateSpeed * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void MovePlayer()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveVec = transform.forward * v + transform.right * h;
        transform.position += moveVec.normalized * speed * Time.deltaTime;
        animator.SetFloat("Velocity", (moveVec.normalized * speed).x);

    }
}