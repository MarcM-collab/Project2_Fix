using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invoker : MonoBehaviour
{
    private static Queue<ICommand> CommandQueue;

    private static List<ICommand> CommandHistory;
    private static int _currentIndex;
    public bool ExecuteInmedaite;

    // Start is called before the first frame update
    void Awake()
    {
        CommandQueue = new Queue<ICommand>();
        CommandHistory = new List<ICommand>();
    }

    // Update is called once per frame
    void Update()
    {
       if(ExecuteInmedaite)
            ExecuteAll();
    }

    public static void ExecuteAll()
    {
        while (CommandQueue.Count > 0)
        {
            ICommand command = CommandQueue.Dequeue();
            command.Excecute();

            CommandHistory.Add(command);
            _currentIndex++;
        }
    }

    public static void AddCommand(ICommand command)
    {
        if (CommandQueue == null)
            CommandQueue = new Queue<ICommand>();

        CommandQueue.Enqueue(command);

        while(CommandHistory.Count >_currentIndex)
        {
            CommandHistory.RemoveAt(_currentIndex);
        }
    }

    public static void Undo()
    {
        if (_currentIndex > 0)
        {
            _currentIndex--;
            CommandHistory[_currentIndex].Undo();
        }
        
    }

    public static void Redo()
    {
        if (_currentIndex < CommandHistory.Count)
        {
            
            CommandHistory[_currentIndex].Excecute();
            _currentIndex++;
        }

    }
}
