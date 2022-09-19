using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 _moveDirection;
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _speed;
    [SerializeField] private bool _freezeInputs;
    [SerializeField] private bool _interact;
    [SerializeField] private Collider2D _interactableObject;
    
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        DialogueEvents.conversationStartedEvent += Listening;
        DialogueEvents.endConversation += FinishListening;
    }

    public void OnDestroy()
    {
        DialogueEvents.conversationStartedEvent -= Listening;
        DialogueEvents.endConversation -= FinishListening;
    }

    private void Update()
    {
        HandleInput();
        HandleInteraction();
    }

    private void FixedUpdate()
    {
        Move(_moveDirection, _speed);
    }

    private void HandleInput()
    {
        if (_freezeInputs == true)
        {
            return;
        }
        
        _moveDirection.x = Input.GetAxisRaw("Horizontal");
        _interact = Input.GetKeyDown(KeyCode.E);
    }

    private void Move(Vector2 p_direction, float p_speed)
    {
        _rigidbody2D.position += p_direction * _speed * Time.deltaTime;
    }

    private void HandleInteraction()
    {
        if(_interact == false) {return;} 
        if (_interactableObject == null) { return; }

        NpcController npc = _interactableObject.GetComponent<NpcController>();
        npc.OnInteracted();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _interactableObject = other;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_interactableObject == other)
        {
            _interactableObject = null;
        }
    }

    private void Listening()
    {
        _freezeInputs = true;
        _moveDirection = Vector3.zero;
    }

    private void FinishListening()
    {
        _freezeInputs = false;
    }
}
