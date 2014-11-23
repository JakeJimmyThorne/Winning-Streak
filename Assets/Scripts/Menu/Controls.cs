﻿using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour
{

	//public GameObject player;


	public AudioClip toHats;
	public AudioClip fromHats;
	AudioSource sound;

	int cameraSwing = 75;

	public static bool inHatPicker = false;
	bool transitioning = false;

	// Use this for initialization
	void Start ()
	{
		AIAttack.endGame = false;
		sound = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		float x = Input.GetAxis("Horizontal");


		if(!transitioning)
		{
			if( x>0.5f && !inHatPicker )
			{
				sound.PlayOneShot(toHats,0.5f);
				inHatPicker = true;
				transitioning = true;
			}

			if( x<-0.5f && inHatPicker )
			{
				sound.PlayOneShot(fromHats,0.5f);
				inHatPicker = false;
				transitioning = true;
			}
		}
		else
		{	
			//going to hatPicker
			if(inHatPicker)
			{
				Quaternion toRot = Quaternion.identity;

				toRot.eulerAngles = new Vector3(0,73,0);

				this.transform.rotation = Quaternion.Lerp (this.transform.rotation,toRot,Time.deltaTime * 10);

				if(this.transform.eulerAngles.y > 71)
				{
					transitioning = false;
				}
			}

			//going to mainmenu
			if(!inHatPicker)
			{
				Quaternion toRot = Quaternion.identity;
				
				toRot.eulerAngles = new Vector3(0,0,0);
				
				this.transform.rotation = Quaternion.Lerp (this.transform.rotation,toRot,Time.deltaTime * 10);
				
				if(this.transform.eulerAngles.y < 1)
				{
					transitioning = false;
				}
			}
		}
	
	}
}
