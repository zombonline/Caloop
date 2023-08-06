using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpineController : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;
    [SerializeField] Button btnPlay;
    [SerializeField] float logosecondsToStart, sfxSecondsToStart,buttonSecondsTostart;
    private void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
        Invoke(nameof(SetAnimation), logosecondsToStart);
        Invoke(nameof(StartSfX), sfxSecondsToStart);
        Invoke(nameof(EnablePlayButton), buttonSecondsTostart);
    }

    private void EnablePlayButton()
    {
        btnPlay.gameObject.SetActive(true);
    }
    private void SetAnimation()
    {
        spineAnimationState.SetAnimation(0, "animation", false);
    }

    private void StartSfX()
    {
        FindObjectOfType<SFXManager>().PlayAudioEvent("event:/SFX/Menu_Animation_SFX");
    }
}
