using System;
using Player;
using Solders;
using UnityEngine;
using UnityEngine.Serialization;
using Vehicle;

public class VehicleController : MonoBehaviour
{
    private float _horizontalInput, _verticalInput;
    private float _currentSteerAngle, _currentbreakForce;
    private bool _isBreaking;
    
    // Settings
    [SerializeField] private float motorForce = 1500f;
    [SerializeField] private float breakForce = 3000f;
    [SerializeField] private float maxSteerAngle = 30f;
    // Smoothing factor for acceleration/deceleration
    [SerializeField] private float accelerationSmoothness = 5f; 
    [SerializeField] private float defultBreakForce = 200f;
    // Internal value for smoothing motor torque
    private float currentMotorTorque = 0f;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels (Visual Meshes)
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;
     private VehicalSoundManager vehicleSoundManager;


    private void Awake()
    {
        vehicleSoundManager = GetComponent<VehicalSoundManager>();
    }
    
    

    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -0.5f, 0);

        vehicleSoundManager.CurrentEngineSoundType = VehicalSoundManager.EngineSoundType.Start;

    }

    private void FixedUpdate() {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        ApplyDownforce();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Target target))
        {
            Rigidbody targetRigidbody = target.GetComponent<Rigidbody>();
            Vector3 forceDir = (other.contacts[0].point - transform.position).normalized;
            target.gameObject.GetComponent<Target>().TakeDamage(30);
            targetRigidbody.AddForce(forceDir * 30f, ForceMode.Impulse);
        }
        else if (other.gameObject.TryGetComponent(out SolderAI sodler))
        {
            Rigidbody targetRigidbody = target.GetComponent<Rigidbody>();
            Vector3 forceDir = (other.contacts[0].point - transform.position).normalized;
            
            targetRigidbody.AddForce(forceDir * 30f, ForceMode.Impulse);
        }
    }

    private void GetInput() {
        // Steering Input
        _horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration/Deceleration Input
        _verticalInput = Input.GetAxis("Vertical");

        // Braking Input
        _isBreaking = Input.GetKey(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.G))
        {
            Transform player = transform.GetChild(4);
            player.gameObject.SetActive(true);
            player.SetParent(null);
            VehicaleManager.Instance.InteractPlayerVehicleExit();
        }
    }

    private void HandleMotor() {
         
        float targetTorque = _verticalInput * motorForce;
        
        currentMotorTorque = Mathf.Lerp(currentMotorTorque, targetTorque, Time.fixedDeltaTime * accelerationSmoothness);
        frontLeftWheelCollider.motorTorque = currentMotorTorque;
        frontRightWheelCollider.motorTorque = currentMotorTorque;

        if (_isBreaking)
        {
            vehicleSoundManager.PlayEngineStopSound();
            _currentbreakForce = breakForce;
        }
        else if (_verticalInput is > -0.1f and < 0.1f)
        {
            if (vehicleSoundManager.CurrentEngineSoundType != VehicalSoundManager.EngineSoundType.Running && vehicleSoundManager.CurrentEngineSoundType != VehicalSoundManager.EngineSoundType.Start)
            {
                vehicleSoundManager.CurrentEngineSoundType = VehicalSoundManager.EngineSoundType.Running;
            }
            _currentbreakForce = defultBreakForce;
        }
        else
        {
             if (vehicleSoundManager.CurrentEngineSoundType != VehicalSoundManager.EngineSoundType.Acceleration)
            {
                vehicleSoundManager.CurrentEngineSoundType = VehicalSoundManager.EngineSoundType.Acceleration;
            }
            _currentbreakForce = 0f;
        }
        ApplyBreaking();
    }
    
    void ApplyDownforce() {
        float downforce = GetComponent<Rigidbody>().linearVelocity.magnitude * 10f;  
        GetComponent<Rigidbody>().AddForce(-transform.up * downforce);
    }

    private void ApplyBreaking() {
        frontRightWheelCollider.brakeTorque = _currentbreakForce;
        frontLeftWheelCollider.brakeTorque = _currentbreakForce;
        rearLeftWheelCollider.brakeTorque = _currentbreakForce;
        rearRightWheelCollider.brakeTorque = _currentbreakForce;
    }

    private void HandleSteering() {
        _currentSteerAngle = maxSteerAngle * _horizontalInput;
        frontLeftWheelCollider.steerAngle = _currentSteerAngle;
        frontRightWheelCollider.steerAngle = _currentSteerAngle;
    }

    private void UpdateWheels() {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) {
        Vector3 pos;
        Quaternion rot; 
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
