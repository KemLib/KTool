using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KTool.Attribute;

namespace KTool.Loading
{
	public interface ILoader
	{
		
        #region Progperties

        #endregion

        #region Method
        public void GameLoad(Entri entri);
		public void GameInit(Entri entri);
		public void GameStart();
        #endregion
	}
}
