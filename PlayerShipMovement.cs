//Unity Headers
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipMovement : MonoBehaviour
{
    //creating variables.
    [SerializeField] float XSpeed = 10f;
    [SerializeField] float YSpeed = 10f;
   
    [SerializeField] float Xrotate = -5f;
    [SerializeField] float Xcontroll = -70f;

    [SerializeField] float Yrotate = 5f;
    [SerializeField] float Zcontroll = -70f;
    
    //multiple guns can be attached, caching them in an array
    // for easiness in using them.
    [SerializeField] GameObject[] Guns;

    float HorizontalThrow;
    float VerticalThrow;

    bool Alive = true;
    
    void Update()
    {
        //while the shp is alive
        if (Alive)
        {
            //functions for controlling the movements
            ShipMovement();
            ShipRotation();
            //function for shooting
            ShootController();
        }
    }
    
    //function for destroying the ship
   public void DestroyShip()
    {
        Alive = false;
    }
    
    //function for ship movement
    private void ShipMovement()
    {
        //contolling horizontal ovement
        HorizontalThrow = Input.GetAxis("Horizontal") * XSpeed * Time.deltaTime;
        float NewXpos = transform.localPosition.x + HorizontalThrow;
        
        //controlling vertical movement
        VerticalThrow = Input.GetAxis("Vertical") * YSpeed * Time.deltaTime;
        float NewYpos = transform.localPosition.y + VerticalThrow;
        
        transform.localPosition = new Vector3(Mathf.Clamp(NewXpos, -5f, 5f), Mathf.Clamp(NewYpos, -3.5f, 3.5f), transform.localPosition.z);
    }
    
    //function for controlling ship rotation
    private void ShipRotation()
    {
        //controlling rotation
        float Pitch = transform.localPosition.y * Xrotate + VerticalThrow * Xcontroll;
        float Raw = transform.localPosition.x * Yrotate;
        float Roll = HorizontalThrow * Zcontroll;

        transform.localRotation = Quaternion.Euler(Pitch, Raw, Roll);
    }

    //function responsible for shooting
    void ShootController()
    {
        //while the button is being pressed, fire continously.
        if(Input.GetButton("Fire1"))
        {
            Fire(true);
        }
        else
        {
            Fire(false);
        }
    }
    
    //function responsible for firing shots
    private void Fire(bool isTrue)
    {
        //multiple guns can be attached, and do the firing for all of them
        foreach (GameObject gun in Guns)
        {   
            //fire the shots.
            var EmissionModule = gun.GetComponent<ParticleSystem>().emission;
            EmissionModule.enabled = isTrue;
        }
    }
}
