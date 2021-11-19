namespace Algorithm.Util {
	//测试物品类
	public class Article {
		public int Weight { get; set; }
		public int Value { get; set; }
		public double ValueDensity { get; set; }

		public Article(int weight, int value) {
			Weight = weight;
			Value = value;
			ValueDensity = value * 1.0 / weight;
		}
		public override string ToString() {
			return "Weight:" + Weight + "  Value: " + Value + "  ValueDensity: " + ValueDensity;
		}
	}
}
