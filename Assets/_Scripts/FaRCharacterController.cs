using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Utils;

namespace FaRUtils.FPSController
{
    [RequireComponent(typeof(CharacterController))]
    public class FaRCharacterController : MonoBehaviour
    {
        public static FaRCharacterController instance;

        private CharacterController _controller;
        
        [SerializeField] private Camera cam;
        [SerializeField] private float movementSpeed = 15f;

        [SerializeField] private float defaultWalkSpeed = 6f;
        [SerializeField] private float defaultMovementSpeed = 15f;
        [SerializeField] public float lookSensitivity = 0.05f;
        
        private float _xRotation = 0f;

        [Header("Parámetros de movimiento")]
        private Vector3 _velocity;
        public float gravity = -9.81f;
        private bool _grounded;
        public float jumpSpeed;
        public bool doWalk;

        [Header("Parámetros de zoom")]
        public float zoomFOV = 35.0f;
        public float zoomSpeed = 9f;
        private float _targetFOV;
        private float _baseFOV;

        [Header("Parámetros de agacharse")]
        private float _initHeight;
        [SerializeField] private float crouchHeight;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else instance = this;
        }

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            if (cam == null) cam = GetComponentInChildren<Camera>();
            _initHeight = _controller.height;
            Cursor.lockState = CursorLockMode.Locked;
            SetBaseFOV(cam.fieldOfView);
        }


        private void OnEnable()
        {
            EnableInputActions();
            SceneManager.activeSceneChanged += SceneChangedHandler;
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= SceneChangedHandler;
        }

        private void SceneChangedHandler(Scene arg0 = default, Scene arg1 = default)
        {
            Invoke(nameof(EnableInputActions), 0.1f); 
        }

        public void EnableInputActions()
        {
            GameInput.playerInputActions.Player.Enable();
        }

        private void Update()
        {
            DoMovement();
            DoLooking();
            DoWalk();
        }

        public void DoLooking()
        {
            Vector2 looking = GetPlayerLook();
            float lookX = looking.x * lookSensitivity;
            float lookY = looking.y * lookSensitivity;

            _xRotation -= lookY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
            
            cam.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            
            transform.Rotate(Vector3.up * lookX);
        }

        private void DoMovement()
        {
            _grounded = _controller.isGrounded;
            if (_grounded)
            {
                _velocity.y = -2f;

                if (GameInput.playerInputActions.Player.Jump.WasPressedThisFrame())
                {
                    _velocity.y = jumpSpeed;
                }
            }
            else
            {
                _velocity.y += gravity * Time.deltaTime;
            }

            Vector2 movement = GetPlayerMovement();
            Vector3 move = transform.right * movement.x + transform.forward * movement.y;

            // Check for obstacles before moving
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, move, out hit, movementSpeed * Time.deltaTime + 0.1f))
            {
                _controller.Move(move * movementSpeed * Time.deltaTime);
            }
            else
            {
                // If there's an obstacle, try to slide along it
                Vector3 slideDirection = Vector3.ProjectOnPlane(move, hit.normal).normalized;
                _controller.Move(slideDirection * movementSpeed * Time.deltaTime);
            }

            _velocity.y += gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }

        private void DoWalk()
        {
            if (!doWalk) return;

            if (GameInput.playerInputActions.Player.Sprint.WasPressedThisFrame())
            {
                movementSpeed = defaultWalkSpeed;
            }
            else if (GameInput.playerInputActions.Player.Sprint.WasReleasedThisFrame())
            {
                movementSpeed = defaultMovementSpeed;
            }
        }

        public void SetBaseFOV(float fov)
        {
            _baseFOV = fov;
        }


        public Vector2 GetPlayerMovement()
        {
            return GameInput.playerInputActions.Player.Movement.ReadValue<Vector2>();
        }

        public Vector2 GetPlayerLook()
        {
            return GameInput.playerInputActions.Player.Look.ReadValue<Vector2>();
        }

        public void Teleport(Transform newPosition)
        {
            _controller.enabled = false;
            transform.SetPositionAndRotation(newPosition.position, newPosition.rotation);
            _controller.enabled = true;
            
        }
    }
}