using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Grid;

public class ConcreteGrid : NetworkBehaviour {

	// Estados de una celda de la malla.
	public enum Cell {
		FREE_P1 = 0, // La casilla está libre y puede ser ocupada por el jugador 1.
		FREE_P2 = 1, // La casilla está libre y puede ser ocupada por el jugador 2.
		OWNER_GAME = 2, // La casilla está bloqueada. No pertenece a ningún jugador.
		OWNER_P1 = 3, // La casilla está ocupada y pertenece al jugador 1.
		OWNER_P2 = 4, // La casilla está ocupada y pertenece al jugador 2.
	};

	// Estructura de datos que almacena el estado de las casillas.
	[SyncVar]
	RegularGrid grid;

	// Valores por defecto del script.
	public const double dxmin = 0, dymin = 0, dxmax = 100, dymax = 100;
	public const uint dcols = 1, drows = 1;

	// Valores concretos de las dimensiones de la superficie que cubre la malla.
	public double xmin = dxmin, ymin = dymin, xmax = dxmax, ymax = dymax;
	public uint cols = dcols, rows = drows;

	/**
	 * Devuelve la celda contenida para el punto de superficie dado.
	 * Si el punto sobresale la malla, se devuelve Cell.OWNER_GAME
	 */
	public Cell getCell(double x, double y) {
		if (grid.isContained(x, y)) {
			return (Cell) grid.getPoint (x, y);
		}
		return Cell.OWNER_GAME;
	}

	/**
	 * Establece el valor de una celda para el punto de superficie dado.
	 * Si el punto sobresale de la malla, no hace nada.
	 */
	public void setCell(double x, double y, int cell) {
		if (grid.isContained (x, y)) {
			grid.setPoint (x, y, (int) cell);
		}
	}

	/**
	 * Establece la casilla con propiedad para el juego.
	 */
	public void setCellOwnerGame(double x, double y) {
		setCell (x, y, (int) Cell.OWNER_GAME);
	}

	/**
	 * Establece la casilla con propiedad para el jugador (identificado por su id)
	 */
	public void setCellOwnerPlayer(double x, double y, int player) {
		setCell(x, y, (int) (Cell.FREE_P1 + player)); 
	}

	/**
	 * El jugador ocupa una casilla si está libre y le pertenece.
	 * Devuelve False si no se pudo cometer la acción.
	 */
	public bool setCellOwnerPlayerIfFree(double x, double y) {
		Cell cell = getCell (x, y);
		if (cell < Cell.OWNER_GAME) {
			setCell (x, y, (int) (cell + 2));
			return true;
		}
		return false;
	}

	/**
	 * El jugador libera una casilla que haya ocupado.
	 * Devuelve False si no se pudo cometer la acción.
	 */
	public bool setCellFreePlayerIfTaken(double x, double y) {
		Cell cell = getCell (x, y);
		if (cell > Cell.OWNER_GAME) {
			setCell (x, y, (int) (cell - 2));
			return true;
		}
		return false;
	}

	/**
	 * Consigue el número de columnas de la malla.
	 */
	public uint getCols() {
		return grid.getRows ();
	}

	/**
	 * Consigue el número de filas de la malla.
	 */
	public uint getRows() {
		return grid.getRows ();
	}

	/**
	 * Obtiene el ancho de la casilla de la malla.
	 */
	public double getWidth() {
		return grid.getWidth();
	}

	/**
	 * Obtiene el alto de la casilla de la malla.
	 */
	public double getHeight() {
		return grid.getHeight();
	}

	/**
	 * Instanciación de las casillas del juego.
	 */
	void Start() {
		grid = new RegularGrid(cols, rows, xmin, ymin, xmax, ymax);
		/*
		for (uint col = 0; col < cols; ++col) {
			for (uint row = 0; row < rows; ++row) {
				grid.setCell (col, row, (int) Cell.OWNER_GAME);
			}
		}
		*/
	}
}
