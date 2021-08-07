using System;

namespace PWC.Infrastructure
{
    /// <summary>
    /// Defines an interface for objects whose creation and modified dates are kept track of.
    /// </summary>
    public interface IObsoletable
    {
        DateTime? ObsoletionDate { get; set; }


    }
}

