namespace DataStructure.队列 {
	public abstract class MyQueue<T> {
		protected int size;
		public int Size => size;
		public abstract bool IsEmpty();
		public abstract void Clear();
		public abstract void Enqueue(T element);
		public abstract T Dequeue();
		public abstract T GetFront();
	}
}
