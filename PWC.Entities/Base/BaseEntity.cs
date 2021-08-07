using System;
using PWC.Entities.Interface;

namespace PWC.Entities.Base
{
    [Serializable]
    public class BaseEntity : IEntity
    {
        #region Properties

        public int? ID { set; get; }

        #endregion
    }
}
