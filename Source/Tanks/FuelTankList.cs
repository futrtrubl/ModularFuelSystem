using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections.ObjectModel;
using KSPAPIExtensions;
using KSPAPIExtensions.PartMessage;

// ReSharper disable InconsistentNaming, CompareOfFloatsByEqualityOperator

namespace RealFuels.Tanks
{
	public class FuelTankList : KeyedCollection<string, FuelTank>, IConfigNode
	{
		public FuelTankList ()
		{
		}

		public FuelTankList (ConfigNode node)
		{
			Load (node);
		}

		protected override string GetKeyForItem (FuelTank item)
		{
			return item.name;
		}

		public void Load (ConfigNode node)
		{
			if (node == null)
				return;
			foreach (ConfigNode tankNode in node.GetNodes ("TANK")) {
                if (tankNode.HasValue("name"))
                {
                    if (!Contains(tankNode.GetValue("name")))
                        Add(new FuelTank(tankNode));
                    else
                    {
                        Debug.LogWarning("[MFS] Ignored duplicate definition of TANK of type " + tankNode.GetValue("name"));
                    }
                }
                else
                    Debug.LogWarning("[MFS] TANK node invalid, lacks name");
			}
		}

		public void Save (ConfigNode node)
		{
			foreach (FuelTank tank in this) {
				ConfigNode tankNode = new ConfigNode ("TANK");
				tank.Save (tankNode);
				node.AddNode (tankNode);
			}
		}

        public void TechAmounts()
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (!this[i].canHave)
                    this[i].maxAmount = 0;
            }
        }
	}
}
