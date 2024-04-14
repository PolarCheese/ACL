namespace ACL.IO;

public interface IFileManager
{
    public void SaveFile(string path, string content);
    public void RemoveFile(string path);
    public string LoadFile(string path);

    public void CreateDirectory(string path);
    public void RemoveDirectory(string path);
}