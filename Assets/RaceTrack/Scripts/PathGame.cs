using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGame : MonoBehaviour {
    public Color lineColor;
    private List<Transform> node = new List<Transform>();
    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;
        Transform[] path = GetComponentsInChildren<Transform>();

        node = new List<Transform>();

        for(int i = 0; i < path.Length; i++)
        {
            if (path[i] != transform)
            {
                node.Add(path[i]);
            }
        }
        for(int i = 0; i < node.Count; i++)
        {
            Vector3 currentNode = node[i].position;
            Vector3 pre=Vector3.zero;
            if (i > 0)
            {
                pre = node[i - 1].position;

            }else if(i==0 && node.Count > 1)
            {
                pre = node[node.Count - 1].position;
            }
            Gizmos.DrawLine(pre, currentNode);
            Gizmos.DrawWireSphere(currentNode, 0.3f);
        }
    }
}
