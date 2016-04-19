using UnityEngine;
using System.Collections;

public class freeCam : MonoBehaviour {
	public float ScrollSpeed = 30; //Velocidad de desplazamiento de la cámara.
	private float ScrollEdge = 0.01f; //Margen de detección de movimiento.
	public float PanSpeed = 30; //Velocidad de desplazamiento realizando PAN.

	private float limitMap = 250;
	private float zoomMin = 1f; //Valor mínimo de tamaño para la proyección ortográfica.
	private float zoomMax = 30f; //Valor máximo de tamaño para la proyección ortográfica.

	private bool playerOne = true;

	void Update () {

		if (Input.GetKeyDown(KeyCode.C)) //Cambiar a vista del jugador contrario.
		{ 
			if(playerOne) transform.eulerAngles = new Vector3(30, 225, 0);
			else transform.eulerAngles = new Vector3(30, 45, 0);
			playerOne = !playerOne;
		}
		if (Input.GetKey (KeyCode.Mouse1)) panCam ();
		else if(Input.GetKey(KeyCode.Space)) translateCam ();
		zoomCam ();
		rotateCam ();
	}

	void translateCam()
	{
		if ((Input.GetKey (KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width * (1 - ScrollEdge)) && ((transform.position.x <= limitMap) && (transform.position.z >= -limitMap)))
		{
			transform.Translate (Vector3.right * Time.deltaTime * ScrollSpeed, Space.Self);
		} else if ((Input.GetKey (KeyCode.LeftArrow) || Input.mousePosition.x <= Screen.width * ScrollEdge) && ((transform.position.x >= -limitMap) && (transform.position.z <= limitMap)))
		{
			transform.Translate (Vector3.right * Time.deltaTime * -ScrollSpeed, Space.Self);
		}

		if ((Input.GetKey (KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height * (1 - ScrollEdge)) && ((transform.position.x <= limitMap) && (transform.position.z <= limitMap)))
		{
			if (playerOne) 
			{
				transform.Translate (Vector3.forward * Time.deltaTime * ScrollSpeed, Space.World);
				transform.Translate (Vector3.right * Time.deltaTime * ScrollSpeed, Space.World);
			} else 
			{
				transform.Translate (Vector3.forward * Time.deltaTime * -ScrollSpeed, Space.World);
				transform.Translate (Vector3.right * Time.deltaTime * -ScrollSpeed, Space.World);
			}

		} else if ((Input.GetKey (KeyCode.DownArrow) || Input.mousePosition.y <= Screen.height * ScrollEdge) && ((transform.position.x >= -limitMap) && (transform.position.z >= -limitMap)))
		{
			if (playerOne) 
			{
				transform.Translate (Vector3.forward * Time.deltaTime * -ScrollSpeed, Space.World);
				transform.Translate (Vector3.right * Time.deltaTime * -ScrollSpeed, Space.World);
			} else 
			{
				transform.Translate (Vector3.forward * Time.deltaTime * ScrollSpeed, Space.World);
				transform.Translate (Vector3.right * Time.deltaTime * ScrollSpeed, Space.World);
			}
		}
	}

	void panCam()
	{
		if (playerOne) {
			transform.Translate (Vector3.forward * Time.deltaTime * PanSpeed * (Input.mousePosition.y - Screen.height * 0.5f) / (Screen.height * 0.5f), Space.World);
			transform.Translate (Vector3.right * Time.deltaTime * PanSpeed * (Input.mousePosition.x - Screen.width * 0.5f) / (Screen.width * 0.5f), Space.World);
		} else {
			transform.Translate (Vector3.forward * Time.deltaTime * -PanSpeed * (Input.mousePosition.y - Screen.height * 0.5f) / (Screen.height * 0.5f), Space.World);
			transform.Translate (Vector3.right * Time.deltaTime * -PanSpeed * (Input.mousePosition.x - Screen.width * 0.5f) / (Screen.width * 0.5f), Space.World);
		}
	}

	void zoomCam()
	{
		if (Input.GetAxis("Mouse ScrollWheel") < -0) // forward
		{
			Camera.main.orthographicSize *= 1.1f;
		}
		if (Input.GetAxis("Mouse ScrollWheel") > -0) // back
		{
			Camera.main.orthographicSize *= 0.9f;
		}

		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, zoomMin, zoomMax );
	}

	void rotateCam(){
		if (Input.GetKeyUp (KeyCode.Comma) || Input.GetKeyUp (KeyCode.Period))
		{
			if(!playerOne) transform.eulerAngles = new Vector3(30, 225, 0);
			else transform.eulerAngles = new Vector3(30, 45, 0);
		}
		if (Input.GetKey (KeyCode.Comma)) transform.eulerAngles = new Vector3(30, transform.eulerAngles.y + 0.5f, 0); 
		if (Input.GetKey (KeyCode.Period)) transform.eulerAngles = new Vector3(30, transform.eulerAngles.y - 0.5f, 0); 
	}
}
