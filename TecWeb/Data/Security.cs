using System;
using System.Collections.Generic;

namespace TecWeb.Data;

public partial class Security
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Role { get; set; } = null!;
}
