using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactRandomJokes.Data
{
    public enum InteractionStatus
    {
        Unauthenticated,
        Liked,
        Disliked,
        NeverInteracted,
        CanNoLongerInteract
    }
}
