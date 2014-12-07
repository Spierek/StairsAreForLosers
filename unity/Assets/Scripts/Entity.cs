using UnityEngine;
using System.Collections;
using DG.Tweening;
public class Entity : MonoBehaviour
{
    public bool doNotTween;
    public bool delayTween;
    public Vector3 finalPosition;

    protected virtual void Start()
    {
        if (!doNotTween)
        {
            transform.DOMove(finalPosition, 0.5f + (delayTween?0.25f:0f)).OnStart(OnStartFalling).OnComplete(OnFallen).SetEase(Ease.InQuint);
            GetComponentInChildren<SpriteRenderer>().material.DOColor(new Color(1f, 1f, 1f, 1f), 0.5f);
        }
    }

    protected virtual void OnStartFalling()
    {

    }

    protected virtual void OnFallen()
    {
        MainDebug.WriteLine("YOU NOT SUPPOSED TO BE HERE! :/");
    }


}
