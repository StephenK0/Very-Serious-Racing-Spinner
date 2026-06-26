using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
  [SerializeField] List<WheelCollider> steerWheels; //The wheels to apply steering to. 
  [SerializeField] List<WheelCollider> motorWheels; //The wheels to apply motor driving to. 
  [SerializeField] List<WheelCollider> brakeWheels; //The wheels to apply braking to. 

  // NOTE: had to change the maxMotorSpeed to public to slow down the car whenever it collides with a SlowDown obstacle.

  public int Slowdown;
  [SerializeField] public float maxMotorSpeed { get; private set; } = 300000;
  [SerializeField] public float minMotorSpeed { get; private set; } = -20000;
  float deltaMotorTorque = 10000;
  float deltaBrakeTorque = 10000;
  [SerializeField] public float currentSpeed { get; private set; } //Calculated as an average of the motor wheel speeds. 

  [SerializeField] float deltaSteer = 0.5f; //How quickly the steering wheels change direction. 
  [SerializeField] float maxSteer = 10; //The maximum range they can turn. 
  float currentSteer = 0;

  float motorAxis;
  float steerAxis;
  bool isBraking;
  bool isSlow;

  float effectiveMotorAxis;
  
  void Update() {
    if(Slowdown > 0) {
      Slowdown--;
      isSlow = true;
    }
    else isSlow = false;

    //Apply braking (or lack thereof). 
    if(isBraking || isSlow) {
      effectiveMotorAxis = 0;
      foreach(WheelCollider wheel in brakeWheels) {
        wheel.brakeTorque = deltaBrakeTorque;
      }
    }
    else {
      effectiveMotorAxis = motorAxis;
      foreach(WheelCollider wheel in brakeWheels) {
        wheel.brakeTorque = 0;
      }
    }

    
    //Apply motors and calculate average wheel speed
    currentSpeed = 0;
    foreach(WheelCollider wheel in motorWheels) {
      wheel.motorTorque = motorAxis * deltaMotorTorque;
      wheel.rotationSpeed = Mathf.Max(wheel.rotationSpeed, minMotorSpeed);
      wheel.rotationSpeed = Mathf.Min(wheel.rotationSpeed, maxMotorSpeed);
      currentSpeed += wheel.rotationSpeed;
    }
    currentSpeed /= motorWheels.Count;
    Debug.Log(currentSpeed);

    //Calculate steering. 
    if(steerAxis == 0) {
      if(currentSteer > 0) currentSteer = Mathf.Max(currentSteer - deltaSteer, 0);
      else if(currentSteer < 0) currentSteer = Mathf.Min(currentSteer + deltaSteer, 0);
    }
    currentSteer += steerAxis * deltaSteer;
    currentSteer = Mathf.Min(currentSteer, maxSteer);
    currentSteer = Mathf.Max(currentSteer, -maxSteer);

    //Apply steering. 
    foreach(WheelCollider wheel in steerWheels) {
      wheel.steerAngle = currentSteer;
    }

  }

  void OnMove(InputValue inp) {
    Vector2 direction = inp.Get<Vector2>();

    steerAxis = direction[0];
    motorAxis = direction[1];
  }

  void OnBrake(InputValue inp) {
    isBraking = true;
  }

  void OnStopBrake(InputValue inp) {
    isBraking = false;
  }
}
