namespace Rick.RandomBags{
	public interface Bag<t>  {
		
		t next();
		
		void reset();
	}
}