using System;

namespace Force.Meta
{
    public class MaskAttribute: Attribute
    {
        public MaskAttribute(string mask)
        {
            Mask = mask;
        }

        public string Mask { get; protected set; }
    }
}