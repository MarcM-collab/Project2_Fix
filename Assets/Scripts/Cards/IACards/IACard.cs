using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IACard : MonoBehaviour
{
    [Serializable]
    public struct CardType
    {
        public Card card;
        public int priority;
    }

    public List<CardType> IADeck;//deck hecho por nosotros
    public List<Card> IAHand; //de 0 a 6 cartas q tendrá en la mano

    public Whiskas _whiskas;

    private int _whiskasCombinationAccumulate = 0;
    private int _cardStatsAccumulates = 0;
    private int _currentWhiskas => _whiskas.currentWhiskas;

    LinkedList<string> q = new LinkedList<string>();
    public List<List<int>> _combinations = new List<List<int>>(); //remover publicas
    public List<int> _valueCardStats = new List<int>(); //remover publicas

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            RandomCardChosen();
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (IAHasSpells())
            {
                //función que realiza el hechizo.
            }
            else
            {
                //busca combinaciones de ls posibles cartas con los mana CombinationCard();
                //elif. BestCombination();
            }
        }
    }
    /*
    SE ELIGEN (DOS) CARTAS ALEATORIAS
    DEL DECK (NO DE LA MANO).
    */
    private void RandomCardChosen()
    {
        int random1 = UnityEngine.Random.Range(0, IADeck.Count);
        int random2 = UnityEngine.Random.Range(0, IADeck.Count);

        while (random1 == random2)
        {
            random2 = UnityEngine.Random.Range(0, IADeck.Count);
        }
        IAHand.Add(ComproveHand(random1,random2));
    }
    /*
    LA FUNCIÓN REVISA SI LAS (DOS) CARTAS
    QUE ROBA A LA VEZ LA IA LAS TIENE O NO PARA
    NO REPETIR EN PRIMER LUGAR.

    SI UNA DE LAS (DOS) CARTAS NO LAS TIENE,
    COGERÁ LA QUE NO TIENE.

    EN CASO DE QUE SE REPITAN (AMBAS), 
    SELECCIONARÁ LA CARTA CON MAYOR 
    PRIORIDAD
    */
    private Card ComproveHand(int random1, int random2)//comprueba que cartas tiene la IA en su mano.
    {
        bool _firstCardRepe = false;
        bool _secondCardRepe = false;
        //miramos en la mano cuales tiene.
        for (int i = 0; i < IAHand.Count; i++)
        {
            if (IAHand[i].name == IADeck[random1].card.name ) //si el nombre es diferente =>  no la tiene| coge esta y no comprueba las otras.
            {
                print("PRIMERA repe " + IADeck[random1].card.name);
                _firstCardRepe = true;
            }
            else if (IAHand[i].name == IADeck[random2].card.name) //si el nombre es diferente =>  no la tiene
            {
                print("SEGUNDA repe " + IADeck[random2].card.name);
                _secondCardRepe = true;
            }
        }
        if (!_firstCardRepe)
            return IADeck[random1].card;
        else if (!_secondCardRepe)
            return IADeck[random2].card;
        else //sistema de prioridad.
        {
            if (IADeck[random1].priority < IADeck[random2].priority)
                return IADeck[random2].card;
            else
                return IADeck[random1].card;
        }                        
    }
    /*
    COMPRUEBA SI LA MANO DE LA IA TIENE
    ALGÚN HECHIZO. SI ES ASÍ LO GUARDARÁ
    PARA LUEGO COMPROBAR CUAL ES EL HECHIZO
    CON MAYOR PRIORIDAD PARA ASÍ, FACILITAR
    LA POSICIÓN EN LA QUE SE ENCUENTRA EL 
    HECHIZO.
    */
    private bool IAHasSpells()
    {
        int maxPriority = 0;//prioridad mas alta
        int spellPriorityPos = 0; //pos 0 por default.
        int sCounter = 0;

        List<Spells> spellsList = new List<Spells>();
        //vamos a revisar que cartas de hechizo hay y cual es la mayor prioridad.
        for (int i = 0; i < IAHand.Count; i++)
        {
            if(IAHand[i] is Spells)
            {
                spellsList.Add(IAHand[i] as Spells);
                print("algun hezho entró");
                if(spellsList[sCounter].Priority > maxPriority)
                {
                    maxPriority = spellsList[sCounter].Priority;
                    spellPriorityPos = i;
                    sCounter++;
                }
            }          
        }
        print(spellsList.Count);
        return spellsList.Count !=0;
    }
    
    /*
    ESTA FUNCIÓN REALIZA LAS COMBINACIONES
    POSIBLES POR LA IA MEDIANTE EL MANÁ(WHISKAS)
    DISPONIBLE.
    */
    private LinkedList<string> CombinationCard()
    {
        _combinations = new List<List<int>>();
        _valueCardStats = new List<int>();
        q = new LinkedList<string>();

        // Create an empty queue of strings
        int lenghtHand = IAHand.Count;
        // Enqueue the first binary number
        q.AddLast("1");

        // This loops is like BFS of a tree
        // with 1 as root 0 as left child
        // and 1 as right child and so on
        float e = Mathf.Pow(2, lenghtHand)-1;
        while (e-- > 0)
        {
            List<int> tempList = new List<int>();
            List<Unit> tempUnit = new List<Unit>();
            // print the front of queue
            string s1 = q.First.Value;
            q.RemoveFirst();
            int count = 0;
           
            for (int i = s1.Length-1; i >= 0; i--)
            {
                if(s1[i] == '1')
                {
                    _whiskasCombinationAccumulate += IAHand[count].Whiskas;
                    if(IAHand[count] is Unit)
                    {
                        tempUnit.Add(IAHand[count] as Unit);
                        _cardStatsAccumulates += tempUnit[count].Health + tempUnit[count].Power + tempUnit[count].Whiskas + 1;
                    }
                   
                    tempList.Add(count);//añadimos posición
                }
                
                print(IAHand[count].name);
                count++;
            }
            print(s1+":posiciones |  mana total:     "+_whiskasCombinationAccumulate);

            if (_whiskasCombinationAccumulate <= _currentWhiskas)
            {
                _combinations.Add(tempList);//añadimos la combinación con su posición
                _valueCardStats.Add(_cardStatsAccumulates); // su valor
            }
            
            _whiskasCombinationAccumulate = 0;
           
            // Store s1 before changing it
            string s2 = s1;

            // Append "0" to s1 and enqueue it
            q.AddLast(s1 + "0");

            // Append "1" to s2 and enqueue it.
            // Note that s2 contains the previous front
            q.AddLast(s2 + "1");
        }
        return q;
    }
    /*
    ESTA FUNCIÓN REALIZA LA COMBINACIÓN
    QUE MÁS PUNTOS DE STATS CONTIENE.

    STATS: SUMA DE VALORES.
    SUMA VIDA, WHISKAS Y PODER DE TODAS 
    LAS CARTAS DE LA COMBINACIÓN, MÁS LA CANTIDAD
    DE CARTAS QUE TIENE LA COMBINACIÓN.

    (EJ: SI SON 3 CARTAS = +3)
    */
    private int BestCombination()//elige la carta con mayor stats independientemente del valor del mana (siempre < o = que current)
    {
        int max = 0;
        int maxPos = 0;

        for (int i = 0; i < _valueCardStats.Count; i++)
        {
            if (_valueCardStats[i] > max)
            {
                max = _valueCardStats[i];
                maxPos = i;

            }
        }
        return maxPos;
    }
}
