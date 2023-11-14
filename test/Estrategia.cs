
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSpace
{
    class Estrategia
    {
        public String Consulta1(ArbolGeneral<Planeta> arbol)
        {
            string msj = "La distancia entre el planeta del Bot y la raíz es: " + nivelBot(arbol);

            return msj;
        }
        
        //================================ METODO PARA CONSULTA 1 =============================================
        private int nivelBot(ArbolGeneral<Planeta> arbol)    
        {	// Devuelve la profundidad a la que se encuentra el planeta del Bot en el árbol
            // Creación de una cola para realizar un recorrido por niveles con separadores
            Cola<ArbolGeneral<Planeta>> c = new Cola<ArbolGeneral<Planeta>>();
            ArbolGeneral<Planeta> arbolAux;

            // Se encola el árbol raíz y un separador (null) para indicar el nivel
            c.encolar(arbol);
            c.encolar(null);

            //Variable de nivel
            int nivel = 0;
			
            //Mientras cola no este vacia
            while (!c.esVacia())
            {	//Desencolo elemento en arbol aux
                arbolAux = c.desencolar();

                // Si es un separador
                if (arbolAux == null)
                {
                    if (!c.esVacia())
                    {   // Encolamos otro separador para indicar el siguiente nivel
                        c.encolar(null);
                        // Incrementamos el nivel porque se avanzo al siguiente nivel
                        nivel++; 
                    }
                }
                // Si es un nodo (arbol)
                else
                {	// Se procesa el nodo actual
                    // Si se encuentra el planeta del Bot
                    if (arbolAux.getDatoRaiz().EsPlanetaDeLaIA())
                    	//Devolvemos el nivel actual
                        return nivel; 

                    // Si no, se encola los hijos del nodo para explorar niveles inferiores
                    foreach (var hijo in arbolAux.getHijos())
                        c.encolar(hijo);
                }
            }
            // Si no se encontró el planeta del Bot en el árbol, se devuelve el nivel como -1
            return -1;
        }

        public String Consulta2(ArbolGeneral<Planeta> arbol)
        {
            String msj = "\nPlanetas descendientes del Bot: " + hijosBot(arbol);

            return msj;
        }
        
        //====================== METODOS PARA CONSULTA 2 ====================================
        private string hijosBot(ArbolGeneral<Planeta> arbol)
        {
            // Recorrido por niveles utilizando una cola
            Cola<ArbolGeneral<Planeta>> c = new Cola<ArbolGeneral<Planeta>>();
            ArbolGeneral<Planeta> arbolAux;
            string botHijos = "";

            // Encolamos el árbol raíz
            c.encolar(arbol);

            // Realizamos el recorrido por niveles
            while (!c.esVacia())
            {	//Desencolo elemento en arbol aux
                arbolAux = c.desencolar();

                // Si se encuentra el primer planeta que es del bot
                if (arbolAux.getDatoRaiz().EsPlanetaDeLaIA())
                {
                    // Realizamos un recorrido preorden para obtener la población de los planetas
                    botHijos = preOrdenString(arbolAux);

                    // Se busca y saca el primer elemento de botHijos (La RAIZ, o sea el planeta bot)
                    int primerEspacio = botHijos.IndexOf(" ");
                    botHijos = botHijos.Remove(0, primerEspacio + 1);

                    // Se retorna la representación de la población de los planetas en el subárbol
                    return botHijos;
                }

                // Si no, se encola los hijos del árbol actual para seguir la búsqueda
                foreach (var hijo in arbolAux.getHijos())
                {
                    c.encolar(hijo);
                }
            }
            // Si no se encuentra ningún planeta BOT, se retorna una cadena vacía
            return botHijos;
        }
        
        private string preOrdenString(ArbolGeneral<Planeta> arbol)
        {
            //Declara la variable recorrido que se utiliza para almacenar la población del árbol en cadena
            StringBuilder recorrido = new StringBuilder();

            // Se procesa la población del planeta raíz primero; se agrega poblacion de la raiz al recorrido
            recorrido.Append(arbol.getDatoRaiz().Poblacion().ToString() + "  ");

            // Despues se procesa los hijos recursivamente en preorden
            // Para cada hijo en el arbol; lo agrego al recorrido
            foreach (var hijo in arbol.getHijos())
            {
                recorrido.Append(preOrdenString(hijo));
            }

            // Retorno la representación de la población en forma de cadena
            return recorrido.ToString();
        }

        public String Consulta3(ArbolGeneral<Planeta> arbol)
        {	//Variable que almacena cadena
            String msj = "";
			//Variable que almacena el número del nivel actual.          
            int i = 0;

            //Se recorre una lista de arrays donde cada array representa un nivel. 
            //1er elemento de cada array corresponde a la poblacion total del nivel. 
            //2do elemento de cada array corresponde al promedio de la poblacion por planeta de ese nivel.
            //Recorro lista de arreglos
            foreach (int[] nivel in nivelesPoblacion(arbol))
            {                                             
                msj += "\n\n Nivel " + i + ": " + "la población total es " + nivel[0] + " y la población promedio por planeta es " + nivel[1];
                i++;
            }
            return msj;
        }
        
        //====================== METODO PARA CONSULTA 3 =======================================
        private List<int[]> nivelesPoblacion(ArbolGeneral<Planeta> arbol)
        {	// Declaración de una cola para el recorrido por niveles
            Cola<ArbolGeneral<Planeta>> c = new Cola<ArbolGeneral<Planeta>>();
            // Lista para almacenar la información de población por nivel
            List<int[]> nivelesPobla = new List<int[]>();
            ArbolGeneral<Planeta> arbolAux;

            // Encolamos el árbol raíz y un separador (null) para indicar el nivel
            c.encolar(arbol);
            c.encolar(null);

            int sumaPoblacion = 0;
            int cantPlanetas = 0;

            // Comienza el recorrido por niveles
            while (!c.esVacia())
            {
                arbolAux = c.desencolar();
                // Si es un separador
                if (arbolAux == null)
                {
                    if (!c.esVacia())
                    {	// Encolamos otro separador para indicar el siguiente nivel
                        c.encolar(null);

                        // Calculamos la población total y el promedio de población por planeta del nivel actual
                        int[] nivelPobla = { sumaPoblacion, sumaPoblacion / cantPlanetas };
                        nivelesPobla.Add(nivelPobla);

                        // Reiniciamos las variables para el próximo nivel
                        sumaPoblacion = 0;
                        cantPlanetas = 0;
                    }
                }
                // Si es un nodo (arbol)
                else
                {	// Procesamos la población del planeta actual
                    sumaPoblacion += arbolAux.getDatoRaiz().Poblacion();
                    cantPlanetas++;

                    // Encolamos los hijos del planeta para explorar niveles inferiores
                    foreach (var hijo in arbolAux.getHijos())
                        c.encolar(hijo);
                }
            }

            // Al finalizar el recorrido, agregamos la información del último nivel (raíz)
            int[] nivelPoblaFinal = { sumaPoblacion, sumaPoblacion / cantPlanetas };
            nivelesPobla.Add(nivelPoblaFinal);

            // Se retorna la lista de población por nivel
            return nivelesPobla;
        }

        public Movimiento CalcularMovimiento(ArbolGeneral<Planeta> arbol)
        {	// Se calculan los caminos hacia el bot y el jugador y se los combina
            List<Planeta> caminoHaciaBot = BuscarCaminoHaciaBot(arbol);
            List<Planeta> caminoHaciaJugador = BuscarCaminoHaciaJugador(arbol);
            List<Planeta> caminoBotJugador = BuscarCaminoBotJugador(caminoHaciaBot, caminoHaciaJugador);

            // Estrategia cuando el Bot se encuentra al lado del jugador
            if (caminoBotJugador[1].EsPlanetaDelJugador() && caminoBotJugador[0].Poblacion() / 2 < caminoBotJugador[1].Poblacion())
            {
                // Si el planeta adyacente al bot pertenece al jugador y la población del bot es menor que la mitad de la población del jugador, se implementa una estrategia de refuerzo.
                caminoHaciaBot = BuscarCaminoHaciaBotEstrategia(arbol); 
                //Camino desde el bot con mas naves
                caminoBotJugador = CalcularEstrategia(caminoHaciaBot, caminoHaciaJugador);
            }
            //Se realiza un movimiento entre el bot y el planeta adyacente al mismo
            Movimiento ataque = new Movimiento(caminoBotJugador[0], caminoBotJugador[1]);

            return ataque;
        }

        //============================================ METODOS PARA CALCULAR MOVIMIENTO ======================================
        //Se arma una lista con el camino desde la raiz hasta el primer planeta Bot
        private List<Planeta> BuscarCaminoHaciaBot(ArbolGeneral<Planeta> arbol)
        {
            // Se crea una lista para guardar el camino hacia el planeta del Bot
            List<Planeta> CaminoHaciaBot = new List<Planeta>();

            // Llamo a la función auxiliar para hacer la búsqueda
            BuscarCaminoHaciaBotAux(arbol, CaminoHaciaBot);

            // Retorno la lista
            return CaminoHaciaBot;
        }

        private bool BuscarCaminoHaciaBotAux(ArbolGeneral<Planeta> arbol, List<Planeta> CaminoHaciaBot)
        {	if (arbol == null)
            return false;

            // Se agrega el planeta actual al camino
            CaminoHaciaBot.Add(arbol.getDatoRaiz());

            if (arbol.getDatoRaiz().EsPlanetaDeLaIA())
            {	// Se encontró el planeta del Bot, se termina la búsqueda
                return true;
            }

            // Si no, se recorre los hijos del planeta actual
            foreach (var hijo in arbol.getHijos())
            {	//Si se encontró el planeta del Bot en el subárbol, retornamos true
                if (BuscarCaminoHaciaBotAux(hijo, CaminoHaciaBot))
                {
                	return true;
                }

                // Si no se encontró el planeta en el subárbol, se eliminan los planetas agregados en la lista
                CaminoHaciaBot.RemoveAt(CaminoHaciaBot.Count - 1);
            }

            //Si se retorna false
            return false;
        }

        //Buscar camino hacia el jugador (hace lo mismo que el camino hacia el bot, 
        //una lista con todos los planetas desde la raiz hasta el primer jugador)
        private List<Planeta> BuscarCaminoHaciaJugador(ArbolGeneral<Planeta> arbol)
        {	List<Planeta> CaminoHaciaJugador = new List<Planeta>();

            BuscarCaminoHaciaJugadorAux(arbol, CaminoHaciaJugador);

            return CaminoHaciaJugador;
        }

        private bool BuscarCaminoHaciaJugadorAux(ArbolGeneral<Planeta> arbol, List<Planeta> CaminoHaciaJugador)
        {	CaminoHaciaJugador.Add(arbol.getDatoRaiz());

            if (arbol.getDatoRaiz().EsPlanetaDelJugador())    // Se encontró el planeta del Bot
                return true;

            foreach (var hijo in arbol.getHijos())
            {
                if (BuscarCaminoHaciaJugadorAux(hijo, CaminoHaciaJugador))
                    return true;

                CaminoHaciaJugador.RemoveAt(CaminoHaciaJugador.Count - 1);
            }
            return false;
        }

        // Función para combinar los caminos del bot y el jugador.
        private List<Planeta> BuscarCaminoBotJugador(List<Planeta> caminoHaciaBot, List<Planeta> caminoHaciaJugador)
        {	List<Planeta> caminoBotJugador = new List<Planeta>();
            // Se recorre el camino hacia el bot de forma inversa y se agrega al caminoBotJugador.
            for (int i = caminoHaciaBot.Count - 1; i >= 0; i--)
            {
                caminoBotJugador.Add(caminoHaciaBot[i]);
            }
            // Se recorre el camino hacia el jugador para agregarlo al caminoBotJugador.
            foreach (var planeta in caminoHaciaJugador)
            {	// Si el planeta es del Bot, lo coloca en la primera posición de caminoBotJugador.
                if (planeta.EsPlanetaDeLaIA()) 
                {	//El camino va desde el bot mas cercano al jugador
                    caminoBotJugador[0] = planeta; 
                }
                // Si el planeta es neutral o del jugador, 
                else
                {	//Lo agrega a la siguiente posición de caminoBotJugador.
                    caminoBotJugador.Add(planeta); 
                }
            }
            // Retorna el camino combinado.
            return caminoBotJugador;
        }

        // ESTRATEGIA DE REFUERZO (cuando no alcanzan las naves del bot para atacar al jugador)
        private List<Planeta> BuscarCaminoHaciaBotEstrategia(ArbolGeneral<Planeta> arbol)
        {	List<Planeta> CaminoHaciaBot = new List<Planeta>(); 
            List<Planeta> planetasBot = new List<Planeta>(); 

            // Se obtienen los planetas del BOT y se guardan en la lista planetasBot
            PlanetasBot(arbol, planetasBot);

            // Encontrar el planeta bot con la mayor cantidad de naves
            Planeta planetaMax = PlanetaPoblacionMax(planetasBot);

            // Se busca el camino hacia el planeta del Bot con la mayor cantidad de naves y se guarda en la lista CaminoHaciaBot
            BuscarCaminoHaciaBotEstrategiaAux(arbol, CaminoHaciaBot, planetaMax);
	
            // Retorna la lista del camino calculado anteriormente
            return CaminoHaciaBot; 
        }

        //Funcion recursiva para encontrar los planetas del bot
        private void PlanetasBot(ArbolGeneral<Planeta> arbol, List<Planeta> planetasBot)
        {  // Si el planeta actual es del bot
           if (arbol.getDatoRaiz().EsPlanetaDeLaIA()) 
            {	// Lo agrega a la lista de planetas
                planetasBot.Add(arbol.getDatoRaiz()); 
            }
            foreach (var hijo in arbol.getHijos())
            	// Llama recursivamente a la funcion para los hijos del árbol
                PlanetasBot(hijo, planetasBot); 
        }

        private Planeta PlanetaPoblacionMax(List<Planeta> planetas)
        {	int max = -1;
        	//Recorro la lista de planetas
            foreach (var planeta in planetas) 
            {	// Si la poblacion del planeta actual es mayor a max
                if (planeta.Poblacion() > max) 
                	//El max va a pasar a ser el numero de naves de ese planeta
                    max = planeta.Poblacion(); 
            }
            //Luego se busca al planeta que tenga la poblacion igual a max
            return planetas.Find(p => p.Poblacion() == max); 
        }

        private bool BuscarCaminoHaciaBotEstrategiaAux(ArbolGeneral<Planeta> arbol, List<Planeta> CaminoHaciaBot, Planeta Maximo)
        {	//Agrega el planeta actual al camino hacia el Bot
        	CaminoHaciaBot.Add(arbol.getDatoRaiz());
			//Si se encontró el planeta del Bot con mas naves, retorna true
            if (arbol.getDatoRaiz() == Maximo) 
                return true;

            //Si no, recorre los hijos del planeta actual buscando al planeta Maximo
            foreach (var hijo in arbol.getHijos())
            {
                if (BuscarCaminoHaciaBotEstrategiaAux(hijo, CaminoHaciaBot, Maximo))
                    return true;
                CaminoHaciaBot.RemoveAt(CaminoHaciaBot.Count - 1); 
            }
            return false;
        }

        // Calcular la estrategia para el refuerzo
        private List<Planeta> CalcularEstrategia(List<Planeta> caminoHaciaBot, List<Planeta> caminoHaciaJugador)
        {	// Lista para guardar el camino entre el bot y el jugador
            List<Planeta> caminoBotJugador = new List<Planeta>(); 

            int posPlanetaMax = caminoHaciaBot.Count - 1; 
	 		//Es el ultimo planeta porque en CaminoHaciaBot, va desde la raiz hasta el planeta con mayor cantidad de naves
            if (!caminoHaciaJugador.Contains(caminoHaciaBot[posPlanetaMax]))
            {
                // El camino hacia el jugador no contiene al planeta del Bot con la población máxima
                // Recorre el camino hacia el Bot de forma inversa y lo agrega a caminoBotJugador
                for (int i = posPlanetaMax; i > 0; i--)
                {
                    caminoBotJugador.Add(caminoHaciaBot[i]);
                }
                // Agrega el camino hacia el jugador
                caminoBotJugador.AddRange(caminoHaciaJugador); 
            }
            else
            {
                // El planeta del Bot de población máxima está en el camino hacia el jugador

                caminoBotJugador.Add(caminoHaciaBot[posPlanetaMax]); // Agrega el planeta del Bot al camino

                // Recorre el camino hacia el jugador para agregarlo a caminoBotJugador, pero a partir del planeta del Bot de población máxima
                foreach (var planeta in caminoHaciaJugador)
                {
                    if (!caminoHaciaBot.Contains(planeta)) //si el planeta actual no es del bot 
                        caminoBotJugador.Add(planeta);
                }
            }
            return caminoBotJugador; // Devuelve la estrategia de refuerzo
        }
    }
}