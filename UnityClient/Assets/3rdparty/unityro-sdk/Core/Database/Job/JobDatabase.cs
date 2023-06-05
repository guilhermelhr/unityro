using System.Collections.Generic;
using UnityEngine;

namespace UnityRO.Core.Database {

    [CreateAssetMenu(menuName = "Database/Job")]
    public class JobDatabase : ScriptableObject {
        public List<Job> Values;
    }
}