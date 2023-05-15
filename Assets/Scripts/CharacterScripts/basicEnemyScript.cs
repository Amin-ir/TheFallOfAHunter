using System.Linq;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class basicEnemyScript : MonoBehaviour
{
    public bool AllowedToMove = false;
    public GameObject Arrow;
    public Transform ShootPosition;
    enemyCommonScript _CommonProperties;
    Animator _Animator;
    Rigidbody2D _RigidBody;
    PlayerBehaviour _Player;
    PlayerFightSystem _PlayerFightScript;
    GameObject detector;
    void Start()
    {
        detector = transform.Find("detector").gameObject;
        _Animator = GetComponent<Animator>();
        _CommonProperties = GetComponent<enemyCommonScript>();
        _Player = FindObjectOfType<PlayerBehaviour>();
        _PlayerFightScript = _Player.GetComponent<PlayerFightSystem>();
        _RigidBody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (_CommonProperties.Awake && _CommonProperties.health > 0)
        {
            var DistanceToPlayer = Mathf.Abs(transform.position.x - _Player.transform.position.x);

            if (DistanceToPlayer > _CommonProperties.DistanceToAttack)
                if (AllowedToMove)
                    _RigidBody.velocity = new Vector2(_CommonProperties.Speed, 0);
                else _RigidBody.velocity = Vector2.zero;
            else
                _Animator.SetBool("hit", true);

        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            {
                _Animator.SetBool("alert", true);
                detector.SetActive(false);
            }

        if(col.CompareTag("weapon"))
        {
            _CommonProperties.Awake = true;
            if (_CommonProperties.GetThrown)
            {
                _Animator.Play("getThrown");
                _CommonProperties.GetThrown = false;
            }
            else
            {
                _CommonProperties.health -= _CommonProperties.damageTaken;
                _PlayerFightScript.stamina += _CommonProperties.damageTaken;
                if (_PlayerFightScript.stamina >= _PlayerFightScript.maxStamina)
                    FindObjectsOfType<ButtonHandler>().Where(o => o.gameObject.name == "SpecialAttack")
                        .FirstOrDefault().gameObject.SetActive(true);
                if (_CommonProperties.health <= 0)
                    _Animator.SetBool("die", true);
                _Animator.SetBool("getHit", true);
            }
        }
    }
    void Move()
    {
        _Animator.SetBool("hit", false);
        _Animator.SetBool("getHit", false);
        AllowedToMove = true;
        _CommonProperties.Awake = true;
    }
    void Stop()
    {
        AllowedToMove = false;
        _RigidBody.velocity = Vector2.zero;
    }
    void Die()
    {
        _CommonProperties.health = 0;
        Stop();
    }
    void Deactive()
    {
        var Xp = transform.Find("xp");
        Xp.parent = null;
        Xp.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    void Shoot()
    {
        var arrow = Instantiate(Arrow, ShootPosition.position, Quaternion.identity);
    }
}