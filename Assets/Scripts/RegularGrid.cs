/**
 * Estructura de Malla Regular
 * Discretiza el espacio continuo en una matriz cuyas celdas tienen una dimensión concreta.
 */

public static class RegularGrid<T> {

	/** Divisiones en 2D de la malla. */
	private int cols, rows; // cols = x, rows = y

	/** Matriz de tipo T que almacena un valor para la casilla normalizada (x, y) */
	private T[,] grid;

	/** Magnitud de una casilla. */
	private double width, height;

	/** Límites de la superficie de la malla. */
	private double xmin, ymin, xmax, ymax;

	/**
	 * Constructor de una malla regular.
	 * 
	 * @param cols Número de divisiones en la componente X.
	 * @param rows Número de divisiones en la componente Y.
	 * @param xmin Componente mínima en X de la superficie.
	 * @param xmax Componente máxima en X de la superficie.
	 * @param ymin Componente mínima en Y de la superficie.
	 * @param ymax Componente máxima en Y de la superficie.
	 */
	RegularGrid(uint _x, uint _y, double _xmin, double _ymin, double _xmax, double _ymax) {
		cols   = _x;
		rows   = _y;
		grid   = new T[_x, _y];
		width  = (_xmax - _xmin) / _x;
		height = (_ymax - _ymin) / _y;
		xmin   = _xmin;
		ymin   = _ymin;
		xmax   = _xmax;
		ymax   = _ymax;
	}

	/**
	 * Determina si un punto de superficie está dentro de la malla.
	 * 
	 * @param x Componente X de la superficie.
	 * @param y Componente Y de la superficie.
	 */
	private bool isContained(double x, double y) {
		return (((xmin <= x) && (x < xmax)) && ((ymin <= x) && (y < ymax)));
	}

	/**
	 * Normaliza los valores de un punto de superficie a una casilla de la malla.
	 * Cuando la normalización no es posible (porque el punto sobresalga de la superficie), devuelve False.
	 * 
	 * @param x Parámetro de entrada. Componente X de la superficie
	 * @param y Parámetro de entrada. Componente Y de la superficie
	 * @param c Parámetro de salida.  Columna de la malla.
	 * @param r Parámetro de salida.  Fila de la malla.
	 */
	private bool normalized(double x, double y, ref int c, ref int r) {
		if (isContained (x, y)) {
			c = (int)(((x - xmin) / width) * cols);
			r = (int)(((y - ymin) / height) * rows);
			return true;
		}
		return false;
	}


}
