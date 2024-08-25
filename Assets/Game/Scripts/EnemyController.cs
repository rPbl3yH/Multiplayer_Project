using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

namespace Game.Core
{
    public class EnemyController : MonoBehaviour
    {
        public void OnStateChanged(List<DataChange> changes)
        {
            var position = transform.position;
            
            foreach (DataChange dataChange in changes)
            {
                switch (dataChange.Field)
                {
                    case "x":
                        position.x = (float)dataChange.Value;
                        break;
                    case "y":
                        position.z = (float)dataChange.Value;
                        break;
                    default:
                        Debug.Log($"No handler for data {dataChange.Field}");
                        break;
                }
            }
            
            transform.position = position;
        }
    }
}