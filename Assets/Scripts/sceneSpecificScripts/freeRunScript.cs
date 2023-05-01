using Assets.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class freeRunScript : MonoBehaviour {

	Animator anim;
	Rigidbody2D rgd;
	public float speed = 5f, jump = 5f, pushBackForce = -5f, health, maxHealth = 3f;
	public bool toMove = true,onGround = true;
	public GameObject healthUI;
	float healthBarInitialWidth;
	RectTransform healthBar;
	Vector2 startTouchPoint, endTouchPoint;
	SwipeDirection direction;

	void Start () {
		health = maxHealth;
		anim = GetComponent<Animator>();
		rgd = GetComponent<Rigidbody2D>();
        healthBar = healthUI.transform as RectTransform;
		healthBarInitialWidth = healthBar.rect.width;
	}
	void Update () {

		if (toMove)
		{
			if (Input.GetMouseButtonDown(0))
			{
				startTouchPoint = Input.mousePosition;
			}
			else if (Input.GetMouseButtonUp(0))
			{
				endTouchPoint = Input.mousePosition;
				direction = CustomTools.DetectSwipe(startTouchPoint, endTouchPoint);
			}
			
			rgd.velocity = new Vector2(speed, rgd.velocity.y);
			
			if (direction == SwipeDirection.up && onGround)
				rgd.velocity = new Vector2(rgd.velocity.x, jump);
			if (direction == SwipeDirection.down && onGround)
				anim.Play("slide");
			direction = SwipeDirection.none;
		}
	}
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.CompareTag("Surface"))
		{
			anim.SetBool("onGround", true);
			onGround = true;
		}
		if (col.gameObject.CompareTag("obstacle"))
		{
			anim.Play("failObstacle");
			rgd.AddForce(new Vector2(pushBackForce,0));
			if (--health < 0)
				StartCoroutine(death());
			healthBar.sizeDelta = new Vector2((health / maxHealth) * healthBarInitialWidth, healthBar.sizeDelta.y);
		}
	}
	void OnCollisionExit2D(Collision2D col)
    {
		if (col.gameObject.CompareTag("Surface"))
        {
			anim.SetBool("onGround", false);
			onGround = false;
		}
	}
	void OnTriggerStay2D(Collider2D col)
	{
		if (col.CompareTag("obstacle") && rgd.velocity.y > 0 && onGround)
			anim.Play("climbObstacle");
    }
	public void notMove()
    {
		toMove = false;
    }
	public void move()
    {
		if(health >= 0)
			toMove = true;
    }
	IEnumerator death()
    {
		anim.SetBool("failed", true);
		notMove();
		rgd.velocity = Vector2.zero;
		FindObjectOfType<blackCurtainScript>().unFade();
		yield return new WaitForSeconds(7);
		var currentLevel = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(currentLevel);
	}
}
