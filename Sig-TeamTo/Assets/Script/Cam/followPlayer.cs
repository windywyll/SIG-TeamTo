﻿using UnityEngine;
using System.Collections;

public class followPlayer : MonoBehaviour {
    public GameObject player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.position= new Vector3(player.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
	}
}
