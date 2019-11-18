using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : AbstractCharacter
{
    ///// PUBLIC
    public BM_Gun bm_Gun;
    public BM_Legs bm_Legs;
    public BM_StrongArm bm_StrongArm;
    public BM_Drill bm_Drill;
    public BM_Grapple bm_Grapple;
    public GameManager gameManager;

    public int credit;

    //if we want a body mod to disable user input then  we can set this to true
    //very important to return to false after
    public bool disableInputs = false;

    //should change this to be some kind of ground movement object
    public float walkSpeed = 15;
    public float friction = 0.9f;
    //should change this to be some kind of air movement object
    public float airSpeedAccel = 50f;
    public float airSpeedMax = 100;
    public float airFriction = 0.99f;

    public PhysicsMaterial2D myPhysicsMaterial;

    [System.NonSerialized]
    public int origHealth;

    ///// PRIVATE
    public Animator animator;
    private GameObject playerObject;
    private List<GameObject> interactables;
    private HUD hud;
    private PlayerSound playerSound;

    private AbstractBodyMod armOneMod, armTwoMod, legsMod;
    private List<AbstractBodyMod> unlockedBodyMods, allExistingBodyMods;
    private List<DoorKey> keyList;

    private float originalFriction = 1f;

    //Booleans
    private bool rightPressed, leftPressed, armOnePressed, armTwoPressed, legsPressed, actionPressed, crouchPressed, pausePressed,
        armOneReleased, armTwoReleased, legsReleased;
    private bool pushback, invincible;

    //Vectors
    private Vector3 originalScale;

    //---------------------------------------------------------------- START -------------------------------------------
    void Start()
    {
        playerObject = gameObject;
        originalScale = gameObject.transform.localScale;
        animator = GetComponentInChildren<Animator>();
        playerSound = GetComponentInChildren<PlayerSound>();
        origHealth = health;

        //init lists
        interactables = new List<GameObject>();
        allExistingBodyMods = new List<AbstractBodyMod>();
        unlockedBodyMods = new List<AbstractBodyMod>();
        keyList = new List<DoorKey>();

        //setup Body Mods   <----------- Set which Body Mods are loaded at game startup (Depricated, use for Debug only)
        legsMod = bm_Legs;
       // armOneMod = bm_Drill;
        //armTwoMod = bm_Grapple;
        unlockedBodyMods.Add(bm_Legs);
        //unlockedBodyMods.Add(bm_Grapple);
        //unlockedBodyMods.Add(bm_Drill);
        //unlockedBodyMods.Add(bm_Gun);

        //<------------------------- Make sure to add all Body Mods here
        allExistingBodyMods.Add(bm_Gun); 
        allExistingBodyMods.Add(bm_Legs); 
        allExistingBodyMods.Add(bm_StrongArm);
        allExistingBodyMods.Add(bm_Drill);

        //Load unlocked body mods from saved state
        foreach (AbstractBodyMod abm in allExistingBodyMods)
        {
            if (PlayerPrefs.HasKey(abm.name)) unlockedBodyMods.Add(abm);
        }

        //equip body mods last used
        foreach (AbstractBodyMod abm in unlockedBodyMods)
        {
            if (PlayerPrefs.HasKey("armOneMod")) if (PlayerPrefs.GetString("armOneMod") == abm.name) armOneMod = abm;
        }
        foreach (AbstractBodyMod abm in unlockedBodyMods)
        {
            if (PlayerPrefs.HasKey("armTwoMod")) if (PlayerPrefs.GetString("armTwoMod") == abm.name) armTwoMod = abm;
        }
        foreach (AbstractBodyMod abm in unlockedBodyMods)
        {
            if (PlayerPrefs.HasKey("legsMod")) if (PlayerPrefs.GetString("legsMod") == abm.name) legsMod = abm;
        }

        myPhysicsMaterial.friction = 1f;

        //load health & credit from saved state
        if (PlayerPrefs.HasKey("PlayerHealth")) health = PlayerPrefs.GetInt("PlayerHealth");
        if (PlayerPrefs.HasKey("PlayerCredit")) credit = PlayerPrefs.GetInt("PlayerCredit");

        hud = GameObject.Find("_HUD").GetComponent<HUD>();
        hud.InitializeHUD();
    }

    //---------------------------------------------------------------- UPDATE -------------------------------------------
    void Update()
    {
        //Get all inputs
        InputsUpdate();

        BodyModsUpdate();

        MovementUpdate();

        BasicAttackUpdate();

        if (health <= 0) {
            gameManager.ReloadLevel();
        }
    }

    //---------------------------------------------------------------- UPDATE METHODS -------------------------------------------

    private void InputsUpdate()
    {
        if(!disableInputs){
            rightPressed = inputState.GetButtonValue(Buttons.Right);
            leftPressed = inputState.GetButtonValue(Buttons.Left);
            armOnePressed = inputState.GetButtonValue(Buttons.ArmOne);
            armTwoPressed = inputState.GetButtonValue(Buttons.ArmTwo);
            legsPressed = inputState.GetButtonValue(Buttons.Legs);
            actionPressed = inputState.GetButtonValue(Buttons.Action);
            crouchPressed = inputState.GetButtonValue(Buttons.Crouch);
            pausePressed = inputState.GetButtonValue(Buttons.Pause);
        }
        else{
            rightPressed = false;
            leftPressed = false;
            armOnePressed = false;
            armTwoPressed = false;
            legsPressed = false;
            actionPressed = false;
            crouchPressed = false;
            pausePressed = false;
        }
    }

    private void BodyModsUpdate()
    {
        //TODO: if (in state that allows body mod usage) {...}
        if (actionPressed)
        {
            //do attack thing
        }

        //TODO: if (in state that allows body mod usage) {...}
        if (armOnePressed)
        {
            if (armOneMod != null)
            {
                armOneMod.EnableBodyMod();
                //Debug.Log("ArmOne");
            }
            armOneReleased = true;
        }
        else if (armOneReleased)
        {
            if (armOneMod != null)
            {
                armOneMod.DisableBodyMod();
            }
            armOneReleased = false;
        }
        if (armTwoPressed)
        {
            if (armTwoMod != null)
            {
                armTwoMod.EnableBodyMod();
                //Debug.Log("ArmTwo");
            }
            armTwoReleased = true;
        }
        else if (armTwoReleased)
        {
            if (armTwoMod != null)
            {
                armTwoMod.DisableBodyMod();
            }
            armTwoReleased = false;
        }
        if (legsPressed)
        {
            if (legsMod != null)
            {
                legsMod.EnableBodyMod();
            }
            legsReleased = true;
        }
        else if (legsReleased)
        {
            if (legsMod != null)
            {
                legsMod.DisableBodyMod();
            }
            legsReleased = false;
        }

        if (pausePressed) Debug.Log("Player Manager -> wPause Button Pressed");
    }

    private void MovementUpdate()
    {
        if (isGrounded)                                         //Ground Movement
        {
            if ((leftPressed || rightPressed) && !pushback)
            {
                animator.SetBool("run", true);
                if (leftPressed)
                { //(left)
                    body2d.velocity = new Vector2(-walkSpeed * (float)inputState.direction, body2d.velocity.y);
                }
                else
                { //(right)
                    body2d.velocity = new Vector2(walkSpeed * (float)inputState.direction, body2d.velocity.y);
                }
            }
            else
            {
                body2d.velocity = new Vector2(body2d.velocity.x * friction, body2d.velocity.y);
                animator.SetBool("run", false);
            }
        }
        else
        {                                                       //Air Movement
            if ((leftPressed || rightPressed) && !pushback)
            {
                /*
                int accelMultiplier = 1;
                if (leftPressed)
                {
                    accelMultiplier = -1;
                }
                var tmpSpeed = body2d.velocity.x + (airSpeedAccel * accelMultiplier);
                /*
                if (Mathf.Abs(tmpSpeed) > airSpeedMax)
                {
                    tmpSpeed = airSpeedMax * accelMultiplier;
                }*/
                int tmpForce = 120;
                bool accelerate;
                if (leftPressed)
                {
                    tmpForce *= -1;
                    accelerate = body2d.velocity.x > -airSpeedMax;
                } else
                {
                    accelerate = body2d.velocity.x < airSpeedMax;
                }
                if (accelerate) body2d.AddForce(new Vector2(tmpForce, 0));
            }
            else
            {
                body2d.velocity = new Vector2(body2d.velocity.x * airFriction, body2d.velocity.y);
                animator.SetBool("run", false);
            }
        }

        //flip visually
        if (body2d.velocity.x > .5f && !pushback) { playerObject.transform.localScale = originalScale;
            isFacingRight = true; }
        if (body2d.velocity.x < -.5f && !pushback) { playerObject.transform.localScale =
                new Vector3(-originalScale.x, originalScale.y, originalScale.z); isFacingRight = false; }
    }

    private void BasicAttackUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            foreach (GameObject go in interactables)
            {
                if (go.layer == 12)
                {
                    //animator.SetBool("punch", true);
                    Debug.Log("punch");
                    BasicEnemy basicEnemy = go.GetComponent<BasicEnemy>();
                    basicEnemy.HitBy(gameObject);
                }
            }
            Debug.Log("LShift");
        }
    }

    //----------------------------------------------------------------- OTHER METHODS -------------------------------------------

    //------------------------------------------------------------------- Triggers ----------------------------------------------
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 12) {
            HitByEnemy(other.gameObject);
        }

        interactables.Add(other.gameObject);
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.layer == 12) {
            HitByEnemy(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        interactables.Remove(other.gameObject);
    }

    //----------------------------------------------------------------- Colliders -------------------------------------------


    //------------------------------------------------------------- PUBLIC INTERFACE ----------------------------------------

    //------------------------------------------------------------- Behaviors
    public override void SetIsGrounded(bool newGroundedState, string colliderObjectName)
    {
        base.SetIsGrounded(newGroundedState, colliderObjectName);
        if (isGrounded)
        {
            animator.SetBool("jump", false);
            myPhysicsMaterial.friction = originalFriction;
            if (!leftPressed && !rightPressed) playerSound.SoundFootStep1();

        } else
        {
            myPhysicsMaterial.friction = 0f;
        }
    }

    public void HitByEnemy(GameObject enemy)
    {
        if (!invincible)
        {
            bool hitByBullet = false;

            //decrease player health based on enemy's set damage
            if (enemy.GetComponent<AbstractEnemy>() != null) health -= enemy.GetComponent<AbstractEnemy>().damageToPlayerPerHit;
            if (enemy.GetComponent<Bullets>() != null)
            {
                health -= enemy.GetComponent<Bullets>().DamageToCharacter();
                hitByBullet = true;
            }
            hud.SetHealth(health);
            hud.PlayerHitFX();
            PlayerPrefs.SetInt("PlayerHealth", health);

            //bump away enemy
            if (!hitByBullet)
            {
                enemy.GetComponent<AbstractEnemy>().PlayerCollision(gameObject);
                
            }
            Debug.Log("PlayerManager -> HitByEnemy:" + enemy.name + ". New Player Health:" + health);

            //bump player
            StartCoroutine(PlayerHitThrowback(enemy));
        }
    }

    IEnumerator PlayerHitThrowback(GameObject weapon)
    {
        pushback = true;
        invincible = true;
        body2d.velocity = new Vector2(0, 0);
        body2d.AddForce((gameObject.transform.position - weapon.transform.position).normalized * 4000);
        //Debug.Log("PlayerManager -> HitThrowback");
        yield return new WaitForSeconds(0.3f);
        pushback = false;
        yield return new WaitForSeconds(0.3f);
        invincible = false;
    }

    public void ResetPlayer()
    {
        interactables.Clear();
    }

    //------------------------------------------------------------- Get-Methods
    public List<GameObject> GetInteractables()
    {
        Debug.Log("PlayerManager -> Interactables n = " + interactables.Count);
        for (int i = interactables.Count - 1; i >= 0; i--) { if (interactables[i] == null) interactables.Remove(interactables[i]);  }
        return interactables;
    }
    public AbstractBodyMod GetArmOneMod() { return armOneMod; }
    public AbstractBodyMod GetArmTwoMod() { return armTwoMod; }
    public AbstractBodyMod GetLegsMod() { return legsMod; }
    public List<AbstractBodyMod> GetUnlockedBodyMods() { return unlockedBodyMods; }
    public int GetCredit() { return credit; }
    public int GetHealth() { return health; }
    public bool HasKey(DoorKey newKey)
    {
        bool hasKey = false;
        //TODO: key saving system has not been tested yet
        if (keyList.Contains(newKey) || PlayerPrefs.HasKey(newKey.objectID)) hasKey = true;
        return hasKey;
    }
    public PlayerSound GetPlayerSound() { return playerSound;  }

    //------------------------------------------------------------- Set-Methods
    public void RemoveInteractable(GameObject objectToRemove)
    {
        if (interactables.Contains(objectToRemove)) interactables.Remove(objectToRemove);
    }
    public void UnlockBodyMod(AbstractBodyMod newMod)
    {
        if (!unlockedBodyMods.Contains(newMod))
        {
            unlockedBodyMods.Add(newMod);
            
            //save unlocked body mods
            foreach (AbstractBodyMod abm in unlockedBodyMods)
            {
                if (!PlayerPrefs.HasKey(abm.name)) PlayerPrefs.SetInt(abm.name, 1);
            }

            Debug.Log("PlayerManager -> Unlocked: " + newMod);
            playerSound.SoundPickup();

            if (newMod.bodyModType == BodyModType.UPPERBODY)
            {
                if (armOneMod == null) { armOneMod = newMod; hud.UpdateBodyModsDisplay(); }
                else if (armTwoMod == null) { armTwoMod = newMod; hud.UpdateBodyModsDisplay(); }
            }
        }
    }
    public void SetMod(int whichOne, AbstractBodyMod newMod)
    {
        if (whichOne == 0 && newMod != null) { 
            armOneMod = newMod; Debug.Log("PlayerManager -> SetMod(): Arm One Mod, " + newMod.gameObject.name);
            PlayerPrefs.SetString("armOneMod", newMod.name);
        }
        if (whichOne == 1 && newMod != null) { 
            armTwoMod = newMod; Debug.Log("PlayerManager -> SetMod(): Arm Two Mod, " + newMod.gameObject.name);
            PlayerPrefs.SetString("armTwoMod", newMod.name);
        }
        if (whichOne == 2 && newMod != null) { 
            legsMod = newMod; Debug.Log("PlayerManager -> SetMod(): Legs Mod, " + newMod.gameObject.name);
            PlayerPrefs.SetString("legsMod", newMod.name);
        }
    }
    public void AddCredit(int addCredit)
    {
        credit += addCredit;
        hud.SetCredit(credit);
        playerSound.SoundPickup();
        PlayerPrefs.SetInt("PlayerCredit", credit);
    }
    public void Recharge(int recharge)
    {
        health += recharge;
        if (health > origHealth) health = origHealth;
        Debug.Log("PlayerManager -> recharge: " + recharge);
        hud.SetHealth(health);
        playerSound.SoundPickup();
        PlayerPrefs.SetInt("PlayerHealth", health);
    }
    public void AddKey(DoorKey newKey)
    {
        keyList.Add(newKey);
        playerSound.SoundPickup();

        //save that the key has been collected
        PlayerPrefs.SetInt(newKey.objectID, 1);
    }
    public void DecreaseHealth(int healthDecrease)
    {
        health -= healthDecrease;
        hud.SetHealth(health);
    }
}
