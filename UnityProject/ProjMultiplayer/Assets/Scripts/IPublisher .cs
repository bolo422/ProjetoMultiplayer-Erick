using UnityEditor;
using UnityEngine;

public interface IPublisher
{
    void Subscribe(ISubscriber subs);
}