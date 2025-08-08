using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift 
{

    public GiftData data;

    public TiktokUser user;

    public class GiftData
    {
        public int giftId;

        public string giftName;

        public int count;

        public int value;

        public GiftData()
        {

        }

        public override bool Equals(object obj)
        {
            if (!(obj is GiftData)) return false;

            GiftData other = (GiftData) obj;

            return giftId == other.giftId;
        }

        public override int GetHashCode()
        {
            return giftId.GetHashCode();
        }
    }
}
