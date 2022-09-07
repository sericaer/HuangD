using HuangD.Interfaces;
using HuangD.Mods.Interfaces;
using Maths;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Entities
{
    public partial class Country
    {
        private static ICountryDef def;

        public static class Builder
        {
            private static IEnumerable<string> names = "夏 周 齐 楚 燕 韩 赵 魏 秦 汉 晋 吴 宋 唐 梁 陈 代 郑 卫 鲁 徐 许 隋 殷 顺 申 虢 虞 雍"
                .Split(" ");

            private static float[] reds;
            private static float[] greens;
            private static float[] blues;

            public static IEnumerable<ICountry> Build(int count, string seed, ICountryDef def)
            {
                Country.def = def;

                var random = new GRandom(seed);
                GenerateRandomColor(random);

                var randomNames = Country.def.names.OrderBy(_ => random.getNum(0, int.MaxValue)).ToArray();

                var list = new List<ICountry>();
                for (int i = 0; i < count; i++)
                {
                    list.Add(new Country(randomNames[i], (reds[i], greens[i], blues[i])));
                }

                return list;
            }

            private static void GenerateRandomColor(GRandom random)
            {
                reds = Enumerable.Range(0, 255).Select(x => x / 255f).OrderBy(_ => random.getNum(0, int.MaxValue)).ToArray();
                greens = Enumerable.Range(0, 255).Select(x => x / 255f).OrderBy(_ => random.getNum(0, int.MaxValue)).ToArray();
                blues = Enumerable.Range(0, 255).Select(x => x / 255f).OrderBy(_ => random.getNum(0, int.MaxValue)).ToArray();

            }
        }
    }
}
