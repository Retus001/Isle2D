using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlatformBehaviour : MonoBehaviour
{
    public Vector2 platformMovement;
    public float movementDuration;

    private void Awake()
    {
        DOTween.Init();
    }

    void Start()
    {
        Vector3 originalPos = transform.position;

        Sequence movementSequence= DOTween.Sequence();

        movementSequence.Append(
            transform.DOMoveY(originalPos.y + platformMovement.y, movementDuration)).Append(
            transform.DOMoveY(originalPos.y, movementDuration)).SetLoops(-1).SetEase(Ease.InOutSine);

        movementSequence.Play();
    }
}
