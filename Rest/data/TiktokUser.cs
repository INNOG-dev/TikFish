using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiktokUser
{
    public string uniqueId;

    public string userId;

    public TiktokUser(string uniqueId, string userId)
    {
        this.uniqueId = uniqueId;
        this.userId = userId;
    }

    public override bool Equals(object obj)
    {
        if(obj is TiktokUser)
        {
            TiktokUser user = (TiktokUser)obj;
            if (user.uniqueId == this.uniqueId && user.userId == userId) return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return uniqueId.GetHashCode() + userId.GetHashCode();
    }

    public override string ToString()
    {
        return $"{{ tiktok username = {uniqueId} userid = {userId} }}";
    }
}

