using iText.Layout.Borders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TE.Core.Domain;
using TE.Data;

namespace TE.Core.Services
{
    public class AdminService
    {
        private readonly TEContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminService(TEContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IList<Examen> ExamenEnAttente()
        {
            return _context.Examens.Where(x => x.StatutExamen == StatutExamen.EnAttente).ToList();
        }
        public Examen FaireExamen(long IdExamen,long idAdmin)
        {
            Examen exam = _context.Examens.FirstOrDefault(x=>x.IdExamen == IdExamen);
            if (exam == null)
            {
                return null;
            }
            Admin admin = (Admin)_context.Users.FirstOrDefault(x => x.Id == idAdmin);
            if (admin == null) {
                return null; }

            exam.StatutExamen = StatutExamen.EnCours;
            exam.DateExamen = DateTime.Now;
            exam.Labo = admin;
            exam.LaboId = idAdmin;
            _context.SaveChanges();
            return exam;
        }
        public async Task<bool> MettreAJourResultatExamenAsync(long examenId, List<string> lignesResultat, string? commentaire, List<IFormFile>? images = null)
        {
            var examen = await _context.Examens
                .Include(e => e.Patient)
                .Include(e => e.Labo)
                .FirstOrDefaultAsync(e => e.IdExamen == examenId);

            if (examen == null) return false;

            var dossierRacine = Path.Combine(_env.WebRootPath, "examens", examen.IdExamen.ToString());
            Directory.CreateDirectory(dossierRacine);

            // Sauvegarder les images si envoyées
            if (images != null && images.Any())
            {
                foreach (var image in images)
                {
                    var cheminImage = Path.Combine(dossierRacine, image.FileName);
                    using (var stream = new FileStream(cheminImage, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    // Stocker le chemin relatif en format URL (avec /)
                    examen.Images.Add(Path.Combine("examens", examen.IdExamen.ToString(), image.FileName).Replace("\\", "/"));
                }
            }

            // Générer contenu PDF selon le type d'examen
            var contenuResultat = new StringBuilder();
            contenuResultat.AppendLine("Résultats d'examen médical");
            contenuResultat.AppendLine($"Nom du patient : {examen.Patient?.Name ?? "N/A"}");
            contenuResultat.AppendLine($"Type d'examen : {examen.TypeExamen}");
            contenuResultat.AppendLine($"Code examen : {examen.CodeExamen}");
            contenuResultat.AppendLine($"Date de réalisation : {DateTime.Now:dd/MM/yyyy}");
            contenuResultat.AppendLine($"Effectué par : {examen.Labo?.Name ?? "N/A"}");
            contenuResultat.AppendLine("-------------------------");
            foreach (var ligne in lignesResultat)
            {
                contenuResultat.AppendLine(ligne);
            }
            if (!string.IsNullOrEmpty(commentaire))
            {
                contenuResultat.AppendLine();
                contenuResultat.AppendLine("Commentaires :");
                contenuResultat.AppendLine(commentaire);
            }

            // Génération du PDF (exemple simple avec iTextSharp ou autre lib)
            var nomPdf = $"resultat_{examen.CodeExamen}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            var cheminPdf = Path.Combine(dossierRacine, nomPdf);

            // Exemple basique d’écriture texte dans un PDF (tu peux remplacer par ta lib PDF préférée)
            await GenererPdfSimple(contenuResultat.ToString(), cheminPdf);

            // Mise à jour de l'entité Examen
            examen.FichierResultats = Path.Combine("examens", examen.IdExamen.ToString(), nomPdf).Replace("\\", "/");
            examen.IsResultatDisponible = true;
            examen.DateResultatRecu = DateTime.Now;
            examen.Commentaires = commentaire;
            examen.StatutExamen = StatutExamen.Terminé;

            await _context.SaveChangesAsync();
            return true;
        }

      
        private async Task GenererPdfSimple(string contenu, string cheminPdf)
        {
            using var writer = new iText.Kernel.Pdf.PdfWriter(cheminPdf);
            using var pdf = new iText.Kernel.Pdf.PdfDocument(writer);
            var document = new iText.Layout.Document(pdf);
            document.SetMargins(40, 40, 60, 40);

            var bold = iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
            var regular = iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);

            // Titre
            document.Add(new iText.Layout.Element.Paragraph("Résultats d'examen médical")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(bold)
                .SetFontSize(18)
                .SetMarginBottom(20));

            // Extraire les lignes clés
            var lignes = contenu.Split(Environment.NewLine, StringSplitOptions.None);
            var infos = new List<(string Label, string Valeur)>();
            var resultats = new List<string>();
            string? commentaire = null;
            bool inCommentaire = false;

            foreach (var ligne in lignes)
            {
                if (string.IsNullOrWhiteSpace(ligne)) continue;

                if (ligne.StartsWith("Commentaires"))
                {
                    inCommentaire = true;
                    continue;
                }

                if (inCommentaire)
                {
                    commentaire ??= "";
                    commentaire += ligne + "\n";
                }
                else if (ligne.Contains(":"))
                {
                    var parts = ligne.Split(':', 2);
                    infos.Add((parts[0].Trim(), parts.Length > 1 ? parts[1].Trim() : ""));
                }
                else
                {
                    resultats.Add(ligne);
                }
            }

            // Table de métadonnées (nom, date, code, labo…)
            var tableInfo = new iText.Layout.Element.Table(new float[] { 150, 350 });
            tableInfo.SetWidth(iText.Layout.Properties.UnitValue.CreatePercentValue(100));

            foreach (var (label, valeur) in infos)
            {
                tableInfo.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(label).SetFont(bold)).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY));
                tableInfo.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(valeur).SetFont(regular)));
            }

            document.Add(tableInfo);
            document.Add(new iText.Layout.Element.Paragraph("\n"));

            // Titre pour les résultats
            document.Add(new iText.Layout.Element.Paragraph("Données / Résultats de l'examen :")
                .SetFont(bold)
                .SetFontSize(13)
                .SetUnderline()
                .SetMarginBottom(10));

            // Résultats sous forme de tableau
            var tableResultats = new iText.Layout.Element.Table(new float[] { 1 });
            tableResultats.SetWidth(iText.Layout.Properties.UnitValue.CreatePercentValue(100));

            foreach (var res in resultats)
            {
                tableResultats.AddCell(new iText.Layout.Element.Cell()
                    .Add(new iText.Layout.Element.Paragraph(res).SetFont(regular))
                    .SetPadding(5));
            }

            document.Add(tableResultats);
            document.Add(new iText.Layout.Element.Paragraph("\n"));

            // Commentaires
            if (!string.IsNullOrWhiteSpace(commentaire))
            {
                document.Add(new iText.Layout.Element.Paragraph("Commentaires :")
                    .SetFont(bold)
                    .SetFontSize(13)
                    .SetMarginTop(10));
                document.Add(new iText.Layout.Element.Paragraph(commentaire.Trim())
                    .SetFont(regular)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT));
            }

            // Espace pour la signature
            document.Add(new iText.Layout.Element.Paragraph("\n\n"));
            var signatureTable = new iText.Layout.Element.Table(1);
            signatureTable.SetWidth(iText.Layout.Properties.UnitValue.CreatePercentValue(40));
            signatureTable.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT);
            signatureTable.AddCell(new iText.Layout.Element.Cell(4, 1)
                .SetHeight(80)
                .SetBorder(new SolidBorder(1))
                .Add(new iText.Layout.Element.Paragraph("Signature du laboratoire").SetFont(regular).SetFontSize(10).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)));

            document.Add(signatureTable);

            document.Close();
            await Task.CompletedTask;
        }



    }
}
