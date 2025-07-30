﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    [StructLayout(LayoutKind.Sequential,Pack =1)]
   public struct BagItem
    {
        public ushort ItemId;
        public ushort Count;
        public static BagItem zero = new BagItem { ItemId =0 ,Count = 0 };
        public BagItem(int itemId,int count)
        {
            this.ItemId = (ushort)itemId;
            this.Count = (ushort)count;
        }

        public static bool operator ==(BagItem lhs, BagItem rhs)
        {
            return lhs.ItemId == rhs.ItemId && lhs.Count == rhs.Count;
        }
        public static bool operator !=(BagItem lhs, BagItem rhs)
        {
            return !(lhs == rhs) ;
        }
        /// <summary>
        ///     <para>Returns true if the objects are equal.</para>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is BagItem)
            {
                return Equals((BagItem)obj);
            }
            return false;
        }
        public bool Equals(BagItem other)
        {
            return this == other;
        }
        public override int GetHashCode()
        {
            return ItemId.GetHashCode() ^ (Count.GetHashCode() << 2);
        }

    }
}
