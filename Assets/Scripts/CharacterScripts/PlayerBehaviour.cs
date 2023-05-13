using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerBehaviour : MonoBehaviour {
    
    public int armorUpgradeTemp = 0, swordUpgradeTemp = 0, axeUpgradeTemp = 0, xp = 0;
    public Rigidbody2D _PlayerRigidBody;
    public float speed = 25f, jumpHeight = 50f;
    public Animator playerAnimator;
    public PlayerFightSystem playerScr;
    public bool idol = true, onSurface = true,allowToMove = true;
    public Text xpUI;
    public GameObject injury1, injury2, injury3, injury4;
    public float climbX,climbY;
    public Transform BombCloudInstantiationPosition, StormCloudInstantiatePosition;
    public GameObject BombCloud, StormCloud;
    Vector2 touchStartPoint, touchEndPoint;

    //Initialization & retrieving PlayerPrefs values
    public void Start () {

        playerScr = GetComponent<PlayerFightSystem>();
        playerAnimator = GetComponent<Animator>();

        if (PlayerPrefs.GetInt("checkpoint") != 0)
        {
            var checkpointPosition = FindObjectsOfType<checkpointScript>().
                 Where(x => x.id == PlayerPrefs.GetInt("checkpoint")).FirstOrDefault().
                     transform.position;
            transform.position = new Vector3(checkpointPosition.x, checkpointPosition.y,transform.position.z);
            foreach (var item in FindObjectsOfType<enemyCommonScript>())
            {
                if (item.transform.position.x < checkpointPosition.x)
                    item.gameObject.SetActive(false);
            }
        }

        if (PlayerPrefs.HasKey("xp"))
            xp = PlayerPrefs.GetInt("xp");
        _PlayerRigidBody = GetComponent<Rigidbody2D>();
        xpUI.text = xp + " X ";
    }
    public void Update() {
        xpUI.text = xp + " X ";

        playerAnimator.SetBool("idol", idol);
        playerAnimator.SetFloat("verticalVelocity",_PlayerRigidBody.velocity.y);
        playerAnimator.SetBool("onSurface",onSurface);

        if (Mathf.Sign(_PlayerRigidBody.transform.localScale.x * _PlayerRigidBody.velocity.x) == -1)
            _PlayerRigidBody.transform.localScale = new Vector3(-_PlayerRigidBody.transform.localScale.x, _PlayerRigidBody.transform.localScale.y, _PlayerRigidBody.transform.localScale.z);

        if(allowToMove){

            _PlayerRigidBody.velocity = new Vector2
                (CrossPlatformInputManager.GetAxis("Horizontal") * speed, _PlayerRigidBody.velocity.y);

            if (CrossPlatformInputManager.GetButtonDown("Jump") && onSurface)
            {
                _PlayerRigidBody.velocity = new Vector2
                    (_PlayerRigidBody.velocity.x,jumpHeight);
            }

            if (_PlayerRigidBody.velocity.x == 0)
                idol = true;
            else
                idol = false;
        }
        matchHealthWithUI();
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Surface"))
            onSurface = true;
    }
    void OnTriggerStay2D(Collider2D col){
        if (col.gameObject.CompareTag("edge") && _PlayerRigidBody.velocity.y > 0)
        {
            playerAnimator.Play("climb");
            allowToMove = false;
            _PlayerRigidBody.velocity = Vector2.zero;
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.transform.CompareTag("Surface"))
            onSurface = false;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        onSurface = (collision.gameObject.CompareTag("Surface"));
    }
    void matchHealthWithUI()
    {
        if (playerScr.currentHealth != playerScr.maxHealth)
        {
            if (playerScr.currentHealth >= 0.75 * playerScr.maxHealth)
            {
                injury1.SetActive(false);
                injury2.SetActive(false);
                injury3.SetActive(false);
                injury4.SetActive(true);
            }
            else if (playerScr.currentHealth < 0.75 * playerScr.maxHealth && playerScr.currentHealth >= 0.5 * playerScr.maxHealth)
            {
                injury1.SetActive(false);
                injury2.SetActive(false);
                injury3.SetActive(true);
                injury4.SetActive(true);
            }
            else if (playerScr.currentHealth < 0.5 * playerScr.maxHealth && playerScr.currentHealth >= 0.25 * playerScr.maxHealth)
            {
                injury1.SetActive(false);
                injury2.SetActive(true);
                injury3.SetActive(true);
                injury4.SetActive(true);
            }
            else if (playerScr.currentHealth < 0.25 * playerScr.maxHealth && playerScr.currentHealth > 0)
            {
                injury1.SetActive(true);
                injury2.SetActive(true);
                injury3.SetActive(true);
                injury4.SetActive(true);
            }
        }
        else
        {
            injury1.SetActive(false);
            injury2.SetActive(false);
            injury3.SetActive(false);
            injury4.SetActive(false);
        }
    }
    public void climbed()
    {
        if (!allowToMove)
        {
            allowToMove = true;
            if(transform.localScale.x > 0)
                transform.position = new Vector3(transform.position.x + climbX, transform.position.y + climbY, transform.position.z);
            else transform.position = new Vector3(transform.position.x - climbX, transform.position.y + climbY, transform.position.z);
        }
        playerScr.SetAnimatorStyleParameter();
    }
    public void move() 
    {
        allowToMove = true;
        playerScr.mayPlayGetHitAnimation = true;
    }
    public void stop() 
    {
        allowToMove = false;
        _PlayerRigidBody.velocity = Vector2.zero;
        playerScr.mayPlayGetHitAnimation = false;
    }
    void SimulateBombCloud()
    {
        var cloud = Instantiate(BombCloud, BombCloudInstantiationPosition.position, Quaternion.identity);
        CheckInstantiatedObjectDirection(cloud);
    }
    void SimulateStorm()
    {
        var storm = Instantiate(StormCloud, StormCloudInstantiatePosition.position, Quaternion.identity);
        CheckInstantiatedObjectDirection(storm);
    }
    void CheckInstantiatedObjectDirection(GameObject instance)
    {
        var PlayerLocalScale = transform.localScale.x;
        instance.transform.localScale = new Vector2(Mathf.Abs(PlayerLocalScale) * Mathf.Sign(PlayerLocalScale),
            transform.localScale.y);
    }
}
