﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorSintactico
{
    class AutomataPila
    {
        private List<Regla> reglas;

        private string cadena;
        private bool aceptado;

        public AutomataPila(string cadena, List<Regla> reglas)
        {
            this.cadena = cadena;
            aceptado = false;
            this.reglas = reglas;
        }

        public void Simular()
        {
            Stack<string> pila = new Stack<string>();
            pila.Push("z0");

            ProbarRegla("q0", 0, pila, new List<Regla>());

            if (aceptado)
            {
                Console.WriteLine("Aceptado");
            }
            else
            {
                Console.WriteLine("No aceptado");
            }
            
        }

        private void ProbarRegla(string estado, int posicion, Stack<string> pila, List<Regla> camino)
        {
            if (!aceptado)
            {
                String caracter = "";
                //El caracter actual
                if (posicion < cadena.Length)
                {
                    caracter = cadena[posicion] + "";
                }

                String estadoOriginal = estado;
                //Revisamos todas las reglas
                for (int i = 0; i < reglas.Count; i++)
                {
                    Stack<String> pilaCopia = new Stack<String>(pila.Reverse());

                    List<Regla> caminoCopia = new List<Regla>();
                    for(int k = 0; k < camino.Count; k++)
                    {
                        caminoCopia.Add(camino[k]);
                    }

                    estado = estadoOriginal;

                    Regla regla = reglas[i];

                    //Comprobamos si es la regla adecuada 
                    if (regla.GetEstadoActual().Equals(estado))
                    {
                        //Comprobamos el tope de la pila
                        if (regla.GetCimaPila().Equals(pilaCopia.ElementAt(pilaCopia.Count - 1)) || regla.GetCimaPila().Equals("Z"))
                        {
                            //Si la entrada está bien probamos una nueva regla
                            if (regla.GetEntrada().Equals(caracter))
                            {
                                estado = regla.GetEstadoNuevo();
                                //Apilamos o desapilamos
                                if (regla.GetAccion().Equals("#"))
                                {
                                    pilaCopia.Pop();
                                }
                                else if (!regla.GetAccion().Equals("Z"))
                                {
                                    for (int j = 0; j < regla.GetAccion().Length; j++)
                                    {
                                        pilaCopia.Push(regla.GetAccion()[j] + "");
                                    }
                                }
                                caminoCopia.Add(regla);
                                List<Regla> caminoCopia2 = new List<Regla>();
                                for (int k = 0; k < caminoCopia.Count; k++)
                                {
                                    caminoCopia2.Add(caminoCopia[k]);
                                }

                                ProbarRegla(estado, posicion + 1, new Stack<String>(pilaCopia.Reverse()), caminoCopia2);
                            }
                            else if (regla.GetEntrada().Equals("#"))
                            {
                                estado = regla.GetEstadoNuevo();
                                //Apilamos o desapilamos
                                if (regla.GetAccion().Equals("#"))
                                {
                                    pilaCopia.Pop();
                                }
                                else if (!regla.GetAccion().Equals("Z"))
                                {
                                    for (int j = 0; j < regla.GetAccion().Length; j++)
                                    {
                                        pilaCopia.Push(regla.GetAccion()[j]+"");
                                    }
                                }

                                caminoCopia.Add(regla);
                                List<Regla> caminoCopia2 = new List<Regla>();
                                for (int k = 0; k < caminoCopia.Count; k++)
                                {
                                    caminoCopia2.Add(caminoCopia[k]);
                                }

                                ProbarRegla(estado, posicion + 1, new Stack<String>(pilaCopia.Reverse()), caminoCopia2);
                            }
                        }
                    }
                }
            }

            if (posicion == cadena.Length || (posicion == 0 && cadena.Equals("#")))
            {
                if (pila.Count - 1 >= 0 && pila.ElementAt(pila.Count - 1).Equals("z0"))
                {
                    aceptado = true;
                }
            }
        }

    }
}