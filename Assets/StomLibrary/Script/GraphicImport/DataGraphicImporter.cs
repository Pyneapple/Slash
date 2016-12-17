using UnityEngine;
using System.Collections;

namespace Stom
{
#if UNITY_EDITOR
    public enum TypeCopy
    {
        Default,
        PrefixName
    }

    public class DataGraphicImporter : ScriptableObject
    {
        public string graphicPath;
        public string assetPath;
        public string texturePackerPath;

        public TypeCopy typeCopy;
        public string nameFolder;
        public string sourcePath;
        public string destinationPath;
    }
#endif
}
