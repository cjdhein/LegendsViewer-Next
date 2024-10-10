using LegendsViewer.Backend.Legends.WorldObjects;
using Microsoft.AspNetCore.Mvc;
using LegendsViewer.Backend.Legends.Interfaces;
using LegendsViewer.Backend.Legends.EventCollections;

namespace LegendsViewer.Backend.Controllers;

public class DanceFormController(IWorld worldDataService) : WorldObjectGenericController<DanceForm>(worldDataService.DanceForms, worldDataService.GetDanceForm)
{
}

public class MusicalFormController(IWorld worldDataService) : WorldObjectGenericController<MusicalForm>(worldDataService.MusicalForms, worldDataService.GetMusicalForm)
{
}

public class PoeticFormController(IWorld worldDataService) : WorldObjectGenericController<PoeticForm>(worldDataService.PoeticForms, worldDataService.GetPoeticForm)
{
}

public class WrittenContentController(IWorld worldDataService) : WorldObjectGenericController<WrittenContent>(worldDataService.WrittenContents, worldDataService.GetWrittenContent)
{
}

public class LandmassController(IWorld worldDataService) : WorldObjectGenericController<Landmass>(worldDataService.Landmasses, worldDataService.GetLandmass)
{
}

public class RiverController(IWorld worldDataService) : WorldObjectGenericController<River>(worldDataService.Rivers, worldDataService.GetRiver)
{
}

public class SiteController(IWorld worldDataService) : WorldObjectGenericController<Site>(worldDataService.Sites, worldDataService.GetSite)
{
}

public class RegionController(IWorld worldDataService) : WorldObjectGenericController<WorldRegion>(worldDataService.Regions, worldDataService.GetRegion)
{
}

public class UndergroundRegionController(IWorld worldDataService) : WorldObjectGenericController<UndergroundRegion>(worldDataService.UndergroundRegions, worldDataService.GetUndergroundRegion)
{
}

public class ArtifactController(IWorld worldDataService) : WorldObjectGenericController<Artifact>(worldDataService.Artifacts, worldDataService.GetArtifact)
{
}

public class EntityController(IWorld worldDataService) : WorldObjectGenericController<Entity>(worldDataService.Entities, worldDataService.GetEntity)
{
    [HttpGet("civs")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<List<Entity>> GetMainCivilizations()
    {
        return Ok(AllElements.Where(x => x.IsCiv || (x.EntityType == Legends.Enums.EntityType.Civilization && x.SiteHistory.Count > 0)));
    }
}

public class HistoricalFigureController(IWorld worldDataService) : WorldObjectGenericController<HistoricalFigure>(worldDataService.HistoricalFigures, worldDataService.GetHistoricalFigure)
{
}

public class MountainPeakController(IWorld worldDataService) : WorldObjectGenericController<MountainPeak>(worldDataService.MountainPeaks, worldDataService.GetMountainPeak)
{
}

public class StructureController(IWorld worldDataService) : WorldObjectGenericController<Structure>(worldDataService.Structures, worldDataService.GetStructure)
{
}

public class ConstructionController(IWorld worldDataService) : WorldObjectGenericController<WorldConstruction>(worldDataService.WorldConstructions, worldDataService.GetWorldConstruction)
{
}

public class EraController(IWorld worldDataService) : WorldObjectGenericController<Era>(worldDataService.Eras, worldDataService.GetEra)
{
}

public class WarController(IWorld worldDataService) : WorldObjectGenericController<War>(worldDataService.Wars, worldDataService.GetEventCollection<War>)
{
}

public class BattleController(IWorld worldDataService) : WorldObjectGenericController<Battle>(worldDataService.Battles, worldDataService.GetEventCollection<Battle>)
{
}

public class DuelController(IWorld worldDataService) : WorldObjectGenericController<Duel>(worldDataService.Duels, worldDataService.GetEventCollection<Duel>)
{
}

public class InsurrectionController(IWorld worldDataService) : WorldObjectGenericController<Insurrection>(worldDataService.Insurrections, worldDataService.GetEventCollection<Insurrection>)
{
}

public class PersecutionController(IWorld worldDataService) : WorldObjectGenericController<Persecution>(worldDataService.Persecutions, worldDataService.GetEventCollection<Persecution>)
{
}

public class PurgeController(IWorld worldDataService) : WorldObjectGenericController<Purge>(worldDataService.Purges, worldDataService.GetEventCollection<Purge>)
{
}

public class RaidController(IWorld worldDataService) : WorldObjectGenericController<Raid>(worldDataService.Raids, worldDataService.GetEventCollection<Raid>)
{
}

public class SiteConqueredController(IWorld worldDataService) : WorldObjectGenericController<SiteConquered>(worldDataService.SiteConquerings, worldDataService.GetEventCollection<SiteConquered>)
{
}

public class BeastAttackController(IWorld worldDataService) : WorldObjectGenericController<BeastAttack>(worldDataService.BeastAttacks, worldDataService.GetEventCollection<BeastAttack>)
{
}
