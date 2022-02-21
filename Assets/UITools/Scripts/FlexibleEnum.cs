using System;

namespace UITools
{
    [Serializable]
    public class FlexibleEnum
    {
        public enum FlexibleEnumTypes
        {
            Text = 0,
            TextMesh,
            Button,
            Toggle
        }

        public string enumSelected;
        public FlexibleEnumTypes flexibleEnumType;

        public FlexibleEnum(FlexibleEnumTypes flexibleEnumType)
        {
            this.flexibleEnumType = flexibleEnumType;
        }
    }
}