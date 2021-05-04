using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpawningAI : CardAIBehaviour
{
    private int _whiskasCombinationAccumulate = 0;
    private int _cardStatsAccumulates = 0;

    private LinkedList<string> qLinkedList = new LinkedList<string>();
    private List<List<int>> _combinations = new List<List<int>>(); //remover publicas
    private List<int> _valueCardStats = new List<int>(); //remover publicas

    public int minimumScore = 7;
    //control de la visualización de las cartas de la  IA
    public Transform IAHandCanvas;
    public float scale = 1f;

    [Header("Display settings")]
    public float betweenSpawnTime = 2;
    public Color selectCardColor;
    public float cardUsageWait = 2;
    [Range(0, 1)] public float cardUsageRandomicity = 0.25f;

    private bool inTurn = false;
    public CardSpawner spawner;

    private float timer;
    private int count;
    private List<int> combinations;
    private bool spellCheck;
    private bool spawnCheck;
    private bool cardSelected;
    private void OnEnable()
    {
        SpawningBehaviour.OnSpawningEnter += SpawningEnter;
        SpawningBehaviour.OnSpawningUpdate += SpawningUpdate;
    }
    private void OnDisable()
    {
        SpawningBehaviour.OnSpawningEnter -= SpawningEnter;
        SpawningBehaviour.OnSpawningUpdate -= SpawningUpdate;
    }

    private void SpawningEnter()
    {
        combinations = CombinationCard(UnitList());
        spellCheck = false;
        spawnCheck = false;
    }
    private void SpawningUpdate(Animator animator)
    {
        Spell priorSpell = GetPriorSpell();
        if (priorSpell) //Has any spell in hand
        {
            SetSelectedHandCard(Random.Range(0, IAHand.Count));
            if (priorSpell.executed && cardUsageWait <= timer)
            {
                Debug.Log("A");
                RemoveCardHand(priorSpell);
                timer = 0;
                spellCheck = true;
            }
        }
        else
        {
            spellCheck = true;
        }

        if (combinations != null)
        {
            combinations.Sort(); //First order [3, 1, 2] --> [1,2,3], then iterate reversly --> [3,2,1] so removing will not be out of the range.
            if (count < combinations.Count)
            {
                if (!cardSelected)
                {
                    SetSelectedHandCard(Random.Range(0, IAHand.Count));
                    cardSelected = true;
                    //indexToRemove.Add(combinations[i]); //Add the index to remove it later, removing an element of the list will change the indexes causing an index out of range error when accessing to an element. List [1] --> List [0] when removing.
                }
                if (cardSelected && cardUsageWait + Random.Range(-cardUsageRandomicity, cardUsageRandomicity) <= timer)//Adds random to make it feel human.
                {
                    spawner.SpawnCard(IAHand[combinations[count]], spawner.GetValidRandomPos(4, 6, -3, 3), Team.TeamAI);
                    RemoveCardHand(IAHand[combinations[count]]);
                    timer = 0;
                    cardSelected = false;
                    count++;
                }
            }
            else
            {
                spawnCheck = true;
            }
        }
        else
        {
            spawnCheck = true;
        }

        if (betweenSpawnTime >= timer && spellCheck && spawnCheck) 
        { 
            EndTurn();
            timer = 0;
            animator.SetBool("IsDragging", false);
        }

        Debug.Log("spellCheck " + spellCheck);
        Debug.Log("spawnCheck " + spawnCheck);
        Debug.Log("timer " + (betweenSpawnTime >= timer));

        timer += Time.deltaTime;
    }
    private Spell GetPriorSpell()  //Will only use the one with more priority so it will not always be throwing spells
    {
        Spell priorSpell = null;

        for (int i = 0; i < IAHand.Count; i++)
        {
            if (IAHand[i] is Spell)
            {
                Spell currentSpell = IAHand[i] as Spell;
                if (!priorSpell || currentSpell.Priority > priorSpell.Priority)
                {
                    priorSpell = currentSpell;
                }
            }
        }
        if (!priorSpell) //Any spell has been added
            return null;

        return priorSpell;
    }
    private void RemoveCardHand(Card cardToRemove)
    {
        IAHand.Remove(cardToRemove);
        TurnManager.SubstractMana(cardToRemove.Whiskas);
        Destroy(cardToRemove.gameObject); //To make it visible that a card has been used.
    }
    private List<int> CombinationCard(List<Card> list)
    {
        _combinations = new List<List<int>>();
        _valueCardStats = new List<int>();
        var qLinkedList = new LinkedList<string>(); // Create an empty queue of strings

        // Enqueue the first binary number
        qLinkedList.AddLast("1");

        // This loops is like BFS of a tree
        // with 1 as root 0 as left child
        // and 1 as right child and so on
        float e = Mathf.Pow(2, list.Count) - 1;
        while (e-- > 0)
        {
            List<int> tempList = new List<int>();
            List<Unit> tempUnit = new List<Unit>();
            // print the front of queue
            string s1 = qLinkedList.First.Value;
            qLinkedList.RemoveFirst();
            int count = 0;
            _cardStatsAccumulates = 0;//e
            for (int i = s1.Length - 1; i >= 0; i--)
            {
                if (s1[i] == '1')
                {
                    _whiskasCombinationAccumulate += list[count].Whiskas;
                    tempUnit.Add(list[count] as Unit);

                    _cardStatsAccumulates += tempUnit[tempUnit.Count - 1].character.Health + tempUnit[tempUnit.Count - 1].character.AttackPoints + tempUnit[tempUnit.Count - 1].Whiskas + 1;

                    tempList.Add(count);//añadimos posición
                }
                count++;
            }

            if (_whiskasCombinationAccumulate <= TurnManager.currentMana)
            {
                _combinations.Add(tempList);//añadimos la combinación con su posición
                _valueCardStats.Add(_cardStatsAccumulates); // su valor
            }

            _whiskasCombinationAccumulate = 0;

            // Store s1 before changing it
            string s2 = s1;

            // Append "0" to s1 and enqueue it
            qLinkedList.AddLast(s1 + "0");

            // Append "1" to s2 and enqueue it.
            // Note that s2 contains the previous front
            qLinkedList.AddLast(s2 + "1");
        }
        int bestCombinationIndex = BestCombination();

        if (_combinations.Count == 0 || _valueCardStats[bestCombinationIndex] < minimumScore)
            return null;

        return _combinations[bestCombinationIndex];

    }
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
    private void SetSelectedHandCard(int v)
    {
        Image[] cardsDiplays = IAHandCanvas.GetComponentsInChildren<Image>();
        for (int i = 0; i < cardsDiplays.Length; i++)
        {
            cardsDiplays[i].GetComponent<Image>().color = Color.white;
        }
        if (v != -1)
            cardsDiplays[v].GetComponent<Image>().color = selectCardColor;
    }
    private void EndTurn()
    {
        TurnManager.Spawned = true;
        SetSelectedHandCard(-1);
    }
    private List<Card> UnitList()
    {
        List<Card> e = new List<Card>();
        for (int i = 0; i < IAHand.Count; i++)
        {
            if (IAHand[i] is Unit)
            {
                e.Add(IAHand[i]);
            }
        }
        return e;

    }
}
