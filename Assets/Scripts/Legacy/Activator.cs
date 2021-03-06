﻿using UnityEngine;
using System.Collections;

public class Activatorr : MonoBehaviour {

    public float distanceToActivate = 5;
    public float distanceToDeactivate = 0;
    public MonoBehaviour toActivate;
    public BoxCollider2D activationZone;
    public BoxCollider2D endZone;

    private PlayerController playerCtrl;

	// Use this for initialization
	void Start () {
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Reset();
        Run();
	}

	// Update is called once per frame
	void FixedUpdate () {
        /*
        // TODO check if playing
        if (!activated) {
            if (transform.position.y > playerCtrl.transform.position.y + distanceToActivate) {
                return;
            } else {
                toActivate.enabled = true;
                activated = true;
            }
        } else {
            if (transform.position.y > playerCtrl.transform.position.y + distanceToDeactivate) {
                Reset();
            }
        }
        */
	}

    IEnumerator RunActivator() {
        while (!activationZone.bounds.Contains(playerCtrl.transform.position)) {
            yield return null;
        }
        toActivate.enabled = true;

        while (!endZone.bounds.Contains(playerCtrl.transform.position)) {
            yield return null;
        }
        toActivate.enabled = false;
    }

    public void Reset() {
        Debug.Log("reset to false");
        //toActivate.StopAllCoroutines();
        StopAllCoroutines();
        toActivate.enabled = false;
    }

    public void Run() {
        StartCoroutine(RunActivator());
    }
}
