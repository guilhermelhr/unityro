using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Billboard : MonoBehaviour {
    public void LateUpdate() {
        transform.localRotation = Core.MainCamera.transform.rotation;
    }
}
