using PV178.Homeworks.HW03.Model;
using System.Collections.ObjectModel;

namespace PV178.Homeworks.HW03.DataLoading.DataImporter
{
    public interface IDataImporter
    {
        IReadOnlyList<AttackedPerson> ListAllAttackedPeople();
        ReadOnlyCollection<Country> ListAllCountries();
        IReadOnlyList<SharkAttack> ListAllSharkAttacks();
        ReadOnlyCollection<SharkSpecies> ListAllSharkSpecies();
    }
}