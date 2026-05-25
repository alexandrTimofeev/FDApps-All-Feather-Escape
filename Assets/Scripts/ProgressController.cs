using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressController : MonoBehaviour
{
    public List<Sprite> progresses;
    public Sprite SetProgress(int progress)
    {
        return progresses[progress];
    }
}
