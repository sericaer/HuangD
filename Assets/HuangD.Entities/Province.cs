using HuangD.Interfaces;

namespace HuangD.Entities
{
    public partial class Province : IProvince
    {
        public string name { get; set; }

        public (float r, float g, float b) color { get; set; }

        public Province(string name, (float, float, float) color)
        {
            this.name = name;
            this.color = color;
        }
    }
}
