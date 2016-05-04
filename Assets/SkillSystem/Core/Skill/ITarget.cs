using UnityEngine;
using System.Collections;

public interface ITarget {
    Transform Transform { get; }
    Transform GetMount(string name);
}
