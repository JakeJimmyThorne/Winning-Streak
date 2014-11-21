﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using XInputDotNetPure;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float jumpSpeed;

	public List<GameObject> spawnPoints = new List<GameObject>();

	public bool physicsMovement = false;

	//XInput stuff
	PlayerIndex index = new PlayerIndex();

	GamePadState controllerState = new GamePadState();
	GamePadState controllerStatePrev = new GamePadState();

	public bool isConnected = false;

	bool canMove = true;

	public float dodgeInterval = 1;
	float dodgeTimer = 0;

	void Start () 
	{
		InitialiseController();
		dodgeTimer = dodgeInterval;

		if(physicsMovement)
			speed *= 100;
	}

	void Update () 
	{
		if(!canMove)
			return;

		dodgeTimer -= Time.deltaTime;

		//controller movement
		if(isConnected)
		{
			ControllerMovement();
		}
		else
		{
			KeyboardMovement();
		}
	}

	void RotatePlayerController()
	{
		Quaternion rot = this.transform.rotation;
		
		rot.eulerAngles = new Vector3(this.transform.eulerAngles.x,
		                              Mathf.Atan2(TestLeftStick("X"),TestLeftStick("Y")) * Mathf.Rad2Deg,
		                              this.transform.eulerAngles.z);
		
		//smooth transitioning for rotation, also makes the rotation and movement more human like
		this.transform.rotation = Quaternion.Lerp (this.transform.rotation,
		                                           rot,
		                                           Time.deltaTime * 6);
	}

	void RotatePlayerKeyboard()
	{
		//todo sort out rotate for keyboard
		Quaternion rot = this.transform.rotation;
		
		rot.eulerAngles = new Vector3(this.transform.eulerAngles.x,
		                              Mathf.Atan2(TestLeftStick("X"),TestLeftStick("Y")) * Mathf.Rad2Deg,
		                              this.transform.eulerAngles.z);
		
		//smooth transitioning for rotation, also makes the rotation and movement more human like
		this.transform.rotation = Quaternion.Lerp (this.transform.rotation,
		                                           rot,
		                                           Time.deltaTime * 6);
	}

	void ControllerMovement()
	{
		//player movement
		if(physicsMovement)
		{
			//left/down movement
			if(TestLeftStick("X") != 0)
			{
				this.rigidbody.AddForce(TestLeftStick("X") * speed * Time.deltaTime,0,0);
			}
			
			//up/down movement
			if(TestLeftStick("Y") != 0)
			{
				this.rigidbody.AddForce(0,0,TestLeftStick("Y") * speed * Time.deltaTime);
			}
		}
		else
		{
			//left/down movement
			if(TestLeftStick("X") != 0)
			{
				this.transform.position += new Vector3(TestLeftStick("X") * speed * Time.deltaTime,0,0);
			}
			
			//up/down movement
			if(TestLeftStick("Y") != 0)
			{
				this.transform.position += new Vector3 (0,0,TestLeftStick("Y") * speed * Time.deltaTime);
			}
		}
		
		//player rotation towards direction
		if(TestLeftStick("X") != 0 || TestLeftStick("Y") != 0)
		{
			RotatePlayerController();
		}

		if(dodgeTimer < 0)
		{
			if(TestButton("LB"))
			{
				DodgeLeft();
				dodgeTimer = dodgeInterval;
			}

			if(TestButton("RB"))
			{
				DodgeRight();
				dodgeTimer = dodgeInterval;
			}
		}

		UpdateController();
	}

	void KeyboardMovement()
	{
		//player movement
		if(physicsMovement)
		{
			//left movement
			if(Input.GetKey(KeyCode.A))
			{
				this.rigidbody.AddForce(-speed * Time.deltaTime,0,0);
			}

			//right movement
			if(Input.GetKey(KeyCode.D))
			{
				this.rigidbody.AddForce(speed * Time.deltaTime,0,0);
			}

			
			//up movement
			if(Input.GetKey(KeyCode.W))
			{
				this.rigidbody.AddForce(0,0,speed * Time.deltaTime);
			}

			//down
			if(Input.GetKey(KeyCode.S))
			{
				this.rigidbody.AddForce(0,0,-speed * Time.deltaTime);
			}

			RotatePlayerKeyboard();
		}
		else
		{
			//left movement
			if(Input.GetKey(KeyCode.A))
			{
				this.transform.position += new Vector3(-speed * Time.deltaTime,0,0);
			}
			
			//right movement
			if(Input.GetKey(KeyCode.D))
			{
				this.transform.position += new Vector3(speed * Time.deltaTime,0,0);
			}
			
			
			//up movement
			if(Input.GetKey(KeyCode.W))
			{
				this.transform.position += new Vector3(0,0,speed * Time.deltaTime);
			}
			
			//down
			if(Input.GetKey(KeyCode.S))
			{
				this.transform.position += new Vector3(0,0,-speed * Time.deltaTime);
			}

			RotatePlayerKeyboard();
		}
	}


	void DodgeRight()
	{
		if(physicsMovement)
		{
			this.rigidbody.AddForce(this.transform.right * 300);
		}
		else
		{
			this.rigidbody.AddForce(this.transform.right * 300);
			//this.transform.position += this.transform.right;
		}
	}

	void DodgeLeft()
	{
		if(physicsMovement)
		{
			this.rigidbody.AddForce(-this.transform.right * 300);
		}
		else
		{
			this.rigidbody.AddForce(-this.transform.right * 300);
			//this.transform.position += this.transform.right;
		}
	}


	//CONTROLLER STUFF

	void UpdateController()
	{
		controllerStatePrev = controllerState;
		controllerState = GamePad.GetState(index);
	}
	
	void InitialiseController()
	{
		PlayerIndex testPlayerIndex = (PlayerIndex)0;
		GamePadState testState = GamePad.GetState(testPlayerIndex);

		if(testState.IsConnected)
		{
			Debug.Log ("GamePad found");
			index = testPlayerIndex;
			isConnected = true;
		}
	}

	//controller getters
	bool TestButton(string button)
	{
		switch(button)
		{
		case "Y":
			if(controllerStatePrev.Buttons.Y == ButtonState.Released &&
			   controllerState.Buttons.Y == ButtonState.Pressed)
			{
				return true;
			}
			break;
		case "B":
			if(controllerStatePrev.Buttons.B == ButtonState.Released &&
			   controllerState.Buttons.B == ButtonState.Pressed)
			{
				return true;
			}
			break;
		case "A":
			if(controllerStatePrev.Buttons.A == ButtonState.Released &&
			   controllerState.Buttons.A == ButtonState.Pressed)
			{
				return true;
			}
			break;
		case "X":
			if(controllerStatePrev.Buttons.X == ButtonState.Released &&
			   controllerState.Buttons.X == ButtonState.Pressed)
			{
				return true;
			}
			break;

		case "LB":
			if(controllerStatePrev.Buttons.LeftShoulder == ButtonState.Released &&
			   controllerState.Buttons.LeftShoulder == ButtonState.Pressed)
			{
				return true;
			}
			break;
		case "RB":
			if(controllerStatePrev.Buttons.RightShoulder == ButtonState.Released &&
			   controllerState.Buttons.RightShoulder == ButtonState.Pressed)
			{
				return true;
			}
			break;

		case "START":
			if(controllerStatePrev.Buttons.Start == ButtonState.Released &&
			   controllerState.Buttons.Start == ButtonState.Pressed)
			{
				return true;
			}
			break;
		case "BACK":
			if(controllerStatePrev.Buttons.Back == ButtonState.Released &&
			   controllerState.Buttons.Back == ButtonState.Pressed)
			{
				return true;
			}
			break;

		}

		return false;
	}
	float TestLeftStick(string axis)
	{
		if(axis == "X")
			return controllerState.ThumbSticks.Left.X;
		else if(axis == "Y")
			return controllerState.ThumbSticks.Left.Y;

		return 0;
	}	
	float TestRightStick(string axis)
	{
		if(axis == "X")
			return controllerState.ThumbSticks.Right.X;
		else if(axis == "Y")
			return controllerState.ThumbSticks.Right.Y;
		
		return 0;
	}

}




















