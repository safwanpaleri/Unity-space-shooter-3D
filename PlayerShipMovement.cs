using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipMovement : MonoBehaviour
{
    [SerializeField] float XSpeed = 10f;
    [SerializeField] float YSpeed = 10f;
   
    [SerializeField] float Xrotate = -5f;
    [SerializeField] float Xcontroll = -70f;

    [SerializeField] float Yrotate = 5f;
    [SerializeField] float Zcontroll = -70f;

    [SerializeField] GameObject[] Guns;

    float HorizontalThrow;
    float VerticalThrow;

    bool Alive = true;

    void Update()
    {
        if (Alive)
        {
            ShipMovement();
            ShipRotation();
            ShootController();
        }
    }
   public void DestroyShip()
    {
        Alive = false;
    }
    private void ShipMovement()
    {
        HorizontalThrow = Input.GetAxis("Horizontal") * XSpeed * Time.deltaTime;
        float NewXpos = transform.localPosition.x + HorizontalThrow;

        VerticalThrow = Input.GetAxis("Vertical") * YSpeed * Time.deltaTime;
        float NewYpos = transform.localPosition.y + VerticalThrow;

        transform.localPosition = new Vector3(Mathf.Clamp(NewXpos, -5f, 5f), Mathf.Clamp(NewYpos, -3.5f, 3.5f), transform.localPosition.z);
    }

    private void ShipRotation()
    {
        float Pitch = transform.localPosition.y * Xrotate + VerticalThrow * Xcontroll;
        float Raw = transform.localPosition.x * Yrotate;
        float Roll = HorizontalThrow * Zcontroll;

        transform.localRotation = Quaternion.Euler(Pitch, Raw, Roll);
    }

    void ShootController()
    {
        if(Input.GetButton("Fire1"))
        {
            FireInTheHole(true);
        }
        else
        {
            FireInTheHole(false);
        }
    }

    private void FireInTheHole(bool confirmation)
    {
        foreach (GameObject gun in Guns)
        {
            var EmissionModule = gun.GetComponent<ParticleSystem>().emission;
            EmissionModule.enabled = confirmation;
        }
    }
}
