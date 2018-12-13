using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.Events;

public class PlayerAnimation : MonoBehaviour
{
    /// starting scale.x of the player
    private float startXScale;
    /// meshRenderer of the player
    private MeshRenderer meshRenderer;
    public MeshRenderer MeshRenderer { get { return meshRenderer; } }
    /// script controlling player's behaviour
    private PlayerController playerController;
    // script controlling the game
    private GameController gameController;

    // Spine API
    // Spine's SkeletonAnimation script, controlls animations
    public SkeletonAnimation skeletonAnimation;
    // Reference to a Transform holding the graphics of the player
    public Transform GFX;
    // Animation names
    [SpineAnimation] public string idleAnimation;
    [SpineAnimation] public string runAnimation;
    [SpineAnimation] public string attackAnimation;
    [SpineAnimation] public string deathAnimation;
    [SpineAnimation] public string fallDeathAnimation;
    [SpineAnimation] public string woundAnimation;
    [SpineAnimation] public string jumpAnimation;
    [SpineAnimation] public string secondJumpAnimation;
    [SpineAnimation] public string fallAnimation;
    [SpineAnimation] public string landAnimation;
    [SpineAnimation] public string runningLandAnimation;
    [SpineAnimation] public string ladderAnimation;
    [SpineAnimation] public string slideAnimation;
    [SpineAnimation] public string crouchAnimation;
    [SpineAnimation] public string owlAppearAnimation;
    [SpineAnimation] public string owlFlyAnimation;
    [SpineAnimation] public string owlDisappearAnimation;
    [SpineAnimation] public string tigerAnimation;
    [SpineAnimation] public string glowwormAnimation;
    [SpineAnimation] public string elephantAnimation;
    [SpineAnimation] public string eelIdleAnimation;
    [SpineAnimation] public string eelHandAnimation;
    [SpineAnimation] public string eelHandRunAnimation;
    [SpineAnimation] public string tortoiseAnimation;

    // Spine Animation Events
    // Invoked when a certain animation begins/ends
    [SpineEvent] public string tigerBeginEvent;
    Spine.EventData tigerBeginEventData;
    // Player making a step on the ground
    [SpineEvent] public string stepEvent;
    Spine.EventData stepEventData;
    [SpineEvent] public string whipBeginEvent;
    Spine.EventData whipBeginEventData;
    [SpineEvent] public string whipEndEvent;
    Spine.EventData whipEndEventData;
    // Make sound after the whip
    [SpineEvent] public string makeSoundEvent;
    Spine.EventData makeSoundEventData;

    // CUrrent animation being played
    protected string currentAnimation = "";
    public string CurrentAnimation { get { return currentAnimation; } set { currentAnimation = value; } }
    // Is the GFX facing right?
    protected bool facingRight = true;
    public bool FacingRight { get { return facingRight; } set { facingRight = value; } }
    // Is the attack animation being played?
    protected bool attacking = false;
    public bool Attacking { get { return attacking; } set { attacking = value; } }
    // Is the die animation being played?
    protected bool dying = false;
    public bool Dying { get { return dying; } set { dying = value; } }
    // if true - enable default animation set, that is walk/run/idle
    bool canWalk = true;
    public bool CanWalk { get { return canWalk; } set { canWalk = value; } }
    // Invoked when the attack animation goes in
    public UnityAction<Types.AbilityType, bool> onAttackEvent;
    // When death animation has just finished
    public UnityAction onDeathFinishedEvent;
    // An abilitiy usage has finished
    public UnityAction<Types.AnimType> onAbilityAnimFinishedEvent;
    // Began attacking
    protected bool attackBeginEvent = true;
    // Has the run animation subscribed for the step sound event?
    bool runSubscribedForEvent = false;

    // Spine function, invoked when an animation is completed
    protected void OnCompleteAnim(Spine.TrackEntry trackEntry)
    {        
        if (trackEntry.animation.name == attackAnimation)
        {            
            //attacking = false;                        
            canWalk = true;
            skeletonAnimation.AnimationState.Complete -= OnCompleteAnim;
        }
        else if (trackEntry.animation.name == deathAnimation)
        {         
            //dying = false;
            skeletonAnimation.AnimationState.Complete -= OnCompleteAnim;
            if (onDeathFinishedEvent != null) onDeathFinishedEvent();
        }
        else if (trackEntry.animation.name == woundAnimation)
        {         
            canWalk = true;            
            skeletonAnimation.AnimationState.Complete -= OnCompleteAnim;
            //skeletonAnimation.state.ClearTrack(trackEntry.trackIndex);                             
        }        
        else if (trackEntry.animation.name == landAnimation)
        {         
            canWalk = true;
            skeletonAnimation.AnimationState.Complete -= OnCompleteAnim;
        }
        else if (trackEntry.animation.name == runningLandAnimation)
        {
            canWalk = true;
            skeletonAnimation.AnimationState.Complete -= OnCompleteAnim;
        }
        else if(trackEntry.animation.name == tigerAnimation)
        {
            // print("TigerDihnish");
            canWalk = true;
            if (onAbilityAnimFinishedEvent != null) onAbilityAnimFinishedEvent(Types.AnimType.Tiger);            
            skeletonAnimation.AnimationState.Complete -= OnCompleteAnim;
            skeletonAnimation.AnimationState.Interrupt -= OnCompleteAnim;
            skeletonAnimation.AnimationState.Event -= OnEvent;            
        }
        else if (trackEntry.animation.name == owlAppearAnimation)
        {            
            skeletonAnimation.AnimationState.Complete -= OnCompleteAnim;
            //skeletonAnimation.AnimationState.Interrupt -= OnCompleteAnim;
            OwlFlyAnim();
        }
        else if (trackEntry.animation.name == owlFlyAnimation)
        {            
            if (onAbilityAnimFinishedEvent != null)
            {
                onAbilityAnimFinishedEvent(Types.AnimType.OwlEnd);
            }
            skeletonAnimation.AnimationState.Interrupt -= OnCompleteAnim;            
        }
        else if (trackEntry.animation.name == owlDisappearAnimation)
        {            
            if (onAbilityAnimFinishedEvent != null)
            {
                onAbilityAnimFinishedEvent(Types.AnimType.OwlEnd);                
            }
            skeletonAnimation.AnimationState.Complete -= OnCompleteAnim;
            skeletonAnimation.AnimationState.Interrupt -= OnCompleteAnim;
            canWalk = true;
        }
        else if (trackEntry.animation.name == eelHandAnimation)
        {           
            trackEntry.Complete -= OnCompleteAnim;
            trackEntry.Interrupt -= OnCompleteAnim;            
            if (onAbilityAnimFinishedEvent != null) onAbilityAnimFinishedEvent(Types.AnimType.Eel);
        }
        else if (trackEntry.animation.name == eelHandRunAnimation)
        {
            canWalk = true;
            trackEntry.Complete -= OnCompleteAnim;
            trackEntry.Interrupt -= OnCompleteAnim;
            if (onAbilityAnimFinishedEvent != null) onAbilityAnimFinishedEvent(Types.AnimType.Eel);
        }
        else if (trackEntry.animation.name == eelIdleAnimation)
        {            
            canWalk = true;            
            trackEntry.Complete -= OnCompleteAnim;
            trackEntry.Interrupt -= OnCompleteAnim;
            if (onAbilityAnimFinishedEvent != null) onAbilityAnimFinishedEvent(Types.AnimType.Eel);
        }
        else if(trackEntry.animation.name == tortoiseAnimation)
        {
            print("TortoiseFinish");
            canWalk = true;
            trackEntry.Complete -= OnCompleteAnim;
            trackEntry.Interrupt -= OnCompleteAnim;
            if (onAbilityAnimFinishedEvent != null) onAbilityAnimFinishedEvent(Types.AnimType.Tortoise);
        }
    }

    // Method for setting an animation
    protected Spine.TrackEntry SetAnimation(string name, bool loop, float animSpeed = 1f, int trackIndex = 0)
    {
        if (name == null) Debug.LogError(name + ": animation not set for this character");
        //skeletonAnimation.state.ClearTrack(1);
        if (currentAnimation == name && name != attackAnimation && name != eelHandAnimation && name != eelIdleAnimation && name != jumpAnimation && name != secondJumpAnimation)
        {            
            if (skeletonAnimation.state.GetCurrent(0).timeScale != animSpeed)
            {
                skeletonAnimation.state.GetCurrent(0).timeScale = animSpeed;
            }

            return null;
        }
        if (name != runAnimation && name != idleAnimation && name != eelHandAnimation && name != woundAnimation) canWalk = false;
        if(currentAnimation == runAnimation)
            if(runSubscribedForEvent)
            {
                runSubscribedForEvent = false;
                skeletonAnimation.AnimationState.Event -= OnEvent;
            }

        var trackEntry = skeletonAnimation.state.SetAnimation(trackIndex, name, loop);
        trackEntry.timeScale = animSpeed;
        currentAnimation = name;
        return trackEntry;

    }

    // Flip in X when player changes direction
    public void FlipX(bool flip)
    {        
        if ((flip && facingRight) || (!flip && !facingRight))
        {
            Vector3 s = transform.localScale;
            transform.localScale = new Vector3(-s.x, s.y, s.z);
            facingRight = !facingRight;
        }        
    }

    //All animations:

    public void DeathAnim()
    {
        //dying = true;
        //canWalk = false;
        SetAnimation(deathAnimation, false);
        //skeletonAnimation.AnimationState.Complete += OnCompleteAnim;
    }

    public void FallDeathAnim()
    {
        SetAnimation(fallDeathAnimation, false);
        //skeletonAnimation.AnimationState.Complete += OnCompleteAnim;
    }

    public void WoundAnim(float animSpeed = 1f)
    {
        //canWalk = false;
        SetAnimation(woundAnimation, false, animSpeed, 2);
        skeletonAnimation.AnimationState.Complete += OnCompleteAnim;
        
    }

    public void IdleAnim(bool loop = true)
    {
        //SetAnimation(idleAnimation, true);
        SetAnimation(idleAnimation, loop);
    }

    public void RunAnim(bool loop = true)
    {
        //SetAnimation(runAnimation, true);
        SetAnimation(runAnimation, loop);
        if (!runSubscribedForEvent)
        {
            skeletonAnimation.AnimationState.Event += OnEvent;
            runSubscribedForEvent = true;
        }
    }

    public void JumpAnim()
    {
        SetAnimation(jumpAnimation, false);
    }

    public void SecondJumpAnim()
    {
        SetAnimation(secondJumpAnimation, false);
    }

    public void FallAnim()
    {
        SetAnimation(fallAnimation, false);        
    }

    public void LandAnim(float animSpeed = 1.75f)
    {
        canWalk = false;
        SetAnimation(landAnimation, false, animSpeed);
        skeletonAnimation.AnimationState.Complete += OnCompleteAnim;
    }

    public void RunningLandAnim()
    {
        canWalk = false;
        SetAnimation(runningLandAnimation, false);
        skeletonAnimation.AnimationState.Complete += OnCompleteAnim;
    }

    public void LadderAnim(float animSpeed = 1f)
    {
        canWalk = false;
        SetAnimation(ladderAnimation, true, animSpeed);        
    }

    public void SlideAnim()
    {
        canWalk = false;
        SetAnimation(slideAnimation, false);        
    }

    public void CrouchAnim(float animSpeed = 2f)
    { 
        canWalk = false;
        SetAnimation(crouchAnimation, true, animSpeed);
    }

    public void TigerAnim(float animSpeed = 1f)
    {
        canWalk = false;
        SetAnimation(tigerAnimation, false, animSpeed);
        skeletonAnimation.state.Event += OnEvent;
        skeletonAnimation.state.Complete += OnCompleteAnim;
        skeletonAnimation.state.Interrupt += OnCompleteAnim;
    }

    public void TortoiseAnim(float animSpeed = 1f)
    {
        canWalk = false;
        SetAnimation(tortoiseAnimation, false, animSpeed);
        skeletonAnimation.state.Complete += OnCompleteAnim;
        skeletonAnimation.state.Interrupt += OnCompleteAnim;
    }

    public void EelHandAnim(float animSpeed = 1f)
    {        
        var trackEntry = SetAnimation(eelHandAnimation, false, animSpeed, 1);
        if (trackEntry != null)
        {
            trackEntry.Complete += OnCompleteAnim;
            trackEntry.Interrupt += OnCompleteAnim;
            trackEntry.Event += OnEvent;
        }                             
    }    

    public void EelIdleAnim(float animSpeed = 1f)
    {
        ResetEelHand();
        var trackEntry = SetAnimation(eelIdleAnimation, false, animSpeed);
        if (trackEntry != null)
        {
            trackEntry.Complete += OnCompleteAnim;
            trackEntry.Interrupt += OnCompleteAnim;
            trackEntry.Event += OnEvent;
        }
    }

    public void EelHandRunAnim(float animSpeed = 1f)
    {
        ResetEelHand();
        var trackEntry = SetAnimation(eelHandRunAnimation, false, animSpeed, 1);
        if (trackEntry != null)
        {
            trackEntry.Complete += OnCompleteAnim;
            trackEntry.Interrupt += OnCompleteAnim;
            trackEntry.Event += OnEvent;
        }
    }

    public void OwlAppearAnim(float animSpeed = 3f)
    {
        ResetEelHand();
        canWalk = false;
        SetAnimation(owlAppearAnimation, false, animSpeed);
        skeletonAnimation.state.Complete += OnCompleteAnim;
        //skeletonAnimation.AnimationState.Interrupt += OnCompleteAnim;
    }

    public void OwlFlyAnim()
    {
        canWalk = false;
        SetAnimation(owlFlyAnimation, true);
        skeletonAnimation.AnimationState.Interrupt += OnCompleteAnim;        
    }

    public void OwlDisappearAnim(float animSpeed = 5f)
    {        
        canWalk = false;
        SetAnimation(owlDisappearAnimation, false, animSpeed);
        skeletonAnimation.AnimationState.Complete += OnCompleteAnim;
        skeletonAnimation.AnimationState.Interrupt += OnCompleteAnim;
    }
    
    // Quite self-explanatory :p
    public void PauseAnimation()
    {
        //skeletonAnimation.state.ClearTracks();
        //currentAnimation = "";
        if (skeletonAnimation.state.GetCurrent(0).timeScale != 0f)
            skeletonAnimation.state.GetCurrent(0).timeScale = 0f;
    }

    // This one too :pp
    public void InterruptAnimation()
    {        
        canWalk = true;
        //gameController.CantMoveOrDead = false;        
    }

    // Initialize variables and events
    private new void Start()
    {
        gameController = FindObjectOfType<GameController>();
        meshRenderer = skeletonAnimation.gameObject.GetComponent<MeshRenderer>();
        playerController = FindObjectOfType<PlayerController>();
        startXScale = transform.localScale.x;

        tigerBeginEventData = skeletonAnimation.Skeleton.Data.FindEvent(tigerBeginEvent);
        stepEventData= skeletonAnimation.Skeleton.Data.FindEvent(stepEvent);
        whipBeginEventData = skeletonAnimation.Skeleton.Data.FindEvent(whipBeginEvent);
        whipEndEventData = skeletonAnimation.Skeleton.Data.FindEvent(whipEndEvent);
        makeSoundEventData = skeletonAnimation.Skeleton.Data.FindEvent(makeSoundEvent);
    }

    private void LateUpdate()
    {
        LadderSound();        
        if (gameController.CantMoveOrDead || playerController.PlayerOnLadder) return;        
        // Default animation set
        if(canWalk)
        {
            Run();
            Idle();
        }
    }    

    public void Idle()
    {
        //if (playerController.IsTigerAttacking) return;
        if (Mathf.Abs(playerController.MovementDirection.x) <= 0.2f && playerController.rb.velocity.y == 0
            )//&& !playerController.isCrouching)
            //PlaySegment(41, 70);   
            IdleAnim();
        //else if (target.currentLabel == Consts.AnimationNames.Idle)
        //    target.stop();
    }

    public void Run()
    {
        //if (playerController.MovementDirection.x != 0 && playerController.grounded && playerController.rb.velocity.y <= 1
        //    && !CheckIfDashing() && !playerController.isCrouching)
        //if (playerController.MovementDirection.x != 0 && playerController.Grounded && playerController.rb.velocity.y <= 1
        //    )//&& !playerController.isCrouching)
        if (Mathf.Abs(playerController.MovementDirection.x) > 0.2f && playerController.Grounded && playerController.rb.velocity.y <= 1
            )//&& !playerController.isCrouching)
            //PlaySegment(2, 40);
            RunAnim();
        //else if (currentAnimation == runAnimation && !gameController.CantMoveOrDead)
        //    //target.stop();
        //    StopAnimating();
    }



    public void Jump()
    {        
        JumpAnim();
        SoundController.Instance.PlayOneShot(Consts.SoundIndex.Jump);
    }

    public void SecondJump()
    {
        SecondJumpAnim();
        SoundController.Instance.PlayOneShot(Consts.SoundIndex.Jump);
    }

    public void Land()
    {       
        if (currentAnimation == runAnimation || currentAnimation == slideAnimation) return;
        //skeletonAnimation.state.ClearTrack(1);
        LandAnim();
    }

    public void Dash()
    {
        //target.stop();        
        SlideAnim();
        //StartCoroutine(AnimateDash());
    }    

    public void StopDashing()
    {                
        canWalk = true;        
    }

    public void Crouch()
    {
        //if (target.currentLabel == Consts.AnimationNames.Slide && CheckIfDashing()) return;
        if (currentAnimation == slideAnimation) return;

        //if (target.isPlaying && target.currentLabel != Consts.AnimationNames.Crouch)
        //    target.stop();

        //PlaySegment(151, 180);
        CrouchAnim();
    }

    public bool CheckIfDashing()
    {
        return currentAnimation == slideAnimation;
    }

    public void FallToDeath()
    {
        //target.stop();
        //PlaySegment(301, 330);
        FallDeathAnim();
    }

    public void DamageDeath()
    {
        //target.stop();
        //PlaySegmentAndStopAtEnd(221, 300);
        DeathAnim();
    }

    public void WalkOnLadder()
    {       
        //if (playerController.MovementDirection.y != 0)
        //{
            //SoundController.Instance.ToggleRunningSound(false);            
            LadderAnim();
        //}       
    }

    public void LadderSound()
    {
        if (currentAnimation == ladderAnimation && playerController.MovementDirection.y != 0
            && !playerController.Grounded && !playerController.cantGoUp && Time.timeScale > 0)
            SoundController.Instance.ToggleLadderSound(true);
        else
            SoundController.Instance.ToggleLadderSound(false);
    }    
    
    // Spine's function, invoked on certain events if subscribed
    protected void OnEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data == stepEventData)
        {
            SoundController.Instance.RunningSound();

        }
        else if (e.Data == makeSoundEventData)
        {
            SoundController.Instance.EelSound();
        }
        else if (e.Data == tigerBeginEventData)
        {            
            if (onAttackEvent != null)
            {
                onAttackEvent(Types.AbilityType.Tiger, attackBeginEvent);
            }
            if (!attackBeginEvent)
            {
                //skeletonAnimation.AnimationState.Event -= OnEvent;
            }
            attackBeginEvent = !attackBeginEvent;
        }        
        else if (e.Data == whipBeginEventData)
        {         
            if (onAttackEvent != null) onAttackEvent(Types.AbilityType.Eel, true);
        }
        else if (e.Data == whipEndEventData)
        {            
            if (onAttackEvent != null) onAttackEvent(Types.AbilityType.Eel, false);            
            skeletonAnimation.AnimationState.Event -= OnEvent;
        }
    }

    // Reset the hand used for the eel-whip animation back to its default position
    public void ResetEelHand()
    {
        skeletonAnimation.state.ClearTrack(1);
    }

}
