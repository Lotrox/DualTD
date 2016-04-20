/**
 * Estructura de Malla Regular
 * Discretiza el espacio continuo en una matriz cuyas celdas tienen una dimensión concreta.
 */

using System;
using UnityEditor;
using UnityEngine;

namespace Grid {
	[Serializable]
	public class RegularGrid  {

		/** Divisiones en 2D de la malla. */
		[SerializeField]
		private uint cols, rows; // cols = x, rows = y

		/** Matriz de tipo int que almacena un valor para la casilla normalizada (x, y) */
		[SerializeField]
		private int[,] grid;

		/** Magnitud de una casilla. */
		[SerializeField]
		private double width, height;

		/** Límites de la superficie de la malla. */
		[SerializeField]
		private double xmin, ymin, xmax, ymax;

		/**
		 * Constructor de una malla regular.
		 * 
		 * @param x Número de divisiones en la componente X.
		 * @param y Número de divisiones en la componente Y.
		 * @param xmin Componente mínima en X de la superficie.
		 * @param xmax Componente máxima en X de la superficie.
		 * @param ymin Componente mínima en Y de la superficie.
		 * @param ymax Componente máxima en Y de la superficie.
		 */
		public RegularGrid(uint _x, uint _y, double _xmin, double _ymin, double _xmax, double _ymax) {
			cols   = _x;
			rows   = _y;
			grid   = new int[_x, _y];
			width  = (_xmax - _xmin) / _x;
			height = (_ymax - _ymin) / _y;
			xmin   = _xmin;
			ymin   = _ymin;
			xmax   = _xmax;
			ymax   = _ymax;
		}

		/**
		 * Normaliza los valores de un punto de superficie a una casilla de la malla.
		 * 
		 * @param x Parámetro de entrada. Componente X de la superficie.
		 * @param y Parámetro de entrada. Componente Y de la superficie.
		 * @param c Parámetro de salida.  Columna de la malla.
		 * @param r Parámetro de salida.  Fila de la malla.
		 */
		private void normalized(double x, double y, out uint c, out uint r) {
			c = (uint) ((x - xmin) / width);
			r = (uint) ((y - ymin) / height);
		}

		/**
		 * Determina si un punto de superficie está dentro de la malla.
		 * 
		 * @param x Componente X de la superficie.
		 * @param y Componente Y de la superficie.
		 */
		public bool isContained(double x, double y) {
			return (((xmin <= x) && (x < xmax)) && ((ymin <= x) && (y < ymax)));
		}

		/**
		 * Dado un punto de superficie, establece su valor.
		 * 
		 * @param x Componente X de la superficie.
		 * @param y Componente Y de la superficie.
		 * @param data Valor que será establecido en la casilla que ocupa el punto.
		 */
		public void setPoint(double x, double y, int data) {
			uint c, r;
			normalized(x, y, out c, out r);
			grid [c, r] = data;
		}

		/**
		 * Devuelve el valor almacenado en la casilla para un punto de la superficie.
		 * 
		 * @param x Componente X de la superficie.
		 * @param y Componente Y de la superficie.
		 */
		public int getPoint(double x, double y) {
			uint c, r;
			normalized (x, y, out c, out r);
			return grid [c, r];
		}

		/**
		 * Devuelve el número de columnas (eje de abscisas, eje X) de la malla almacenada.
		 */
		public uint getCols() {
			return cols;
		}

		/**
		 * Devuelve el número de filas (eje de ordenadas, eje Y) de la malla almacenada.
		 */
		public uint getRows() {
			return rows;
		}

		/**
		 * Devuelve el valor almacenado en una celda.
		 * 
		 * @param col Columna de la celda.
		 * @para row Fila de la celda.
		 */
		public int getCell(uint col, uint row) {
			return grid [col, row];
		}

		/**
		 * Establece el valor para una celda.
		 * 
		 * @param col Columna de la celda.
		 * @para row Fila de la celda.
		 */
		public void setCell(uint col, uint row, int data) {
			grid [col, row] = data;
		}
	}
}