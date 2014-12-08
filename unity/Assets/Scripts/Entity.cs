using UnityEngine;
using System.Collections;
using DG.Tweening;
public class Entity : MonoBehaviour
{
    public bool doNotTween;
    public bool delayTween;
    public bool tweenComponent;
    public Vector3 finalPosition;
    public SpriteRenderer spriteRenderer;
    public Transform spriteContainer;

    protected virtual void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteContainer = transform.Find("SpriteContainer");
        if (tweenComponent)
        {
            if (!doNotTween)
            {
                spriteRenderer.transform.localPosition = new Vector3(0.0f, 1.01f, -4.14f);
                spriteRenderer.transform.DOMove(finalPosition, 0.5f + (delayTween ? 0.25f : 0f)).OnStart(OnStartFalling).OnComplete(OnFallen).SetEase(Ease.InQuint);
            }
            else
            {
                spriteRenderer.gameObject.transform.localPosition = Vector3.zero;

            }
        }
        else
        {
            if(!doNotTween)
            {
               spriteContainer.localPosition = new Vector3(0.0f, 1.01f, -4.14f);
               spriteContainer.DOMove(finalPosition, 0.5f + (delayTween ? 0.25f : 0f)).OnStart(OnStartFalling).OnComplete(OnFallen).SetEase(Ease.InQuint);
            }
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
