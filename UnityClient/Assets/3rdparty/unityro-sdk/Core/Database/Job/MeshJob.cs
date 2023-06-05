using System.Collections.Generic;
using UnityEngine;
using UnityRO.Core.GameEntity;

namespace UnityRO.Core.Database {
    
    [CreateAssetMenu(menuName = "Database Entry/Mesh Job")]
    public class MeshJob : Job {
        public CoreMeshGameEntity Female;
        public CoreMeshGameEntity Male;
        public List<Material> ColorsMale;
        public List<Material> ColorsFemale;
    }
}