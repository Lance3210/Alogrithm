using DataStructure.哈希表.哈希函数;
using System;

namespace DataStructure.测试
{
	public class Person : IComparable<Person>
	{
		public int age = 0;
		public float height = 0f;
		public string name = "";

		public Person(int age, float height, string name)
		{
			this.age = age;
			this.height = height;
			this.name = name;
		}

		//重写GetHashCode
		public override int GetHashCode()
		{
			int hashCode = age.GetHashCodeFunc();//将其所有信息当做一个string，每个信息当做一个char
			hashCode = (hashCode << 5) - hashCode + height.GetHashCodeFunc();
			hashCode = (hashCode << 5) - hashCode + (name != null ? name.GetHashCodeFunc() : 0);
			return hashCode;
		}

		//重写Equals
		public override bool Equals(object obj)
		{
			//内存地址相同
			if (this == obj)
			{
				return true;
			}
			//能调用该方法必不为空，以及类型判断（不推荐用 is 因为有可能涉及到继承）
			if (obj == null || obj.GetType() != GetType())
			{
				return false;
			}
			//能到这里必是同类型，比较成员变量即可
			Person p = obj as Person;
			return (p.age == age) && (p.height == height) && (p.name == null ? name == null : p.name.Equals(name));
		}

		//要求实现CompareTo
		public int CompareTo(Person p2)
		{
			if (age > p2.age || height > p2.height)
			{
				return 1;
			}
			else if (age < p2.age || height < p2.height)
			{
				return -1;
			}
			else
			{
				return 0;
			}
		}
	}
}