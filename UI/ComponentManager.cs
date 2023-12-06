namespace ACL.UI
{
    public class ComponentManager
    {
        public List<Component> ComponentsList = new List<Component>();

        #region List Methods

        // Methods for adding/removing a component from the list.
        public void AddComponents(params Component[] Paramcomponents)
        {
            foreach (var component in Paramcomponents)
            {
                ComponentsList.Add(component);
            }
        }

        public void RemoveComponents(params Component[] Paramcomponents)
        {
            foreach (var component in Paramcomponents)
            {
                ComponentsList.Remove(component);
            }
        }

        #endregion

        #region Manage Methods

        public void DisableComponents(params Component[] Paramcomponents)
        {
            foreach (var component in Paramcomponents)
            {
                component.ToDraw = false; component.ToUpdate = false;
            }
        }

        public void EnableComponents(params Component[] Paramcomponents)
        {
            foreach (var component in Paramcomponents)
            {
                component.ToDraw = true; component.ToUpdate = true;
            }
        }

        public void ModifyComponents(IEnumerable<Component> Components, Action<Component> modificationAction)
        {
            // Go through every Paracomponent and change a property given as a parameter to a value also given as a parameter.
            foreach(var component in Components) 
            {
                modificationAction(component);
            }
        }

        #endregion
    }
}