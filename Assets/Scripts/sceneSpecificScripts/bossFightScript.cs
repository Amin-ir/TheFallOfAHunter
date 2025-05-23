﻿using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;

public class bossFightScript : MonoBehaviour {

    public bool allowedToMove = true, stunned = false, IsWaiting = false;
    public int angerLevel = 1, SecondsToWaitBetweenRangedAttacks = 2;
    public float movementSpeed = 10f, health, maxHealth = 100f, 
        distanceToAttackShortRange = 30f, distanceToAttackMidRange = 50f, distanceToAttackLongRange = 100f, aggressiveProtection = 100f;
    public GameObject platformFor3rdAngerObstacles;
    enemyCommonScript _commonScript;
    Animator _animator;
    PlayerFightSystem _player;
    PlayerBehaviour _playerBehaviour;
    Rigidbody2D _rigidBody;
    Vector2 healthUIInitialWidth;
    Vector3 mainCameraInitialPosition;
    float cameraInitialFieldOfView;

    public GameObject axeClone, waveClone, lightningClone, knifeClone;
    public Image healthUI;
    public Transform axeInstantiationPosition, waveInstantiationPosition, lightningStartRange, lightningEndRange, knifeInstantiationPosition;

    float[] playerSpecialAttacks = new float[2];
    float distanceBetweenPlayerAndBoss = 0;

    void Start()
    {
        _commonScript = GetComponent<enemyCommonScript>();
        _animator = GetComponent<Animator>();
        _player = FindObjectOfType<PlayerFightSystem>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerBehaviour = _player.gameObject.GetComponent<PlayerBehaviour>();
        platformFor3rdAngerObstacles = GameObject.Find("3rdAngerObstacles");
        platformFor3rdAngerObstacles.SetActive(false);
        healthUIInitialWidth = healthUI.rectTransform.localScale;
        mainCameraInitialPosition = Camera.main.transform.position;
        cameraInitialFieldOfView = Camera.main.fieldOfView;

        health = maxHealth;
        playerSpecialAttacks = new float[] { _player.stormDamage, _player.bombDamage };
    }
    void Update()
    {
        if (!stunned && allowedToMove && !IsWaiting)
        {
            _animator.SetFloat("speed", Mathf.Abs(movementSpeed));
            distanceBetweenPlayerAndBoss = Mathf.Abs(_player.transform.position.x - transform.position.x);
            switch (angerLevel)
            {
                case 1:
                    fightEasy();
                    break;
                case 2:
                    fightMedium();
                    break;
                case 3:
                    fightAggressive();
                    break;
            }
        }
    }
    void fightEasy()
    {
        if (distanceBetweenPlayerAndBoss > distanceToAttackShortRange)
            getVelocity();
        else _animator.Play("swordCloseHit");
    }
    void fightMedium()
    {
        if (distanceBetweenPlayerAndBoss <= distanceToAttackShortRange)
            _animator.Play("swordCloseHit");
        else if (distanceBetweenPlayerAndBoss <= distanceToAttackMidRange)
        {
            var randomNumber = Random.Range(1, 3);
            chooseFromAttackList(randomNumber);
        }
        else getVelocity();
    }
    void fightAggressive()
    {
        if (distanceBetweenPlayerAndBoss <= distanceToAttackShortRange)
            _animator.Play("swordCloseHit");
        else if (distanceBetweenPlayerAndBoss <= distanceToAttackMidRange)
        {
            var randomNumber = Random.Range(1, 3);
            chooseFromAttackList(randomNumber);
        }
        else if (distanceBetweenPlayerAndBoss <= distanceToAttackLongRange)
        {
            var randomNumber = Random.Range(4, 6);
            chooseFromAttackList(randomNumber);
        }
        else getVelocity();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("weapon"))
            switch (angerLevel)
            {
                case 1:
                    if (!playerSpecialAttacks.Contains(_commonScript.damageTaken))
                    {
                        _animator.Play("defense");
                        _player.stamina += _commonScript.damageTaken;
                    }
                    else getStunned();
                    break;
                case 2:
                    if (other.gameObject.GetComponent<ropeScript>() != null)
                        {
                            getStunned();
                            FindObjectOfType<ropeScript>().CloneSpikeyBall();
                        }
                    else defeatThrowables(other.gameObject);
                    break;
                case 3:
                    if (aggressiveProtection > 0)
                    {
                        aggressiveProtection -= _commonScript.damageTaken;
                        _animator.Play("defense");
                    }
                    else getStunned();
                    break;
            }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _playerBehaviour._PlayerRigidBody.velocity == Vector2.zero 
            && _player.playerStyle == 0 && stunned)
        {
            _player.gameObject.SetActive(false);
            getCameraOnBoss();
            switch (angerLevel)
            {
                case 1:
                    _animator.Play("interact1");
                    break;
                case 2:
                    _animator.Play("interact2");
                    break;
                case 3:
                    _animator.Play("interact3");
                    break;
            }
        }
    }
    void getCameraOnBoss()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, mainCameraInitialPosition.z);
        Camera.main.fieldOfView = 125f;
    }
    public void releaseCamera()
    {
        Camera.main.transform.position = mainCameraInitialPosition;
        Camera.main.fieldOfView = cameraInitialFieldOfView;
    }
    void getStunned()
    {
        stunned = true;
        _animator.Play("stunned");
    }
    void defeatThrowables(GameObject obj)
    {
        obj.SetActive(false);
        if (obj.GetComponent<cloneAxeScript>() != null)
            _animator.Play("throwAxe");
        else if (obj.GetComponent<playerArrowScript>() != null)
            _animator.Play("takeArrows");
        else _animator.Play("defense");
    }
    public void dontMove()
    {
        allowedToMove = false;
        _rigidBody.velocity = Vector2.zero;
    }
    public void move()
    {
        if(stunned)
            aggressiveProtection = 100;
        stunned = false;
        allowedToMove = true;
    }
    public void reactivatePlayer()
    {
        _player.gameObject.SetActive(true);
    }
    public void getVelocity()
    {
        if (_player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            movementSpeed = Mathf.Abs(movementSpeed);
        }
        else 
        { 
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
            movementSpeed = -Mathf.Abs(movementSpeed);
        }
        _rigidBody.velocity = new Vector2(movementSpeed, 0);
    }
    void chooseFromAttackList(int attackNumber)
    {
        switch (attackNumber)
        {
            case 1:
                _animator.Play("throwKnife");
                break;
            case 2:
                _animator.Play("wave");
                break;
            case 3:
                _animator.Play("lightningAttack");
                break;
            case 4:
                _animator.Play("lightningAttackAbove");
                break;
        }
        IsWaiting = true;
        _animator.SetFloat("speed",0);
        StartCoroutine(WaitAfterAttack());
    }
    public void throwAxe()
    {
        var axe = Instantiate(axeClone, axeInstantiationPosition.position, Quaternion.identity);
    }
    public void throwKnife()
    {
        var knife = Instantiate(knifeClone, knifeInstantiationPosition.position, Quaternion.identity);
    }
    public void createWave()
    {
        var wave = Instantiate(waveClone, waveInstantiationPosition.position, Quaternion.identity);
    }
    public void createVerticalLightning()
    {
        for (int i = 0; i < 3; i++)
        {
            var randomHorizontalPosition = Random.Range(lightningStartRange.position.x, lightningEndRange.position.x);
            Vector3 randomPosition = new Vector3(randomHorizontalPosition, transform.position.y, transform.position.z);
            var lightning = Instantiate(lightningClone, randomPosition, Quaternion.identity);
        }
    }
    public void decreaseHealth()
    {
        angerLevel++;
        _player.currentHealth += _player.maxHealth / 2;
        if(_player.currentHealth > _player.maxHealth)
            _player.currentHealth = _player.maxHealth;
        if(angerLevel == 2)
        {
            platformFor3rdAngerObstacles.SetActive(true);
        }
        else platformFor3rdAngerObstacles.SetActive(false);
        health -= 0.3f * maxHealth;
        healthUI.rectTransform.localScale = new Vector2((health / maxHealth) * healthUIInitialWidth.x, healthUIInitialWidth.y);
    }
    IEnumerator WaitAfterAttack(){
        yield return new WaitForSeconds(SecondsToWaitBetweenRangedAttacks);
        IsWaiting = false;
    }
    void LoadNextScene(){
		CustomTools.FetchNextLevelInstantly();
    }
}
