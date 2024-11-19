using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //references
    Move _movement;
    BombBag _bombing;

    //mouvements
    Task _currentMovementTask;
    Vector2 movementInput;
    [SerializeField] LayerMask _layerMask;

    private void Awake()
    {
        TryGetComponent<Move>(out _movement);
        TryGetComponent<BombBag>(out _bombing);
    }

    void Update()
    {
        if (movementInput != Vector2Int.zero && (_currentMovementTask == null || _currentMovementTask.IsCompleted))
        {
            Vector2 targetPosition = ((Vector2)transform.position + movementInput).Round();
            if (!Physics2D.OverlapPoint(targetPosition, _layerMask))  _currentMovementTask = _movement.MoveToPoint(Graph.Instance.Nodes[ targetPosition.RoundToInt()], _movement._moveSpeed);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movementInput=context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            movementInput = Vector2.zero;
        }

        if (Mathf.Abs(movementInput.x) > Mathf.Abs(movementInput.y)) movementInput.y = 0; else movementInput.x = 0;
    }

    public void TryToUseBomb(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _bombing.TryToUseBomb();
        }
    }
}
