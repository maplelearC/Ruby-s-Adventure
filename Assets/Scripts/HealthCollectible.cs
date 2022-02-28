using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public AudioClip audioClip;

    public GameObject effectParticle;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RubyController rubyController = collision.GetComponent<RubyController>();
        if (rubyController != null)//判断是否有RubyController脚本
        {
            if (rubyController.Health < rubyController.maxHealth)//判断是否满血
            {
                rubyController.ChangeHealth(1);
                rubyController.PlaySound(audioClip);
                Instantiate(effectParticle, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
