namespace ParametersManager;

/// <summary>
/// Simple parameter in Windows OS.
/// </summary>
public class SimpleWindowsParameter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleWindowsParameter"/> class.
    /// </summary>
    /// <param name="path">Path of registry parameter.</param>
    /// <param name="name">Name of registry parameter.</param>
    public SimpleWindowsParameter(string? path, string? name)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException("An empty path was passed!");
        }

        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("An empty name was passed!");
        }

        Path = path;
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleWindowsParameter"/> class.
    /// </summary>
    /// <param name="name">Name of registry parameter.</param>
    /// <param name="value">Value of registry parameter.</param>
    public SimpleWindowsParameter(string? name, object? value)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("An empty name was passed!");
        }

        if (value == null)
        {
            throw new ArgumentException("An empty value was passed!");
        }

        Name = name;
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleWindowsParameter"/> class.
    /// </summary>
    /// <param name="path">Path of registry parameter.</param>
    /// <param name="name">Name of registry parameter.</param>
    /// <param name="value">Value of registry parameter.</param>
    public SimpleWindowsParameter(string? path, string? name, object? value)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException("An empty path was passed!");
        }

        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("An empty name was passed!");
        }

        if (value == null)
        {
            throw new ArgumentException("An empty value was passed!");
        }

        Path = path;
        Name = name;
        Value = value;
    }

    /// <summary>
    /// Path of registry parameter.
    /// </summary>
    public string? Path { get; protected set; }

    /// <summary>
    /// Name of registry parameter.
    /// </summary>
    public string? Name { get; protected set; }

    /// <summary>
    /// Value of registry parameter.
    /// </summary>
    public object? Value { get; protected set; }

    // TODO Defining parameter type
}