﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoRagdoll : MonoBehaviour {

	public List<GameObject> ragdollParts;
	public GameObject player;

	public Animator animator;

	void Start()
	{
		if(this.GetComponent<Animator>())
		{
			animator = this.GetComponent<Animator>();  
		}
	}

	public void KillPlayer()
	{
		if(this.GetComponent<Animator>())
		animator.enabled = false;

		if( player )
		{
			player.GetComponent<PlayerController>().canMove = false;
		}
		
		foreach(GameObject go in ragdollParts)
		{
			if(go.GetComponent<Rigidbody>())
			go.GetComponent<Rigidbody>().isKinematic = false;
		}
	}

	public void RevivePlayer()
	{
		animator.enabled = true;
		
		if( player )
			player.GetComponent<PlayerController>().canMove = true;
		
		foreach(GameObject go in ragdollParts)
		{
			go.GetComponent<Rigidbody>().isKinematic = true;
		}
	}
}
