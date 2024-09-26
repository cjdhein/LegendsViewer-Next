using LegendsViewer.Backend.Legends.WorldObjects;
using LegendsViewer.Backend.Legends;
using LegendsViewer.Backend.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LegendsViewer.Backend.Controllers;

public class DanceFormController(IWorld worldDataService) : GenericController<DanceForm>(worldDataService.DanceForms, worldDataService.GetDanceForm)
{
}

public class MusicalFormController(IWorld worldDataService) : GenericController<MusicalForm>(worldDataService.MusicalForms, worldDataService.GetMusicalForm)
{
}

public class PoeticFormController(IWorld worldDataService) : GenericController<PoeticForm>(worldDataService.PoeticForms, worldDataService.GetPoeticForm)
{
}

public class WrittenContentController(IWorld worldDataService) : GenericController<WrittenContent>(worldDataService.WrittenContents, worldDataService.GetWrittenContent)
{
}

public class LandmassController(IWorld worldDataService) : GenericController<Landmass>(worldDataService.Landmasses, worldDataService.GetLandmass)
{
}

public class RiverController(IWorld worldDataService) : GenericController<River>(worldDataService.Rivers, worldDataService.GetRiver)
{
}

public class SiteController(IWorld worldDataService) : GenericController<Site>(worldDataService.Sites, worldDataService.GetSite)
{
}

public class RegionController(IWorld worldDataService) : GenericController<WorldRegion>(worldDataService.Regions, worldDataService.GetRegion)
{
}

public class UndergroundRegionController(IWorld worldDataService) : GenericController<UndergroundRegion>(worldDataService.UndergroundRegions, worldDataService.GetUndergroundRegion)
{
}

public class ArtifactController(IWorld worldDataService) : GenericController<Artifact>(worldDataService.Artifacts, worldDataService.GetArtifact)
{
}

public class EntityController(IWorld worldDataService) : GenericController<Entity>(worldDataService.Entities, worldDataService.GetEntity)
{
    [HttpGet("civs")]
    public IActionResult GetMainCivilizations()
    {
        return Ok(AllElements.Where(x => x.IsCiv || (x.Type == Legends.Enums.EntityType.Civilization && x.SiteHistory.Count > 0)));
    }
}

public class HistoricalFigureController(IWorld worldDataService) : GenericController<HistoricalFigure>(worldDataService.HistoricalFigures, worldDataService.GetHistoricalFigure)
{
}

public class MountainPeakController(IWorld worldDataService) : GenericController<MountainPeak>(worldDataService.MountainPeaks, worldDataService.GetMountainPeak)
{
}

public class StructureController(IWorld worldDataService) : GenericController<Structure>(worldDataService.Structures, worldDataService.GetStructure)
{
}

public class ConstructionController(IWorld worldDataService) : GenericController<WorldConstruction>(worldDataService.WorldConstructions, worldDataService.GetWorldConstruction)
{
}
