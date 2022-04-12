using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    List<(T, float)> elements;

    public PriorityQueue(){
        elements = new List<(T, float)>();
    }

    public int Count{
        get { return elements.Count; }
    }
    public void Enqueue(T element, float priority){
        elements.Add((element, priority));
    }

    public T DeQueue(){
        int bestIndex = 0;
        for (int i = 0; i < Count; i++)
        {
            if (elements[i].Item2 < elements[bestIndex].Item2){
                bestIndex = i;
            }
        }

        T bestElement = elements[bestIndex].Item1;
        elements.RemoveAt(bestIndex);
        return bestElement;
    }

    public bool isEmpty(){
        return Count == 0;
    }

    // public void Peek(){}
}
