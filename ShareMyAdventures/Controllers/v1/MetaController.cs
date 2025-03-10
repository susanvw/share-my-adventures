using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ShareMyAdventures.Controllers.v1;

public class MetaController<T> : ApiControllerBase
{
    public MetaController() : base()
    {
    }

    [HttpGet("/info")]
    public ActionResult<string> Info()
    {
        var assembly = typeof(T).Assembly;

        var creationDate = System.IO.File.GetCreationTime(assembly.Location);
        var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

        return Ok($"Version: {version}, Last Updated: {creationDate}");
    }
}