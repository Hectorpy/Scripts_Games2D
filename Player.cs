using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Animator anim;
    public float speed;
    public Sprite doctorSprite;

    private Rigidbody2D rb;
    private Vector2 movement;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.magnitude);
    }

    void FixedUpdate()
    {
        Vector2 newPosition = rb.position + movement * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }
}
