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
    private readonly int _aniMoveUp = Animator.StringToHash("Anim_player_ide_up");
    private readonly int _aniIdeUp = Animator.StringToHash("Anim_player_move_up");
    private readonly int _aniMoveDown = Animator.StringToHash("Anim_player_move_down");
    private readonly int _aniIdeDown = Animator.StringToHash("Anim_player_ide_down");
    private readonly int _aniIdeStanby = Animator.StringToHash("IDE_NOT_MOVE");
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
        if (!GameState.CanControlPlayer)
            return;
        MovementUpdate();
    }
    #endregion

    #region Input Logic
    private void GatherInput()
    {
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");
       
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
        else if (_moveDir.y != 0)
        {
            if (_moveDir.y > 0)
            {
                _faceDir = Directions.UP;
            }
            else
            {
                _faceDir = Directions.DOWN;
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

        var isMoving = _moveDir.sqrMagnitude > 0;
        var animation = _aniIdeStanby;
       // print(_faceDir);
        switch (_faceDir)
        {
            case Directions.LEFT:
                animation=isMoving? _aniMoveRight:_aniIdeRight;
                break; 
            case Directions.RIGHT:
                animation = isMoving ? _aniMoveRight : _aniIdeRight;
                break;
            case Directions.UP:
                animation = isMoving ? _aniMoveUp : _aniIdeUp;
                break;
            case Directions.DOWN:
                animation = isMoving ? _aniMoveDown : _aniIdeDown;
                break;
            default:
                animation = isMoving ? _aniIdeStanby : _aniIdeStanby;
                break;
        }
        _animator.CrossFade(animation, 0);
    }
}
