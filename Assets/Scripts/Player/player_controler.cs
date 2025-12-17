using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
public class player_controler : MonoBehaviour
{
    #region Internal Data
    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 100f;
    [Header("Dependecies")]
    [SerializeField] Rigidbody2D _rb;
    private Vector2 _moveDir = Vector2.zero;
    
    #endregion

    private void Update()
    {
        GatherInput();

    }
    private void FixedUpdate()
    {
        MovementUpdate();
    }
    private void GatherInput()
    {
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");
        print(_moveDir);
    }

    private void MovementUpdate()
    {
        _rb.velocity = _moveDir. * _moveSpeed * Time.fixedDeltaTime;
    }
}
