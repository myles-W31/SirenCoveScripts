using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundFromSelf : StateMachineBehaviour
{
    public AudioSource source;
    public AudioClip soundPlayingOnce;
    public AudioClip soundRepeating;
    public float pitch;
    public float volume;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        source = animator.gameObject.GetComponent<AudioSource>();
        source.pitch = pitch;
        source.volume = volume;
        if (soundPlayingOnce)
        {
            source.PlayOneShot(soundPlayingOnce);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (source.isPlaying == false && soundRepeating)
        {
            source.PlayOneShot(soundRepeating);
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (soundRepeating)
        {
            source.Stop();
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
