using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller_B : MonoBehaviour
{
    //オブジェクトのRigidbody用の変数
    [System.NonSerialized] public Rigidbody P_Rig;

    //無敵状態（ノックバック中）
    [System.NonSerialized] public bool Invincible;
    
    //移動に関する変数
    public float AcceleSpeed = 200; //加速度
    public float MaxSpeed    =  20; //最高速度
    public float BrakePower  =  10; //停止時の減速
    public float JumpPower 　=  80; //ジャンプ力
    public float AirResist   =  10; //空中での移動抵抗
    public float Gravity     =  70; //落下速度

    //インプットの値を代入する変数
    float h1;
    float v1;

    //移動の力を代入
    Vector3 MoveDirection;

    //ジャンプ中にボタンを離したかのフラグ
    bool JumpFlag;

    /// カメラの水平回転を参照する用
    Vector3 cameraForward;
    Vector3 moveForward;

    //地面に設置しているかどうかをチェックする子オブジェクトのスクリプト用の変数
    Player_JumpCheck P_JumpCheck;

    //弾丸のスポーンポイントである子オブジェクトのスクリプト用の変数
    Player_BulletSpawn P_BulletSpawn;

    //攻撃時のアニメーション用のフラグ
    bool AttackFLG;

    //Animation
    Animator animator;

    //Rayの可視化用　距離（Debug用）
    public float DebugDrawRayDistance = 200f;
    
    //Rayと交差する地面と距離
    Plane plane = new Plane();
    float distance = 0;


    void Start()
    {
        //オブジェクトのRigidbodyをGetComponentで取得
        P_Rig = GetComponent<Rigidbody>();

        //オブジェクトのAnimatorコンポーネントを取得
        animator = GetComponent<Animator>();

        //Playerの子オブジェクトであるJumpCheckerからスクリプトを取得
        //GetChild()の()内の数値は子オブジェクトの順番(一番上から0,1,2と番号が割り振られている)
        P_JumpCheck = transform.GetChild(0).gameObject.GetComponent<Player_JumpCheck>();

        //Playerの子オブジェクトであるBulletSpawnからスクリプトを取得
        P_BulletSpawn = transform.GetChild(1).gameObject.GetComponent<Player_BulletSpawn>();

    }

    void Update()
    {
        //アニメーションを切り替え
        animator.SetFloat("Walk", P_Rig.velocity.magnitude);    //歩きモーション
        animator.SetBool("Jump", !P_JumpCheck.isGround);        //ジャンプモーション
        animator.SetBool("Damage", Invincible);                 //ダメージモーション
        animator.SetBool("Attack", AttackFLG);                  //攻撃モーション（別レイヤー）

        //MainGame中のみ操作可能
        if (GameManager.Instance.MainPlay == true)
        {
            //無敵状態（ノックバック中）ではない時のみ入力が可能
            if (Invincible == false)
            {
                //Edit => InputManagerに設定されている入力を変数に代入
                h1 = Input.GetAxis("Horizontal");
                v1 = Input.GetAxis("Vertical");

                //カメラの向きに合わせて進む方向を変えたい
                //カメラの方向から、X-Z平面の単位ベクトルを取得
                cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

                //方向キーの入力値とカメラの向きから、移動方向を決定
                moveForward = cameraForward * v1 + Camera.main.transform.right * h1;

                //Playerの向きをマウスカーソルの方へ向ける処理へ
                LookAtMouse();

                //↓キー入力処理

                //ボタンを押したら弾丸を発射
                if (Input.GetButton("Fire1"))
                {
                    //攻撃時のフラグ
                    AttackFLG = true;

                    //Player_BulletSpawnのBulletSpawnメソッドを実行
                    P_BulletSpawn.BulletSpawn();
                }
                else
                {
                    //攻撃時のフラグ
                    AttackFLG = false;
                }

                //ボタンを押したら弾丸をリロード
                if (Input.GetButtonDown("Fire2"))
                {
                    //Player_BulletSpawnのBulletReloadメソッドを実行
                    P_BulletSpawn.BulletReload();

                    //リロードのSE
                    SoundManager.Instance.PlayGameSE(2);
                }

                //ボタンを押したらジャンプ
                if (Input.GetButtonDown("Jump"))
                {
                    Jump(); //ジャンプ処理へ
                }
            }
        }
    }


    void FixedUpdate()
    {
        //MainGame中
        if (GameManager.Instance.MainPlay == true)
        {
            //無敵状態（ノックバック中）ではない時
            if (Invincible == false)
            {
                //地上に立っている時
                if (P_JumpCheck.isGround == true)
                {
                    //MaxSpeed未満の時はAddForceで徐々に加速していく
                    if (P_Rig.velocity.magnitude < MaxSpeed)
                    {
                        //移動方向にスピードを掛ける
                        MoveDirection = moveForward * AcceleSpeed;
                        P_Rig.AddForce(MoveDirection);
                    }

                    //移動方向とは反対方向に速度を加え減速
                    P_Rig.AddForce(BrakePower * (moveForward - P_Rig.velocity));
                }
                //空中にいる時
                else
                {
                    //移動方向にスピードを掛ける
                    MoveDirection = moveForward * AcceleSpeed / AirResist;
                    P_Rig.AddForce(MoveDirection);

                    //引力をより強く
                    P_Rig.AddForce(-transform.up * Gravity);

                    //ジャンプボタンを押す長さでジャンプの高度を変えられるように.
                    //空中でジャンプボタンを離したらそこでジャンプ終了
                    if (!Input.GetButton("Jump") && JumpFlag == true)
                    {
                        if (P_Rig.velocity.y >= 0)
                        {
                            P_Rig.velocity = new Vector3(P_Rig.velocity.x, 0, P_Rig.velocity.z);
                        }
                        JumpFlag = false;
                    }
                }
            }
        }
        else
        {
            //デモ演出中地上にいたら物理演算で動かない
            if (P_JumpCheck.isGround == true)
            {
                //停止
                P_Rig.velocity = Vector3.zero;
            }
            else
            {
                //引力をより強く
                P_Rig.AddForce(-transform.up * Gravity);
            }
        }
    }


    //ジャンプ処理
    public void Jump()
    {
        //地面に立っている時のみ
        if (P_JumpCheck.isGround == true)
        {
            //ForceMode.Impulseで瞬発的に力を加える
            P_Rig.AddForce(transform.up * JumpPower, ForceMode.Impulse);

            //ジャンプフラグをON
            JumpFlag = true;

            //ジャンプエフェクト生成
            Instantiate(EffectManager.Instance.Player_Jump,
                new Vector3(transform.position.x, transform.position.y - 0.4f, transform.position.z), transform.rotation);
        }
    }


    //Playerの向きをマウスカーソルの方へ向ける処理
    public void LookAtMouse()
    {
        //メインカメラからマウスの位置に対しての光線（Ray）を飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //プレイヤーの高さにPlaneを更新して、カメラの情報を元に地面判定して距離を取得
        plane.SetNormalAndPosition(Vector3.up, transform.position);
        if (plane.Raycast(ray, out distance))
        {
            //距離を元にRayと地面の交点を算出し、交点の方を向く
            Vector3 lookPoint = ray.GetPoint(distance);
            transform.LookAt(lookPoint);
        }

        //Rayの可視化(シーンビューでRayを確認できる)
        Debug.DrawRay(ray.origin, ray.direction * DebugDrawRayDistance, Color.green, 5, false);
    }


    public void Player_Walk_SE()
    {
        //足音のSE（Animationで鳴らす）
        SoundManager.Instance.PlayGameSE(7);
    }
}