using UnityEngine;
using System.Collections;
using DG.Tweening;
public class Entity : MonoBehaviour
{
    public bool doNotTween;
    public Vector3 finalPosition;

    protected virtual void Start()
    {
        if (!doNotTween)
        {
            transform.DOMove(finalPosition, 0.5f).OnComplete(OnFallen).SetEase(Ease.InQuint);
            GetComponentInChildren<SpriteRenderer>().material.DOColor(new Color(1f, 1f, 1f, 1f), 0.5f);
        }
    }

    protected virtual void OnFallen()
    {
        MainDebug.WriteLine("YOU NOT SUPPOSED TO BE HERE! :/");
    }


}
