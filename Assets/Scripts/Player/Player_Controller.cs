using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
public class player_controler : MonoBehaviour
{
    private enum Directions { UP,DOWN,LEFT,RIGHT}


    #region Editor Data
    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 100f;

    [Header("Dependecies")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator   _animator ;
    [SerializeField] SpriteRenderer _spriteRenderer;
    #endregion
    #region Internal Data
    private Vector2 _moveDir = Vector2.zero;
    private Directions _faceDir = Directions.RIGHT;
    private readonly int _aniMoveRight = Animator.StringToHash("Anim_player_move_right");
    private readonly int _aniIdeRight  = Animator.StringToHash("Anim_player_ide_right");
    #endregion

    #region Tick
    private void Update()
    {
        GatherInput();
        CalculateFaceDir();
        UpdateAnimation();
    }
    private void FixedUpdate()
    {
        MovementUpdate();
    }
    #endregion

    #region Input Logic
    private void GatherInput()
    {
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");
        print(_moveDir);
    }

    private void MovementUpdate()
    {
        _rb.velocity = _moveDir.normalized * _moveSpeed * Time.fixedDeltaTime;
    }
    #endregion

    private void CalculateFaceDir()
    {
        if(_moveDir.x != 0)
        {
            if (_moveDir.x > 0)
            {
                _faceDir = Directions.RIGHT;
            }
            else
            {
                _faceDir = Directions.LEFT;
            }
        }
    }
    private void UpdateAnimation()
    {
        if (_faceDir == Directions.LEFT)
        {
            _spriteRenderer.flipX = true;
        }
        else if (_faceDir == Directions.RIGHT)
        {
            _spriteRenderer.flipX = false;
        }

        if(_moveDir.sqrMagnitude > 0)
        {
            _animator.CrossFade(_aniMoveRight,0);
        }
        else
        {
            _animator.CrossFade(_aniIdeRight, 0);
        }
    }
}
