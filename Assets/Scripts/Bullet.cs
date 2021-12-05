using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 발사자 정보
public enum OwnerSide : int
{
    Player = 0,
    Enemy
}
public class Bullet : MonoBehaviour
{
    const float LifeTime = 15.0f;

    OwnerSide ownerSide = OwnerSide.Player;

    [SerializeField]
    Vector3 MoveDirection = Vector3.zero;

    [SerializeField]
    float Speed = 0.0f;

    bool NeedMove = false;

    bool Hited = false;
    float Fired = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ProcessDisappearCondition())
        { return; }
        UpdateMove();
    }

    void UpdateMove()
    {
        if (!NeedMove)
        {
            return;
        }

        Vector3 moveVector = MoveDirection.normalized * Speed * Time.deltaTime;
        moveVector = AdjustMove(moveVector);
        transform.position += moveVector;
    }

    public void Fire(OwnerSide FireOwner, Vector3 firePosition, Vector3 direction, float speed)
    {
        ownerSide = FireOwner;
        transform.position = firePosition;
        MoveDirection = direction;
        Speed = speed;

        NeedMove = true;
        Fired = Time.time;

    }

    Vector3 AdjustMove(Vector3 moveVector)
    {
        RaycastHit hitInfo;

        if(Physics.Linecast(transform.position, transform.position + moveVector, out hitInfo))
        {
            Debug.Log("Hit");
            moveVector = hitInfo.point - transform.position;
            onBulletCollision(hitInfo.collider);
        }

        return moveVector;
    }

    void onBulletCollision(Collider collider)
    {

        if (Hited)
        {
            return;
        }

        Collider myCollider = GetComponentInChildren<Collider>();
        myCollider.enabled = false;

        Hited = true;
        NeedMove = false;

        

        if(ownerSide == OwnerSide.Player)
        {
            Enemy enemy = collider.GetComponentInParent<Enemy>();
            Debug.Log("OnBulletCollision collider = " + "Enemy");
        }
        else
        {
            Player player = collider.GetComponentInParent<Player>();
            Debug.Log("OnBulletCollision collider = "  +"Player");

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OntriggerEnter" + other.name);
        onBulletCollision(other);
    }

    bool ProcessDisappearCondition()
    {
        if(transform.position.x > 15.0f || transform.position.x < -15.0f
            || transform.position.y > 15.0f || transform.position.y < -15.0f
            )
        {
            Disappear();
            return true;
        }
        else if(Time.time - Fired > LifeTime)
        {
            Disappear();
            return true;
        }

        return false;
    }

    void Disappear()
    {
        Destroy(gameObject);
    }
}
