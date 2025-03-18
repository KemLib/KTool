using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KTool
{
    public class Entri : IEntri
    {
        #region Progperties
        private string name;
        private float progress;
        private bool isDone;

        public string Name
        {
            get => name;
            set => name = value ?? string.Empty;
        }
        public float Progress
        {
            get => progress;
            set => progress = Mathf.Clamp(value, 0, 1);
        }
        public bool IsDone => isDone;
        #endregion

        #region Construction
        public Entri()
        {
            name = string.Empty;
            progress = 0;
            isDone = false;
        }
        #endregion

        #region Method
        public void Done()
        {
            isDone = true;
        }
        #endregion
    }
}
