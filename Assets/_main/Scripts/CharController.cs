using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharController : MonoBehaviour
{
    [Header("References")]
    public Animator anim;
    public Rigidbody2D rig;
    public ParticleSystem deathVFX;
    public SpriteRenderer spriteRend;
    public Transform sheepPivot;

    [Header("Stats")]
    public float movSpeed;
    public float jumpForce;
    public int maxHP;

    private int m_currentHP;

    private float m_movInput;
    private float m_currentSpeed;
    private bool m_jump = false;
    private bool m_jumping = false;

    private Vector3 spawnPos;

    private List<GameObject> m_sheepCollection = new List<GameObject>();
    private PortalBehaviour portalBehaviour;

    private void OnCollisionEnter2D(Collision2D _col)
    {
        if (_col.gameObject.CompareTag("HurtBox"))
        {
            ReceiveDamage(_col.gameObject.GetComponent<HurtBox>().damage);
        }
    }

    private void OnTriggerStay2D(Collider2D _col)
    {
        if (_col.gameObject.CompareTag("Floor") && m_jumping)
        {
            Debug.Log("found floor");
            anim.SetBool("Jumping", false);
            m_jumping = false;
        }

        if (_col.gameObject.CompareTag("Sheep"))
        {
            if (!m_sheepCollection.Contains(_col.gameObject))
            {
                _col.gameObject.GetComponent<Rigidbody2D>().simulated = false;
                _col.gameObject.transform.parent = sheepPivot;
                _col.gameObject.transform.localPosition = new Vector3(0, 3f * m_sheepCollection.Count, 0);
                m_sheepCollection.Add(_col.gameObject);
            }
        }

        if (_col.gameObject.CompareTag("Portal"))
        {
            portalBehaviour = _col.gameObject.GetComponent<PortalBehaviour>();
            StartCoroutine("DropSheep");
        }
    }

    private void Start()
    {
        spawnPos = transform.position;
        m_currentHP = maxHP;
    }

    void Update()
    {
        // Input
        m_movInput = Input.GetAxis("Horizontal");
        m_jump = Input.GetButtonDown("Jump");

        spriteRend.flipX = m_movInput < 0;

        // Movement and Animation triggers
        m_currentSpeed = m_movInput * movSpeed;

        anim.SetBool("Moving", m_movInput != 0);

        if (!m_jumping && m_jump)
        {
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetBool("Jumping", true);
            anim.SetTrigger("JumpStart");
            m_jumping = true;
        }

        Vector2 newVel = new Vector2(m_currentSpeed, rig.velocity.y);
        rig.velocity = newVel;
    }

    public void ReceiveDamage(int _dmg)
    {
        m_currentHP -= _dmg;
        if(m_currentHP <= 0)
        {
            Death();
            m_currentHP = 0;
        }
    }

    private void Death()
    {
        rig.simulated = false;
        spriteRend.enabled = false;
        deathVFX.Play();
        Invoke("Respawn", 3f);
    }

    private void Respawn()
    {
        m_currentHP = maxHP;
        transform.position = spawnPos;
        rig.simulated = true;
        spriteRend.enabled = true;
    }

    public IEnumerator DropSheep()
    {
        if(m_sheepCollection.Count > 0)
        {
            rig.simulated = false;

            m_sheepCollection.Last().transform.parent = null;
            m_sheepCollection.Last().transform.position = portalBehaviour.transform.position;
            portalBehaviour.linkedSheep = m_sheepCollection.Last();
            m_sheepCollection.Remove(m_sheepCollection.Last());
            portalBehaviour.TriggerTP();

            yield return new WaitForSeconds(0.5f);

            StartCoroutine("DropSheep");
        } else
        {
            rig.simulated = true;
        }
    }
}
