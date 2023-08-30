using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace krak.Api
{
    public enum Opcode
    {
        GetServerVersion,
        JoinWorld,
        GetGems,
        Restart,
        Leave,
        LockWorldData, 
        itemIDs,
        Say,
        Move,
        World,
        Gems
    }
}
