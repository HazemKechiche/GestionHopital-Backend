using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TE.Core.Domain;
using TE.Core.Interfaces;
using TE.Data;

namespace TE.Core.Services
{
    public class FicheMedicaleService
    {
        private readonly TEContext _uow;
        public FicheMedicaleService(TEContext uow)
        {
            _uow = uow;
        }
        public List<FicheMedical> getAllFiches()
        {
            return _uow.FichesMedicales.ToList();
        }
        public FicheMedical addFiche(FicheMedical ficheMedical)
        {
           var result = _uow.FichesMedicales.Add(ficheMedical);
            _uow.SaveChanges();
            return result.Entity;
        }
        public FicheMedical? UpdateFicheForPatient(int patientId, FicheMedical updatedFiche)
        {
            var fiche = _uow.FichesMedicales
                .FirstOrDefault(f => f.patient != null && f.patient.Id == patientId);

            if (fiche == null)
                return null;

            // Mise à jour des champs simples
            if (updatedFiche.poids.HasValue)
                fiche.poids = updatedFiche.poids;

            if (updatedFiche.taille.HasValue)
                fiche.taille = updatedFiche.taille;

            fiche.age = updatedFiche.age ?? fiche.age;
            fiche.sexe = updatedFiche.sexe ?? fiche.sexe;
            fiche.Fummeur = updatedFiche.Fummeur ?? fiche.Fummeur;
            fiche.Alcolique = updatedFiche.Alcolique ?? fiche.Alcolique;
            fiche.typeSang = updatedFiche.typeSang ?? fiche.typeSang;

            // Calcul automatique de l'IMC (IMC = poids(kg) / taille(m)^2)
            if (fiche.poids.HasValue && fiche.taille.HasValue && fiche.taille != 0)
            {
                fiche.IMC = fiche.poids.Value / (fiche.taille.Value * fiche.taille.Value);
            }

            // Ajout aux listes sans écraser les existantes

            void AddDistinct(List<string>? existingList, List<string>? newList)
            {
                if (newList == null || newList.Count == 0)
                    return;

                if (existingList == null)
                    existingList = new List<string>();

                foreach (var item in newList)
                {
                    if (!existingList.Contains(item))
                        existingList.Add(item);
                }
            }

            AddDistinct(fiche.antecedentsFamiliaux, updatedFiche.antecedentsFamiliaux);
            AddDistinct(fiche.antecedentsMedicaux, updatedFiche.antecedentsMedicaux);
            AddDistinct(fiche.traitementChroniques, updatedFiche.traitementChroniques);
            AddDistinct(fiche.AllergiesConnus, updatedFiche.AllergiesConnus);

            _uow.SaveChanges();

            return fiche;
        }


    }
}
