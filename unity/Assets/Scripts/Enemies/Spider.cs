﻿using UnityEngine;

public enum SpiderState {
    Follow, DropDown, Idle, WasHit, PullUp
}

public class Spider : Enemy {
    #region Variables
    private Vector2 followDurationRange = new Vector2(2f, 5f);
    private Vector2 idleDurationRange = new Vector2(0.8f, 1f);

    private SpiderState spiderState;
    private Vector2 dir;
    private float timer;
    private float followDuration;
    private float idleDuration;

    //private Vector2 playerDir;
    #endregion

    #region Monobehaviour Methods
    protected override void Awake() {
        base.Awake();
        spiderState = SpiderState.Follow;

        followDuration = Random.Range(followDurationRange.x, followDurationRange.y);
        idleDuration = Random.Range(idleDurationRange.x, idleDurationRange.y);
    }
    #endregion

    #region Methods
    protected override void CheckState() {
        //MainDebug.WriteLine("timer", timer.ToString());
        //MainDebug.WriteLine("follow", followDuration.ToString());

        switch (spiderState) {
            case SpiderState.Follow:
                if (timer > followDuration) {
                    timer = 0;
                    spiderState = SpiderState.DropDown;
                    followDuration = Random.Range(followDurationRange.x, followDurationRange.y);
                }
                break;
            case SpiderState.DropDown:
                spiderState = SpiderState.Idle;
                break;
            case SpiderState.Idle:
                if (timer > idleDuration) {
                    timer = 0;      // TODO: this will potentially allow the spider to pull up and move at the same time
                    spiderState = SpiderState.PullUp;
                    idleDuration = Random.Range(idleDurationRange.x, idleDurationRange.y);
                }
                break;
            case SpiderState.WasHit:
                spiderState = SpiderState.PullUp;
                break;
            case SpiderState.PullUp:
                spiderState = SpiderState.Follow;

                // choose a random direction
                dir = Random.insideUnitSphere;
                dir = dir.normalized;

                // modify speed
                dir.x *= movementSpeed.x;
                dir.y *= movementSpeed.y;
                dir *= Time.deltaTime;
                break;
        }

        animator.SetInteger("state", (int)spiderState);
        timer += Time.deltaTime;
    }

    protected override void Movement() {
        if (spiderState == SpiderState.Follow) {
            // apply force
            rigidbody2D.AddForce(dir, ForceMode2D.Impulse);
            Debug.DrawRay(transform.position, dir, Color.red);      //DEBUG: direction
        }
    }
    #endregion
}
