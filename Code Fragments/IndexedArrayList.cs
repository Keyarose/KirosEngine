namespace KirosEngine.Collections
{
    class IndexedArrayList<T>
    {
        enum IndexSchema
        {
            Alphabetic,
            Numeric,
            AlphaNumeric,
            Unicode
        }
        
        protected IndexSchema _schema;
        protected bool _forceSchema;
        
        protected int[] _indexs;
        protected ArrayList<T>;
        
        public IndexedArrayList(IndexSchema schema)
        {
            _schema = schema;
            switch(schema)
            {
                case Alphabetic:
                {
                    if(T.type.isEqual(int) || T.type.isEqual(float) || T.type.isEqual(double))//or any other numeric
                    {
                        throw new SchemaMismatchException(this, "Schema is set to Alphabetic, while the type is Numeric.  Ether Numeric or Alphanumeric is recomended.");
                        
                        if(!_forceSchema)
                        {
                            return null;
                        }
                    }
                    
                    _indexs = new int[26];
                    break;
                }
                
                case Numeric:
                {
                    _indexs = new int[10];
                    break;
                }
                
                case AlphaNumeric
                {
                    _indexs = new int[36];
                    break; 
                }
                
                case Unicode:
                {
                    _indexes = new int[int.MAX];
                    break;
                }
            }
        }
        
        public bool Insert(T item)
        {
        
        }
    }
}