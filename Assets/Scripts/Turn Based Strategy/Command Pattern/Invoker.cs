using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Invoker
{
    private static Queue<ICommand> CommandQueue;
    private static List<ICommand> CommandHistory;

    public static int CurrentIndex;

    [RuntimeInitializeOnLoadMethod]
    private static void InitCommands()
    {
        CommandQueue = new Queue<ICommand>();
        CommandHistory = new List<ICommand>();
    }

    public static void ExecuteNext()
    {
        while (CommandQueue.Count > 0)
        {
            ICommand command = CommandQueue.Dequeue();
            command.Excecute();

            CommandHistory.Add(command);
            CurrentIndex++;
        }
    }

    public static void AddCommand(ICommand command)
    {
        if (CommandQueue == null)
            CommandQueue = new Queue<ICommand>();

        CommandQueue.Enqueue(command);

        while (CommandHistory.Count > CurrentIndex)
        {
            CommandHistory.RemoveAt(CurrentIndex);
        }
    }
}