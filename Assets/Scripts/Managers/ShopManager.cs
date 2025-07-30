using Assets.Scripts.Services;
using Common.Data;
using Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Managers
{
    public class ShopManager : Singleton<ShopManager>
    {
        UIShop uiShop = null;
        public void Init()
        {
            NPCManager.Instance.RegisterNpcEvent(Common.Data.NpcFunction.InvokeShop, OnpenShop);
        }

        private bool OnpenShop(NpcDefine npc)
        {
            this.ShowShop(npc.Param);
            return true;
        }

        public void ShowShop(int shopId)
        {
            uiShop = UIManager.Instance.Show<UIShop>();
            if (uiShop != null && uiShop.gameObject != null)
            {
                ShopDefine shop;
                if (DataManager.Instance.Shops.TryGetValue(shopId, out shop))
                {
                    if (uiShop != null)
                    {
                        uiShop.SetShop(shop);
                    }
                }
            }
        }

        public void RefreshShopMoney()
        {
            if (uiShop != null && uiShop.gameObject != null)
            {
                uiShop.UpdateMoney();
            }
        }

        public bool BuyItem(int shopId, int shopItemId)
        {
            ItemService.Instance.SendBuyItem(shopId, shopItemId);
            return true;
        }
    }
}
