using UnityEngine;
using System.Collections;

public class TowerInfo : MonoBehaviour {

	public int id = 0;
	public int cost = 10; // Coste para comprar la torre.
	public double range = 2; // Distancia de ataque.
	public double damagePerHit = 8; // Unidades de daño por golpe.
	public double damagePerArea = 0; // Unidades de daño en área.
	public double speedAttack = 1; // Velocidad de ataque en milisegundos.

}
