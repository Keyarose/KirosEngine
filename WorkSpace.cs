namespace KirosProject
{
    public class WorkSpace
    {
        private Blueprint _activeBlueprint;
        private List<Item> _itemsInWorkspace;
        
        public void SetBlueprint(Blueprint blueprint)
        {
            _activeBlueprint = blueprint;
        }
        
        public bool CanBuild()
        {
            bool result = true;
            
            List<ComponentAlignment> components = _activeBlueprint.GetComponents();
            
            //can't build since not all required items are in place
            if(_itemsInWorkspace.Count != components.Count)
            {
                return false;
            }
            
            foreach(ComponentAlignment ca in components)
            {
                //find an item in _itemsInWorkspace that matches the conditions of ca
                //if one cant be found return false
                if(_itemsInWorkspace.Find(x => (x.ItemType == ca.ItemType) && (x.MaterialType == ca.MaterialType)) == null)
                {
                    return false;
                }
            }
            
            return result
        }
    }
}