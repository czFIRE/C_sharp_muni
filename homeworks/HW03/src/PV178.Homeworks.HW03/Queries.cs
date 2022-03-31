using PV178.Homeworks.HW03.DataLoading.DataContext;
using PV178.Homeworks.HW03.DataLoading.Factory;
using PV178.Homeworks.HW03.Model;
using PV178.Homeworks.HW03.Model.Enums;

namespace PV178.Homeworks.HW03
{
    public class Queries
    {
        private IDataContext? _dataContext;
        public IDataContext DataContext => _dataContext ??= new DataContextFactory().CreateDataContext();

        /// <summary>
        /// SFTW si vyžiadala počet útokov, ktoré sa udiali v krajinách začinajúcich na písmeno <'A', 'G'>,
        /// a kde obete boli muži v rozmedzí <15, 40> rokov.
        /// </summary>
        /// <returns>The query result</returns>
        public int AttacksAtoGCountriesMaleBetweenFifteenAndFortyQuery()
        {
            var IDsWithPersons = DataContext.AttackedPeople.Where(person => person.Age >= 15 && person.Age <= 40 && person.Sex == Sex.Male).Select(x => x.Id).ToHashSet();

            var IDsWithCountryName = DataContext.Countries.Where(country => country.Name.FirstOrDefault('\0') >= 'A'
                                                                         && country.Name.FirstOrDefault('\0') <= 'G').Select(x => x.Id).ToHashSet();

            var result = DataContext.SharkAttacks.Count(attack => (attack.CountryId != null ? IDsWithCountryName.Contains((int)attack.CountryId) : false)
                                                               && (attack.AttackedPersonId != null ? IDsWithPersons.Contains((int)attack.AttackedPersonId) : false));
            return result;
        }

        /// <summary>
        /// Vráti zoznam, v ktorom je textová informácia o každom človeku,
        /// ktorého meno nie je známe (začína na malé písmeno alebo číslo) a na ktorého zaútočil žralok v štáte Bahamas.
        /// Táto informácia je v tvare:
        /// {meno človeka} was attacked in Bahamas by {latinský názov žraloka}
        /// </summary>
        /// <returns>The query result</returns>
        public List<string> InfoAboutPeopleWithUnknownNamesAndWasInBahamasQuery()
        {
            int BahamasID = DataContext.Countries.Where(country => country.Name == "Bahamas").FirstOrDefault().Id;

            var IDsAttackedOnBahamas = DataContext.SharkAttacks.Where(attack => attack.CountryId == BahamasID)
                                       .Select(attack => new { attack.AttackedPersonId, attack.SharkSpeciesId });

            // get person names
            var joinedNames = DataContext.AttackedPeople.Where(person => person.Name == String.Empty || !Char.IsUpper(person.Name, 0))
                                                        .Join(IDsAttackedOnBahamas, tmp1 => tmp1.Id, tmp2 => tmp2.AttackedPersonId,
                                                             (tmp1, tmp2) => new { tmp1.Name, tmp2.SharkSpeciesId });

            // get shark names
            var joinedSharks = joinedNames.Join(DataContext.SharkSpecies.Where(shark => shark.LatinName != null), tmp1 => tmp1.SharkSpeciesId, tmp2 => tmp2.Id,
                                                (tmp1, tmp2) => (tmp1.Name + " was attacked in Bahamas by " + tmp2.LatinName));

            return joinedSharks.ToList();
        }

        /// <summary>
        /// Prišla nám ďalšia požiadavka od našej milovanej SFTW. 
        /// Chcú od nás 5 názvov krajín s najviac útokmi, kde žraloky merali viac ako 3 metre.
        /// Požadujú, aby tieto data boli zoradené abecedne.
        /// </summary>
        /// <returns>The query result</returns>
        public List<string> FiveCountriesWithTopNumberOfAttackSharksLongerThanThreeMetersQuery()
        {
            var validSharkIDs = DataContext.SharkSpecies.Where(shark => shark.Length > 3).Select(shark => shark.Id);

            var countryIDs = DataContext.SharkAttacks.Where(attack => attack.CountryId != null && validSharkIDs.Contains(attack.SharkSpeciesId))
                                                     .GroupBy(attack => attack.CountryId)
                                                     .Select(attack => new { CountryID = attack.Key, Count = attack.Count() }).OrderByDescending(tmp => tmp.Count)
                                                     .Select(tmp => tmp.CountryID).Take(5);

            var names = DataContext.Countries.Join(countryIDs, tmp1 => tmp1.Id, tmp2 => tmp2.Value, (tmp1, tmp2) => tmp1.Name);

            return names.ToList();
        }

        /// <summary>
        /// SFTW chce zistiť, či žraloky berú ohľad na pohlavie obete. 
        /// Vráti informáciu či každý druh žraloka, ktorý je dlhší ako 2 metre
        /// útočil aj na muža aj na ženu.
        /// </summary>
        /// <returns>The query result</returns>
        public bool AreAllLongSharksGenderIgnoringQuery()
        {
            var validSharkIDs = DataContext.SharkSpecies.Where(shark => shark.Length > 2).Select(shark => shark.Id);

            // This could be sped up by using join istead of contains => but joins were used later
            var sharkIDPersonSex = DataContext.SharkAttacks.Where(attack => attack.AttackedPersonId != null && validSharkIDs.Contains(attack.SharkSpeciesId))
                                                          .Join(DataContext.AttackedPeople, attack => attack.AttackedPersonId, person => person.Id,
                                                                (attack, person) => new { attack.SharkSpeciesId, person.Sex });

            var smth = sharkIDPersonSex.GroupBy(attack => attack.SharkSpeciesId)
                                       .Select(tmp => new { id = tmp.Key, ignore = tmp.Any(tmp1 => tmp1.Sex == Sex.Male) && tmp.Any(tmp1 => tmp1.Sex == Sex.Female) })
                                       .All(tmp => tmp.ignore);

            return smth;
        }

        /// <summary>
        /// Každý túži po prezývke a žralok nie je výnimkou. Keď na Vás pekne volajú, hneď Vám lepšie chutí. 
        /// Potrebujeme získať všetkých žralokov, ktorí nemajú prezývku(AlsoKnownAs) a k týmto žralokom krajinu v ktorej najviac útočili.
        /// Samozrejme to SFTW chce v podobe Dictionary, kde key bude názov žraloka a value názov krajiny.
        /// Len si predstavte tie rôznorodé prezývky, napr. Devil of Kyrgyzstan.
        /// </summary>
        /// <returns>The query result</returns>
        public Dictionary<string, string> SharksWithoutNickNameAndCountryWithMostAttacksQuery()
        {
            var relevantSharks = DataContext.SharkSpecies.Where(shark => shark.AlsoKnownAs == String.Empty && shark.Name != String.Empty)
                                                         .Select(shark => new { shark.Id, shark.Name });

            var sharkCountryID = DataContext.SharkAttacks.Where(attack => attack.CountryId != null)
                                                         .Join(relevantSharks, attack => attack.SharkSpeciesId, shark => shark.Id,
                                                                (attack, shark) => new { attack.CountryId, shark.Name });

            var sharkCountryIDTop = sharkCountryID.GroupBy(tmp => tmp.Name).Select(tmp => new
            {
                sharkName = tmp.Key,
                countryID = tmp.ToList().GroupBy(tmp1 => tmp1.CountryId).Select(tmp1 => new { id = tmp1.Key, count = tmp1.Count() })
                                                .OrderByDescending(tmp1 => tmp1.count).Select(tmp1 => tmp1.id).FirstOrDefault()
            });

            var sharkCountryName = sharkCountryIDTop.Join(DataContext.Countries.Where(country => country.Name != null), tmp1 => tmp1.countryID, tmp2 => tmp2.Id,
                                                         (shark, country) => new { shark.sharkName, country.Name });

            return sharkCountryName.ToDictionary(key => key.sharkName, el => el.Name);
        }

        /// <summary>
        /// Ohúrili ste SFTW natoľko, že si u Vás objednali rovno textové výpisy. Samozrejme, že sa to dá zvladnúť pomocou LINQ. 
        /// Chcú aby ste pre všetky fatálne útoky v štátoch na písmenko 'D' a 'E', urobili výpis v podobe: 
        /// "{Meno obete} (iba ak sa začína na veľké písmeno) was attacked in {názov štátu} by {latinský názov žraloka}"
        /// Získané pole zoraďte abecedne a vraťte prvých 5 viet.
        /// </summary>
        /// <returns>The query result</returns>
        public List<string> InfoAboutPeopleAndCountriesOnDorEAndFatalAttacksQuery()
        {
            var relevantStateIDs = DataContext.Countries.Where(country => country.Name != String.Empty && (country.Name.First() == 'E' || country.Name.First() == 'D'))
                                                        .Select(country => new { country.Id, country.Name });

            var tmpQuerry = DataContext.SharkAttacks.Where(attack => attack.AttackSeverenity == AttackSeverenity.Fatal && attack.CountryId != null && attack.AttackedPersonId != null)
                                                    .Join(relevantStateIDs, tmp1 => tmp1.CountryId, tmp2 => tmp2.Id,
                                                    (attack, country) => new { attack.AttackedPersonId, CountryName = country.Name, attack.SharkSpeciesId });

            var personTmpQuerry = DataContext.AttackedPeople.Where(person => person.Name != String.Empty && Char.IsUpper(person.Name.FirstOrDefault()))
                                                            .Join(tmpQuerry, tmp1 => tmp1.Id, tmp2 => tmp2.AttackedPersonId,
                                                            (person, querry) => new { person.Name, querry.CountryName, querry.SharkSpeciesId });

            var sharkTmpQuerry = DataContext.SharkSpecies.Where(shark => shark.LatinName != String.Empty).Join(personTmpQuerry, tmp1 => tmp1.Id, tmp2 => tmp2.SharkSpeciesId,
                                                          (shark, querry) => querry.Name + " was attacked in " + querry.CountryName
                                                                             + " by " + shark.LatinName);

            return sharkTmpQuerry.OrderBy(name => name).Take(5).ToList();
        }

        /// <summary>
        /// SFTW pretlačil nový zákon. Chce pokutovať štáty v Afrike.
        /// Každý z týchto štátov dostane pokutu za každý útok na ich území a to buď 250 meny danej krajiny alebo 300 meny danej krajiny (ak bol fatálny).
        /// Ak útok nebol preukázany ako fatal alebo non-fatal, štát za takýto útok nie je pokutovaný. Vyberte prvých 5 štátov s najvyššou pokutou.
        /// Vety budú zoradené zostupne podľa výšky pokuty.
        /// Opäť od Vás požadujú neštandardné formátovanie: "{Názov krajiny}: {Pokuta} {Mena danej krajiny}"
        /// Egypt: 10150 EGP
        /// Senegal: 2950 XOF
        /// Kenya: 2800 KES
        /// </summary>
        /// <returns>The query result</returns>
        public List<string> InfoAboutFinesOfAfricanCountriesTopFiveQuery()
        {
            var relevantStates = DataContext.Countries.Where(country => country.Continent == "Africa" && country.Name != String.Empty && country.CurrencyCode != String.Empty)
                                                      .Select(country => new { country.Id, country.Name, country.CurrencyCode });


            var nameFeeCurrencyCode = DataContext.SharkAttacks.Where(attack => attack.AttackSeverenity != null).Join(relevantStates, tmp1 => tmp1.CountryId, tmp2 => tmp2.Id,
                                                    (attack, country) => new { country.Name, feeValue = SeverityToFee(attack.AttackSeverenity), country.CurrencyCode });

            var summedFees = nameFeeCurrencyCode.GroupBy(tmp => new { tmp.Name, tmp.CurrencyCode }).Select(tmp => new { tmp.Key.Name, Sum = tmp.Sum(tmp1 => tmp1.feeValue), tmp.Key.CurrencyCode })
                                 .OrderByDescending(tmp => tmp.Sum).Take(5).Select(tmp => tmp.Name + ": " + tmp.Sum + " " + tmp.CurrencyCode);

            return summedFees.ToList();
        }

        private int SeverityToFee(AttackSeverenity? severenity)
        {
            return severenity switch
            {
                AttackSeverenity.Fatal => 300,
                AttackSeverenity.NonFatal => 250,
                _ => 0,
            };
        }

        /// <summary>
        /// CEO chce kandidovať na prezidenta celej planéty. Chce zistiť ako ma štylizovať svoju rétoriku aby zaujal čo najviac krajín.
        /// Preto od Vás chce, aby ste mu pomohli zistiť aké percentuálne zastúpenie majú jednotlivé typy vlád.
        /// Požaduje to ako jeden string: "{typ vlády}: {percentuálne zastúpenie}%, ...". 
        /// Výstup je potrebné mať zoradený, od najväčších percent po najmenšie a percentá sa budú zaokrúhľovať na jedno desatinné číslo.
        /// Pre zlúčenie použite Aggregate(..).
        /// </summary>
        /// <returns>The query result</returns>
        public string GovernmentTypePercentagesQuery()
        {
            // I don't really see why use and aggregate function here (aggregate alias foldl)?
            // It could be done in one go through the collection, but the operation of collection size should be quick
            // And yes, I know how to use aggregate function => FastestVsSlowestSharkQuery

            float totalGoverments = DataContext.Countries.Count;

            var formPercentage = DataContext.Countries.GroupBy(country => country.GovernmentForm).Select(tmp => new { Form = tmp.Key.ToString(), Perc = 100 * tmp.Count() / totalGoverments })
                                            .OrderByDescending(tmp => tmp.Perc).Select(tmp => tmp.Form + ": " + tmp.Perc.ToString("0.0") + "%");

            return String.Join(", ", formPercentage);
        }

        /// <summary>
        /// Oslovili nás surfisti. Chcú vedieť, či sú ako skupina viacej ohrození žralokmi. 
        /// Súrne potrebujeme vedieť koľko bolo fatálnych útokov na surfistov("surf", "Surf", "SURF") 
        /// a aký bol ich premierný vek(zaokrúliť na 2 desatinné miesta). 
        /// Zadávateľ úlohy nám to, ale skomplikoval. Tieto údaje chce pre každý kontinent.
        /// </summary>
        /// <returns>The query result</returns>
        public Dictionary<string, ValueTuple<int, double>> InfoForSurfersByContinentQuery()
        {
            var attacks = DataContext.SharkAttacks.Where(attack => attack.AttackSeverenity == AttackSeverenity.Fatal && attack.CountryId != null
                                                        && attack.Activity != null && attack.Activity.ToLower().Contains("surf") && attack.AttackedPersonId != null)
                                                  .Select(attack => new { attack.CountryId, attack.AttackedPersonId });

            // Why not exclude people without age???????????????
            // This should be here :reee: .Where(person => person.Age != null)
            var personAges = DataContext.AttackedPeople.Join(attacks, tmp1 => tmp1.Id, tmp2 => tmp2.AttackedPersonId,
                                                            (person, attack) => new { person.Age, attack.CountryId });

            var continentAge = personAges.Join(DataContext.Countries.Where(country => country.Continent != null), tmp1 => tmp1.CountryId, tmp2 => tmp2.Id,
                                               (peAg, country) => new { country.Continent, peAg.Age });

            var together = continentAge.GroupBy(tmp => tmp.Continent).Select(tmp => new { Continent = tmp.Key, Count = tmp.Count(), Avg = tmp.Average(x => x.Age) })
                                  .ToDictionary(x => x.Continent, x => new ValueTuple<int, double>(x.Count, Math.Round((double)x.Avg, 2)));

            return together;
        }

        /// <summary>
        /// Zaujíma nás "10 najťažších žralokov na planéte" a "krajiny Severnej Ameriky". 
        /// CEO požaduje zoznam dvojíc, kde pre každý štát z danej množiny bude uvedený zoznam žralokov z danej množiny, ktorí v tom štáte útočili.
        /// Pokiaľ v nejakom štáte neútočil žiaden z najťažších žralokov, zoznam žralokov bude prázdny.
        /// SFTW požaduje prvých 5 položiek zoznamu dvojíc, zoradeného abecedne podľa mien štátov.

        /// </summary>
        /// <returns>The query result</returns>
        public List<ValueTuple<string, List<SharkSpecies>>> HeaviestSharksInNorthAmericaQuery()
        {
            var NACountriesIDs = DataContext.Countries.Where(country => country.Continent == "North America").Select(country => new { country.Id, country.Name }).OrderBy(tmp => tmp.Name).Take(5);

            var heaviestSharks = DataContext.SharkSpecies.OrderByDescending(shark => shark.Weight).Take(10);


            var result = NACountriesIDs.GroupJoin(DataContext.SharkAttacks, tmp2 => tmp2.Id, tmp1 => tmp1.CountryId, (country, attack) =>
                                                        new ValueTuple<string, List<SharkSpecies>>(country.Name, attack.Join(heaviestSharks, tmp1 => tmp1.SharkSpeciesId, tmp2 => tmp2.Id,
                                                                (att, shark) => shark).Distinct().ToList()));

            return result.ToList();
        }

        /// <summary>
        /// Zistite nám prosím všetky útoky spôsobené pri člnkovaní (attack type "Boating"), ktoré mal na vine žralok s prezývkou "White death". 
        /// Zaujímajú nás útoky z obdobia po 3.3.1960 (vrátane) a ľudia, ktorých meno začína na písmeno z intervalu <U, Z>.
        /// Výstup požadujeme ako zoznam mien zoradených abecedne.
        /// </summary>
        /// <returns>The query result</returns>
        public List<string> NonFatalAttemptOfWhiteDeathOnPeopleBetweenUAndZQuery()
        {
            var validPeoples = DataContext.AttackedPeople.Where(person => person.Name.FirstOrDefault('\0') >= 'U' && person.Name.FirstOrDefault('\0') <= 'Z').Select(person => new { person.Name, person.Id });

            var sharkWhiteDeathID = DataContext.SharkSpecies.Where(shark => shark.AlsoKnownAs == "White death").Select(shark => shark.Id).FirstOrDefault();

            var temp = DataContext.SharkAttacks.Where(attack => attack.SharkSpeciesId == sharkWhiteDeathID && attack.Type == AttackType.Boating
                                                                && DateTime.Parse("1960/03/03 00:00:00.000") <= attack.DateTime && attack.AttackedPersonId.HasValue)
                                               .Select(attack => attack.AttackedPersonId);

            var people = validPeoples.Join(temp, tmp1 => tmp1.Id, tmp2 => tmp2.Value, (person, _) => person.Name);

            return people.ToList();
        }

        /// <summary>
        /// Myslíme si, že rýchlejší žralok ma plnší žalúdok. 
        /// Požadujeme údaj o tom koľko percent útokov má na svedomí najrýchlejší a najpomalší žralok.
        /// Výstup požadujeme vo formáte: "{percentuálne zastúpenie najrýchlejšieho}% vs {percentuálne zastúpenie najpomalšieho}%"
        /// Perc. zastúpenie zaokrúhlite na jedno desatinné miesto.
        /// </summary>
        /// <returns>The query result</returns>
        public string FastestVsSlowestSharkQuery()
        {
            var orderedSharks = DataContext.SharkSpecies.Where(shark => shark.TopSpeed.HasValue).OrderBy(shark => shark.TopSpeed);

            var slowestID = orderedSharks.First().Id;
            var quickestID = orderedSharks.Last().Id;

            var occurenceCounts = DataContext.SharkAttacks.Aggregate((total: 0, quickest: 0, slowest: 0), (acc, curr) => HelperFunc(acc, curr, slowestID, quickestID),
                                                        acc => acc);

            //acc => (acc.quickest/(float) acc.total).ToString("0.0") + "% vs " + (acc.slowest / (float) acc.total).ToString("0.0") + "%"

            var quickPercentage = (occurenceCounts.quickest / (float)occurenceCounts.total) * 100;
            var slowPercentage = (occurenceCounts.slowest / (float)occurenceCounts.total) * 100;

            return quickPercentage.ToString("0.0") + "% vs " + slowPercentage.ToString("0.0") + "%";
        }

        private (int, int, int) HelperFunc((int total, int quickest, int slowest) acc, SharkAttack attack, int slowestID, int quickestID)
        {
            acc.total++;

            if (attack.SharkSpeciesId == slowestID)
            {
                acc.slowest++;
            }

            if (attack.SharkSpeciesId == quickestID)
            {
                acc.quickest++;
            }

            return acc;
        }

        /// <summary>
        /// Prišla nám požiadavka z hora, aby sme im vrátili zoznam, 
        /// v ktorom je textová informácia o KAŽDOM človeku na ktorého zaútočil žralok v štáte Bahamas.
        /// Táto informácia je taktiež v tvare:
        /// {meno človeka} was attacked by {latinský názov žraloka}
        /// 
        /// Ale pozor váš nový nadriadený ma panický strach z operácie Join alebo GroupJoin.
        /// Nariadil vám použiť metódu Zip.
        /// Zistite teda tieto informácie bez spojenia hocijakých dvoch tabuliek a s použitím metódy Zip.
        /// </summary>
        /// <returns>The query result</returns>
        public List<string> AttackedPeopleInBahamasWithoutJoinQuery()
        {
            // This is so horribly inefficient I wanna cry, but you asked for this shit


            var bahamasID = DataContext.Countries.Where(country => country.Name == "Bahamas").Select(country => country.Id).FirstOrDefault();

            var relevantAttacks = DataContext.SharkAttacks.Where(attack => attack.CountryId == bahamasID && attack.AttackedPersonId.HasValue)
                                                          .Select(attack => new { attack.AttackedPersonId, attack.SharkSpeciesId });


            var relevantPeople = DataContext.AttackedPeople.Where(person => relevantAttacks.Select(x => x.AttackedPersonId).Contains(person.Id))
                                                           .OrderBy(prs => prs.Id).Select(person => person.Name);


            Dictionary<int, string> sharkNameDict = DataContext.SharkSpecies.ToDictionary(x => x.Id, x => x.LatinName);

            var sharks = relevantAttacks.OrderBy(atck => atck.AttackedPersonId).Select(atck => sharkNameDict[atck.SharkSpeciesId]);

            var result = relevantPeople.Zip(sharks, (person, shark) => person + " was attacked by " + shark);

            return result.ToList();
        }

        /// <summary>
        /// Vráti počet útokov podľa mien žralokov, ktoré sa stali v Austrálii, vo formáte {meno žraloka}: {počet útokov}
        /// </summary>
        /// <returns>The query result</returns>
        public List<string> MostThreateningSharksInAustralia()
        {
            var countryIDs = DataContext.Countries.Where(country => country.Continent == "Australia").Select(country => country.Id);

            var sharks = DataContext.SharkSpecies.Where(shark => shark.Name != String.Empty).Select(shark => new { shark.Name, shark.Id });

            var sharkInAustralia = DataContext.SharkAttacks.Join(countryIDs, tmp1 => tmp1.CountryId, tmp2 => tmp2, (attack, _) => attack.SharkSpeciesId);

            var result = sharks.Join(sharkInAustralia, tmp1 => tmp1.Id, tmp2 => tmp2, (shark, attack) => shark.Name).GroupBy(tmp => tmp)
                               .Select(tmp => tmp.Key + ": " + tmp.Count());

            return result.ToList();
        }
    }
}
