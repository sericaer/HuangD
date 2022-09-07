using HuangD.Interfaces;
using HuangD.Mods;
using HuangD.Mods.Interfaces;
using Maths;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Entities
{
    public partial class Province
    {
        private static IEnumerable<string> names = "充 耀 眉 肃 葛 盖 柏 樊 昆 辉 英 应 岐 恒 宛 尧 舜 禹 雁 延 苍 原 运 瀚 淄 蔚 华 同 坊 丹 宁 庆 定 绥 银 灵 盐 铜 丰 会 宥 胜 麟 孟 虢 许 蔡 亳 濮 泗 海 兖 宿 密 青 棣 慈 岚 石 朔 云 怀 相 澶 博 贝 洛 磁 邢 冀 深 沧 景 德 祁 易 瀛 莫 幽 涿 檀 平 顺 归 营 威 慎 玄 崇 夷 师 鲜 带 黎 沃 昌 义 瑞 信 凛 襄 兴 凤 利 洋 泽 合 集 巴 蓬 壁 商 金 开 渠 渝 邓 均 房 郢 复 夔 万 忠 楚 和 濠 寿 光 蕲 申 黄 安 舒 润 湖 越 明 睦 建 汀 宣 池 洪 虔 抚 吉 江 袁 岳 潭 衡 朗 永 道 邵 连 辰 施 巫 播 思 费 溪 珍 鄯 成 临 河 武 廓 叠 宕 甘 彭 剑 梓 遂 普 陵 资 荣 简 嘉 邛 雅 茂 涂 炎 彻 向 冉 穹 笮 戎 嵩 松 文 扶 当 悉 恭 保 真 霸 柘 循 贺 端 康 封 高 藤 窦 勤 昭 富 梧 蒙 龚 浔 郁 琴 宾 澄 象 融 邕 横 严 峦 容 岩 芝 禄 长 廉 雷 化 振"
            .Split(" ");

        private static float[]  reds;
        private static float[]  greens;
        private static float[]  blues;

        private static IProvinceDef def;

        public static class Builder
        {
            public static IEnumerable<IProvince> Build(int count, string seed, IProvinceDef def)
            {
                Province.def = def;

                var random = new GRandom(seed);
                GenerateRandomColor(random);

                var randomNames = Province.def.names.OrderBy(_ => random.getNum(0, int.MaxValue)).ToArray();

                var list = new List<IProvince>();
                for (int i = 0; i < count; i++)
                {
                    list.Add(new Province(randomNames[i], (reds[i], greens[i], blues[i])));
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
