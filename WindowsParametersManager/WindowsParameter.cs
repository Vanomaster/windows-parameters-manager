using System.Diagnostics;
using Microsoft.Win32;

namespace ParametersManager;

/// <summary>
/// Parameter in Windows OS.
/// </summary>
public class WindowsParameter : SimpleWindowsParameter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsParameter"/> class.
    /// </summary>
    /// <param name="path">Path of registry parameter.</param>
    /// <param name="name">Name of registry parameter.</param>
    public WindowsParameter(string? path, string? name)
        : base(path, name)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsParameter"/> class.
    /// </summary>
    /// <param name="name">Name of registry parameter.</param>
    /// <param name="value">Value of registry parameter.</param>
    public WindowsParameter(string? name, object? value)
        : base(name, value)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsParameter"/> class.
    /// </summary>
    /// <param name="path">Path of registry parameter.</param>
    /// <param name="name">Name of registry parameter.</param>
    /// <param name="value">Value of registry parameter.</param>
    public WindowsParameter(string? path, string? name, object? value)
        : base(path, name, value)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsParameter"/> class.
    /// </summary>
    /// <param name="path">Path of registry parameter.</param>
    /// <param name="name">Name of registry parameter.</param>
    /// <param name="value">Value of registry parameter.</param>
    /// <param name="additions">Additions of registry parameter.</param>
    public WindowsParameter(string? path, string? name, object? value, List<SimpleWindowsParameter> additions)
        : base(path, name, value)
    {
        if (additions == null)
        {
            throw new ArgumentException("An empty additions was passed!");
        }

        Additions = additions;
    }

    /// <summary>
    /// Additional registry parameters required for this registry parameter.
    /// </summary>
    public List<SimpleWindowsParameter> Additions { get; } = new ();

    // TODO different paths of different parameters

    /// <summary>
    /// Get registry parameter value.
    /// </summary>
    /// <returns>Registry parameter value.</returns>
    public object? GetRegistryValue()
    {
        var key = Registry.CurrentUser.OpenSubKey(Path!);
        Value = key?.GetValue(Name);

        return Value;
    }

    /// <summary>
    /// Get additional registry parameters required for this registry parameter.
    /// </summary>
    /// /// <param name="additionsPath">Additions path.</param>
    /// <exception cref="ArgumentNullException">Additions path is invalid.</exception>
    /// <returns>Additional registry parameters required for this registry parameter.</returns>
    public List<SimpleWindowsParameter> GetRegistryAdditions(string? additionsPath)
    {
        if (string.IsNullOrEmpty(additionsPath))
        {
            throw new ArgumentException("An empty additions path!");
        }

        var key = Registry.CurrentUser.OpenSubKey(additionsPath);
        var detailsNames = key?.GetValueNames().ToList();
        detailsNames?.ForEach(detailName => Additions
            .Add(new WindowsParameter(additionsPath, detailName, key?.GetValue(detailName))));

        return Additions;
    }

    /// <summary>
    /// Set registry parameter value.
    /// </summary>
    /// <param name="newValue">New registry parameter value.</param>
    /// <exception cref="ArgumentException">New value is invalid.</exception>
    public void SetRegistryValue(string? newValue)
    {
        if (string.IsNullOrEmpty(newValue))
        {
            throw new ArgumentException("An empty new value was passed!");
        }

        var key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default)
            .OpenSubKey(Path!, true);
        key?.SetValue(Name, newValue);

        GetRegistryValue();
        UpdateGroupPolicy();
    }

    /// <summary>
    /// Set additional registry parameters required for this registry parameter.
    /// </summary>
    /// <param name="additions">Additional registry parameters required for this registry parameter.</param>
    /// <exception cref="ArgumentException">Additions or path to additions is invalid.</exception>
    public void SetRegistryAdditions(List<SimpleWindowsParameter> additions)
    {
        if (additions == null)
        {
            throw new ArgumentException("An empty additions was passed!");
        }

        string? additionsPath = additions[0].Path;

        if (string.IsNullOrEmpty(additionsPath))
        {
            throw new ArgumentException("An empty path to additions was passed!");
        }

        var key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default)
            .OpenSubKey(additionsPath, true);
        additions.ForEach(parameter => key?.SetValue(parameter.Name, parameter.Value!));

        GetRegistryAdditions(additionsPath);
        UpdateGroupPolicy();
    }

    /// <summary>
    /// Update group policy.
    /// </summary>
    private static void UpdateGroupPolicy()
    {
        var executableFile = new FileInfo(@"gpupdate.exe");
        var process = new Process
        {
            StartInfo =
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = executableFile.Name,
                Arguments = "/force",
            },
        };

        process.Start();
    }

    /// <summary>
    /// Create registry parameter value.
    /// </summary>
    /// <param name="value">Value of registry parameter.</param>
    /// <exception cref="ArgumentException">Value is invalid.</exception>
    public void CreateRegistryParameter(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("An empty value was passed!");
        }

        var key = Registry.CurrentUser.CreateSubKey(Path!, true);
        key.SetValue(Name, value);

        GetRegistryValue();
    }

    /// <summary>
    /// Create additional registry parameters required for this registry parameter.
    /// </summary>
    /// <param name="additions">Additional registry parameters required for this registry parameter.</param>
    /// <exception cref="ArgumentException">Additions or path to additions is invalid.</exception>
    public void CreateRegistryAdditions(List<SimpleWindowsParameter> additions)
    {
        if (additions == null)
        {
            throw new ArgumentException("An empty additions was passed!");
        }

        string? additionsPath = additions[0].Path;

        if (string.IsNullOrEmpty(additionsPath))
        {
            throw new ArgumentException("An empty path to additions was passed!");
        }

        var key = Registry.CurrentUser.CreateSubKey(additionsPath, true);
        additions.ForEach(parameter => key.SetValue(parameter.Name, parameter.Value!));

        GetRegistryAdditions(additionsPath);
    }
}