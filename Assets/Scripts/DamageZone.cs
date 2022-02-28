using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        RubyController rubyController = collision.GetComponent<RubyController>();
        if (rubyController != null)//判断是否有RubyController脚本
        {
            rubyController.ChangeHealth(-1);
            //Debug.Log("Health: " + rubyController.Health);
        }
    }
}
