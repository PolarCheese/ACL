using ACL.UI;

namespace ACL;
public interface INode
{
    
    public void AddSubcomponents(params Component[] Components);

    public void AddSubcomponents(IEnumerable<Component> Components);

    public void RemoveSubcomponents(params Component[] Components);

    public void RemoveSubcomponents(IEnumerable<Component> Components);
}