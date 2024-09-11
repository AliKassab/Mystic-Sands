using System.Collections.Generic;
using UnityEngine;

public class Offseter : MonoBehaviour
{
    private Animator animator;
    string randomState;
    float randomOffset;
    float randomSpeed;
    // Define animation state names in the Animator
    [SerializeField] private List<string> animationStates = new List<string>();

    // Define minimum and maximum values for speed
    [SerializeField] private float minSpeed = 0.8f;
    [SerializeField] private float maxSpeed = 1.2f;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Clear the list to avoid duplicates and ensure it's empty
        animationStates.Clear();

        // Populate the list with animation state names
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            animationStates.Add(clip.name);
        }

        if (animationStates.Count == 0)
        {
            Debug.LogWarning("No animation states found.");
            return;
        }

        randomState = animationStates[Random.Range(0, animationStates.Count)];
        randomOffset = Random.Range(0f, 1f);
        randomSpeed = Random.Range(minSpeed, maxSpeed);
        animator.speed = randomSpeed;

        animator.Play(randomState, -1, randomOffset);
    }

}
