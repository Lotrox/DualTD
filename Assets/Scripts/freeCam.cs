using UnityEngine;
using System.Collections;

public class freeCam : MonoBehaviour {
	public float ScrollSpeed = 30; //Velocidad de desplazamiento de la cámara.
	public float ScrollEdge = 0.02f; //Margen de detección de movimiento.
	public float PanSpeed = 30; //Velocidad de desplazamiento realizando PAN.
	public float zoomMin = 2f; // Valor mínimo de tamaño para la proyección ortográfica.
	public float zoomMax = 18f; // Valor máximo de tamaño para la proyección ortográfica.

	/*--Limites del mapa para la cámara, por defecto J1--*/
	private float lim_x = 32.2f - 18f * 2f;
	private float lim_X = 67.6f + 18f * 2f;
	private float lim_z = -6f - 18f * 2f;
	private float lim_Z = 33.4f + 18f * 2f;

	private bool playerOne = true; // Booleano que nos indica si este jugador es el 1 o no.

	public Texture2D normalCursor;
	public Texture2D aimCursor;
	private Vector2 hotSpot = Vector2.zero;

	void Start() {
		set_aimCursor (false);
	}

	public void set_aimCursor(bool b){
		if(b)
			Cursor.SetCursor(aimCursor, hotSpot, CursorMode.Auto);
		else
			Cursor.SetCursor(normalCursor, hotSpot, CursorMode.Auto);
	}

	void Update () {
		if (!ClockTimer.updateable)
			return;
		
		if (Input.GetKey (KeyCode.Mouse1)) panCam ();
		// else translateCam ();
		zoomCam ();
		rotateCam ();
	}

	void translateCam()
	{
		if ((Input.GetKey (KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width * (1 - ScrollEdge)) && (transform.position.x) <= (lim_X - Camera.main.orthographicSize*2))
		{
			transform.Translate (Vector3.right * Time.deltaTime * ScrollSpeed, Space.Self);
		} else if ((Input.GetKey (KeyCode.LeftArrow) || Input.mousePosition.x <= Screen.width * ScrollEdge) && transform.position.x >= lim_x + Camera.main.orthographicSize*2)
		{
			transform.Translate (Vector3.right * Time.deltaTime * -ScrollSpeed, Space.Self);
		}

		if ((Input.GetKey (KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height * (1 - ScrollEdge)) && transform.position.z <= lim_Z - Camera.main.orthographicSize*2)
		{

			if (playerOne) 
			{
				transform.Translate (Vector3.forward * Time.deltaTime * ScrollSpeed, Space.World);
			} else 
			{
				transform.Translate (Vector3.forward * Time.deltaTime * -ScrollSpeed, Space.World);
			}

		} else if ((Input.GetKey (KeyCode.DownArrow) || Input.mousePosition.y <= Screen.height * ScrollEdge) && transform.position.z >= lim_z + Camera.main.orthographicSize*2)
		{
			if (playerOne) 
			{
				transform.Translate (Vector3.forward * Time.deltaTime * -ScrollSpeed, Space.World);
				//transform.Translate (Vector3.right * Time.deltaTime * -ScrollSpeed, Space.World);
			} else 
			{
				transform.Translate (Vector3.forward * Time.deltaTime * ScrollSpeed, Space.World);
				//transform.Translate (Vector3.right * Time.deltaTime * ScrollSpeed, Space.World);
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

	public void updateCamera(bool playerTwo){
		if (!playerTwo) {
			transform.eulerAngles = new Vector3 (30, 0, 0);
			lim_x = 32.2f - 18f * 2f;
			lim_X = 67.6f + 18f * 2f;
			lim_z = -6f - 18f * 2f;
			lim_Z = 33.4f + 18f * 2f;
		}else {
			transform.eulerAngles = new Vector3 (30, 180, 0);
			playerOne = false;
			lim_x = 13.57f;
			lim_z = -23.2f;
			lim_X = 86.42f;
			lim_Z = 46.68f;

		}
	}

}
