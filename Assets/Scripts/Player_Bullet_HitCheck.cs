using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Bullet_HitCheck : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            BulletDestroy();
        }

        if(collision.gameObject.tag == "Enemy")
        {
            BulletDestroy();
        }
    }

    void BulletDestroy()
    {
        //着弾時のエフェクト生成
        Instantiate(EffectManager.Instance.Bullet_Hit, transform.position, transform.rotation);

        //自分を消去
        Destroy(gameObject);
    }
    

}
