using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[DisallowMultipleComponent]
public class PlayerMovement : MonoBehaviour {

	[Header("Movement")]
	[SerializeField] float movementSpeed = 1;

	[Header("Invisibility")]
	[SerializeField] float maxInvisibilityTime = 1;
	[SerializeField] float invisibilityThreshold = 0.5f;
	[SerializeField] float minInvisibility = 0.2f;
	float invisibility = 1;
	float invisibilityTimer = 0;
	bool canGoInvisible = true;

	[Header("References")]
	Player rewiredPlayer;
	SpriteRenderer spriteRenderer;
	Rigidbody2D rb2d;

	void Awake () {

		rewiredPlayer = ReInput.players.GetPlayer(0);
		spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
		rb2d = this.GetComponentInChildren<Rigidbody2D>();
	}

	void Update () {
		
		UpdateVariables();
		ProcessMovementInput();
		ProcessActions();
		HandleInvisibility();
	}

	void UpdateVariables () {

		invisibility = spriteRenderer.color.a;
	}

	void ProcessMovementInput () {

		float horizontalInput = rewiredPlayer.GetAxis("Horizontal");
		float verticalInput = rewiredPlayer.GetAxis("Vertical");
		Vector3 movementVector = new Vector3(horizontalInput, verticalInput, 0);

		if(horizontalInput < 0 && spriteRenderer.flipX == true) {

			spriteRenderer.flipX = false;
		}
		else if(horizontalInput > 0 && spriteRenderer.flipX == false) {

			spriteRenderer.flipX = true;
		}

		if(movementVector != Vector3.zero) {

			rb2d.MovePosition(rb2d.transform.position + movementSpeed * movementVector);
		}
	}

	void ProcessActions () {

		if(rewiredPlayer.GetButton("Invisibility") && canGoInvisible) {

			IncreaseInvisibility();
		}
		else {

			DecreaseInvisibility();
		}
	}

	void IncreaseInvisibility () {

		invisibility -= Time.deltaTime;
		UpdateSpriteInvisibility();
	}

	void DecreaseInvisibility () {

		invisibility += Time.deltaTime;
		UpdateSpriteInvisibility();
	}

	void UpdateSpriteInvisibility () {

		invisibility = Mathf.Clamp(invisibility, minInvisibility, 1);
		spriteRenderer.color = new Color(1, 1, 1, invisibility);
	}

	void HandleInvisibility () {

		if(invisibility <= minInvisibility) {

			invisibilityTimer += Time.deltaTime;
			canGoInvisible = (invisibilityTimer >= maxInvisibilityTime) ? false : true;
			// Update UI Bar?
		}
	}
}
