using System;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
	    [Header("Player Movement")]
	    [SerializeField] float _sprintSpeed = 8.0f;
	    [SerializeField] float _moveSpeed = 4.0f;
	    [SerializeField] float _jumpHeight = 1.0f;
	    [SerializeField] float _speed = 0.0f;
	    
	    [Space(5)]
	    [Header("Ground check attributes")]
	    [SerializeField] float _gravityMultiplier = 2.5f;
	    [SerializeField] float _verticalVelocity = 0.0f;
	    [SerializeField] float _gravity = -9.81f;
	    [SerializeField] float _speedChangeRate = 10.0f;
	    [SerializeField] float _groundedOffset = -0.14f;
	    [SerializeField] float _groundedRadius = 0.5f;
	    [SerializeField] LayerMask _groundLayers;
	    [SerializeField] Transform _groundCheck;
		
	    
	    
	    [Space(5)]
	    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
	    public float JumpTimeout = 0.1f;
	    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
	    public float FallTimeout = 0.15f;
	    
	    private float _jumpTimeoutDelta;
	    private float _fallTimeoutDelta;
	    [Space(5)]
	    [Header("References")]
	    [SerializeField] CharacterController _controller;
					
	    

	    public void Move(bool isSprinting, Vector2 moveDirection)
		{
	        float targetSpeed = isSprinting ? _sprintSpeed : _moveSpeed;
	        if (moveDirection == Vector2.zero) targetSpeed = 0.0f;
	        
	        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
	        
	        float speedOffset = 0.1f;
	        float inputMagnitude = 1f;
	        
	        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
	        {
		        _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * _speedChangeRate);
		        _speed = Mathf.Round(_speed * 1000f) / 1000f;
	        }
	        else
	        {
		        _speed = targetSpeed;
	        }
	        
	        Vector3 inputDirection = new Vector3(moveDirection.x, 0.0f, moveDirection.y).normalized;
	        
	        if (moveDirection != Vector2.zero)
	        {
		        inputDirection = transform.right * moveDirection.x + transform.forward * moveDirection.y;
	        }
	        
	        _controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}
        
        public void Jump()
		{
			
	        if (IsGrounded())
	        {
		        _fallTimeoutDelta = FallTimeout;
		        if (_verticalVelocity < 0.0f)
		        {
			        _verticalVelocity = -2f;
		        }
		        
		        if (_jumpTimeoutDelta <= 0.0f)
		        {
			        _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
		        }
		        
		        if (_jumpTimeoutDelta >= 0.0f)
		        {
			        _jumpTimeoutDelta -= Time.deltaTime;
		        }
	        }
	        else
	        {
		        _jumpTimeoutDelta = JumpTimeout;
		        if (_fallTimeoutDelta >= 0.0f)
		        {
			        _fallTimeoutDelta -= Time.deltaTime;
		        }
	        }
		}
		
		public void ApplyGravity()
		{
			
	        if (!IsGrounded())
	        {
		        _verticalVelocity += _gravity * _gravityMultiplier * Time.deltaTime;
	        }
		}
		
		public bool IsGrounded()
		{
	        return Physics.CheckSphere(_groundCheck.position, _groundedRadius, _groundLayers);
		}
		
		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (true) Gizmos.color = transparentGreen;
			

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(_groundCheck.position.x, _groundCheck.position.y - _groundedOffset, _groundCheck.position.z), _groundedRadius);
		}
		
    }
}