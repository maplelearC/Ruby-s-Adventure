using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rigidbody2d;
    public int maxHealth = 5;//最大生命值
    private int currentHealth;//当前生命值
    public int speed = 10;//Ruby的速度
    public int Health { get { return currentHealth; } }

    //Ruby的无敌时间
    public float timeInvincible = 2.0f;
    private bool isInvincible;
    private float isInvincibleTimer;//计时器

    private Vector2 lookDirection = new Vector2(1, 0);
    private Animator animator;

    public GameObject projectilePrefab;//子弹预制体

    public AudioSource audioSource;

    public AudioSource WalkAudioSouce;
    public AudioClip PlayerHit;
    public AudioClip attackSoundClip;

    public AudioClip WalkSound;

    private Vector3 respawnPosition;
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        //audioSource = GetComponent<AudioSource>();
        respawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //玩家输入监听
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);
        //当前玩家输入的某个轴向值不为0
        if (!Mathf.Approximately(move.x, 0) || !Mathf.Approximately(move.y, 0))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
            if (!WalkAudioSouce.isPlaying)
            {
                WalkAudioSouce.Play();
                WalkAudioSouce.clip = WalkSound;
            }

        }
        else
        {
            WalkAudioSouce.Stop();
        }

        //动画控制
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        //移动
        Vector2 position = transform.position;
        //Ruby的水平移动
        // position.x = position.x + speed * horizontal * Time.deltaTime;
        //Ruby的垂直移动
        // position.y = position.y + speed * vertical * Time.deltaTime;

        position = position + speed * move * Time.deltaTime;
        //transform.position = position;
        rigidbody2d.MovePosition(position);

        //无敌时间
        if (isInvincible)
        {
            isInvincibleTimer -= Time.deltaTime;
            if (isInvincibleTimer <= 0)
            {
                isInvincible = false;
            }
        }

        //攻击方法
        if (Input.GetKeyDown(KeyCode.H))
        {
            Launch();
        }
        //检测是否与NPC对话
        if (Input.GetKeyDown(KeyCode.T))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NPCDialog npcDialog = hit.collider.GetComponent<NPCDialog>();
                if (npcDialog != null)
                {
                    npcDialog.DisplayDialog();
                }
            }
        }
    }

    public void ChangeHealth(int amount)//生命值的变化
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            //收到伤害
            isInvincible = true;
            isInvincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
            PlaySound(PlayerHit);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (currentHealth <= 0)
        {
            Respawn();
        }
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    public void Launch()
    {
        if (UIHealthBar.instance.hasTask == false)
        {
            return;
        }
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        projectile.Launch(lookDirection, 300);
        animator.SetTrigger("Launch");
        PlaySound(attackSoundClip);
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
    private void Respawn()
    {
        ChangeHealth(maxHealth);
        transform.position = respawnPosition;
    }
}
