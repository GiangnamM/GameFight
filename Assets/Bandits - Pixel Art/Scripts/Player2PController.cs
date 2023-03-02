using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2PController : MonoBehaviour
{
    


    //Health
    [SerializeField] private int m_maxHealth = 100;
    private int m_currentHealth;
    public HeartBar m_healthBar;



    // Movement2
    [SerializeField] private float m_Movespeed = 8f;
    [SerializeField] private float m_JumpForce = 16f;
    [SerializeField] private Rigidbody2D m_rb;

    private float m_horizontal;
    private bool m_IsGrounded;
    private bool m_isFacingRight = false;
    [SerializeField] private Animator m_Anim;

    // Check IsGround
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private LayerMask m_GroundLayer;


    // Attack
    [SerializeField] private float m_AttackTimeDelay = 0.5f;
    [SerializeField] private float m_AttackTime = 1f;
    [SerializeField] private bool m_IsAttack;
    [SerializeField] private int m_AttackDamage = 40;

    // Check Enemy
    public Transform m_attackPoint;
    public float m_attackRange = 0.5f;
    public LayerMask m_enemyLayers;

    // Show GameplayPanel

    [SerializeField] private GameObject m_GameplayPanel;
    

    // Start is called before the first frame update
    void Start()
    {

        m_Anim = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody2D>();
        m_currentHealth = m_maxHealth;

        m_healthBar.SetMaxHealth(m_maxHealth);
        m_healthBar.SetHealth(m_currentHealth);
        

    }

    
    private void FixedUpdate()
    {
           
        m_AttackTime -= Time.fixedDeltaTime;
        P2Movement();
        Flip();

    }
    private void Flip()
    {

        if (m_isFacingRight && m_horizontal < 0f || !m_isFacingRight && m_horizontal > 0f)
        {

            m_isFacingRight = !m_isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

    }
    private void P2Movement()
    {
        m_horizontal = Input.GetAxisRaw("Horizontal2");

        if (m_AttackTime > 0)
        {
            m_IsAttack = true;
        }
        else
        {
            m_IsAttack = false;
        }
        if (m_IsAttack == true)
        {
            m_horizontal = 0;
        }

        // Attack
        if (Input.GetKey(KeyCode.Keypad1) && GroundCheck() && m_IsAttack == false)
        {
            Attack2P();
            m_AttackTime = m_AttackTimeDelay;
            m_IsAttack = true;
        } // Jump
        else if (Input.GetKey(KeyCode.Keypad2) && GroundCheck())
        {
            m_rb.velocity = new Vector2(m_rb.velocity.x, m_JumpForce);
            m_Anim.Play("Jump");
            m_Anim.SetBool("Grounded", false);

        } // Run
        else if (m_horizontal != 0 && GroundCheck())
        {
            m_Anim.SetBool("Grounded", true);
            m_Anim.SetInteger("AnimState", 2);
        } // Idle
        else if (m_horizontal == 0 && GroundCheck())
        {
            m_Anim.SetBool("Grounded", true);
            m_Anim.SetInteger("AnimState", 0);
        }
        m_rb.velocity = new Vector2(m_horizontal * m_Movespeed, m_rb.velocity.y);

    }

    private void Attack2P()
    {
        m_Anim.SetTrigger("Attack");

        Collider2D[] HitEnemies = Physics2D.OverlapCircleAll(m_attackPoint.position, m_attackRange, m_enemyLayers);

        // Phat hien Enemy trong pham vi
        foreach (Collider2D enemy in HitEnemies)
        {
            enemy.GetComponent<Player2PController>().TakeDamage(m_AttackDamage);
            Debug.Log("We hit" + enemy.name);
        }
    }
    private bool GroundCheck()
    {
        return Physics2D.OverlapCircle(m_GroundCheck.position, 0.2f, m_GroundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        if (m_attackPoint == null)
            return;
        Gizmos.DrawWireSphere(m_attackPoint.position, m_attackRange);
    }


    //2P

    public void TakeDamage(int damage)
    {
        m_currentHealth -= damage;
        m_healthBar.SetHealth(m_currentHealth);

        // Play hurt Animation
        m_Anim.SetTrigger("Hurt");

        if (m_currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        m_Anim.SetTrigger("Death");
        Debug.Log("Player Die");

        //GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        AfterPlayerDie();
        m_GameplayPanel.SetActive(true);

    }
    private IEnumerator AfterPlayerDie()
    {
       GameManager.Instance.ShowWin(1);
       yield return new WaitForSeconds(3f);
    }

}
