using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class IACard : MonoBehaviour
{
    [Serializable]
    public struct CardType
    {
        public Card card;
        public int priority;
    }

    public List<CardType> IADeck;//deck hecho por nosotros
    private List<Card> IAHand = new List<Card>(); //de 0 a 6 cartas q tendrá en la mano


    private int _whiskasCombinationAccumulate = 0;
    private int _cardStatsAccumulates = 0;

    private LinkedList<string> qLinkedList = new LinkedList<string>();
    private List<List<int>> _combinations = new List<List<int>>(); //remover publicas
    private List<int> _valueCardStats = new List<int>(); //remover publicas

    public int minimumScore = 7;
    //control de la visualización de las cartas de la  IA
    public Transform IAHandCanvas;
    public float scale = 1f;
    private float maxCardsInHand = 6;
    private Transform cardInstance;
    //-----------------------------

    [Header("Display settings")]
    public Sprite cardSprites;
    public Image[] selectableStartCards;
    public float selectWait = 2;
    [Range(0, 1)] public float selectWaitRandomicity = 0.25f;
    private float currentWaitSelect = 0;
    [Range(0, 2)]public float selectCardFrequency = 1;
    private float currentSelectFrequency = 0;
    public Color selectCardColor;
    public float cardUsageWait = 2;
    [Range(0, 1)] public float cardUsageRandomicity = 0.25f;

    private bool inTurn = false;
    public CardSpawner spawner;

    //public static bool StartAI;
    private void Update()
    {
        //if (TurnManager.TeamTurn == Team.TeamAI)
        //{
        //    if (!inTurn)
        //    {
        //        //if (currentWaitSelect == 0) //Init, avoids extra iterations
        //        //    ShowInitialCards();

        //        //if (currentWaitSelect < selectWait)
        //        //{
        //        //    currentWaitSelect += Time.deltaTime;

        //        //    if (currentWaitSelect >= currentSelectFrequency + Random.Range(-selectWaitRandomicity, selectWaitRandomicity))
        //        //    {
        //        //        SetSelectedInitialCard(Random.Range(0, selectableStartCards.Length));
        //        //        currentSelectFrequency += selectCardFrequency;
        //        //    }
        //        //}
        //        //else if (Mathf.FloorToInt(currentWaitSelect) == Mathf.FloorToInt(selectWait)) //avoids extra executions
        //        //{
        //        //    inTurn = true;
        //        //    currentWaitSelect++;
        //        //    HideInitialCards();
        //        //    StartCoroutine(IATurn());
        //        //}
        //    }
        //}
        //else
        //    inTurn = false;

    }

    //private void ShowInitialCards()
    //{
    //    for (int i = 0; i < selectableStartCards.Length; i++)
    //    {
    //        selectableStartCards[i].gameObject.SetActive(true);
    //        selectableStartCards[i].sprite = cardSprites;
    //    }
    //}
    //private void HideInitialCards()
    //{
    //    SetSelectedInitialCard(-1);
    //    for (int i = 0; i < selectableStartCards.Length; i++)
    //        selectableStartCards[i].gameObject.SetActive(false); //hide cards
    //}
    //private void SetSelectedInitialCard(int v)
    //{
    //    for (int i = 0; i < selectableStartCards.Length; i++)
    //    {
    //        selectableStartCards[i].color = Color.white;
    //    }

    //    if (v != -1) //reset colors
    //        selectableStartCards[v].color = selectCardColor;
    //}

    //private IEnumerator IATurn()
    //{
    //    //if (IAHand.Count < maxCardsInHand)
    //    //    RandomCardChosen();

    //    Spell priorSpell = GetPriorSpell();
    //    if (priorSpell) //Has any spell in hand
    //    {
    //        SetSelectedHandCard(Random.Range(0, IAHand.Count));
    //        yield return new WaitForSeconds(cardUsageWait);
    //        if (priorSpell.executed)
    //            RemoveCardHand(priorSpell);
    //    }


    //    List<int> combinations = CombinationCard(UnitList());
    //    if (combinations != null)
    //    {
    //        combinations.Sort(); //First order [3, 1, 2] --> [1,2,3], then iterate reversly --> [3,2,1] so removing will not be out of the range.
    //        for (int i = combinations.Count - 1; i >= 0; i--)
    //        {
    //            SetSelectedHandCard(Random.Range(0, IAHand.Count));
    //            yield return new WaitForSeconds(cardUsageWait + Random.Range(-cardUsageRandomicity, cardUsageRandomicity)); //Adds random to make it feel human.
    //            spawner.SpawnCard(IAHand[combinations[i]], spawner.GetValidRandomPos(4, 6, -3, 3), Team.TeamAI);
    //            RemoveCardHand(IAHand[combinations[i]]);
    //            //indexToRemove.Add(combinations[i]); //Add the index to remove it later, removing an element of the list will change the indexes causing an index out of range error when accessing to an element. List [1] --> List [0] when removing.
    //        }
    //    }
    //    yield return new WaitForSeconds(selectWait);
    //    EndTurn();
    //}

    //private void EndTurn()
    //{
    //    Debug.Log("A");
    //    //turn.StartMovingIA();
    //    if (EntityManager.GetActiveCharacters(Team.TeamAI).Length > 0)
    //    {
    //        StartAI = true;
    //    }

    //    if (EntityManager.GetActiveCharacters(Team.TeamAI).Length == 0)
    //    {
    //        Debug.Log("A");
    //        TurnManager.NextTurn();
    //    }
    //    SetSelectedHandCard(-1);

    //    currentWaitSelect = 0;
    //    currentSelectFrequency = 0;
    //}
    //private void SetSelectedHandCard(int v)
    //{
    //    Image[] cardsDiplays = IAHandCanvas.GetComponentsInChildren<Image>();
    //    for (int i = 0; i < cardsDiplays.Length; i++)
    //    {
    //        cardsDiplays[i].GetComponent<Image>().color = Color.white;
    //    }
    //    if (v != -1)
    //        cardsDiplays[v].GetComponent<Image>().color = selectCardColor;
    //}
    //private void RemoveCardHand(Card cardToRemove)
    //{
    //    IAHand.Remove(cardToRemove);
    //    TurnManager.SubstractMana(cardToRemove.Whiskas);
    //    Destroy(cardToRemove.gameObject); //To make it visible that a card has been used.
    //}
    //private void AddCardHand(Card toSpawn)
    //{
    //    cardInstance = Instantiate(toSpawn, IAHandCanvas.position, Quaternion.identity).transform;
    //    IAHand.Add(cardInstance.GetComponent<Card>()); //Avoids modifing the prefab
    //    cardInstance.GetComponent<Button>().enabled = false; //Avoids interaction with player
    //    cardInstance.GetComponent<Image>().sprite = cardSprites;
    //    Text[] texts = cardInstance.GetComponentsInChildren<Text>();
    //    foreach (Text t in texts)
    //    {
    //        t.text = "";
    //    }
    //    cardInstance.SetParent(IAHandCanvas);
    //    cardInstance.localScale = new Vector3(scale, scale, scale);//escalamos las cartas que se ven en la mano.
    //}
    /*
    SE ELIGEN (DOS) CARTAS ALEATORIAS
    DEL DECK (NO DE LA MANO).
    */
    //private void RandomCardChosen()
    //{
    //    int random1 = Random.Range(0, IADeck.Count);
    //    int random2 = Random.Range(0, IADeck.Count);

    //    while (random1 == random2)
    //    {
    //        random2 = Random.Range(0, IADeck.Count);
    //    }

    //    AddCardHand(ComproveHand(random1, random2));
    //}
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
    //private Card ComproveHand(int random1, int random2)//comprueba que cartas tiene la IA en su mano.
    //{
    //    // if one and only one of the cards is repited...
    //    bool _firstCardRepe = false;
    //    bool _secondCardRepe = false;

    //    //miramos en la mano cuales tiene.
    //    for (int i = 0; i < IAHand.Count; i++)
    //    {
    //        if (IAHand[i].name == IADeck[random1].card.name) //si el nombre es diferente =>  no la tiene| coge esta y no comprueba las otras.
    //        {
    //            _firstCardRepe = true;
    //        }
    //        else if (IAHand[i].name == IADeck[random2].card.name) //si el nombre es diferente =>  no la tiene
    //        {
    //            _secondCardRepe = true;
    //        }
    //    }
    //    if (!_firstCardRepe && _secondCardRepe)
    //    {
    //        return IADeck[random1].card;
    //    }
    //    else if (!_secondCardRepe && _firstCardRepe)
    //    {
    //        return IADeck[random2].card;
    //    }
    //    else //sistema de prioridad.
    //    {
    //        if (IADeck[random1].priority >= IADeck[random2].priority) //They shouldn't be equal but if they are it will be chosen randomly (the one on the first spot which was randomly assigned)
    //            return IADeck[random1].card;
    //        else
    //            return IADeck[random2].card;
    //    }
    //}
    /*
    COMPRUEBA SI LA MANO DE LA IA TIENE
    ALGÚN HECHIZO. SI ES ASÍ LO GUARDARÁ
    PARA LUEGO COMPROBAR CUAL ES EL HECHIZO
    CON MAYOR PRIORIDAD PARA ASÍ, FACILITAR
    LA POSICIÓN EN LA QUE SE ENCUENTRA EL 
    HECHIZO.
    */
    //private Spell GetPriorSpell()  //Will only use the one with more priority so it will not always be throwing spells
    //{
    //    Spell priorSpell = null;

    //    for (int i = 0; i < IAHand.Count; i++)
    //    {
    //        if (IAHand[i] is Spell)
    //        {
    //            Spell currentSpell = IAHand[i] as Spell;
    //            if (!priorSpell || currentSpell.Priority > priorSpell.Priority)
    //            {
    //                priorSpell = currentSpell;
    //            }
    //        }
    //    }
    //    if (!priorSpell) //Any spell has been added
    //        return null;

    //    return priorSpell;
    //}
    /*
    ESTA FUNCIÓN REALIZA LAS COMBINACIONES
    POSIBLES POR LA IA MEDIANTE EL MANÁ(WHISKAS)
    DISPONIBLE.
    */
    //private List<int> CombinationCard(List<Card> list)
    //{
    //    _combinations = new List<List<int>>();
    //    _valueCardStats = new List<int>();
    //    qLinkedList = new LinkedList<string>(); // Create an empty queue of strings

    //    // Enqueue the first binary number
    //    qLinkedList.AddLast("1");

    //    // This loops is like BFS of a tree
    //    // with 1 as root 0 as left child
    //    // and 1 as right child and so on
    //    float e = Mathf.Pow(2, list.Count) - 1;
    //    while (e-- > 0)
    //    {
    //        List<int> tempList = new List<int>();
    //        List<Unit> tempUnit = new List<Unit>();
    //        // print the front of queue
    //        string s1 = qLinkedList.First.Value;
    //        qLinkedList.RemoveFirst();
    //        int count = 0;
    //        _cardStatsAccumulates = 0;//e
    //        for (int i = s1.Length - 1; i >= 0; i--)
    //        {
    //            if (s1[i] == '1')
    //            {
    //                _whiskasCombinationAccumulate += list[count].Whiskas;
    //                tempUnit.Add(list[count] as Unit);

    //                _cardStatsAccumulates += tempUnit[tempUnit.Count - 1].character.Health + tempUnit[tempUnit.Count - 1].character.AttackPoints + tempUnit[tempUnit.Count - 1].Whiskas + 1;

    //                tempList.Add(count);//añadimos posición
    //            }
    //            count++;
    //        }

    //        if (_whiskasCombinationAccumulate <= TurnManager.currentMana)
    //        {
    //            _combinations.Add(tempList);//añadimos la combinación con su posición
    //            _valueCardStats.Add(_cardStatsAccumulates); // su valor
    //        }

    //        _whiskasCombinationAccumulate = 0;

    //        // Store s1 before changing it
    //        string s2 = s1;

    //        // Append "0" to s1 and enqueue it
    //        qLinkedList.AddLast(s1 + "0");

    //        // Append "1" to s2 and enqueue it.
    //        // Note that s2 contains the previous front
    //        qLinkedList.AddLast(s2 + "1");
    //    }
    //    int bestCombinationIndex = BestCombination();

    //    if (_combinations.Count == 0|| _valueCardStats[bestCombinationIndex] < minimumScore)
    //        return null;

    //    return _combinations[bestCombinationIndex];

    //}






    //private List<Card> UnitList()
    //{
    //    List<Card> e = new List<Card>();
    //    for (int i = 0; i < IAHand.Count; i++)
    //    {
    //        if (IAHand[i] is Unit)
    //        {
    //            e.Add(IAHand[i]);
    //        }
    //    }
    //    return e;

    //}
    /*
    ESTA FUNCIÓN REALIZA LA COMBINACIÓN
    QUE MÁS PUNTOS DE STATS CONTIENE.

    STATS: SUMA DE VALORES.
    SUMA VIDA, WHISKAS Y PODER DE TODAS 
    LAS CARTAS DE LA COMBINACIÓN, MÁS LA CANTIDAD
    DE CARTAS QUE TIENE LA COMBINACIÓN.

    (EJ: SI SON 3 CARTAS = +3)
    */
    //private int BestCombination()//elige la carta con mayor stats independientemente del valor del mana (siempre < o = que current)
    //{
    //    int max = 0;
    //    int maxPos = 0;

    //    for (int i = 0; i < _valueCardStats.Count; i++)
    //    {
    //        if (_valueCardStats[i] > max)
    //        {
    //            max = _valueCardStats[i];
    //            maxPos = i;

    //        }
    //    }
    //    return maxPos;
    //}
}
