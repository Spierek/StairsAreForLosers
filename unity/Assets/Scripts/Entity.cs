using UnityEngine;
using System.Collections;
using DG.Tweening;
public class Entity : MonoBehaviour
{
    public bool doNotTween;
    public bool delayTween;
    public float delayVal = 0.25f;
    public bool tweenComponent;
    public Vector3 finalPosition;
    public SpriteRenderer spriteRenderer;
    public Transform spriteContainer;
    public bool hurtOnFall;
    public bool isFalling = true;
    private Transform collider;

    protected virtual void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteContainer = transform.Find("SpriteContainer");
    }
   
    protected virtual void Start()
    {
        if(hurtOnFall)
        {
            collider = transform.Find("FallHitbox");
        }
        float tweenTime = 0.5f + (delayTween ? delayVal : 0f);
        if (tweenComponent)
        {
            if (!doNotTween)
            {
                spriteRenderer.transform.localPosition = new Vector3(0.0f, 1.01f, -4.14f);
                spriteRenderer.transform.DOMove(finalPosition, tweenTime).OnStart(OnStartFalling).OnComplete(OnFallen).SetEase(Ease.InQuint);
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
               spriteContainer.DOMove(finalPosition, tweenTime).OnStart(OnStartFalling).OnComplete(OnFallen).SetEase(Ease.InQuint);

            }
        }
        Invoke("OnMidTween", tweenTime / 2);
        Invoke("OnEndTween", tweenTime);
    }


    protected void OnMidTween()
    {
        if(collider!=null)
            collider.gameObject.SetActive(true);
    }

    protected void OnEndTween()
    {
        if (collider != null)
            collider.gameObject.SetActive(false);
        isFalling = false;
    }

    protected virtual void OnStartFalling()
    {

    }

    protected virtual void OnFallen()
    {
        MainDebug.WriteLine("YOU NOT SUPPOSED TO BE HERE! :/");
    }


}
