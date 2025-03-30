using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;
    [SerializeField] private PlayerManager _playerManager;
    public Camera _camera;
    
    [Header("Settings")] 
    public Transform _camPivot;
    private Vector3 camVeclocity;
    private float camSmooth = 1;
    [SerializeField] private float horizontalRotationSpeed = 200;
    [SerializeField] private float verticalRotationSpeed = 200;
    public float horizontalAngle;
    public float verticalAngle;
    
    [Title("MIN & MAX VALUE FOR VERTICAL")]
    [SerializeField] private float minValue = -30; 
    [SerializeField] private float maxValue = 60;

    [Title("CAMERA COLLISION VALUE")]
    [SerializeField] private float currentCamPosZ;
    [SerializeField] private float newCamPosZ;
    [SerializeField] private float collisionOffset = 0.2f;
    [SerializeField] private LayerMask collisionLayers;
    
    [Title("LOCK ON VALUE")]
    [SerializeField] private float lockOnRadius = 20f;
    [SerializeField] private float minViewableAngle = -50f;
    [SerializeField] private float maxViewableAngle = 50f;
    [SerializeField] private float lockOnTargetFollowSpeed = 0.2f;
    [Space] [Range(-1f, 1f)] 
    [SerializeField] private float configPivotHeight = -0.4f;
    public float localPivotHeight;
    [Space]
    public CharacterManager currentLockOnTarget;
    public CharacterManager leftLockOnTarget;
    public CharacterManager rightLockOnTarget;
    
    private List<CharacterManager> lockOnTargetList = new List<CharacterManager>();
    
    private Vector3 camPos;

    [Title("AIMING VALUE")] 
    [SerializeField] private Transform TfWhenAiming;
    public Vector3 aimDir;

    [Title("FLAGS")] 
    public bool isDeadCam;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        currentCamPosZ = _camera.transform.localPosition.z;
    }
    public void HandleCamera()
    {
        if (_playerManager != null && !_playerManager.isDead)
        {   
            HandleMovement();
            HandleRotation();
            HandleCollision();
        }
    }

    private void HandleMovement()
    {
        if (_playerManager._controlCombat.isAiming)
        {
            //var camPosition = Vector3.SmoothDamp(transform.position, _playerManager.transform.position,
            var camPosition = Vector3.SmoothDamp(transform.position, _playerManager.aimingCamFollowTf.position,
                ref camVeclocity, camSmooth * Time.deltaTime);
            transform.position = camPosition;
        }
        else
        {
            var camPosition = Vector3.SmoothDamp(transform.position, _playerManager.transform.position,
                ref camVeclocity, camSmooth * Time.deltaTime);
            transform.position = camPosition;
        }
    }
    private void HandleRotation()
    {
        if (_playerManager._controlCombat.isAiming)
        {
            HandleAimRotation();
        }
        else
        {
            HandleNormalRotation();
        }
    }

    private void HandleAimRotation()
    {
        if (!_playerManager.isGrounded) _playerManager._controlCombat.isAiming = false;
        if (!_playerManager.canRotate) return;

        _camPivot.localRotation = Quaternion.Euler(0, -15, 0);
        _camPivot.localPosition = new Vector3(-0.4f, _camPivot.localPosition.y, 1.5f);
        horizontalAngle += (InputManager.Instance.lookInputValue.x * horizontalRotationSpeed) * Time.deltaTime;
        verticalAngle -= (InputManager.Instance.lookInputValue.y * verticalRotationSpeed) * Time.deltaTime;
        verticalAngle = Mathf.Clamp(verticalAngle, minValue, maxValue);

        // Y <-> X BECAUSE OF ROTATION
        var rotationValue = Vector3.zero;
        rotationValue.y = horizontalAngle;
        var camRotation = Quaternion.Euler(rotationValue);
        transform.rotation = camRotation;
        
        rotationValue = Vector3.zero;
        rotationValue.x = verticalAngle;
        camRotation = Quaternion.Euler(rotationValue);
        _camera.transform.localRotation = camRotation;
    }
    
    private void HandleNormalRotation()
    {
        _camPivot.localPosition = new Vector3(0.3f, _camPivot.localPosition.y, 2.1f);
        _camera.transform.localRotation = Quaternion.Euler(0, 0, 0);
        // IF LOCK-ON -> LOCK-ON ROTATION
        if (_playerManager.isLockedOn)
        {
            // ROTATE THIS GAME OBJECT
            var rotationDir = _playerManager._controlCombat.target._controlCombatBase.lockOnTransform.position -
                              transform.position;
            rotationDir.Normalize();
            rotationDir.y = 0f;
            
            var targetRotation = Quaternion.LookRotation(rotationDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnTargetFollowSpeed);

            // ROTATE THE PIVOT GAME OBJECT
            rotationDir = _playerManager._controlCombat.target._controlCombatBase.lockOnTransform.position -
                          _camPivot.position;
            rotationDir.Normalize();
            rotationDir.y = configPivotHeight; // CONFIG PIVOT HEIGHT WHEN LOCK ON
            targetRotation = Quaternion.LookRotation(rotationDir);
            _camPivot.rotation = Quaternion.Slerp(_camPivot.rotation, targetRotation, lockOnTargetFollowSpeed);

            // SAVE CURRENT ROTATION SO IT DOES NOT SNAP WHEN LOCK OFF
            horizontalAngle = transform.eulerAngles.y;
            verticalAngle = _camPivot.eulerAngles.x;
        }
        // ELSE NORMAL ROTATION
        else
        {
            horizontalAngle += (InputManager.Instance.lookInputValue.x * horizontalRotationSpeed) * Time.deltaTime;
            verticalAngle -= (InputManager.Instance.lookInputValue.y * verticalRotationSpeed) * Time.deltaTime;
            verticalAngle = Mathf.Clamp(verticalAngle, minValue, maxValue);

            // Y <-> X BECAUSE OF ROTATION
            var rotationValue = Vector3.zero;
            rotationValue.y = horizontalAngle;
            var camRotation = Quaternion.Euler(rotationValue);
            transform.rotation = camRotation;
        
            rotationValue = Vector3.zero;
            rotationValue.x = verticalAngle;
            camRotation = Quaternion.Euler(rotationValue);
            _camPivot.localRotation = camRotation;
        }
    }
    private void HandleCollision()
    {
        newCamPosZ = currentCamPosZ;
        var dir = _camera.transform.position - _camPivot.position;
        dir.Normalize();
        if (Physics.SphereCast(_camPivot.position, collisionOffset, dir, out var hit, Mathf.Abs(newCamPosZ),collisionLayers))
        {
            var distance = Vector3.Distance(_camPivot.position, hit.point);
            newCamPosZ = collisionOffset - distance;
        }
        if (Mathf.Abs(newCamPosZ) < collisionOffset)
        {
            newCamPosZ = -collisionOffset;
        }
        
        if(_playerManager._controlCombat.isAiming)
        {
            // camPos.z = 0;
            // _camera.transform.localPosition = camPos;
            // return;
        }
        camPos.z = Mathf.Lerp(_camera.transform.localPosition.z, newCamPosZ, 0.1f);
        _camera.transform.localPosition = camPos;
    }

    public void HandleLocatingLockOnTarget()
    {
        Debug.Log("FINDING LOCK ON TARGET");
        var shortDistance = Mathf.Infinity;
        var shortDistanceOfRightTarget = Mathf.Infinity;
        var shortDistanceOfLeftTarget = -Mathf.Infinity;

        var colliders = Physics.OverlapSphere(_playerManager.transform.position, lockOnRadius, WorldUltilityManager.Instance.CharactersLayers);
        for (var i = 0; i < colliders.Length; i++)
        {
            var target = colliders[i].GetComponent<CharacterManager>();
            if (target != null)
            {
                // IF TARGET IS DEAD -> SKIP
                if(target.isDead) continue;
                
                // IF TARGET IS PLAYER -> SKIP
                if(target.transform.root == _playerManager.transform.root) continue;
                
                // IF CAN NOT DAMAGE TARGET -> SKIP
                if (!WorldUltilityManager.CheckIfCharacterCanDamageAnother(_playerManager._controlCombat.charactersGroup, 
                                                                                target._controlCombatBase.charactersGroup)) continue;
                
                //CHECK FIELD OF VIEW
                var targetDir = target.transform.position - _playerManager.transform.position;
                var viewableAngle = Vector3.Angle(targetDir, _camera.transform.forward);
                
                if (viewableAngle > minViewableAngle && viewableAngle < maxViewableAngle)
                {
                    Debug.Log("TARGET FOUND");
                    // ADD LAYER MASK FOR ENVIRONMENT LAYERS ONLY (TO DO)
                    
                    if (Physics.Linecast(_playerManager._controlCombat.lockOnTransform.position,
                            target._controlCombatBase.lockOnTransform.position, out var hit, 
                            WorldUltilityManager.Instance.EnvironmentLayers))
                    {
                        continue;
                    }
                    Debug.Log("TARGET FOUND 1");
                    lockOnTargetList.Add(target);
                }
            }
        }

        for (var i = 0; i < lockOnTargetList.Count; i++)
        {
            if (lockOnTargetList[i] != null)
            {
                var distanceFromTarget = Vector3.Distance(_playerManager.transform.position,
                    lockOnTargetList[i].transform.position);
                var targetDir = lockOnTargetList[i].transform.position - _playerManager.transform.position;
                if (distanceFromTarget < shortDistance)
                {
                    shortDistance = distanceFromTarget;
                    currentLockOnTarget = lockOnTargetList[i];
                }
                
                // SWITCH LOCK ON TARGET
                if (_playerManager.isLockedOn)
                {
                    var enemyPos =
                        _playerManager.transform.InverseTransformPoint(lockOnTargetList[i].transform.position);
                    var distanceFromLeftTarget = enemyPos.x;
                    var distanceFromRightTarget = enemyPos.x;
                    
                    if(lockOnTargetList[i] == _playerManager._controlCombat.target) continue;
                    if (distanceFromLeftTarget <= 0 && distanceFromLeftTarget > shortDistanceOfLeftTarget)
                    {
                        shortDistanceOfLeftTarget = distanceFromTarget;
                        leftLockOnTarget = lockOnTargetList[i];
                    }
                    else if(distanceFromLeftTarget >= 0 && distanceFromLeftTarget < shortDistanceOfRightTarget)
                    {
                        shortDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockOnTarget = lockOnTargetList[i];
                    }
                }
            }
            else
            {
                _playerManager.isLockedOn = false;
                ClearLockOnTargetList();
            }
        }
    }

    public IEnumerator FindNewLockOnTarget()
    {
        while (_playerManager.isDoingAction)
        {
            yield return null;
        }
        ClearLockOnTargetList();
        HandleLocatingLockOnTarget();
        if(currentLockOnTarget != null)
        {
            _playerManager._controlCombat.SetTarget(currentLockOnTarget);
            _playerManager.isLockedOn = true;
        }
        yield return null;
    }
    public void ClearLockOnTargetList()
    {
        if(currentLockOnTarget == null) return;
        currentLockOnTarget = null;
        leftLockOnTarget = null;
        rightLockOnTarget = null;
        lockOnTargetList.Clear();
    }

    public void SetCameraBehindPlayer()
    {
        _camera.transform.localRotation = Quaternion.Euler(0, 0, 0);
        horizontalAngle = _playerManager.transform.rotation.eulerAngles.y;
        verticalAngle = 20;
    }

    public void HandleZoomInForDialogue()
    {
        StartCoroutine(ZoomInCamera(-3, 5));
    }
    
    public void HandleZoomOutOfDialogue()
    {
        StartCoroutine(ZoomOutCamera(-4, 5));
    }

    private IEnumerator ZoomInCamera(float endValue, float speed)
    {
        while (currentCamPosZ < endValue)
        {
            currentCamPosZ += Time.deltaTime * speed;
            yield return null;
        }
        yield return null;
    }
    
    private IEnumerator ZoomOutCamera(float endValue, float speed)
    {
        while (currentCamPosZ > endValue)
        {
            currentCamPosZ -= Time.deltaTime * speed;
            yield return null;
        }
        yield return null;
    }
}
