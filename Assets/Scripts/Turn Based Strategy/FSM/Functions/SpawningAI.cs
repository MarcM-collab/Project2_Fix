using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpawningAI : CardAIBehaviour
{
    private Animator anim;
    private int _whiskasCombinationAccumulate = 0;
    private int _cardStatsAccumulates = 0;
    private bool NoPossibleTiles;

    //private LinkedList<string> qLinkedList = new LinkedList<string>();
    private List<List<Unit>> _combinations = new List<List<Unit>>(); //remover publicas
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
    public SpellSpawner spellSpawn;
    private void OnEnable()
    {
        SpawningBehaviour.OnSpawningEnter += SpawningEnter;
    }
    private void OnDisable()
    {
        SpawningBehaviour.OnSpawningEnter -= SpawningEnter;
    }
    private void Start()
    {
        anim = GetComponentInParent<Animator>();
    }
    private void SpawningEnter()
    {
        StartCoroutine(IATurn());
    }
    private IEnumerator IATurn()
    {
        //if (IAHand.Count < maxCardsInHand)
        //    RandomCardChosen();

        Spell priorSpell = GetPriorSpell();
        if (priorSpell && priorSpell.CanBeUsed()) //Has any spell in hand, spells are only used if there are units of the player ingame
        {
            SetSelectedHandCard(Random.Range(0, IAHand.Count));
            yield return new WaitForSeconds(cardUsageWait);
            spellSpawn.IASpawn(priorSpell);
            RemoveCardHand(priorSpell);
        }


        List<Unit> combinations = CombinationCard(UnitList());
        if (combinations != null)
        {
            //combinations.Sort(); //First order [3, 1, 2] --> [1,2,3], then iterate reversly --> [3,2,1] so removing will not be out of the range.
            for (int i = 0; i < combinations.Count; i++)
            {
                SetSelectedHandCard(Random.Range(0, IAHand.Count));

                yield return new WaitForSeconds(cardUsageWait + Random.Range(-cardUsageRandomicity, cardUsageRandomicity)); //Adds random to make it feel human.
                var position = GetValidRandomPos(2, 4, -2, 2);
                if (!NoPossibleTiles) 
                {
                    spawner.SpawnCard(combinations[i], position, Team.TeamAI);
                    RemoveCardHand(combinations[i]);
                }
                else
                {
                    Debug.Log("NoTilesToSpawn");
                    break;
                }
                //indexToRemove.Add(combinations[i]); //Add the index to remove it later, removing an element of the list will change the indexes causing an index out of range error when accessing to an element. List [1] --> List [0] when removing.
            }
        }
        yield return new WaitForSeconds(cardUsageWait);
        EndTurn();
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
    private List<Unit> CombinationCard(List<Card> list)
    {
        _combinations = new List<List<Unit>>();
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
            //List<int> tempList = new List<int>();
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

                    _cardStatsAccumulates += (int)tempUnit[tempUnit.Count - 1].character.HP + tempUnit[tempUnit.Count - 1].character.AttackPoints + tempUnit[tempUnit.Count - 1].Whiskas + 1;

                    //tempList.Add(count);//añadimos posición
                }
                count++;
            }

            if (_whiskasCombinationAccumulate <= TurnManager.currentMana)
            {
                //_combinations.Add(tempList);//añadimos la combinación con su posición
                _combinations.Add(tempUnit);
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
        anim.SetBool("IsDragging", false);
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
    public Vector2 GetValidRandomPos(int minX, int maxX, int minY, int maxY)
    {
        NoPossibleTiles = false;
        int tilesLength = 0;
        for (int i = minX; i < maxX; i++)
        {
            for (int v = minY; v < maxY; v++)
            {
                tilesLength++;
            }
        }
        List<Vector2> testedPos = new List<Vector2>();
        Vector2 pos;
        do
        {
            pos = new Vector2(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY));

            if (!testedPos.Contains(pos))
                testedPos.Add(pos);

            if (testedPos.Count >= tilesLength)
            {
                NoPossibleTiles = true;
                break;
            }
        }
        while (!CheckPos(pos));

        return pos;
    }
    public bool CheckPos(Vector2 pos)
    {
        var vector = new Vector3(pos.x, pos.y) + TileManager.CellSize;
        RaycastHit2D rayCast = Physics2D.Raycast(vector, Vector3.zero, Mathf.Infinity);
        var rayCastCollider = rayCast.collider;
        if (rayCastCollider != null)
        {
            var gameObject = rayCastCollider.gameObject;
            if (!(gameObject.GetComponent("Character") as Character is null))
            {
                return false;
            }
        }
        return true;
    }
}
